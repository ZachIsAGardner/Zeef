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
    }
}