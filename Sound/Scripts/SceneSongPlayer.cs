using UnityEngine;

namespace Zeef.Sound {

    public class SceneSongPlayer : MonoBehaviour {

        public SongScriptable songScriptable;
        public string songScriptableName;

        protected virtual void Start() {
            if (songScriptable) AudioManager.ChangeSong(songScriptable);
            else AudioManager.ChangeSong(AudioContent.GetSong(songScriptableName));
        }
    }
}