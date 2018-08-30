using UnityEngine;

namespace Zeef.Sound {

    public class SceneSongPlayer : MonoBehaviour {

        public SongScriptable songObject;

        protected virtual void Start() {
            AudioManager.ChangeSong(songObject);
        }
    }
}