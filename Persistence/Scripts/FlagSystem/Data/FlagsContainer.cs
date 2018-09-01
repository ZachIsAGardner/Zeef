using System.Collections.Generic;
using System.Linq;

namespace Zeef.Persistence {
    // this is not abstract for a reason
    [System.Serializable]
    public class FlagsContainer {
        
        public string SceneName;
        public List<Flag> Flags;

        public virtual List<Flag> DefineFlags() { return null; }
        protected virtual string GetSceneName() { return null; }

        public Flag GetFlag(int id) {
            return Flags.First(f => f.ID == id);
        }

        public bool GetFlagValue(int id) {
            return GetFlag(id).Value;
        }

        public FlagsContainer() {
            Flags = DefineFlags();
            SceneName = GetSceneName();
        }
    }
}
