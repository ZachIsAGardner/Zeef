using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef;
using Zeef.TwoDimensional;

namespace Zeef.TwoDimensional 
{
    public class OneShotAnimation : AnimatedSprite
    {
        [UnityEngine.SerializeField] private float speed = 1;
        [UnityEngine.SerializeField] private bool loop = false;

        void Awake()
        {
            State = new AnimationState(
                name: "One Shot", 
                range: new IntegerRange(0, 1000),
                loop: loop,
                speed: speed
            );
        }
    }
}

