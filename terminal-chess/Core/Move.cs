using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terminal_chess.Core.Models;

namespace terminal_chess.Core
{
    public class Move
    {
        public Position From;
        public Position To;
        public PieceType PromotionPiece;

        public Move(Position From1, Position To1, PieceType PromotionPiece1 = PieceType.None)
        {
            From = From1;
            To = To1;
            PromotionPiece = PromotionPiece1;
        }
    }
}
