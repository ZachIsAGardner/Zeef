using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class SpritesContent : SingleInstance<SpritesContent> {

        [SerializeField] List<SpritesScriptable> spritesScriptables;

        public static SpritesScriptable GetSprites(string name) => Instance.spritesScriptables.First(sp => sp.name.ToLower() == name.ToLower()); 
    }    
}