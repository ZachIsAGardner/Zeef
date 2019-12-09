using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Perform
{    
    public class Branch
    {
        public List<Section> Sections { get; private set; }
        public List<Path> Paths { get; private set; }
        public TextBoxUIPartialModel TextBoxUIPartialModel { get; private set; }
        public PerformanceModel PerformanceModel { get; private set; }
     
        public Branch() { }

        public Branch (List<Section> sections, List<Path> paths = null, TextBoxUIPartialModel textBoxModel = null, PerformanceModel performanceModel = null)
        {
            Sections = sections;
            Paths = paths;
            TextBoxUIPartialModel = textBoxModel;
            PerformanceModel = performanceModel;
        }
    }
}