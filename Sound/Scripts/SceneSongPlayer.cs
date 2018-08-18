using UnityEngine;

namespace Zeef.Sound {
    public class SceneSongPlayer : MonoBehaviour {
        public SongObject songObject;

        protected virtual void Start() {
            PlaySong();
        }

        public void PlaySong() {
            AudioManager.ChangeSong(songObject);
        }
    }
}