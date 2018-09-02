using System;
using UnityEngine;

namespace Zeef {

    public static class GameObjectExtensions {
        
        public static T GetComponentWithError<T> (this GameObject gameObject) where T : Component {
            T result = gameObject.GetComponent<T>();
            if (result == null) throw new MissingComponentException($"Could not find a {result.GetType().ToString()} component attached to {gameObject.name}");
            return result;
        }

        public static T GetComponentInChildrenWithError<T> (this GameObject gameObject) where T : Component {
            T result = gameObject.GetComponentInChildren<T>();
            if (result == null) throw new MissingComponentException($"Could not find a {result.GetType().ToString()} component attached to {gameObject.name} or any of it's children");
            return result;
        }

        public static T GetComponentInParentWithError<T> (this GameObject gameObject) where T : Component {
            T result = gameObject.GetComponentInParent<T>();
            if (result == null) throw new MissingComponentException($"Could not find {result.GetType().ToString()} component.");
            return result;
        }
    }
}