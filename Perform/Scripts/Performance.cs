using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef.Extension;
using Zeef.GameManager;

namespace Zeef.Perform {
	[Serializable]
	public abstract class Performance : MonoBehaviour {

		protected PerformanceReference performanceRef;
		protected PerformanceTrigger trigger;
		protected Game game;

		GameObject container;

		TextBox textBox;
		ResponseBox responseBox;

		// is currently performing
		[HideInInspector]
		public bool performing;

		protected abstract Branch BranchStart();
		protected virtual void AdditionalSetup() { }
		protected virtual void AdditionalEnd() { }

		#region LifeCycle

		void Start() {
			game = Game.Main();
			performanceRef = PerformanceReference.Main();
			container = Container.FindContainerObjectInParent(
				ContainerID.PerformanceElements, 
				Game.Canvas().transform
			);
		}

		public void StartPerformance(PerformanceTrigger trigger) {
			if (performing) return;

			AdditionalSetup();
			
			game.EnterCutscene();

			performing = true;
			this.trigger = trigger;

			CreateBorder();

			StartCoroutine(DigestBranch(BranchStart()));
		}
		
		protected virtual void EndPerformance() {
			container.transform.DestroyChildren();
			
			game.ExitCutscene();

			if (trigger) trigger.SetTriggered(false);
			performing = false;

			AdditionalEnd();
		}

		#endregion

		#region Utility

		bool AnyOtherActivePerformances() {
			return FindObjectsOfType<Performance>().Where(p => p.performing).Count() > 0;
		}

		#endregion

		#region HandleContent

		IEnumerator DigestBranch(Branch branch) {
			List<Section> sections = branch.Sections;
			bool end = true;

			int i = 0;
			foreach (var section in sections) {
				yield return StartCoroutine(PlaySection(section, branch));	

				if (i == sections.Count - 1 && branch.Paths != null) {
					CoroutineWithData cd = new CoroutineWithData(
						this, 
						GetResponse(branch.Paths)
					);
					yield return cd.coroutine;	
					int response = (int)cd.result;
					Path path = branch.Paths[response];

					if (path.sideEffect != null) path.sideEffect();
					StartCoroutine(DigestBranch(path.branch));

					end = false;
					break;
				}

				i++;
			}

			if (end) {
				EndPerformance();
			}
		}

		IEnumerator PlaySection(Section section, Branch branch) {
			if (section.action != null) section.action();

			CreateTextBox(section, branch);
			yield return StartCoroutine(WaitInput());
		}

		IEnumerator GetResponse(List<Path> paths) {
			CreateResponseBox(paths);
			CoroutineWithData cd = new CoroutineWithData(
				this,
				WaitResponse()
			);
			yield return cd.coroutine;
			yield return cd.result;
		}

		#endregion

		#region UI

		void CreateBorder() {
			Instantiate(performanceRef.border, container.transform);
		}

		void CreateTextBox(Section section, Branch branch) {
			if (String.IsNullOrEmpty(section.text) && textBox) {
				textBox.Die();
			}
			TextBoxOptions options = (section.options != null) ? section.options : branch.Options;
			if (textBox) {
				textBox.Execute(section, options);
			} else {
				textBox = TextBox.Initialize(
					Instantiate(performanceRef.textBoxPrefab, container.transform),
					section, 
					options
				);
			}
		}

		void CreateResponseBox(List<Path> paths) {
			responseBox = ResponseBox.Initialize(
				Instantiate(performanceRef.responseBoxPrefab, container.transform),
				paths
			);
		}

		#endregion

		#region UserInput

		IEnumerator WaitInput() {
			bool wait = true;
			while (wait) {
				if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) {
					if (!textBox.Crawling) {
						wait = false;
					} else {
						textBox.SetSpeed(0);
					}
				}
				yield return null;
			}
			yield return null;
		}

		IEnumerator WaitResponse() {
			bool wait = true;
			int result = -1;
			while (wait) {

				if (Input.GetKeyDown(KeyCode.UpArrow)) {
					responseBox.DecrementChoice();
				}
				if (Input.GetKeyDown(KeyCode.DownArrow)) {
					responseBox.IncrementChoice();
				}

				if (Input.GetButtonDown("Fire1")) {
					result = responseBox.Choice;
					wait = false;
				}
				yield return null;
			}

			responseBox.Die();
			yield return result;
		}

		#endregion
	}
}
