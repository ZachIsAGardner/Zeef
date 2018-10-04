using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class ParticleCreator : MonoBehaviour {
        
        [SerializeField] Particle particle;

        [SerializeField] bool particlesBecomeChildren;
        [SerializeField] bool startPositionFromBoxCollider2DBounds = true;
        [SerializeField] FloatRange lifeTime = new FloatRange(1, 2);

        [SerializeField] int amount = 5;

        [SerializeField] Vector2 velMin;
        [SerializeField] Vector2 velMax;
        [SerializeField] float grav;

        [SerializeField] bool fadeOverTime = false;
        [SerializeField] bool fadeOnDestroy = false;

        // time between bursts
        [SerializeField] float loopLength = -1;
        // time between individual particle generations
        [SerializeField] float offsetLength = -1;

        void Start() {
            StartCoroutine(RunCoroutine());
        }

        private IEnumerator RunCoroutine() {
            if (loopLength > 0) {
                while (true) yield return StartCoroutine(CreateParticlesCoroutine());
            } else {
                StartCoroutine(CreateParticlesCoroutine());
                while (transform.childCount > 0) yield return null;
            }
            Destroy(gameObject);
        }

        private IEnumerator CreateParticlesCoroutine() {
            int i = 0;

            while (i < amount) {
                Vector2 pos = Vector2.zero;
                BoxCollider2D col = GetComponent<BoxCollider2D>();
                if (col != null && startPositionFromBoxCollider2DBounds) {
                    FloatRange x = new FloatRange(col.bounds.min.x, col.bounds.max.x);
                    FloatRange y = new FloatRange(col.bounds.min.y, col.bounds.max.y);
                    pos = new Vector2(x.RandomValue(), y.RandomValue());
                }
                
                Particle.Initialize(
                    prefab: particle.gameObject, 
                    lifeTime: lifeTime.RandomValue(), 
                    vel: new Vector2(
                        new Vector2(velMin.x, velMax.x).RandomValue() * transform.localScale.x,
                        new Vector2(velMin.y, velMax.y).RandomValue() * transform.localScale.y
                    ),
                    fadeOverTime: fadeOverTime, 
                    fadeOnDestroy: fadeOnDestroy, 
                    grav: grav,
                    parent: (particlesBecomeChildren) ? transform : null,
                    pos: pos
                );

                if (offsetLength > 0) yield return new WaitForSeconds(offsetLength);
                
                i++;
            }

            if (loopLength > 0) yield return new WaitForSeconds(loopLength); 
        }
    }
}