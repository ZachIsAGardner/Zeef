using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
    [CreateAssetMenu(menuName = "SOs Container/Sprites")]
    public class SpritesObjectsContainer : ScriptableObject
    {   
        public List<SpritesObject> spritesObjects;
    }    
}