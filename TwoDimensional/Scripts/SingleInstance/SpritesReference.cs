using System;
using System.Linq;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional 
{
    [RequireComponent(typeof (SingleInstanceChild))]
    public class SpritesReference : MonoBehaviour 
    {
        [SerializeField] SpritesObjectsContainer spritesObjectsContainer;

        public static SpritesReference Main() {
            return SingleInstance.Main().GetComponentInChildren<SpritesReference>();
        }

        public SpritesObject GetSpritesObject(SpritesEnum id) {
            SpritesObject result = spritesObjectsContainer.spritesObjects.FirstOrDefault(sp => sp.id == id);
            if (result == null) throw new Exception($"Couldn't find a sprites object with an id matching {id}");
            return result;
        }
    }    
}