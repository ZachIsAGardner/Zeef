using System;
using UnityEngine;

namespace Zeef.Perform 
{    
    public class Path 
    {
        public string Text { get; set; }
        public Branch Branch { get; set; }
        public Action SideEffect { get; set; }

        public Path(string name, Branch branch, Action sideEffect = null) 
        {
            this.Text = name;
            this.Branch = branch;
            this.SideEffect = sideEffect;
        }
    }
}