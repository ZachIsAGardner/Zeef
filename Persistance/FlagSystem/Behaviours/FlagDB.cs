using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
// ---
using Zeef.GameManagement;

namespace Zeef.Persistance {

    [RequireComponent(typeof (SingleInstanceChild))]
    public class FlagDB : MonoBehaviour {
        
        [SerializeField] FlagsSeed flagSeed;
        
        public List<FlagsContainer> FlagsContainers { get; set; }

        void Awake() {
            if (flagSeed == null) throw new Exception("FlagDB is missing a class to seed from");
            FlagsContainers = flagSeed.Seed();
        }

        public static FlagDB Main() => 
            SingleInstance.Main().GetComponentInChildren<FlagDB>();

        public void SetFlag(int id, bool value) => 
            GetFlagsContainer(SceneManager.GetActiveScene().name).GetFlag(id).Value = value;

        public FlagsContainer GetFlagsContainer(string sceneName) => 
            FlagsContainers.First(c => c.SceneName == sceneName);
        
        public void SetFlagsContainers(List<FlagsContainer> flagsContainers) => 
            this.FlagsContainers = flagsContainers;
        
    }
}