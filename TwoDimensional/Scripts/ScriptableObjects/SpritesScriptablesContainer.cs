using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {
    
    [CreateAssetMenu(menuName = "SOs Container/Sprites")]
    public class SpritesScriptablesContainer : ScriptableObject {   
        public List<SpritesScriptable> SpritesObjects;
    }    
}