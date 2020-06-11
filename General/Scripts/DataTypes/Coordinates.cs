using System;
using System.Numerics;

namespace Zeef 
{
    public class Coordinates  
    {
        public int Row;
        public int Col;

        public static Coordinates Zero { get { return new Coordinates(); } }
        
        public Coordinates() 
        {
            Col = 0;
            Row = 0;
        }
        public Coordinates(int col, int row) 
        {
            Col = col;
            Row = row;
        }
        public Coordinates(Coordinates old) 
        {
            Col = old.Col;
            Row = old.Row;
        }

        public bool SameAs(Coordinates comparer) 
        {
            return comparer != null && Row == comparer.Row && Col == comparer.Col;
        }
    }
}

