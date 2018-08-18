using UnityEngine;

namespace Zeef.Sound {
    
    [CreateAssetMenu(menuName = "SO/Song")]
    public class SongObject : ScriptableObject {

        public SongsEnum id;
        public AudioClip clip;
        // loop start, loop end
        public FloatRange loopTimes;
    }
}