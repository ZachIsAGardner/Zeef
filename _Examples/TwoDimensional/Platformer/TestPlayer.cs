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

    public class TestPlayer : MonoBehaviour
    {
        public float MoveSpeed = 1;
        public float Acceleration = 0.01f;
        public float Gravity = 2;
        public float JumpVelocity = 5;

        [HideInInspector] 
        public TestPlayerStatus Status = TestPlayerStatus.Idling;

        private SpriteRenderer spriteRenderer;
        private MovingObject2D movingObject;
        private AnimatedSprite animatedSprite;

        private TestPlayerInput input;
        private Vector2 startPosition;

        // --

        protected void Start()
        {
            startPosition = transform.position;

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            movingObject = GetComponentInChildren<MovingObject2D>();
            animatedSprite = GetComponentInChildren<AnimatedSprite>();
        }

        protected void Update()
        {
            // Reset if fell.
            if (transform.position.y < -20)
            {
                Respawn();
            }

            UpdateStatus();
            UpdateAnimation();
            UpdateInput();
            UpdateMovement();
        }

        public void Respawn() 
        {
            movingObject.Velocity = Vector2.zero;
            transform.position = startPosition;
        }

        void UpdateStatus() 
        {
            if (movingObject.Velocity.y > 0 && !movingObject.Collision.Collisions.Down)
            {
                Status = TestPlayerStatus.Jumping;
                return;
            }

            if (movingObject.Velocity.y < 0 && !movingObject.Collision.Collisions.Down)
            {
                Status = TestPlayerStatus.Falling;
                return;
            }

            if ((movingObject.Velocity.x > 0.05f || movingObject.Velocity.x <= -0.05f) && input.Direction.x != 0)
            {
                Status = TestPlayerStatus.Running;
                return;
            }
                
            Status = TestPlayerStatus.Idling;
        }

        void UpdateAnimation() 
        {
            switch (Status)
            {
                case TestPlayerStatus.Idling:    
                    animatedSprite.State = new AnimationState(
                        name: "Idling",
                        range: new IntegerRange(0,0),
                        loop: true
                    );
                    break;
                
                case TestPlayerStatus.Running:    
                    animatedSprite.State = new AnimationState(
                        name: "Running",
                        range: new IntegerRange(1,4),
                        loop: true,
                        speed: 1.35f
                    );
                    break;

                case TestPlayerStatus.Jumping:    
                    animatedSprite.State = new AnimationState(
                        name: "Jumping",
                        range: new IntegerRange(5,5),
                        loop: true
                    );
                    break;

                case TestPlayerStatus.Falling:    
                    animatedSprite.State = new AnimationState(
                        name: "Falling",
                        range: new IntegerRange(6,6),
                        loop: true
                    );
                    break;

                default:
                    throw new System.Exception("Invalid Animation State");
            } 
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
            
            movingObject.Velocity.x = movingObject.Velocity.x.MoveOverTime(MoveSpeed * input.Direction.x, Acceleration);

            if (movingObject.Collision.Collisions.Down) 
                movingObject.Velocity.y = 0;

            movingObject.Velocity.y -= Gravity * Time.deltaTime;

            if (movingObject.Collision.Collisions.Down && ControlManager.GetInputPressed(ControlManager.Accept))
                movingObject.Velocity.y = JumpVelocity;
        }
    }
}
