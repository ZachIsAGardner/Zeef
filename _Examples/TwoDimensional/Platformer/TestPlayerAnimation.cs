using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional.Example
{
    public class TestPlayerAnimation : AnimatedSprite<TestPlayer>
    {
        protected override AnimationState GetAnimationState()
        {
            switch (advisor.Status)
            {
                case TestPlayerStatus.Idling:    
                    return new AnimationState(
                        name: "Idling",
                        range: new IntegerRange(0,0),
                        loop: true
                    );
                
                case TestPlayerStatus.Running:    
                    return new AnimationState(
                        name: "Running",
                        range: new IntegerRange(1,4),
                        loop: true,
                        speed: 1.35f
                    );

                case TestPlayerStatus.Jumping:    
                    return new AnimationState(
                        name: "Jumping",
                        range: new IntegerRange(5,5),
                        loop: true
                    );

                case TestPlayerStatus.Falling:    
                    return new AnimationState(
                        name: "Falling",
                        range: new IntegerRange(6,6),
                        loop: true
                    );

                default:
                    throw new System.Exception("Invalid Animation State");
            }  
        }
    }
}

