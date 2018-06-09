using System.Collections.Generic;
using System.Linq;

namespace Zeef.Persistance {
    // this is not abstract for a reason
    [System.Serializable]
    public class FlagsContainer {
        public string sceneName;
        public List<Flag> flags;

        public virtual List<Flag> DefineFlags() { return null; }
        protected virtual string GetSceneName() { return null; }

        public Flag GetFlag(int id) {
            return flags.First(f => f.id == id);
        }

        public bool GetFlagValue(int id) {
            return GetFlag(id).value;
        }

        public FlagsContainer() {
            flags = DefineFlags();
            sceneName = GetSceneName();
        }
    }
}
