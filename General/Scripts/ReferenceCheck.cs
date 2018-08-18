using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef {

    // Ensures that certain references are assigned in the inspector and 
    // gives better errors when they aren't
    public class ReferenceCheck {

        public Type Type { get; set; }
        public object Reference { get; set; }

        public ReferenceCheck(Type type, object reference) {
            Type = type;
            Reference = reference;
        }

        public static void EnsureNotNull(MonoBehaviour parent, ReferenceCheck check) {
            if (check.Reference == null) throw new Exception($"The '{check.Type}' reference is missing on the '{parent.GetType()}' MonoBehaviour named '{parent.name}'");
        }
        public static void EnsureNotNull(MonoBehaviour parent, List<ReferenceCheck> checks) {
            List<string> nulls = new List<string>();
            foreach (ReferenceCheck check in checks)
                if (check.Reference == null) nulls.Add(check.Type.ToString());

            if (nulls.Count > 0) throw new Exception($"The following references are missing on the '{parent.GetType()}' MonoBehaviour named '{parent.name}': '{nulls.Andify()}'");
        }
    }
}
