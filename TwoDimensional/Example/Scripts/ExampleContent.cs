using System;
using UnityEngine;

namespace Zeef.TwoDimensional.Example {

    public class ExampleContent : MonoBehaviour {

        private static ExampleContent exampleContent;
        private static ExampleContent GetExampleContent() {
            if (exampleContent == null) throw new Exception("No existing ExampleContent.");
            return exampleContent;
        }

        [SerializeField] private GameObject playerPrefab;
        public static GameObject PlayerPrefab { get { return GetExampleContent().playerPrefab; } }

        void Awake() {
            if (exampleContent != null) 
                throw new Exception("Only one ExampleContent may be present at a time.");
            exampleContent = this;
        }
    }
}