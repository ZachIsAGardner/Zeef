using System;
using UnityEngine;

namespace Zeef {

    public static class MonoBehaviourExtensions {
        
        public static T GetComponentWithError<T> (this MonoBehaviour behaviour) where T : Component {
            T result = behaviour.GetComponent<T>();
            if (result == null) throw new MissingComponentException($"Could not find {typeof(T).FullName} component attached to {behaviour.name}");
            return result;
        }

        public static T GetComponentInChildrenWithError<T> (this MonoBehaviour behaviour) where T : Component {
            T result = behaviour.GetComponentInChildren<T>();
            if (result == null) throw new MissingComponentException($"Could not find {typeof(T).FullName} component attached to {behaviour.name} or any of it's children");
            return result;
        }

        public static T GetComponentInParentWithError<T> (this MonoBehaviour behaviour) where T : Component {
            T result = behaviour.GetComponentInParent<T>();
            if (result == null) throw new MissingComponentException($"Could not find {typeof(T).FullName} component.");
            return result;
        }
    }
}