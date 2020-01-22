using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional
{
    public class BoilShader : MonoBehaviour
    {
        [Header("MonoBehaviour Settings")]

        [Required]
        public Material Material;
        public float IntervalTimeLength = 0.25f;
        public float IntervalMax = 3;
        public bool CancelOffsetOnFirstFrame = true;

        [Header("Shader Settings")]

        public float DistortionDamper = 100;
        public float DistortionSpreader = 2;
        public float TimeMultiplier = 0.1f;

        private float intervalCount = 0;
        private float myTime;
        private float intervalTime = 0;

        private Material material;

        // ---

        private void Start() 
        {
            material = gameObject.GetComponent<Renderer>().material;
            gameObject.GetComponent<Renderer>().material = new Material(Material);

            gameObject.GetComponent<Renderer>().material.SetFloat("_DistortionDamper", DistortionDamper);
            gameObject.GetComponent<Renderer>().material.SetFloat("_DistortionSpreader", DistortionSpreader);
            gameObject.GetComponent<Renderer>().material.SetFloat("_TimeMultiplier", TimeMultiplier);

            intervalTime = IntervalTimeLength;
        }

        private void Update() 
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_DistortionDamper", DistortionDamper);
            gameObject.GetComponent<Renderer>().material.SetFloat("_DistortionSpreader", DistortionSpreader);
            gameObject.GetComponent<Renderer>().material.SetFloat("_TimeMultiplier", TimeMultiplier);

            intervalTime -= Time.deltaTime;

            if (intervalTime <= 0) 
            {
                if (myTime == 0 && CancelOffsetOnFirstFrame)
                    gameObject.GetComponent<Renderer>().material.SetFloat("_IsActive", 0);
                else
                    gameObject.GetComponent<Renderer>().material.SetFloat("_IsActive", 1);

                intervalCount++;

                myTime += 1;
                gameObject.GetComponent<Renderer>().material.SetFloat("_MyTime", myTime);

                intervalTime = IntervalTimeLength;
                if (intervalCount >= IntervalMax)
                {
                    myTime = 0;

                    intervalCount = 0;
                }
            }
        }
    }
}

