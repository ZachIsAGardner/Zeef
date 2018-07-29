using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {
    public class ParticleCreator : MonoBehaviour 
    {
        public Particle particle;
        public IntegerPair lifeTime = new IntegerPair(1, 2);

        public int amount = 5;

        public float vel;
        public Vector2Range dir = new Vector2Range(
            new Vector2(-1,-1),
            new Vector2(1,1)
        );
        public float grav;

        public bool fade = false;

        // time between bursts
        public float loopTime = -1;
        // time between individual particle generations
        public float intervalTime = -1;

        void Awake() {
            StartCoroutine(Run());
        }

        IEnumerator Run() {
            if (loopTime > 0) {
                while (true) yield return StartCoroutine(CreateParticles());
            } else {
                StartCoroutine(CreateParticles());
                while (transform.childCount > 0) yield return null;
            }
            Destroy(gameObject);
        }

        IEnumerator CreateParticles() {
            int i = 0;

            while (i < amount) {
                Instantiate(particle, transform).Initialize(lifeTime.RandomValue(), dir.RandomValue() * vel, fade, grav);

                if (intervalTime > 0) {
                    yield return new WaitForSeconds(intervalTime);
                } 

                i++;
            }

            if (loopTime > 0) {
                yield return new WaitForSeconds(loopTime);
            } 
        }
    }
}