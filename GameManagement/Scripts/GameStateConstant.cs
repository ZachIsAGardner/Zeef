using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeef.GameManagement
{
    public partial class GameStateConstant
    {
        public string Name { get; set; }

        private GameStateConstant(string name)
        {
            Name = name;
        }

        public static GameStateConstant Play => new GameStateConstant("Play"); 
        public static GameStateConstant Pause => new GameStateConstant("Pause"); 
        public static GameStateConstant Cutscene => new GameStateConstant("Cutscene"); 
        public static GameStateConstant Loading => new GameStateConstant("Loading"); 
    }
}
