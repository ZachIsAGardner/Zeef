using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Sound {

    [System.Serializable]
    public class LoopTime {
        public float Start;
        // if < 0, then it will use end of song as loop end
        public float End;
    }

    [CreateAssetMenu(menuName = "Scriptables/Song")]
    public class SongScriptable : ScriptableObject {

        public AudioClip Clip;

        [Range (0, 1)]
        public float Volume = 1;

        // loop start, loop end
        public LoopTime LoopTime;
    }
}