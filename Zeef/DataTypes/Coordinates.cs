using System;

namespace Zeef {
    public class Coordinates 
    {
        public int row;
        public int col;
        
        public Coordinates() {
            row = 0;
            col = 0;
        }
        public Coordinates(int row, int col) {
            this.row = row;
            this.col = col;
        }

        public bool SameAs(Coordinates comparer) {
            return row == comparer.row && col == comparer.col;
        }
    }
}

