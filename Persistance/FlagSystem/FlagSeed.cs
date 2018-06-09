using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Persistance 
{
    [System.Serializable]
    public abstract class FlagSeed : MonoBehaviour
    {
        public abstract List<FlagsContainer> Seed();
    }
}