using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.TwoDimensional;

namespace Zeef.Test 
{
    public class TestPlayer : MovingObject2D
    {
        [SerializeField] private float gravity = 2;
        [SerializeField] private float jumpVelocity = 5;

        protected override void Update()
        {
            base.Update();

            bool left = ControlManager.GetInputHeld(ControlManager.Left);
            bool right = ControlManager.GetInputHeld(ControlManager.Right);

            int inputX = 0;

            if (left && right) 
                inputX = 0;
            else if (left)
                inputX = -1;
            else if (right)
                inputX = 1;
            
            Velocity.x = Velocity.x.MoveOverTime(moveSpeed * inputX, acc);

            if (Collision.Collisions.Down) 
                Velocity.y = 0;

            Velocity.y -= gravity * Time.deltaTime;

            if (Collision.Collisions.Down && ControlManager.GetInputPressed(ControlManager.Accept))
                Velocity.y = jumpVelocity;
        }
    }
}
