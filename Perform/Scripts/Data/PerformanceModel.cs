using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ---
using Zeef.Menu;

namespace Zeef.Perform
{
    [System.Serializable]
    public class PerformanceModel
    {
        public TextBoxUI TextBoxPrefab;
        public LinearMenuSelect ResponseBoxPrefab;

        public PerformanceModel() { }
        public PerformanceModel(TextBoxUI textBoxPrefab = null, LinearMenuSelect responseBoxPrefab = null)
        {
            TextBoxPrefab = textBoxPrefab;
            ResponseBoxPrefab = responseBoxPrefab;
        }
    }
}
