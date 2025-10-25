using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terminal_chess.Core.Models;

namespace terminal_chess.Core
{
    public class Board
    {
        public static PlayerColor Side;
        public Piece[,] BoardChess = new Piece[8, 8];

        public Board()
        {
            InitializeBoard();
        }

        public Board(Board board)
        {
            BoardChess = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                BoardChess[i, j] = new Piece(board.BoardChess[i, j]);
        }

        public void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i == 0 || i == 1 || i == 6 || i == 7)
                    {
                        if (i == 0)
                        {
                            if (j == 0 || j == 7)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.Rook,
                                    1 - Side,
                                    5,
                                    Side == PlayerColor.White ? "bR" : "wR",
                                    new Position(i, j)
                                );
                            }

                            if (j == 1 || j == 6)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.Knight,
                                    1 - Side,
                                    3,
                                    Side == PlayerColor.White ? "bN" : "wN",
                                    new Position(i, j)
                                );
                            }

                            if (j == 2 || j == 5)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.Bishop,
                                    1 - Side,
                                    3,
                                    Side == PlayerColor.White ? "bB" : "wB",
                                    new Position(i, j)
                                );
                            }

                            if (j == 3)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.Queen,
                                    1 - Side,
                                    9,
                                    Side == PlayerColor.White ? "bQ" : "wK",
                                    new Position(i, j)
                                );
                            }

                            if (j == 4)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.King,
                                    1 - Side,
                                    0,
                                    Side == PlayerColor.White ? "bK" : "wQ",
                                    new Position(i, j)
                                );
                            }
                        }

                        if (i == 1)
                        {
                            BoardChess[i, j] = new Piece(
                                PieceType.Pawn,
                                1 - Side,
                                1,
                                Side == PlayerColor.White ? "bP" : "wP",
                                new Position(i, j)
                            );
                        }

                        if (i == 6)
                        {
                            BoardChess[i, j] = new Piece(
                                PieceType.Pawn,
                                Side,
                                1,
                                Side == PlayerColor.White ? "wP" : "bP",
                                new Position(i, j)
                            );
                        }

                        if (i == 7)
                        {
                            if (j == 0 || j == 7)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.Rook,
                                    Side,
                                    5,
                                    Side == PlayerColor.White ? "wR" : "bR",
                                    new Position(i, j)
                                );
                            }

                            if (j == 1 || j == 6)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.Knight,
                                    Side,
                                    3,
                                    Side == PlayerColor.White ? "wN" : "bN",
                                    new Position(i, j)
                                );
                            }

                            if (j == 2 || j == 5)
                            {
                                BoardChess[i, j] = new Piece(
                                    PieceType.Bishop,
                                    Side,
                                    3,
                                    Side == PlayerColor.White ? "wB" : "bB",
                                    new Position(i, j)
                                );
                            }

                            if (j == 3 || j == 4)
                                if (Side == PlayerColor.White)
                                {
                                    BoardChess[i, 3] = new Piece(
                                        PieceType.Queen,
                                        Side,
                                        9,
                                        "wQ",
                                        new Position(i, 3)
                                    );
                                    BoardChess[i, 4] = new Piece(
                                        PieceType.King,
                                        Side,
                                        0,
                                        "wK",
                                        new Position(i, 4)
                                    );
                                }
                                else
                                {
                                    BoardChess[i, 3] = new Piece(
                                        PieceType.King,
                                        Side,
                                        0,
                                        "bK",
                                        new Position(i, 3)
                                    );
                                    BoardChess[i, 4] = new Piece(
                                        PieceType.Queen,
                                        Side,
                                        9,
                                        "bQ",
                                        new Position(i, 4)
                                    );
                                }
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                            BoardChess[i, j] = new Piece(
                                PieceType.None,
                                PlayerColor.None,
                                0,
                                j % 2 == 0 ? "⚪" : "⚫",
                                new Position(i, j)
                            );
                        else
                            BoardChess[i, j] = new Piece(
                                PieceType.None,
                                PlayerColor.None,
                                0,
                                j % 2 == 0 ? "⚫" : "⚪",
                                new Position(i, j)
                            );
                    }
                }
            }
        }

        public bool IsInBounds(Position p)
        {
            return p.Row >= 0 && p.Row < 8 && p.Col >= 0 && p.Col < 8;
        }

        public Piece GetPiece(Position p)
        {
            if (!IsInBounds(p))
                return null;
            return BoardChess[p.Row, p.Col];
        }

        public List<Piece> FindPiece(PieceType type, PlayerColor color)
        {
            List<Piece> pieces = new List<Piece>();
            for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if (BoardChess[i, j].Type == type && BoardChess[i, j].Color == color)
                    pieces.Add(BoardChess[i, j]);
            }
            return pieces;
        }

        public string ConvertPositionToNotation(Position pos)
        {
            if (!IsInBounds(pos))
                return null;
            return $"{(char)(pos.Col + 97)}{8 - pos.Row}";
        }

        private (Position? pos, int index) FindAndParseDestinationSquare(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                char file = input[i];
                char rank = input[i + 1];

                if (file >= 'a' && file <= 'h' && rank >= '1' && rank <= '8')
                {
                    int col = Side == PlayerColor.White ? file - 'a' : 'h' - file;
                    int row = Side == PlayerColor.White ? 8 - (rank - '0') : rank - '1';
                    return (new Position(row, col), i);
                }
            }

            return (null, -1);
        }

        public PieceType GetPieceTypeFromChar(char pieceChar)
        {
            switch (char.ToUpper(pieceChar))
            {
                case 'K':
                    return PieceType.King;
                case 'Q':
                    return PieceType.Queen;
                case 'R':
                    return PieceType.Rook;
                case 'B':
                    return PieceType.Bishop;
                case 'N':
                    return PieceType.Knight;
                default:
                    return PieceType.None;
            }
        }

        public int GetPointFromPieceType(PieceType type)
        {
            switch (type)
            {
                case PieceType.Pawn:
                    return 1;
                case PieceType.Knight:
                    return 3;
                case PieceType.Bishop:
                    return 3;
                case PieceType.Rook:
                    return 5;
                case PieceType.Queen:
                    return 9;
                default:
                    return 0;
            }
        }

        public string GetDisplayFromTypeColor(PieceType type, PlayerColor color)
        {
            string display = "";

            switch (color)
            {
                case PlayerColor.White:
                    display += "w";
                    break;
                case PlayerColor.Black:
                    display += "b";
                    break;
                default:
                    break;
            }

            switch (type)
            {
                case PieceType.Pawn:
                    display += "P";
                    break;
                case PieceType.Knight:
                    display += "N";
                    break;
                case PieceType.Bishop:
                    display += "B";
                    break;
                case PieceType.Rook:
                    display += "R";
                    break;
                case PieceType.Queen:
                    display += "Q";
                    break;
                case PieceType.King:
                    display += "K";
                    break;
                default:
                    break;
            }

            return display;
        }

        public bool IsSquareEmpty(Position pos)
        {
            return BoardChess[pos.Row, pos.Col].Type == 0;
        }

        public bool IsEnemyPiece(Piece p1, Piece p2)
        {
            return p1.Color != p2.Color;
        }

        public bool IsSquareAttacked(Position position)
        {
            List<Move> moves = GetLegalMoves(1 - Side, null);
            foreach (Move move in moves)
            {
                if (move.To.Equals(position) && GetPiece(move.From).Color != Side)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Move> GetRookMoves(Position pos)
        {
            List<Move> moves = new List<Move>();
            Piece movingPiece = GetPiece(pos);
            int[,] vectors =
            {
                { 0, 1 },
                { 0, -1 },
                { 1, 0 },
                { -1, 0 },
            };

            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                Position curPos = pos;
                while (true)
                {
                    curPos = new Position(curPos.Row + vectors[i, 0], curPos.Col + vectors[i, 1]);
                    if (!IsInBounds(curPos))
                        break;

                    Piece targetPiece = GetPiece(curPos);
                    if (targetPiece.Color == movingPiece.Color)
                        break;

                    moves.Add(new Move(pos, curPos));
                    // Break if reach a enemy piece
                    if (
                        targetPiece.Color != movingPiece.Color
                        && targetPiece.Type != PieceType.None
                    )
                        break;
                }
            }

            return moves;
        }

        public List<Move> GetBishopMoves(Position pos)
        {
            List<Move> moves = new List<Move>();
            Piece movingPiece = GetPiece(pos);
            int[,] vectors =
            {
                { 1, 1 },
                { 1, -1 },
                { -1, 1 },
                { -1, -1 },
            };

            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                Position curPos = pos;
                while (true)
                {
                    curPos = new Position(curPos.Row + vectors[i, 0], curPos.Col + vectors[i, 1]);
                    if (!IsInBounds(curPos))
                        break;

                    Piece targetPiece = GetPiece(curPos);
                    if (targetPiece.Color == movingPiece.Color)
                        break;

                    moves.Add(new Move(pos, curPos));
                    // Break if reach a enemy piece
                    if (
                        targetPiece.Color != movingPiece.Color
                        && targetPiece.Type != PieceType.None
                    )
                        break;
                }
            }

            return moves;
        }

        public List<Move> GetQueenMoves(Position pos)
        {
            List<Move> moves = new List<Move>();
            Piece movingPiece = GetPiece(pos);
            int[,] vectors =
            {
                { 0, 1 },
                { 0, -1 },
                { 1, 0 },
                { -1, 0 },
                { 1, 1 },
                { 1, -1 },
                { -1, 1 },
                { -1, -1 },
            };

            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                Position curPos = pos;
                while (true)
                {
                    curPos = new Position(curPos.Row + vectors[i, 0], curPos.Col + vectors[i, 1]);
                    if (!IsInBounds(curPos))
                        break;

                    Piece targetPiece = GetPiece(curPos);
                    if (targetPiece.Color == movingPiece.Color)
                        break;

                    moves.Add(new Move(pos, curPos));
                    // Break if reach a enemy piece
                    if (
                        targetPiece.Color != movingPiece.Color
                        && targetPiece.Type != PieceType.None
                    )
                        break;
                }
            }

            return moves;
        }

        public List<Move> GetKnightMoves(Position pos)
        {
            List<Move> moves = new List<Move>();
            Piece movingPiece = GetPiece(pos);
            int[,] vectors =
            {
                { 2, 1 },
                { 2, -1 },
                { -2, 1 },
                { -2, -1 },
                { 1, 2 },
                { 1, -2 },
                { -1, 2 },
                { -1, -2 },
            };

            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                Position curPos = new Position(pos.Row + vectors[i, 0], pos.Col + vectors[i, 1]);
                if (!IsInBounds(curPos))
                    continue;

                Piece targetPiece = GetPiece(curPos);
                if (targetPiece.Color == movingPiece.Color)
                    continue;

                moves.Add(new Move(pos, curPos));
            }

            return moves;
        }

        public List<Move> GetPawnMoves(Position pos, Position? enPassantSq)
        {
            List<Move> moves = new List<Move>();
            Piece movingPiece = GetPiece(pos);
            int dir = movingPiece.Color == PlayerColor.White ? -1 : 1;
            // 1-step moves
            if (
                IsInBounds(new Position(pos.Row + dir, pos.Col))
                && IsSquareEmpty(new Position(pos.Row + dir, pos.Col))
            )
                // Promotion
                if (pos.Row + dir == 0)
                {
                    moves.Add(new Move(pos, new Position(pos.Row + dir, pos.Col), PieceType.Queen));
                    moves.Add(new Move(pos, new Position(pos.Row + dir, pos.Col), PieceType.Rook));
                    moves.Add(
                        new Move(pos, new Position(pos.Row + dir, pos.Col), PieceType.Bishop)
                    );
                    moves.Add(
                        new Move(pos, new Position(pos.Row + dir, pos.Col), PieceType.Knight)
                    );
                }
                else
                    moves.Add(new Move(pos, new Position(pos.Row + dir, pos.Col)));

            // 2-step moves
            if (
                (pos.Row == 6 && Side == PlayerColor.White)
                || (pos.Row == 1 && Side == PlayerColor.Black)
            )
                if (
                    IsSquareEmpty(new Position(pos.Row + dir, pos.Col))
                    && IsSquareEmpty(new Position(pos.Row + 2 * dir, pos.Col))
                )
                {
                    moves.Add(new Move(pos, new Position(pos.Row + 2 * dir, pos.Col)));
                }

            // take pieces
            Position diag1 = new Position(pos.Row + dir, pos.Col - 1);
            Position diag2 = new Position(pos.Row + dir, pos.Col + 1);
            if (IsInBounds(diag1))
            {
                Piece capturedPiece = GetPiece(diag1);
                if (
                    capturedPiece.Type != PieceType.None
                    && IsEnemyPiece(movingPiece, capturedPiece)
                )
                    // Promotion
                    if (diag1.Row == 0)
                    {
                        moves.Add(new Move(pos, diag1, PieceType.Queen));
                        moves.Add(new Move(pos, diag1, PieceType.Rook));
                        moves.Add(new Move(pos, diag1, PieceType.Bishop));
                        moves.Add(new Move(pos, diag1, PieceType.Knight));
                    }
                    else
                        moves.Add(new Move(pos, diag1));
            }
            if (IsInBounds(diag2))
            {
                Piece capturedPiece = GetPiece(diag2);
                if (
                    capturedPiece.Type != PieceType.None
                    && IsEnemyPiece(movingPiece, capturedPiece)
                )
                    // Promotion
                    if (diag2.Row == 0)
                    {
                        moves.Add(new Move(pos, diag2, PieceType.Queen));
                        moves.Add(new Move(pos, diag2, PieceType.Rook));
                        moves.Add(new Move(pos, diag2, PieceType.Bishop));
                        moves.Add(new Move(pos, diag2, PieceType.Knight));
                    }
                    else
                        moves.Add(new Move(pos, diag2));
            }

            // En passant
            if (enPassantSq != null)
            {
                if (
                    pos.Row + dir == enPassantSq.Row
                    && (pos.Col - 1 == enPassantSq.Col || pos.Col + 1 == enPassantSq.Col)
                )
                    moves.Add(new Move(pos, enPassantSq));
            }

            return moves;
        }

        public List<Move> GetKingMoves(Position pos)
        {
            List<Move> moves = new List<Move>();
            Piece movingPiece = GetPiece(pos);
            int[,] vectors =
            {
                { 0, 1 },
                { 0, -1 },
                { 1, 0 },
                { -1, 0 },
                { 1, 1 },
                { 1, -1 },
                { -1, 1 },
                { -1, -1 },
            };

            for (int i = 0; i < vectors.GetLength(0); i++)
            {
                Position curPos = new Position(pos.Row + vectors[i, 0], pos.Col + vectors[i, 1]);
                if (!IsInBounds(curPos))
                    continue;

                Piece targetPiece = GetPiece(curPos);
                if (targetPiece.Color == movingPiece.Color)
                    continue;

                moves.Add(new Move(pos, curPos));
            }

            // Castle king side
            Position orgKingPos = new Position(Side == PlayerColor.White ? 7 : 0, 4);
            Position orgRookPos = new Position(Side == PlayerColor.White ? 7 : 0, 7);
            Piece piece1 = GetPiece(orgKingPos);
            Piece piece2 = GetPiece(orgRookPos);
            // Check original position
            if (piece1.Type == PieceType.King && piece2.Type == PieceType.Rook)
            {
                // Check obstacles
                int dir = 1;
                Piece obstacle1 = GetPiece(new Position(orgKingPos.Row, orgKingPos.Col + 1 * dir));
                Piece obstacle2 = GetPiece(new Position(orgKingPos.Row, orgKingPos.Col + 2 * dir));

                if (obstacle1.Type == PieceType.None && obstacle2.Type == PieceType.None)
                {
                    moves.Add(
                        new Move(orgKingPos, new Position(Side == PlayerColor.White ? 7 : 0, 6))
                    );
                    moves.Add(
                        new Move(orgRookPos, new Position(Side == PlayerColor.White ? 7 : 0, 5))
                    );
                }
            }

            // Castle queen side
            Position orgRookPosQ = new Position(Side == PlayerColor.White ? 7 : 0, 0);
            Piece piece3 = GetPiece(orgRookPos);
            // Check original position
            if (piece1.Type == PieceType.King && piece3.Type == PieceType.Rook)
            {
                // Check obstacles
                int dir = -1;
                Piece obstacle1 = GetPiece(new Position(orgKingPos.Row, orgKingPos.Col + 1 * dir));
                Piece obstacle2 = GetPiece(new Position(orgKingPos.Row, orgKingPos.Col + 2 * dir));
                Piece obstacle3 = GetPiece(new Position(orgKingPos.Row, orgKingPos.Col + 3 * dir));

                if (
                    obstacle1.Type == PieceType.None
                    && obstacle2.Type == PieceType.None
                    && obstacle3.Type == PieceType.None
                )
                {
                    moves.Add(
                        new Move(orgKingPos, new Position(Side == PlayerColor.White ? 7 : 0, 2))
                    );
                    moves.Add(
                        new Move(orgRookPos, new Position(Side == PlayerColor.White ? 7 : 0, 3))
                    );
                }
            }

            return moves;
        }

        public List<Move> GetLegalMoves(PlayerColor color, Position? enPassantSq)
        {
            List<Move> moves = new List<Move>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (BoardChess[i, j].Type != PieceType.None && BoardChess[i, j].Color == color)
                    {
                        switch (BoardChess[i, j].Type)
                        {
                            case PieceType.King:
                                moves.AddRange(GetKingMoves(BoardChess[i, j].Position));
                                break;
                            case PieceType.Queen:
                                moves.AddRange(GetQueenMoves(BoardChess[i, j].Position));
                                break;
                            case PieceType.Knight:
                                moves.AddRange(GetKnightMoves(BoardChess[i, j].Position));
                                break;
                            case PieceType.Bishop:
                                moves.AddRange(GetBishopMoves(BoardChess[i, j].Position));
                                break;
                            case PieceType.Rook:
                                moves.AddRange(GetRookMoves(BoardChess[i, j].Position));
                                break;
                            default:
                                moves.AddRange(
                                    GetPawnMoves(BoardChess[i, j].Position, enPassantSq)
                                );
                                break;
                        }
                    }
                }
            }
            return moves;
        }

        public void MovePiece(Move move, Position? enPassantSq)
        {
            // Check if promotion move
            if (move.PromotionPiece != PieceType.None)
            {
                this.BoardChess[move.To.Row, move.To.Col] = new Piece(
                    move.PromotionPiece,
                    Side,
                    this.GetPointFromPieceType(move.PromotionPiece),
                    this.GetDisplayFromTypeColor(move.PromotionPiece, Side),
                    new Position(move.To.Row, move.To.Col)
                );
            }
            else
            {
                this.BoardChess[move.To.Row, move.To.Col] = this.BoardChess[
                    move.From.Row,
                    move.From.Col
                ];
                this.BoardChess[move.To.Row, move.To.Col].Position = new Position(
                    move.To.Row,
                    move.To.Col
                );
            }
            // Turn old position into none square
            int dir = Side == PlayerColor.White ? 1 : -1;
            if (move.From.Row % 2 == 0)
            {
                this.BoardChess[move.From.Row, move.From.Col] = new Piece(
                    PieceType.None,
                    PlayerColor.None,
                    0,
                    move.From.Col % 2 == 0 ? "⚪" : "⚫",
                    move.From
                );
                // En Passant
                if (
                    this.BoardChess[move.To.Row, move.To.Col].Type == PieceType.Pawn
                    && move.To.Equals(enPassantSq)
                )
                    this.BoardChess[enPassantSq.Row + dir, enPassantSq.Col] = new Piece(
                        PieceType.None,
                        PlayerColor.None,
                        0,
                        move.From.Col % 2 == 0 ? "⚫" : "⚪",
                        move.From
                    );
            }
            else
            {
                this.BoardChess[move.From.Row, move.From.Col] = new Piece(
                    PieceType.None,
                    PlayerColor.None,
                    0,
                    move.From.Col % 2 == 0 ? "⚫" : "⚪",
                    move.From
                );
                // En Passant
                if (
                    this.BoardChess[move.To.Row, move.To.Col].Type == PieceType.Pawn
                    && move.To.Equals(enPassantSq)
                )
                    this.BoardChess[enPassantSq.Row + dir, enPassantSq.Col] = new Piece(
                        PieceType.None,
                        PlayerColor.None,
                        0,
                        move.From.Col % 2 == 0 ? "⚪" : "⚫",
                        move.From
                    );
            }
        }

        public Move ParseMove(string moveInput, CastlingRights castling, Position? enPassantSq)
        {
            // Normalize user's input
            string originalInput = moveInput;
            moveInput = moveInput
                .Trim()
                .Replace("+", "")
                .Replace("#", "")
                .Replace("!", "")
                .Replace("?", "");

            // Get legal moves
            List<Move> legalMoves = GetLegalMoves(Side, enPassantSq);
            List<Move> enemyLegalMoves = GetLegalMoves(1 - Side, enPassantSq);

            // Castle
            if (moveInput == "O-O" || moveInput == "0-0")
            {
                if (
                    (Side == PlayerColor.White && castling.WhiteCanCastleKingside)
                    || (Side == PlayerColor.Black && castling.BlackCanCastleKingside)
                )
                {
                    Position kingSq = new Position(Side == PlayerColor.White ? 7 : 0, 4);
                    Position square1 = new Position(Side == PlayerColor.White ? 7 : 0, 4 + 1);
                    Position square2 = new Position(Side == PlayerColor.White ? 7 : 0, 4 + 2);
                    Position rookSq = new Position(Side == PlayerColor.White ? 7 : 0, 4 + 3);
                    // Check if any enemy pieces can block or check on the king's castle road
                    if (
                        IsSquareAttacked(kingSq)
                        || IsSquareAttacked(square1)
                        || IsSquareAttacked(square2)
                        || IsSquareAttacked(rookSq)
                    )
                        return null;

                    Move kingMove = legalMoves.FirstOrDefault(move =>
                        GetPiece(move.From).Type == PieceType.King
                        && move.To.Col - move.From.Col == 2
                    );
                    Move rookMove = legalMoves.FirstOrDefault(move =>
                        GetPiece(move.From).Type == PieceType.Rook
                        && move.To.Col - move.From.Col == -2
                    );
                    // Move the rook
                    if (kingMove != null && rookMove != null)
                        this.MovePiece(rookMove, null);
                    return kingMove;
                }
            }

            if (moveInput == "O-O-O" || moveInput == "0-0-0")
            {
                if (
                    (Side == PlayerColor.White && castling.WhiteCanCastleQueenside)
                    || (Side == PlayerColor.Black && castling.BlackCanCastleQueenside)
                )
                {
                    Position kingSq = new Position(Side == PlayerColor.White ? 7 : 0, 4);
                    Position square1 = new Position(Side == PlayerColor.White ? 7 : 0, 4 - 1);
                    Position square2 = new Position(Side == PlayerColor.White ? 7 : 0, 4 - 2);
                    Position square3 = new Position(Side == PlayerColor.White ? 7 : 0, 4 - 3);
                    Position rookSq = new Position(Side == PlayerColor.White ? 7 : 0, 4 - 4);
                    // Check if any enemy pieces can block or check on the king's castle road
                    if (
                        IsSquareAttacked(kingSq)
                        || IsSquareAttacked(square1)
                        || IsSquareAttacked(square2)
                        || IsSquareAttacked(square3)
                        || IsSquareAttacked(rookSq)
                    )
                        return null;

                    Move kingMove = legalMoves.FirstOrDefault(move =>
                        GetPiece(move.From).Type == PieceType.King
                        && move.To.Col - move.From.Col == -2
                    );
                    Move rookMove = legalMoves.FirstOrDefault(move =>
                        GetPiece(move.From).Type == PieceType.Rook
                        && move.To.Col - move.From.Col == 3
                    );
                    // Move the rook
                    if (kingMove != null && rookMove != null)
                        this.MovePiece(rookMove, null);
                    return kingMove;
                }
            }

            // Others
            List<Move> candidateMoves = new List<Move>(legalMoves);
            //foreach (Move move in candidateMoves)
            //        Console.WriteLine(move);

            // 1. Destination
            var destResult = FindAndParseDestinationSquare(moveInput);
            if (destResult.pos == null)
                return null;
            Position destination = destResult.pos;
            // Change destination on black side (2-players mode)
            if (Side == PlayerColor.Black)
            {
                destination.Row = 7 - destination.Row;
                destination.Col = 7 - destination.Col;
            }
            int destIndex = destResult.index;

            // Filter moves reaching the destination
            candidateMoves = candidateMoves
                .Where(m => m.To.Row == destination.Row && m.To.Col == destination.Col)
                .ToList();
            //foreach (Move move in candidateMoves)
            //    Console.WriteLine(move);
            if (candidateMoves.Count == 0)
                return null;

            string suffix = moveInput.Substring(destIndex + 2);
            string prefix = moveInput.Substring(0, destIndex);

            // 2. Promotion
            if (suffix.Contains("="))
            {
                char promotionChar = suffix[suffix.IndexOf('=') + 1];
                PieceType promotionType = GetPieceTypeFromChar(promotionChar);
                candidateMoves = candidateMoves
                    .Where(m => m.PromotionPiece == promotionType)
                    .ToList();
            }

            // 3. PieceType
            PieceType pieceType;
            if (string.IsNullOrEmpty(prefix) || char.IsLower(prefix[0]))
            {
                pieceType = PieceType.Pawn;
            }
            else
            {
                pieceType = GetPieceTypeFromChar(prefix[0]);
            }

            // Filter moves base on type
            candidateMoves = candidateMoves.Where(m => GetPiece(m.From).Type == pieceType).ToList();

            // Disambiguation
            if (candidateMoves.Count > 1)
            {
                string disambiguatorStr = prefix.Replace("x", "");
                if (pieceType == PieceType.Pawn)
                {
                    if (disambiguatorStr.Length > 0)
                    {
                        char disambiguator = disambiguatorStr[0];
                        int fromCol =
                            Side == PlayerColor.White
                                ? disambiguator - 'a'
                                : 7 - ('h' - disambiguator);
                        candidateMoves = candidateMoves.Where(m => m.From.Col == fromCol).ToList();
                    }
                }
                else
                {
                    if (disambiguatorStr.Length > 1)
                    {
                        char disambiguator = disambiguatorStr[1];
                        if (disambiguator >= 'a' && disambiguator <= 'h')
                        {
                            int fromCol =
                                Side == PlayerColor.White
                                    ? disambiguator - 'a'
                                    : 7 - ('h' - disambiguator);
                            candidateMoves = candidateMoves
                                .Where(m => m.From.Col == fromCol)
                                .ToList();
                        }
                        else if (disambiguator >= '1' && disambiguator <= '8')
                        {
                            int fromRow = 8 - (disambiguator - '0');
                            candidateMoves = candidateMoves
                                .Where(m => m.From.Row == fromRow)
                                .ToList();
                        }
                    }
                }
            }

            return candidateMoves.Count == 1 ? candidateMoves[0] : null;
        }
    }
}
