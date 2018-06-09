using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.GameManager {
    // this pretty much just replaces tags
    public class Identifier : MonoBehaviour {
        public IdentifierID id;

        public static Identifier FindIdentifier(IdentifierID id) {
            return FindObjectsOfType<Identifier>().FirstOrDefault(i => i.id == id);
        }

        public static GameObject FindIdentifierObject(IdentifierID id) {
            Identifier identifier = FindObjectsOfType<Identifier>().FirstOrDefault(i => i.id == id);
            return (identifier != null) ? identifier.gameObject : null;
        }

        public static List<Identifier> FindIdentifiers(IdentifierID id) {
            return FindObjectsOfType<Identifier>().Where(i => i.id == id).ToList();
        }
    }
}