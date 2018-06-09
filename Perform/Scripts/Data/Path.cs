using System;
using UnityEngine;

namespace Zeef.Perform {
    public class Path {
        public string name;
        public Branch branch;
        public Action sideEffect;

        public Path(string name, Branch branch, Action sideEffect = null) {
            this.name = name;
            this.branch = branch;
            this.sideEffect = sideEffect;
        }
    }
}