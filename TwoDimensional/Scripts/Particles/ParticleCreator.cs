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
        [SerializeField] Particle particle;

        [SerializeField] bool playOnStart = true;

        [SerializeField] bool particlesBecomeChildren;
        [SerializeField] bool startPositionFromBoxCollider2DBounds = true;

        [Header("Particle Settings")]
        [SerializeField] FloatRange lifeTime = new FloatRange(1, 2);

        [SerializeField] int amount = 5;

        [SerializeField] Vector2 velMin;
        [SerializeField] Vector2 velMax;
        [SerializeField] float grav;

        [SerializeField] Vector3 rotateVelMin;
        [SerializeField] Vector3 rotateVelMax;        

        [SerializeField] Vector3 rotateOffsetMin;
        [SerializeField] Vector3 rotateOffsetMax;  

        [SerializeField] bool fadeOverTime = false;
        [SerializeField] bool fadeOnDestroy = false;

        // time between bursts
        [SerializeField] float loopLength = -1;
        // time between individual particle generations
        [SerializeField] float offsetLength = -1;

        void Start() {
            if (playOnStart) 
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

        public IEnumerator CreateParticlesCoroutine() {
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
                        new Vector2(velMin.x, velMax.x).RandomValue() * Math.Sign(transform.localScale.x),
                        new Vector2(velMin.y, velMax.y).RandomValue() * Math.Sign(transform.localScale.y)
                    ),
                    rotationVel: new Vector3(
                        new Vector2(rotateVelMin.x, rotateVelMax.x).RandomValue() * Math.Sign(transform.localScale.x),
                        new Vector2(rotateVelMin.y, rotateVelMax.y).RandomValue() * Math.Sign(transform.localScale.y),
                        new Vector2(rotateVelMin.z, rotateVelMax.z).RandomValue() * Math.Sign(transform.localScale.z)
                    ),
                    rotationOffset: new Vector3(
                        new Vector2(rotateOffsetMin.x, rotateOffsetMax.x).RandomValue() * Math.Sign(transform.localScale.x),
                        new Vector2(rotateOffsetMin.y, rotateOffsetMax.y).RandomValue() * Math.Sign(transform.localScale.y),
                        new Vector2(rotateOffsetMin.z, rotateOffsetMax.z).RandomValue() * Math.Sign(transform.localScale.z)
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