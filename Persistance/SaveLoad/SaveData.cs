using System;
using System.Collections.Generic;
// ---
using Zeef.GameManagement;

namespace Zeef.Persistance {
    [Serializable]
    public class SaveData {
        public SceneInfo SceneInfo { get; private set; }
        public List<FlagsContainer> FlagsContainers { get; private set; }

        public SaveData(SceneInfo sceneInfo, List<FlagsContainer> flagsContainers) {
            SceneInfo = sceneInfo;
            FlagsContainers = flagsContainers;
        }
    }
}