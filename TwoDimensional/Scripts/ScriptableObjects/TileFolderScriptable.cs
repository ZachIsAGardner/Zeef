using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
    [CreateAssetMenu(menuName = "Scriptables/Tile Folder")]
    public class TileFolderScriptable : ScriptableObject 
    {
        public List<GameObject> Tiles;
    }
}