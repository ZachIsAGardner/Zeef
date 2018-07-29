using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeef.GameManagement {

    // What scene to load, how to load it, and define player position and orientation
    [Serializable]
    public class SceneInfo {

        [SerializeField] private string scene;
        public string Scene { get { return scene; } }

        [SerializeField] private int spawn;
        public int Spawn { get { return spawn; } }

        [SerializeField] private FacingsEnum facingID; 
        public FacingsEnum FacingID { get { return facingID; } }

        [SerializeField] private LoadSceneMode loadMode;
        public LoadSceneMode LoadMode { get { return loadMode; } }

        public SceneInfo(string scene) {
            this.scene = scene;
            this.spawn = -1;
            this.facingID = FacingsEnum.Right;
            this.loadMode = LoadSceneMode.Single;
        }

        public SceneInfo(string scene, FacingsEnum facing = FacingsEnum.Right, int spawn = 0, LoadSceneMode loadMode = LoadSceneMode.Single) {
            this.scene = scene;
            this.spawn = spawn;
            this.facingID = facing;
            this.loadMode = loadMode;
        }
    }
}