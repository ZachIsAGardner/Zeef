using System.Collections;
using System.Collections.Generic;
using Zeef;
using Zeef.TwoDimensional;

public class OneShotAnimation : AnimatedSprite<object>
{
    [UnityEngine.Header("Animation Settings")]
    [UnityEngine.SerializeField] private float speed = 1;
    [UnityEngine.SerializeField] private bool loop = false;

    protected override Zeef.TwoDimensional.AnimationState GetAnimationState() =>
        new AnimationState(
            name: "One Shot", 
            range: new IntegerRange(0, 1000),
            loop: loop,
            speed: speed
        );
}
