using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

namespace Zeef.GameManagement 
{
    // 'Databases' holds simple info that changes over time and is saved between play sessions
    [RequireComponent(typeof (SingleInstanceChild))]
    public class GameDB : MonoBehaviour  {

        [SerializeField] private EntitiesEnum playerID;
        public EntitiesEnum PlayerID { get { return playerID; }}

        public static GameDB Main() => SingleInstance.Main().GetComponentInChildren<GameDB>();
        
    }
}
