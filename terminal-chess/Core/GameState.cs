using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terminal_chess.Core.Models;

namespace terminal_chess.Core
{
    public struct CastlingRights
    {
        public bool WhiteCanCastleKingside;
        public bool WhiteCanCastleQueenside;
        public bool BlackCanCastleKingside;
        public bool BlackCanCastleQueenside;

        public CastlingRights()
        {
            WhiteCanCastleKingside = true;
            WhiteCanCastleQueenside = true;
            BlackCanCastleKingside = true;
            BlackCanCastleQueenside = true;
        }

        public CastlingRights(CastlingRights castling)
        {
            WhiteCanCastleKingside = castling.WhiteCanCastleKingside;
            WhiteCanCastleQueenside = castling.WhiteCanCastleQueenside;
            BlackCanCastleKingside = castling.BlackCanCastleKingside;
            BlackCanCastleQueenside = castling.BlackCanCastleQueenside;
        }
    }

    public enum GameStatus
    {
        Ongoing,
        Checkmate,
        Stalemate,
        DrawByRepetition,
        DrawByFiftyMoveRule
    }

    public class GameState
    {
        public Board Board;
        public PlayerColor CurrentPlayer;
        public CastlingRights CastlingRights;
        public Position? EnPassantTargetSquare;
        public int HalfmoveClock;
        public int FullmoveNumber;
        public GameStatus Status;
        public ulong ZobristHash;
        public Dictionary<ulong, int> PositionHistory;

        public GameState(PlayerColor color = PlayerColor.White)
        {
            Board.Side = color;
            Board = new Board();
            CurrentPlayer = color;
            CastlingRights = new CastlingRights();
            HalfmoveClock = 0;
            FullmoveNumber = 1;
            Status = GameStatus.Ongoing;
            PositionHistory = new Dictionary<ulong, int>();
        }

        public GameState(GameState gameState)
        {
            Board = new Board(gameState.Board);
            CurrentPlayer = gameState.CurrentPlayer;
            CastlingRights = new CastlingRights(gameState.CastlingRights);
            HalfmoveClock = gameState.HalfmoveClock;
            FullmoveNumber = gameState.FullmoveNumber;
            Status = gameState.Status;
            PositionHistory = new Dictionary<ulong, int>(gameState.PositionHistory);
        }

        public void RenderBoard(GameState state)
        {
            Console.WriteLine("+--------------------------------+");
            if (state.CurrentPlayer == PlayerColor.White)
            {
                for (int i = 0; i < 8; i++)
                {
                    Console.Write(" " + (8 - i) + " ");
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write(state.Board.BoardChess[i, j].Display + "  ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("   a   b   c   d   e   f   g   h");
            }
            else
            {
                for (int i = 7; i >= 0; i--)
                {
                    Console.Write(" " + (7 - i + 1) + " ");
                    for (int j = 7; j >= 0; j--)
                    {
                        Console.Write(state.Board.BoardChess[i, j].Display + "  ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("   h   g   f   e   d   c   b   a");
            }
            Console.WriteLine("+--------------------------------+");
        }

        public GameState MakeMove(Move move)
        {
            GameState newGameState = new GameState(this);
            ulong newHash = ZobristHash;

            // Update full move number
            if (CurrentPlayer == PlayerColor.Black)
            {
                newGameState.FullmoveNumber = FullmoveNumber + 1;
            }

            Piece movedPiece = Board.BoardChess[move.From.Row, move.From.Col];
            int movedPieceIndex = GetPieceIndex(movedPiece);
            int fromSquareIndex = move.From.Row * 8 + move.From.Col;
            int toSquareIndex = move.To.Row * 8 + move.To.Col;
            Piece targetPiece = Board.BoardChess[move.To.Row, move.To.Col];

            // Update half move clock
            newGameState.HalfmoveClock = HalfmoveClock + 1;
            if (targetPiece.Type != PieceType.None && targetPiece.Color != CurrentPlayer)
                newGameState.HalfmoveClock = 0;
            if (HalfmoveClock == 100)
            {
                newGameState.Status = GameStatus.DrawByFiftyMoveRule;
            }

            // XOR remove old enpassant square on the new game state
            if (EnPassantTargetSquare != null)
            {
                newGameState.EnPassantTargetSquare = null;
                newHash ^= Zobrist.EnPassantFileKeys[EnPassantTargetSquare.Col];
            }

            // XOR remove moving piece from old square
            newHash ^= Zobrist.PieceKeys[movedPieceIndex, fromSquareIndex];
            // XOR add moving piece to new square
            newHash ^= Zobrist.PieceKeys[movedPieceIndex, toSquareIndex];

            // XOR remove captured piece
            if (Board.BoardChess[move.To.Row, move.To.Col].Type != PieceType.None &&
                Board.BoardChess[move.To.Row, move.To.Col].Color != CurrentPlayer)
            {
                Piece capturedPiece = Board.BoardChess[move.To.Row, move.To.Col];
                int capturedPieceIndex = GetPieceIndex(capturedPiece);
                newHash ^= Zobrist.PieceKeys[capturedPieceIndex, toSquareIndex];
            }

            // Update castling rights
            if (movedPiece.Type == PieceType.King)
            {
                if (CurrentPlayer == PlayerColor.White)
                {
                    // XOR remove castle rights
                    if (CastlingRights.WhiteCanCastleKingside)
                        newHash ^= Zobrist.CastlingKeys[0];
                    if (CastlingRights.WhiteCanCastleQueenside)
                        newHash ^= Zobrist.CastlingKeys[1];

                    newGameState.CastlingRights.WhiteCanCastleKingside = false;
                    newGameState.CastlingRights.WhiteCanCastleQueenside = false;
                }
                else
                {
                    // XOR remove castle rights
                    if (CastlingRights.BlackCanCastleKingside)
                        newHash ^= Zobrist.CastlingKeys[2];
                    if (CastlingRights.BlackCanCastleQueenside)
                        newHash ^= Zobrist.CastlingKeys[3];

                    newGameState.CastlingRights.BlackCanCastleKingside = false;
                    newGameState.CastlingRights.BlackCanCastleQueenside = false;
                }
            }

            if (movedPiece.Type == PieceType.Rook)
            {
                if (movedPiece.Position.Col == 0)
                {
                    if (CurrentPlayer == PlayerColor.White)
                    {// XOR remove castle rights
                        if (CastlingRights.WhiteCanCastleQueenside)
                            newHash ^= Zobrist.CastlingKeys[1];
                        newGameState.CastlingRights.WhiteCanCastleQueenside = false;
                    }
                    else
                    {// XOR remove castle rights
                        if (CastlingRights.BlackCanCastleKingside)
                            newHash ^= Zobrist.CastlingKeys[2];
                        newGameState.CastlingRights.BlackCanCastleKingside = false;
                    }
                }
                else if (movedPiece.Position.Col == 7)
                {
                    if (CurrentPlayer == PlayerColor.White)
                    {// XOR remove castle rights
                        if (CastlingRights.WhiteCanCastleKingside)
                            newHash ^= Zobrist.CastlingKeys[0];
                        newGameState.CastlingRights.WhiteCanCastleKingside = false;
                    }
                    else
                    {// XOR remove castle rights
                        if (CastlingRights.BlackCanCastleQueenside)
                            newHash ^= Zobrist.CastlingKeys[3];
                        newGameState.CastlingRights.BlackCanCastleQueenside = false;
                    }
                }
            }

            // En Passant
            if (movedPiece.Type == PieceType.Pawn && Math.Abs(move.To.Row - move.From.Row) == 2)
            {
                Piece piece1 = move.To.Col + 1 < 8 ? Board.BoardChess[move.To.Row, move.To.Col + 1] : null;
                Piece piece2 = move.To.Col - 1 >= 0 ? Board.BoardChess[move.To.Row, move.To.Col - 1] : null;
                if ((piece1 != null && piece1.Type == PieceType.Pawn && piece1.Color != CurrentPlayer) ||
                    (piece2 != null && piece2.Type == PieceType.Pawn && piece2.Color != CurrentPlayer))
                {
                    // Create en passant move on the new game state
                    int dir = 1 - CurrentPlayer == PlayerColor.White ? -1 : 1;
                    newGameState.EnPassantTargetSquare = new Position(move.To.Row + dir, move.To.Col);
                    newHash ^= Zobrist.EnPassantFileKeys[newGameState.EnPassantTargetSquare.Col];
                }
            }

            // Move piece (include the en passant square creating from the previous game state)
            newGameState.Board.MovePiece(move, EnPassantTargetSquare);

            // Switch turn
            Board.Side = 1 - Board.Side;
            newHash ^= Zobrist.BlackToMoveKey;
            newGameState.CurrentPlayer = 1 - CurrentPlayer;

            // Update position history
            newGameState.PositionHistory = new Dictionary<ulong, int>(PositionHistory);
            newGameState.PositionHistory.Add(newHash, newGameState.FullmoveNumber);

            // Update hash
            newGameState.ZobristHash = newHash;
            return newGameState;
        }

        public int GetPieceIndex(Piece piece)
        {
            switch (piece.Type)
            {
                case PieceType.Pawn:
                    return CurrentPlayer == PlayerColor.White ? 0 : 6;
                case PieceType.Knight:
                    return CurrentPlayer == PlayerColor.White ? 1 : 7;
                case PieceType.Bishop:
                    return CurrentPlayer == PlayerColor.White ? 2 : 8;
                case PieceType.Rook:
                    return CurrentPlayer == PlayerColor.White ? 3 : 9;
                case PieceType.Queen:
                    return CurrentPlayer == PlayerColor.White ? 4 : 10;
                case PieceType.King:
                    return CurrentPlayer == PlayerColor.White ? 5 : 11;
                default:
                    return -1;
            }
        }

        public ulong CalculateZobristHash()
        {
            ulong hash = 0;

            // 1. XOR các quân cờ
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = Board.GetPiece(new Position(row, col));
                    if (piece != null && piece.Type != PieceType.None)
                    {
                        int pieceIndex = GetPieceIndex(piece);
                        int squareIndex = row * 8 + col;
                        hash ^= Zobrist.PieceKeys[pieceIndex, squareIndex];
                    }
                }
            }

            // 2. XOR lượt đi
            if (CurrentPlayer == PlayerColor.Black)
            {
                hash ^= Zobrist.BlackToMoveKey;
            }

            // 3. XOR quyền nhập thành
            if (CastlingRights.WhiteCanCastleKingside) hash ^= Zobrist.CastlingKeys[0];
            if (CastlingRights.WhiteCanCastleQueenside) hash ^= Zobrist.CastlingKeys[1];
            if (CastlingRights.BlackCanCastleKingside) hash ^= Zobrist.CastlingKeys[2];
            if (CastlingRights.BlackCanCastleQueenside) hash ^= Zobrist.CastlingKeys[3];

            // 4. XOR ô bắt tốt qua đường
            if (EnPassantTargetSquare != null)
            {
                hash ^= Zobrist.EnPassantFileKeys[EnPassantTargetSquare.Col];
            }

            return hash;
        }
    }
}
