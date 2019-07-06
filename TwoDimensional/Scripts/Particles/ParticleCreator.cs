using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class ParticleCreator : MonoBehaviour {
        
        [Header("Creator Settings")]
        public Particle Particle;

        public bool PlayOnStart = true;

        public bool ParticlesBecomeChildren;
        public bool StartPositionFromBoxCollider2DBounds = true;

        [Header("Particle Settings")]
        public FloatRange lifeTime = new FloatRange(1, 2);

        public bool FadeOverTime = false;
        public bool FadeOnDestroy = false;

        public IntegerRange Amount = new IntegerRange(1, 1);

        // time between bursts
        public float LoopLength = -1;
        // time between individual particle generations
        public float OffsetLength = -1;

        public Color Color = Color.white;
        
        public FloatRange VelX;
        public FloatRange VelY;
        public float Grav;

        public FloatRange RotateVelX;
        public FloatRange RotateVelY;
        public FloatRange RotateVelZ;

        public FloatRange RotateOffsetX;
        public FloatRange RotateOffsetY;
        public FloatRange RotateOffsetZ;

        void Start() {
            if (PlayOnStart) 
                StartCoroutine(RunCoroutine());
        }

        private IEnumerator RunCoroutine() {
            if (LoopLength > 0) {
                while (true) yield return StartCoroutine(CreateParticlesCoroutine());
            } else {
                StartCoroutine(CreateParticlesCoroutine());
                while (transform.childCount > 0) yield return null;
            }

            Destroy(gameObject);
        }

        public IEnumerator CreateParticlesCoroutine() {
            int i = 0;

            while (i < Amount.RandomValue()) {
                Vector3 pos = new Vector3(0,0, transform.position.z);
                BoxCollider2D col = GetComponent<BoxCollider2D>();
                if (col != null && StartPositionFromBoxCollider2DBounds) {
                    FloatRange x = new FloatRange(col.bounds.min.x, col.bounds.max.x);
                    FloatRange y = new FloatRange(col.bounds.min.y, col.bounds.max.y);
                    pos = new Vector3(x.RandomValue(), y.RandomValue(), transform.position.z);
                }

                Particle.Initialize(
                    prefab: Particle.gameObject, 
                    lifeTime: lifeTime.RandomValue(), 
                    vel: new Vector2(
                        VelX.RandomValue() * Math.Sign(transform.localScale.x),
                        VelY.RandomValue() * Math.Sign(transform.localScale.y)
                    ),
                    rotationVel: new Vector3(
                        RotateVelX.RandomValue() * Math.Sign(transform.localScale.x),
                        RotateVelY.RandomValue() * Math.Sign(transform.localScale.y),
                        RotateVelZ.RandomValue() * Math.Sign(transform.localScale.z)
                    ),
                    rotationOffset: new Vector3(
                        RotateOffsetX.RandomValue() * Math.Sign(transform.localScale.x),
                        RotateOffsetY.RandomValue() * Math.Sign(transform.localScale.y),
                        RotateOffsetZ.RandomValue() * Math.Sign(transform.localScale.z)
                    ),
                    fadeOverTime: FadeOverTime, 
                    fadeOnDestroy: FadeOnDestroy, 
                    grav: Grav,
                    parent: (ParticlesBecomeChildren) ? transform : null,
                    pos: pos,
                    color: Color
                );

                if (OffsetLength > 0) yield return new WaitForSeconds(OffsetLength);
                
                i++;
            }

            if (LoopLength > 0) yield return new WaitForSeconds(LoopLength); 
        }
    }
}