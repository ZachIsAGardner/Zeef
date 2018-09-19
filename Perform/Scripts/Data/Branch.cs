using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Perform {
    
    public class Branch {

        public List<Section> Sections { get; private set; }
        public List<Path> Paths { get; private set; }
        public TextBoxUIPartialModel TextBoxUIPartialModel { get; private set; }

        public Branch (List<Section> sections) {
            Sections = sections;
        }
        public Branch (List<Section> sections, TextBoxUIPartialModel textBoxModel) {
            Sections = sections;
            TextBoxUIPartialModel = textBoxModel;
        }
        public Branch (List<Section> sections, List<Path> paths) {
            Sections = sections;
            Paths = paths;
        }
        public Branch (List<Section> sections, List<Path> paths, TextBoxUIPartialModel textBoxModel) {
            Sections = sections;
            Paths = paths;
            TextBoxUIPartialModel = textBoxModel;
        }
    }
}