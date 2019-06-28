using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef 
{
    public class TimedDestruction : MonoBehaviour
    {
        public float lifeTime = 1;
        private float startLifeTime;

        void Awake() 
        {
            startLifeTime = lifeTime;    
        }

        void Update()
        {
            lifeTime -= 1 * Time.deltaTime;

            if (lifeTime <= 0) 
                Destroy(gameObject);        
        }

        public void Reset() 
        {
            if (startLifeTime <= 0) return; 

            lifeTime = startLifeTime;
        }
    }
}
