using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public abstract class InteractableObject : MonoBehaviour {

		[SerializeField] private PromptsEnum promptID;
		[SerializeField] private bool onTouch;

		private GameObject promptClone;
		protected bool triggered;

		protected abstract Task TriggerActionAsync();

		protected void ShowPrompt(GameObject go) {
			if (promptClone) return;

			promptClone = Instantiate(GameContent.GetPrompt(promptID), go.transform);
		}

		protected void HidePrompt() {
			if (promptClone) Destroy(promptClone);	
		}

		void OnTriggerEnter2D(Collider2D col) {
			if (triggered) return;

			if (col.tag == "Player")
				if (!onTouch) ShowPrompt(col.gameObject);
				else TriggerActionAsync();		
		}

		void OnTriggerStay2D(Collider2D col) {
			if (onTouch || triggered) return;

			if (col.tag == "Player" && Input.GetButtonDown("Fire1")) {	
				TriggerActionAsync();
				triggered = true;
			} 
		}

		void OnTriggerExit2D(Collider2D col) {
			if (!onTouch && col.tag == "Player") HidePrompt();	
		}
	}
}