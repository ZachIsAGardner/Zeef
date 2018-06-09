using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {
    public static class ParticleDB {

        public static Dictionary<string, GameObject> GetParticleEffects() {
            Dictionary<string, GameObject> particleEffects = new Dictionary<string, GameObject>();
            foreach (var particleEffect in Resources.LoadAll<GameObject>("Content/ParticleEffects")) {
                particleEffects.Add(particleEffect.name, particleEffect);
            }
            return particleEffects;
        }

        public static void CreateParticleEffect(string name, Transform parent) {
            GameObject go = GameObject.Instantiate(GetParticleEffects()[name], parent);
            go.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }

    }
}
