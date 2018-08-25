using System;
using System.Linq;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class SpritesContent : MonoBehaviour {

        [SerializeField] SpritesObjectsContainer spritesObjectsContainer;

        public static SpritesContent Main() {
            return Utility.FindObjectOfTypeWithError<SpritesContent>();
        }

        public SpritesObject GetSpritesObject(int id) {
            SpritesObject result = spritesObjectsContainer.spritesObjects.FirstOrDefault(sp => sp.ID == id);
            if (result == null) throw new Exception($"Couldn't find a sprites object with an id matching {id}");
            return result;
        }

        public SpritesObject TryGetSpritesObject(int id) {
            SpritesObject result = spritesObjectsContainer.spritesObjects.FirstOrDefault(sp => sp.ID == id);
            return result;
        }
    }    
}