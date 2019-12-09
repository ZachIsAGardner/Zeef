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
			if (Performing) return;
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
			foreach (var section in sections) await PlaySectionAsync(section, branch);	

			// If we've gone through all the sections and there are paths then 
			// the player needs to respond to a question.
			// Else we're done so end the performance.
			if (branch.Paths != null)
            {
				Path path = await GetPathAsync(branch);
				if (path.SideEffect != null) path.SideEffect();
				await DigestBranchAsync(path.Branch);
			} else {
				EndPerformance();
			}
		}

		private async Task PlaySectionAsync(Section section, Branch branch)
        {
			// Execute action if there is one
			if (section.Action != null) section.Action();

			// Combine branch and section models with priority to section
			TextBoxUIFullModel model = new TextBoxUIFullModel(
				section.TextBoxUIModel.Text,
				(!string.IsNullOrEmpty(section.TextBoxUIModel.Speaker)) ? section.TextBoxUIModel.Speaker : branch.TextBoxUIPartialModel?.Speaker,
				(section.TextBoxUIModel.Auto.HasValue) ? section.TextBoxUIModel.Auto : branch.TextBoxUIPartialModel?.Auto,
				(section.TextBoxUIModel.Tone != null) ? section.TextBoxUIModel.Tone : branch.TextBoxUIPartialModel?.Tone,
				(section.TextBoxUIModel.CrawlTime != null) ? section.TextBoxUIModel.CrawlTime : branch.TextBoxUIPartialModel?.CrawlTime
			);

			// Create and execute text box
			if (section.TextBoxUIModel != null)
            {
				if (textBoxUIInstance == null)
                {
					textBoxUIInstance = TextBoxUI.Initialize(
						PerformanceContent.TextBoxPrefab,
						Utility.FindObjectOfTypeWithError<Canvas>().transform,
						Vector2.zero,
						model
					);

					await textBoxUIInstance.ExecuteAsync();
				} else {
					await textBoxUIInstance.ExecuteAsync(model);
				}
			}
		}

		private async Task<Path> GetPathAsync(Branch branch)
        {	
			await new WaitForSeconds(0.25f);

			int attempt = 0;
			Path selection = null;
			while (selection == null)
            {

				selection = (Path)await VerticalMenuSelectUI
					.Initialize(
						PerformanceContent.ResponseBoxPrefab, 
						FindObjectOfType<Canvas>().GetComponent<RectTransform>(),
						branch.Paths.Select(p => new MenuItemUIModel(p, p.Text)).ToList())
					.GetSelectionAsync();

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
