using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.GameManager;

namespace Zeef.TwoDimensional
{
	public abstract class InteractableObject : MonoBehaviour 
	{
		public PromptID promptID;
		GameObject prompt;
		public bool onTouch;
		GameObject promptClone;

		protected bool triggered;

		protected abstract void TriggerAction();

		protected virtual void Start() {
			prompt = GameReference.Main().GetPrompt(promptID);
		}

		protected void ShowPrompt(GameObject go) 
		{
			if (promptClone) return;

			promptClone = Instantiate(prompt, go.transform);
		}

		protected void HidePrompt() 
		{
			if (promptClone) {
				Destroy(promptClone);
			}
		}

		void OnTriggerEnter2D(Collider2D col) 
		{
			if (triggered) return;

			if (col.tag == "Player") {
				if (!onTouch) {
					ShowPrompt(col.gameObject);
				} else {
					TriggerAction();
				}
			}
		}

		void OnTriggerStay2D(Collider2D col) 
		{
			if (onTouch || triggered) return;

			if (col.tag == "Player" && Input.GetButtonDown("Fire1")) {	
				TriggerAction();
				triggered = true;
			} 
		}

		void OnTriggerExit2D(Collider2D col) 
		{
			if (!onTouch && col.tag == "Player") {
				HidePrompt();
			}
		}

	}

}