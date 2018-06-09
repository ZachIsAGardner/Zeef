using System;
using System.Linq;
using UnityEngine;
// ---
using Zeef.GameManager;

namespace Zeef.TwoDimensional 
{
    public class SpritesReference : MonoBehaviour 
    {
        [SerializeField] SpritesObjectContainer spritesObjectContainer;

        public static SpritesReference Main() {
            return Game.Main().GetComponentInChildren<SpritesReference>();
        }

        public SpritesObject GetSpritesObject(SpritesID id) {
            SpritesObject result = spritesObjectContainer.spritesObjects.FirstOrDefault(sp => sp.id == id);
            if (result == null) throw new Exception($"Couldn't find a sprites object with an id matching {id}");
            return result;
        }
    }    
}