using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
    [CreateAssetMenu(menuName = "ScriptableObjectContainers/Sprites")]
    public class SpritesObjectContainer : ScriptableObject
    {   
        public List<SpritesObject> spritesObjects;
    }    
}