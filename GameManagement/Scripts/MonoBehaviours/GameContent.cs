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

        [SerializeField] EntityScriptablesContainer entityObjects;
        [SerializeField] PromptScriptablesContainer promptObjects;

        void Awake() {
            if (gameContent != null) throw new Exception("Only one GameContent can exist at a time.");
            gameContent = this;
            DontDestroyOnLoad(gameObject);
        }

        public static GameObject GetEntity(EntitiesEnum id) => 
            gameContent.entityObjects.Entities.First(e => e.ID == id).Prefab;
        
        public static GameObject GetPrompt(PromptsEnum id) =>
            gameContent.promptObjects.Prompts.First(p => p.ID == id).Prefab;	
    }
}