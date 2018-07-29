using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Perform {
    
    public class Branch {

        public List<Section> Sections { get; private set; }
        public List<Path> Paths { get; private set; }
        public TextBoxOptions Options { get; private set; }

        public Branch (List<Section> sections) {
            Sections = sections;
        }
        public Branch (List<Section> sections, TextBoxOptions options) {
            Sections = sections;
            Options = options;
        }
        public Branch (List<Section> sections, List<Path> paths) {
            Sections = sections;
            Paths = paths;
            Options = Options;
        }
        public Branch (List<Section> sections, List<Path> paths, TextBoxOptions options) {
            Sections = sections;
            Paths = paths;
            Options = options;
        }
    }
}