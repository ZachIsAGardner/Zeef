using System;
using System.Linq;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class SpritesContent : MonoBehaviour {

        [SerializeField] SpritesScriptablesContainer spritesScriptablesContainer;

        public static SpritesContent Main() {
            return Utility.FindObjectOfTypeWithError<SpritesContent>();
        }

        public SpritesScriptable GetSpritesScriptable(int id) {
            SpritesScriptable result = spritesScriptablesContainer.SpritesObjects.FirstOrDefault(sp => sp.ID == id);
            if (result == null) throw new Exception($"Couldn't find a sprites object with an id matching {id}");
            return result;
        }

        public SpritesScriptable TryGetSpritesScriptable(int id) {
            SpritesScriptable result = spritesScriptablesContainer.SpritesObjects.FirstOrDefault(sp => sp.ID == id);
            return result;
        }
    }    
}