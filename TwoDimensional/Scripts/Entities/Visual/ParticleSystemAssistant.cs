using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.TwoDimensional
{
    public class ParticleSystemAssistant : MonoBehaviour
    {
        private ParticleSystem particleSystem;

        void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (GameState.IsPlaying) 
            {
                if (particleSystem.isPaused) particleSystem.Play();
            }
            else 
            {
                particleSystem.Pause();
            }
        }
    }
}

