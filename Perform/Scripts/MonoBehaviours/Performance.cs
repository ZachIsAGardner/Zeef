﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
// ---
using Zeef.GameManagement;
using Zeef.Menu;

namespace Zeef.Perform {
	[Serializable]
	public abstract class Performance : MonoBehaviour {

		// is currently performing
		public bool Performing { get; private set; }

		protected abstract Branch BranchStart();
		protected virtual void AdditionalSetup() { }
		protected virtual void AdditionalEnd() { }

		// All performance ui elements get placed in here
		private GameObject border;

		// ---

		// Start performance
		public async Task ExecuteAsync() {
			if (Performing) return;

			AdditionalSetup();	
			GameManager.Main().EnterCutscene();
			Performing = true;

			border = Instantiate(PerformanceContent.Border, FindObjectOfType<Canvas>().transform);
			await DigestBranchAsync(BranchStart());
		}
		
		protected virtual void EndPerformance() {
			Destroy(border);
			
			GameManager.Main().ExitCutscene();
			Performing = false;

			AdditionalEnd();
		}
		
		// ---
		// Handle Content

		// Play all sections and respond if there are paths
		private async Task DigestBranchAsync(Branch branch) {
			List<Section> sections = branch.Sections;

			// Play sections
			foreach (var section in sections) await PlaySectionAsync(section, branch);	

			// If we've gone through all the sections and there are paths then 
			// the player needs to respond to a question.
			// Else we're done so end the performance.
			if (branch.Paths != null) {
				Path path = await GetPathAsync(branch);
				if (path.SideEffect != null) path.SideEffect();
				await DigestBranchAsync(path.Branch);
			} else {
				EndPerformance();
			}
		}

		private async Task PlaySectionAsync(Section section, Branch branch) {
			// Execute action if there is one
			if (section.Action != null) section.Action();

			if (section.TextBoxUIModel != null) {
				// Create textbox
				TextBoxUI textBoxUI = TextBoxUI.Initialize(
					PerformanceContent.TextBoxPrefab,
					Utility.FindObjectOfTypeWithError<Canvas>().transform,
					Vector2.zero,
					section.TextBoxUIModel
				);

				// Wait for text box to finish running
				await textBoxUI.ExecuteAsync();
			}
		}

		private async Task<Path> GetPathAsync(Branch branch) {			
			Path selection = (Path)await VerticalMenuSelectUI
				.Initialize(
					PerformanceContent.ResponseBoxPrefab, 
					FindObjectOfType<Canvas>().GetComponent<RectTransform>(),
					branch.Paths.Select(p => new MenuItemUIModel(p, p.Text)).ToList()
				).GetSelectionAsync();

			return selection;
		}
	}
}
