using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.GameManagement {
    // this pretty much just replaces tags
    public class Identifier : MonoBehaviour {
        public IdentifiersEnum id;

        public static Identifier FindIdentifier(IdentifiersEnum id) {
            return FindObjectsOfType<Identifier>().FirstOrDefault(i => i.id == id);
        }

        public static GameObject FindIdentifierObject(IdentifiersEnum id) {
            Identifier identifier = FindObjectsOfType<Identifier>().FirstOrDefault(i => i.id == id);
            return (identifier != null) ? identifier.gameObject : null;
        }

        public static List<Identifier> FindIdentifiers(IdentifiersEnum id) {
            return FindObjectsOfType<Identifier>().Where(i => i.id == id).ToList();
        }
    }
}