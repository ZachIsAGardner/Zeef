using System.Collections;
using UnityEngine;

namespace Zeef {
    // The CoroutineWithData class was adapted from the following question
    // https://answers.unity.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
    public class CoroutineWithData {
        public Coroutine Coroutine { get; private set; }
        public object Result { get; set; }

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
            this.Coroutine = owner.StartCoroutine(Run(target));
        }
    
        private IEnumerator Run(IEnumerator target) {
            while(target.MoveNext()) {
                Result = target.Current;
                yield return Result;
            }
        }
    }
}