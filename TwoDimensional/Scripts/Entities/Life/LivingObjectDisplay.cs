using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    [RequireComponent (typeof(LivingObject))]
    public class LivingObjectDisplay : MonoBehaviour {

        [SerializeField] GameObject explosionPrefab;
        [SerializeField] Vector3 explosionOffset = new Vector3(0,0,-10);

        private List<SpriteRenderer> spriteRenderers;
 
        private LivingObject livingObject;
        private List<Color> colors;
        private bool exitBlink;

        // ---

        void Awake() {
            livingObject = GetComponent<LivingObject>();
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();

            colors = spriteRenderers.Select(sp => sp.color).ToList();

            
            livingObject.BeforeFreeze += OnBeforeFreeze;
            livingObject.AfterFreeze += OnAfterFreeze;
            livingObject.BeforeInvincibility += OnBeforeInvincibility;
            livingObject.AfterInvincibility += OnAfterInvincibility;
            livingObject.BeforeDie += OnBeforeDie;
        }

        void Update() {
            if (!livingObject.IsFrozen && !livingObject.IsInvincible) {
                int i = 0;
                foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                    spriteRenderer.color = colors[i];
                    i++;
                }
            }
        }

        private void OnBeforeFreeze(object source, EventArgs args) {
            foreach (var s in spriteRenderers)
                s.color = Color.black;
        }

        private void OnAfterFreeze(object source, EventArgs args) {
            int i = 0;
            foreach (var s in spriteRenderers) {
                s.color = colors[i];
                i++;
            }
        }

        private async void OnBeforeInvincibility(object source, EventArgs args) {
            exitBlink = false;
            await BlinkAsync();
        }

        private void OnAfterInvincibility(object source, EventArgs args) {
            exitBlink = true;
        }

        private void OnBeforeDie(object source, EventArgs args) {
            GameManager.SpawnActor(explosionPrefab, this.transform.position + explosionOffset);
        }

        private async Task BlinkAsync() {
            bool visible = true;

			while(!exitBlink) {
                foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                    Color color = spriteRenderer.color;

                    spriteRenderer.color = (visible) 
                        ? new Color(color.r, color.g, color.b, 0) 
                        : new Color(color.r, color.g, color.b, 1);
                }

                visible = !visible;

                await new WaitForSeconds(0.05f);
			}

            int i = 0;
			foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.color = colors[i];
                i++;
            }
        }
    }
}