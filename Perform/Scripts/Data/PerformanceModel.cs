using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ---
using Zeef.Menu;

namespace Zeef.Perform
{
    public class PerformanceModel
    {
        public TextBoxUI TextBoxPrefab { get; private set; }
        public LinearMenuSelect ResponseBoxPrefab { get; private set; }

        public PerformanceModel() { }
        public PerformanceModel(TextBoxUI textBoxPrefab = null, LinearMenuSelect responseBoxPrefab = null)
        {
            TextBoxPrefab = textBoxPrefab;
            ResponseBoxPrefab = responseBoxPrefab;
        }
    }
}
