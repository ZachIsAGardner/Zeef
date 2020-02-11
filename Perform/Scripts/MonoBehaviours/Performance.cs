using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
// ---
using Zeef.Menu;

namespace Zeef.Perform
{
	[Serializable]
	public abstract class Performance<T> : Performance
    {
		public T Model;

		public async Task ExecuteAsync(T model) 
		{
			Model = model;
			await ExecuteAsync();
		}
	}

	[Serializable]
	public abstract class Performance : MonoBehaviour
    {
		// is currently performing
		public bool Performing { get; private set; }

		private TextBoxUI textBoxUIInstance;
		private GameObject border;

		public event EventHandler BeforePerformanceStart;
		public event EventHandler BeforePerformanceEnd;

		protected abstract Branch BranchStart();
		protected virtual void AdditionalSetup() { }
		protected virtual void AdditionalEnd() { }

		// ---

		// Start performance
		public async Task ExecuteAsync()
        {
			if (Performing) 
				return;
				
			Performing = true;

			OnBeforePerformanceStart();

			AdditionalSetup();	

			border = Instantiate(PerformanceContent.Border, FindObjectOfType<Canvas>().transform);
			await DigestBranchAsync(BranchStart());
		}
		
 		void EndPerformance()
        {
			OnBeforePerformanceEnd();

			if (textBoxUIInstance != null) textBoxUIInstance.Close();

			Destroy(border);
			
			Performing = false;

			AdditionalEnd();
		}
		
		// ---
		// Handle Content

		// Play all sections and respond if there are paths
		private async Task DigestBranchAsync(Branch branch)
        {
			List<Section> sections = branch.Sections;

			// Play sections
			foreach (var section in sections)
                await PlaySectionAsync(section, branch);	

			// If we've gone through all the sections and there are paths then 
			// the player needs to respond to a question.
			// Else we're done so end the performance.
			if (branch.Paths != null)
            {
				Path path = await GetPathAsync(branch);
				if (path.SideEffect != null)
                    path.SideEffect();
				await DigestBranchAsync(path.Branch);
			}
            else
            {
				EndPerformance();
			}
		}

		private async Task PlaySectionAsync(Section section, Branch branch)
        {
			// Execute StartAction
			if (section.StartAction != null) 
				await section.StartAction();

			// Combine branch and section models with priority to section
			TextBoxUIFullModel model = new TextBoxUIFullModel(
				text: section.TextBoxUIModel.Text,
                speaker: (!string.IsNullOrEmpty(section.TextBoxUIModel.Speaker)) 
                    ? section.TextBoxUIModel.Speaker 
                    : branch.TextBoxUIPartialModel?.Speaker,
				auto: section.TextBoxUIModel.Auto ?? branch.TextBoxUIPartialModel?.Auto,
				tone: section.TextBoxUIModel.Tone ?? branch.TextBoxUIPartialModel?.Tone,
				crawlTime: section.TextBoxUIModel.CrawlTime ?? branch.TextBoxUIPartialModel?.CrawlTime,
				closeWhenDone: section.TextBoxUIModel.CloseWhenDone ?? branch.TextBoxUIPartialModel?.CloseWhenDone,
				toneIntervalMax: section.TextBoxUIModel.ToneIntervalMax ?? branch.TextBoxUIPartialModel?.ToneIntervalMax
			);

			// Create and execute text box
			if (section.TextBoxUIModel != null)
            {
				// Create new text box and execute on it.
				if (textBoxUIInstance == null)
                {
					textBoxUIInstance = TextBoxUI.Initialize(
						prefab: branch?.PerformanceModel?.TextBoxPrefab ?? PerformanceContent.TextBoxPrefab,
						position: Vector2.zero,
						model: model
					);

					await textBoxUIInstance.ExecuteAsync();
				} 
				// Execute model on existing text box.
				else 
				{
					await textBoxUIInstance.ExecuteAsync(model);
				}
			}

			// Execute EndAction
			if (section.EndAction != null) 
				await section.EndAction();
		}

		private async Task<Path> GetPathAsync(Branch branch)
        {	
			await new WaitForUpdate();

			int attempt = 0;
			Path selection = null;
			while (selection == null)
            {
                LinearMenuSelect menuSelectInstance = LinearMenuSelect.Initialize(
                    prefab: branch?.PerformanceModel?.ResponseBoxPrefab ?? PerformanceContent.ResponseBoxPrefab,
                    models: branch.Paths
                        .Select(p => new MenuItemUIModel(p, p.Text))
                        .ToList()
                );

                selection = (Path)(await menuSelectInstance.GetSelectionAsync());

				await new WaitForUpdate();

				attempt += 1;
				if (attempt > 10) break;
			}

			return selection;
		}

		// ---
		// Events

		protected virtual void OnBeforePerformanceStart()
        {
			if (BeforePerformanceStart != null) 
				BeforePerformanceStart(this, EventArgs.Empty);
		}

		protected virtual void OnBeforePerformanceEnd()
        {
			if (BeforePerformanceEnd != null) 
				BeforePerformanceEnd(this, EventArgs.Empty);
		}
	}
}
