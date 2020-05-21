using UnityEngine;

namespace Zeef.GameManagement 
{
    public class SceneTransition : MonoBehaviour 
    {
        public bool DidReachHalfway { get; private set; } = false;
        private Animator animator;

        void Awake() 
        {
            animator = GetComponent<Animator>();
            DontDestroyOnLoad(gameObject);
        }

        public void Out() 
        {
            animator.SetBool("Out", true);
        }

        public void In() 
        {
            animator.SetBool("Out", false);
        }

        public void Halfway() 
        {
            DidReachHalfway = true;
        }

        public void Finished() 
        {
            Destroy(gameObject);
        }
    }
}