using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional {

    [CreateAssetMenu(menuName = "Scriptables/TileFolder")]
    public class TileFolderScriptable : ScriptableObject {

        public List<GameObject> Tiles;
    }
}