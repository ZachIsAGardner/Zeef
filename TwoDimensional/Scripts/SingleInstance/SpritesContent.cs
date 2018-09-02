using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

    public class SpritesContent : SingleInstance<SpritesContent> {

        [SerializeField] List<SpritesScriptable> spritesScriptables;

        public SpritesScriptable GetSprites(string name) => spritesScriptables.First(sp => sp.name == name); 
    }    
}