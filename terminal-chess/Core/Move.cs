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

        public Move(Position from1, Position to1, PieceType promotionPiece1 = PieceType.None)
        {
            From = from1;
            To = to1;
            PromotionPiece = promotionPiece1;
        }

        public override string ToString()
        {
            return $"{From}-{To}";
        }
    }
}
