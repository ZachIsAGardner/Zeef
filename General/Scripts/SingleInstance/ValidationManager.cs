using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zeef 
{
    [AttributeUsage(AttributeTargets.Field)]
	public class RequiredAttribute : Attribute { }

    // ---

    class ValidationError 
    {
        public string Error { get; private set; }
        public UnityEngine.Object Subject { get; private set; }

        public ValidationError(string error, UnityEngine.Object subject) {
            Error = error;
            Subject = subject;
        }
    }

    // ---

    /// <summary>
    /// ValidationManager points out potential issues in any given Scene.
    /// <summary>
    public class ValidationManager : MonoBehaviour 
    {
        private void Awake() 
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
        {
            LogValidations();
        }

        // ---

        /// <summary>
        /// Find all fields meant to be set in the inspector that haven't yet been set and warn the user.
        /// </summary>
        public static void LogValidations() 
        {
            List<ValidationError> validations = GetValidationErrors();

            foreach (ValidationError validation in validations)
                Debug.Log(validation.Error, validation.Subject); 
        }

        /// <summary>
        /// A scene is "invalid" when it contains MonoBehaviours with fields that haven't been properly set.
        /// </summary>
        public static bool SceneIsValid() => GetValidationErrors().Count < 1;

        // ---
        
        private static List<ValidationError> GetValidationErrors() 
        {
            List<ValidationError> validations = new List<ValidationError>();
            List<MonoBehaviour> behaviours = FindObjectsOfType<MonoBehaviour>().ToList();

            foreach (MonoBehaviour behaviour in behaviours) 
            {     
                // Get fields
                List<FieldInfo> fieldInfos = behaviour.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => f.GetCustomAttribute(typeof(RequiredAttribute)) != null)
                    .ToList();

                // Go through all fields and check if they have a value
                foreach (FieldInfo fieldInfo in fieldInfos) 
                {
                    object value = fieldInfo.GetValue(behaviour);

                    List<string> names = new List<string>() { behaviour.name };
                    Transform parent = behaviour.transform.parent;
                    int i = 0;
                    while (parent != null) 
                    {
                        names.Add(parent.name);
                        parent = parent.parent;

                        i++;
                        if (i > 20)
                            break;
                    }
                    names.Reverse();

                    string name = String.Join("/", names);

                    if (value == null || value.Equals(null) || string.IsNullOrEmpty(value.ToString())) 
                    {
                        validations.Add(new ValidationError(
                            error: $"UnassignedReference: The field '{fieldInfo.Name}' of '{behaviour.GetType().Name}' on GameObject '{name}' has not been given a value.",
                            subject: behaviour.gameObject
                        ));
                    } 
                }
            }
            
            return validations;
        }	
    }
}