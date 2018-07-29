using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Persistance {

    [System.Serializable]
    public abstract class FlagsSeed : MonoBehaviour {
        
        public abstract List<FlagsContainer> Seed();
    }
}