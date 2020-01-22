using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.TwoDimensional;

namespace Zeef.TwoDimensional.Example 
{
    public enum TestPlayerStatus
    {
        Idling,
        Running,
        Jumping,
        Falling
    }

    public class TestPlayerInput
    {
        public Vector2 Direction;
    }

    public class TestPlayer : MovingObject2D
    {
        [SerializeField] 
        private float gravity = 2;

        [SerializeField] 
        private float jumpVelocity = 5;

        [HideInInspector] 
        public TestPlayerStatus Status = TestPlayerStatus.Idling;

        private SpriteRenderer spriteRenderer;
        private TestPlayerInput input;
        private Vector2 startPosition;

        // --

        protected override void Start()
        {
            base.Start();
            startPosition = transform.position;

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();

            // Reset if fell.
            if (transform.position.y < -20)
            {
                Velocity = Vector2.zero;
                transform.position = startPosition;
            }

            UpdateAnimationState();
            UpdateInput();
            UpdateMovement();
        }

        void UpdateAnimationState() 
        {
            if (Velocity.y > 0 && !Collision.Collisions.Down)
            {
                Status = TestPlayerStatus.Jumping;
                return;
            }

            if (Velocity.y < 0 && !Collision.Collisions.Down)
            {
                Status = TestPlayerStatus.Falling;
                return;
            }

            if ((Velocity.x > 0.05f || Velocity.x <= -0.05f) && input.Direction.x != 0)
            {
                Status = TestPlayerStatus.Running;
                return;
            }
                
            Status = TestPlayerStatus.Idling;
        }

        void UpdateInput() 
        {
            bool left = ControlManager.GetInputHeld(ControlManager.Left);
            bool right = ControlManager.GetInputHeld(ControlManager.Right);

            int inputX = 0;

            if (left && right) 
                inputX = 0;
            else if (left)
                inputX = -1;
            else if (right)
                inputX = 1;
                
            input = new TestPlayerInput() { 
                Direction = new Vector2(inputX, 0)
            };
        }

        void UpdateMovement() 
        {
            if (input.Direction.x != 0)
            {
                spriteRenderer.transform.localScale = new Vector3(
                    Mathf.Abs(spriteRenderer.transform.localScale.x) * Mathf.Sign(input.Direction.x),
                    spriteRenderer.transform.localScale.y,
                    spriteRenderer.transform.localScale.z
                );
            }
            
            Velocity.x = Velocity.x.MoveOverTime(moveSpeed * input.Direction.x, acc);

            if (Collision.Collisions.Down) 
                Velocity.y = 0;

            Velocity.y -= gravity * Time.deltaTime;

            if (Collision.Collisions.Down && ControlManager.GetInputPressed(ControlManager.Accept))
                Velocity.y = jumpVelocity;
        }
    }
}
