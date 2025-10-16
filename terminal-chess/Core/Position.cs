using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terminal_chess.Core.Models
{
    public class Position
    {
        public int Row;
        public int Col;

        public Position(int row1, int col1)
        {
            Row = row1;
            Col = col1;
        }

        public Position(Position pos)
        {
            Row = pos.Row;
            Col = pos.Col;
        }

        public override string ToString()
        {
            return $"[{Row}, {Col}]";
        }

        public bool Equals(Position? p)
        {
            if (p == null) return false;
            return this.Row == p.Row && this.Col == p.Col;
        }
    }
}
