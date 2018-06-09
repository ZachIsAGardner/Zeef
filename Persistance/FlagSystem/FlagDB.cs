using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
// ---
using Zeef.GameManager;

namespace Zeef.Persistance {
    [RequireComponent(typeof (SingleInstanceChild))]
    public class FlagDB : MonoBehaviour 
    {
        [SerializeField] FlagSeed flagSeed;
        [HideInInspector] public List<FlagsContainer> flagsContainers;

        void Awake() 
        {
            if (flagSeed == null) throw new Exception("FlagDB is missing a class to seed from");
            flagsContainers = flagSeed.Seed();
        }

        public static FlagDB Main()
        {
            return SingleInstance.Main().GetComponentInChildren<FlagDB>();
        }

        public void SetFlag(int id, bool value) 
        {
            FlagsContainer container = GetFlagsContainer(SceneManager.GetActiveScene().name);
            container.GetFlag(id).value = value;
        }

        public FlagsContainer GetFlagsContainer(string sceneName) 
        {
            return flagsContainers.First(c => c.sceneName == sceneName);
        }

        public void SetFlagsContainers(List<FlagsContainer> flagsContainers) 
        {
            this.flagsContainers = flagsContainers;
        }
    }
}