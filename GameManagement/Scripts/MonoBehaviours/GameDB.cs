using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

namespace Zeef.GameManagement 
{
    // 'Databases' holds simple info that changes over time and is saved between play sessions
    public class GameDB : MonoBehaviour  {

        [SerializeField] private EntitiesEnum playerID;
        public static EntitiesEnum PlayerID { get { return gameDB.playerID; }}

        private static GameDB gameDB;

        void Awake() {
            if (gameDB != null) throw new Exception("Only one GameDB may exist at a time.");
            gameDB = this;
            DontDestroyOnLoad(gameObject);
        } 
    }
}
