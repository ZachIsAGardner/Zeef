using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeef.Persistence {

    public class FlagDB : MonoBehaviour {
        
        private static FlagDB flagDB;

        [SerializeField] FlagsSeed flagSeed;
        
        public static List<FlagsContainer> FlagsContainers { get; set; }

        void Awake() {
            if (flagDB != null) throw new Exception("Only one FlagDB may exist at a time.");
            flagDB = this;

            if (flagSeed == null) throw new Exception("FlagDB is missing a class to seed from");
            FlagsContainers = flagSeed.Seed();
        }

        public static void SetFlag(int id, bool value) => 
            GetFlagsContainer(SceneManager.GetActiveScene().name).GetFlag(id).Value = value;

        public static FlagsContainer GetFlagsContainer(string sceneName) => 
            FlagsContainers.First(c => c.SceneName == sceneName);
        
        public static void SetFlagsContainers(List<FlagsContainer> flagsContainers) => 
            FlagsContainers = flagsContainers;
        
    }
}