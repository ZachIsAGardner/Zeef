using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional 
{
    public class ExternalTriggerStay2DEventArgs 
    {
        public Collider2D Other { get; private set; }
        public int Defense { get; private set; }

        public ExternalTriggerStay2DEventArgs(Collider2D other, int defense) 
        {
            Other = other;
            Defense = defense;
        }
    }

	[RequireComponent(typeof(BoxCollider2D))]
	public class HurtBox2D : MonoBehaviour 
    {
        public event EventHandler<ExternalTriggerStay2DEventArgs> ExternalTriggerStay2D;

        public int Defense = 0;
        public List<string> Weaknesses = new List<string> { };

		// ---

        private void OnTriggerStay2D(Collider2D other) {
            HitBox2D hitBox = other.GetComponent<HitBox2D>();

            if (hitBox != null && (Weaknesses.IsNullOrEmpty() 
                || Weaknesses.Any(w => String.IsNullOrWhiteSpace(w))
                || Weaknesses.Any(w => hitBox.InteractionTypes.Any(i => i.ToLower() == w.ToLower()))
            ))
            {
			    OnExternalTriggerStay2D(other);
            }
		}

        private void OnExternalTriggerStay2D(Collider2D other) 
        {
            if (ExternalTriggerStay2D != null) 
				ExternalTriggerStay2D(this, new ExternalTriggerStay2DEventArgs(other, Defense));
        }
	}
}
