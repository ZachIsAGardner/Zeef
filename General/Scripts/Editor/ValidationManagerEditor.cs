using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Zeef {

    [CustomEditor(typeof(ValidationManager))]
    public class ValidationManagerEditor : Editor {

        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            GUILayout.TextArea(
                "Unassigned fields decorated with the [Required] attribute will result in 'invalid' MonoBehaviours"
            );

            if (GUILayout.Button("Validate MonoBehaviours")) { 
                Utility.ClearConsole();

                if (ValidationManager.SceneIsValid()) {
                    Debug.Log("Success: Scene is valid.");
                } else {
                    Debug.Log("Invalid: Unassigned references were found in the scene. See following.");
                    ValidationManager.LogValidations();
                }
            }

        }
    }
}