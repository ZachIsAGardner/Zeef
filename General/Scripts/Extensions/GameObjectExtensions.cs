using System;
using UnityEngine;

namespace Zeef {

    public static class GameObjectExtensions {
        
        public static T GetComponentWithError<T> (this GameObject behaviour) where T : Component {
            T result = behaviour.GetComponent<T>();
            if (result == null) throw new MissingComponentException($"Could not find a {result.GetType().ToString()} component attached to {behaviour.name}");
            return result;
        }

        public static T GetComponentInChildrenWithError<T> (this GameObject behaviour) where T : Component {
            T result = behaviour.GetComponentInChildren<T>();
            if (result == null) throw new MissingComponentException($"Could not find a {result.GetType().ToString()} component attached to {behaviour.name} or any of it's children");
            return result;
        }

        public static T FindObjectOfTypeWithError<T> (this UnityEngine.Object unityObject) where T : UnityEngine.Object {
            T result = UnityEngine.Object.FindObjectOfType<T>();
            if (result == null) throw new Exception($"Could not find any objects with the '{typeof(T).ToString()}' component attached.");
            return result;
        }
    }
}