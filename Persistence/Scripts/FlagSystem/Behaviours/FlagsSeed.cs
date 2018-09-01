using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Persistence {

    [System.Serializable]
    public abstract class FlagsSeed : MonoBehaviour {
        
        public abstract List<FlagsContainer> Seed();
    }
}