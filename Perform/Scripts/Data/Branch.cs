using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Perform {
    
    public class Branch {

        public List<Section> Sections { get; private set; }
        public List<Path> Paths { get; private set; }
        public TextBoxUIModel Model { get; private set; }

        public Branch (List<Section> sections) {
            Sections = sections;
        }
        public Branch (List<Section> sections, TextBoxUIModel model) {
            Sections = sections;
            Model = model;
        }
        public Branch (List<Section> sections, List<Path> paths) {
            Sections = sections;
            Paths = paths;
            Model = Model;
        }
        public Branch (List<Section> sections, List<Path> paths, TextBoxUIModel model) {
            Sections = sections;
            Paths = paths;
            Model = model;
        }
    }
}