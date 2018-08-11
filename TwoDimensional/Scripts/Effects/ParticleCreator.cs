using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class ParticleCreator : MonoBehaviour {
        public Particle Particle;

        public bool ParticlesBecomeChildren;
        public bool StartPositionFromBoxCollider2DBounds = true;
        public FloatRange LifeTime = new FloatRange(1, 2);

        public int Amount = 5;

        public float Vel;
        public Vector2Range Dir = new Vector2Range(
            new Vector2(-1,-1),
            new Vector2(1,1)
        );
        public float Grav;

        public bool Fade = false;

        // time between bursts
        public float LoopTime = -1;
        // time between individual particle generations
        public float OffsetTime = -1;

        void Start() {
            StartCoroutine(RunCoroutine());
        }

        private IEnumerator RunCoroutine() {
            if (LoopTime > 0) {
                while (true) yield return StartCoroutine(CreateParticlesCoroutine());
            } else {
                StartCoroutine(CreateParticlesCoroutine());
                while (transform.childCount > 0) yield return null;
            }
            Destroy(gameObject);
        }

        private IEnumerator CreateParticlesCoroutine() {
            int i = 0;

            while (i < Amount) {
                Vector2 pos = Vector2.zero;
                BoxCollider2D col = GetComponent<BoxCollider2D>();
                if (col != null && StartPositionFromBoxCollider2DBounds) {
                    FloatRange x = new FloatRange(col.bounds.min.x, col.bounds.max.x);
                    FloatRange y = new FloatRange(col.bounds.min.y, col.bounds.max.y);
                    pos = new Vector2(x.RandomValue(), y.RandomValue());
                }
                
                Particle.Initialize(
                    prefab: Particle.gameObject, 
                    lifeTime: LifeTime.RandomValue(), 
                    vel: Dir.RandomValue() * Vel, 
                    fade: Fade, 
                    grav: Grav,
                    parent: (ParticlesBecomeChildren) ? transform : null,
                    pos: pos
                );

                if (OffsetTime > 0) yield return new WaitForSeconds(OffsetTime);
                
                i++;
            }

            if (LoopTime > 0) yield return new WaitForSeconds(LoopTime); 
        }
    }
}