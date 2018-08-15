using System;
using System.Numerics;

namespace Zeef {

    public class Coordinates  {

        public int Row;
        public int Col;

        public static Coordinates Zero { get { return new Coordinates(); } }
        
        public Coordinates() {
            Col = 0;
            Row = 0;
        }
        public Coordinates(int col, int row) {
            this.Col = col;
            this.Row = row;
        }

        public bool SameAs(Coordinates comparer) {
            return Row == comparer.Row && Col == comparer.Col;
        }
    }
}

