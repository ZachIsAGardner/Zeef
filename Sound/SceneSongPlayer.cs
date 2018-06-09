using UnityEngine;
using Zeef.GameManager;

namespace Zeef.Sound {
    public class SceneSongPlayer : MonoBehaviour {
        public SongObject songObject;

        protected virtual void Start() {
            PlaySong();
        }

        public void PlaySong() {
            AudioPlayer audioPlayer = Game.Main().GetComponent<AudioPlayer>();
            audioPlayer.ChangeSong(songObject);
        }
    }
}