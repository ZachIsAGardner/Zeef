﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

namespace Zeef.GameManager {
    // 'Databases' holds simple info that changes over time and is saved between play sessions
    [Serializable]
    public class GameDB : MonoBehaviour {
        public EntityID player;

        public static GameDB Main(){
            return Game.Main().GetComponentInChildren<GameDB>();
        }
    }
}
