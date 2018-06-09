// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ExampleAnimation : AnimatedSprite
// {
    
//     SomeClass advisor;

//     protected override void GetAdvisor() {
//         advisor = GetComponent<SomeClass>();
//     }

//     protected override string GetAnimationState() {
//         if (adivsor.someField == true) {
//             return "SomeState";
//         }
//         return "SomeOtherState";
//     }

//     protected override int[] ParseAnimationState(string state) {
//         switch (state)
//         {
//             case "SomeState": 
//                 return new int[]{0,0};

//             case "SomeOtherState": 
//                 return new int[]{1,7};

//             default:
//                 return new int[]{0,0};
//         }
//     }

//     protected override AnimationEvent[] GetAnimationEvents() {

//         AnimationEvent someEvent = new AnimationEvent() {
//             states = new string[] {"SomeState"},
//             frame = 0,
//             action = () => {
//                 // do something
//             }
//         };

//         return new AnimationEvent[]{
//             someEvent
//         };
//     }

// }