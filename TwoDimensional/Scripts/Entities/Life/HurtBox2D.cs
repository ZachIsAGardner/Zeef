using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class ExternalTriggerStay2DEventArgs {
        public Collider2D Other { get; private set; }
        public int Defense { get; private set; }

        public ExternalTriggerStay2DEventArgs(Collider2D other, int defense) {
            Other = other;
            Defense = defense;
        }
    }

	[RequireComponent(typeof(BoxCollider2D))]
	public class HurtBox2D : MonoBehaviour {

        public event EventHandler<ExternalTriggerStay2DEventArgs> ExternalTriggerStay2D;

		[SerializeField] int defense = 0;
        // Built in Editor
        [HideInInspector] public List<string> Weakness = new List<string> { "Any", "Explosion" };

		// ---

        private void OnTriggerStay2D(Collider2D other) {
            HitBox2D hitBox = other.GetComponent<HitBox2D>();

            if (hitBox != null && 
                (
                    Weakness.IsNullOrEmpty() 
                    || Weakness.Any(w => w == InteractionTypeConstant.Any)
                    || Weakness.Any(w => w == hitBox.InteractionType)
                )
            )
            {
			    OnExternalTriggerStay2D(other);
            }
		}

        private void OnExternalTriggerStay2D(Collider2D other) {
            if (ExternalTriggerStay2D != null) 
				ExternalTriggerStay2D(this, new ExternalTriggerStay2DEventArgs(other, defense));
        }
	}
}
