using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Perform {
    
    public class Branch {

        public List<Section> Sections { get; private set; }
        public List<Path> Paths { get; private set; }
        public TextBoxUIModel TextBoxUIModel { get; private set; }

        public Branch (List<Section> sections) {
            Sections = sections;
        }
        public Branch (List<Section> sections, TextBoxUIModel textBoxUIModel) {
            Sections = sections;
            TextBoxUIModel = textBoxUIModel;
        }
        public Branch (List<Section> sections, List<Path> paths) {
            Sections = sections;
            Paths = paths;
            TextBoxUIModel = TextBoxUIModel;
        }
        public Branch (List<Section> sections, List<Path> paths, TextBoxUIModel textBoxUIModel) {
            Sections = sections;
            Paths = paths;
            TextBoxUIModel = textBoxUIModel;
        }
    }
}