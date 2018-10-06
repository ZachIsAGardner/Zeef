using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.TwoDimensional.Example {

    public class PlayerAnimation : AnimatedSprite<PlayerTopDown> {

        AnimationState moving = new AnimationState(
            name: "Moving",
            range: new IntegerRange(2, 5),
            animationEvents: new List<AnimationEvent>() {
                new AnimationEvent(3, () => {
                    Debug.Log("TODO: Create dust"); // :>
                })
            },
            speed: 2
        );

        AnimationState idle = new AnimationState(
            name: "Idle",
            range: new IntegerRange(0, 1)
        );

        protected override AnimationState GetAnimationState() {
            // Return state based on context
            if (ControlManager.GetInputHeld(ControlManager.Left) || 
                ControlManager.GetInputHeld(ControlManager.Right) || 
                ControlManager.GetInputHeld(ControlManager.Down) || 
                ControlManager.GetInputHeld(ControlManager.Up)) {

                return moving;
            } else {
                return idle;
            }
        }
    }
}