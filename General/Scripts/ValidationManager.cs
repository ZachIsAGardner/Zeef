using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeef {

    [AttributeUsage(AttributeTargets.Field)]
	public class RequiredAttribute : Attribute { }

    public class MissingRequiredFieldsException : Exception {

        public List<string> Errors { get; private set; }

        public MissingRequiredFieldsException(List<string> errors) {
            Errors = errors;
        }
    }

    class ValidationError {

        public string Error { get; private set; }
        public UnityEngine.Object Subject { get; private set; }

        public ValidationError(string error, UnityEngine.Object subject) {
            Error = error;
            Subject = subject;
        }
    }

    public class ValidationManager : MonoBehaviour {

        private void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            LogValidations();
        }

        // ---

        // Find all fields meant to be set in the 
        // inspector that havent been set and warn the user
        public static void LogValidations() {
            List<ValidationError> validations = GetValidationErrors();

            foreach (ValidationError validation in validations)
                Debug.Log(validation.Error, validation.Subject); 
        }

        // A scene is "invalid" when it contains MonoBehaviours with 
        // fields that haven't been properly set
        public static bool SceneIsValid() => GetValidationErrors().Count < 1;
        
        private static List<ValidationError> GetValidationErrors() {

            List<ValidationError> validations = new List<ValidationError>();
            List<MonoBehaviour> behaviours = FindObjectsOfType<MonoBehaviour>().ToList();

            foreach (MonoBehaviour behaviour in behaviours) {     
                // Get fields
                List<FieldInfo> fieldInfos = behaviour.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.GetCustomAttribute(typeof(RequiredAttribute)) != null)
                    .ToList();

                // Go through all fields and check if they have a value
                foreach (FieldInfo fieldInfo in fieldInfos) {
                    object value = fieldInfo.GetValue(behaviour);

                    if (value == null || value.Equals(null)) {
                        validations.Add(new ValidationError(
                            error: $"UnassignedReference: The '{fieldInfo.Name}' field of '{behaviour.GetType().Name}' has not been assigned.",
                            subject: behaviour.gameObject
                        ));
                    } 
                }
            }
            return validations;
        }	
    }
}