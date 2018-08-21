using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.GameManagement {

    // 'Content' holds Complicated info like
    // * Prefabs
    // * Images/ Sprites/ Art
    // * Sound Files
    // These should never change at run time
    public class GameContent : MonoBehaviour {

        private static GameContent gameContent;

        [SerializeField] EntityObjectsContainer entityObjects;
        [SerializeField] PromptObjectsContainer promptObjects;

        void Awake() {
            if (gameContent != null) throw new Exception("Only one GameContent can exist at a time.");
            gameContent = this;
            DontDestroyOnLoad(gameObject);
        }

        public static GameObject GetEntity(EntitiesEnum id) => 
            gameContent.entityObjects.entities.First(e => e.id == id).prefab;
        
        public static GameObject GetPrompt(PromptsEnum id) =>
            gameContent.promptObjects.prompts.First(p => p.id == id).prefab;	
    }
}