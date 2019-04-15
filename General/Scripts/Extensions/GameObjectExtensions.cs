using System;
using UnityEngine;

namespace Zeef {

    public static class GameObjectExtensions {
        
        public static T GetComponentWithError<T> (this GameObject gameObject) where T : Component {
            T result = gameObject.GetComponent<T>();
            if (result == null) throw new MissingComponentException($"Could not find {typeof(T).FullName} component attached to {gameObject.name}");
            return result;
        }

        public static T GetComponentInChildrenWithError<T> (this GameObject gameObject) where T : Component {
            T result = gameObject.GetComponentInChildren<T>();
            if (result == null) throw new MissingComponentException($"Could not find {typeof(T).FullName} component attached to {gameObject.name} or any of it's children");
            return result;
        }

        public static T GetComponentInParentWithError<T> (this GameObject gameObject) where T : Component {
            T result = gameObject.GetComponentInParent<T>();
            if (result == null) throw new MissingComponentException($"Could not find {typeof(T).FullName} component.");
            return result;
        }
    }
}