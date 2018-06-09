using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeef.GameManager {
    // What scene to load, how to load it, and define player position and orientation
    [Serializable]
    public class SceneInfo {
        public string scene;
        public FacingID facing = FacingID.Right;
        public int spawn = 0;
        public LoadSceneMode loadMode;

        public SceneInfo(string scene, FacingID facing = FacingID.Right, int spawn = 0, LoadSceneMode loadMode = LoadSceneMode.Single) {
            this.scene = scene;
            this.facing = facing;
            this.spawn = spawn;
            this.loadMode = loadMode;
        }
    }
}