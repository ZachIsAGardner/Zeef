using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderBoil : MonoBehaviour
{
    [Header("MonoBehaviour Settings")]
    public Material Material;
    public float IntervalTimeLength = 0.25f;
    public float IntervalMax = 3;

    [Header("Shader Settings")]
    public float DistortionDamper = 100;
    public float DistortionSpreader = 2;
    public float TimeMultiplier = 0.1f;

    private float intervalCount = 0;
    private float myTime;
    private float intervalTime = 0;

    // ---

    private void Start() 
    {
        gameObject.GetComponent<Renderer>().material = new Material(Material);

        gameObject.GetComponent<Renderer>().material.SetFloat("_DistortionDamper", DistortionDamper);
        gameObject.GetComponent<Renderer>().material.SetFloat("_DistortionSpreader", DistortionSpreader);
        gameObject.GetComponent<Renderer>().material.SetFloat("_TimeMultiplier", TimeMultiplier);

        intervalTime = IntervalTimeLength;
    }

    private void Update() 
    {
        intervalTime -= Time.deltaTime;

        if (intervalTime <= 0) 
        {
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
