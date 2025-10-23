using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terminal_chess.Core.Models
{
    public enum PieceType
    {
        None,
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public enum PlayerColor
    {
        White,
        Black,
        None
    }

    public class Piece
    {
        public PieceType Type;
        public PlayerColor Color;
        public int Point;
        public string Display;
        public Position Position;

        public Piece(PieceType type1, PlayerColor color1, int point1, string display1, Position position1)
        {
            Type = type1;
            Color = color1;
            Point = point1;
            Display = display1;
            Position = new Position(position1);
        }

        public Piece(Piece piece)
        {
            Type = piece.Type;
            Color = piece.Color;
            Point = piece.Point;
            Display = piece.Display;
            Position = new Position(piece.Position);
        }

        public override string ToString()
        {
            return $"{Display}{Position}";
        }
    }
}
