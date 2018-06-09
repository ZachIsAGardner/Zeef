using UnityEngine;
using Zeef.GameManager;

namespace Zeef.Sound 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Song")]
    public class SongObject : ScriptableObject 
    {
        public SongID id;
        public AudioClip clip;
        // loop start, loop end
        public FloatRange loopTimes;
    }
}