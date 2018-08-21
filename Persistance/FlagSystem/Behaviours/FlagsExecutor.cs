using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Text;
using System;

namespace Zeef.Persistance {
    public abstract class FlagsExecutor : MonoBehaviour {
        public string SceneName { get; private set; }

        protected abstract void SetUpScene(FlagsContainer container);
        protected abstract string GetSceneName();

        protected void Start() {
            SceneName = GetSceneName();
            if (SceneManager.GetActiveScene().name != SceneName && SceneName != "Global") ThrowException();
            Execute();
        }

        private void ThrowException() {
            StringBuilder sb = new StringBuilder();
            sb.Append($"The current scene's name '{SceneManager.GetActiveScene().name}' does not match with the SceneName property of '${SceneName}' on ${this}. ");
            sb.Append("The SceneSetup object was placed in a different scene than was expected. ");
            sb.Append("Make sure the SceneSetup object was placed in the intended scene ");
            sb.Append("and that the 'SceneName' property on the SceneSetup object matches with the current scene's name.");
            throw new Exception(sb.ToString());
        }

        private void Execute() {        
            SetUpScene(GetFlagsContainer());
        }

        private FlagsContainer GetFlagsContainer() {
            return FlagDB.GetFlagsContainer(SceneName);
        }
    }
}