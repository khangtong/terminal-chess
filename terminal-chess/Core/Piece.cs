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
        King,
    }

    public enum PlayerColor
    {
        White,
        Black,
        None,
    }

    public class Piece
    {
        public PieceType Type;
        public PlayerColor Color;
        public int Point;
        public string Display;
        public Position Position;

        public Piece(
            PieceType type1,
            PlayerColor color1,
            int point1,
            Position position1,
            string display1 = ""
        )
        {
            Type = type1;
            Color = color1;
            Point = point1;
            Display = string.IsNullOrEmpty(display1)
                ? GetDisplayFromTypeColor(type1, color1)
                : display1;
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

        public string GetDisplayFromTypeColor(PieceType type, PlayerColor color)
        {
            string display = "";
            string darkGray = "\u001b[90m";
            string reset = "\u001b[0m";

            switch (type)
            {
                case PieceType.Pawn:
                    display += "♙";
                    break;
                case PieceType.Knight:
                    display += "♞";
                    break;
                case PieceType.Bishop:
                    display += "♝";
                    break;
                case PieceType.Rook:
                    display += "♜";
                    break;
                case PieceType.Queen:
                    display += "♛";
                    break;
                case PieceType.King:
                    display += "♚";
                    break;
                default:
                    break;
            }

            return color == PlayerColor.White ? display : $"{darkGray}{display}{reset}";
        }

        public override string ToString()
        {
            return $"{Display}{Position}";
        }
    }
}
