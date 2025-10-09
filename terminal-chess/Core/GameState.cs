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
            for (int i = 0; i < 9; i++)
            {
                if (i == 8)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write("   " + (char)(Board.Side == PlayerColor.White ? j + 97 : 104 - j));
                    }
                }
                else
                {
                    Console.Write(" " + (Board.Side == PlayerColor.White ? 8 - i : i + 1) + " ");
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write(state.Board.BoardChess[i, j] + "  ");
                    }
                }
                Console.WriteLine("");
            }
            Console.WriteLine("+--------------------------------+");
        }

        public GameState MakeMove(Move move)
        {
            GameState newGameState = new GameState(this);
            ulong newHash = ZobristHash;

            // Update full move number
            if (CurrentPlayer != Board.Side)
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

            // XOR remove old enpassant square
            if (EnPassantTargetSquare != null)
                newHash ^= Zobrist.EnPassantFileKeys[EnPassantTargetSquare.Col];

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

            // Check if promotion move
            if (move.PromotionPiece != PieceType.None)
            {
                newGameState.Board.BoardChess[move.To.Row, move.To.Col] = new Piece(move.PromotionPiece, Board.Side, Board.GetPointFromPieceType(move.PromotionPiece), Board.GetDisplayFromTypeColor(move.PromotionPiece, Board.Side), new Position(move.To.Row, move.To.Col));
            }
            else
            {
                newGameState.Board.BoardChess[move.To.Row, move.To.Col] = Board.BoardChess[move.From.Row, move.From.Col];
                newGameState.Board.BoardChess[move.To.Row, move.To.Col].Position = new Position(move.To.Row, move.To.Col);
            }
            // Turn old position into none square
            if (move.From.Row % 2 == 0)
                newGameState.Board.BoardChess[move.From.Row, move.From.Col] = new Piece(PieceType.None, PlayerColor.None, 0, move.From.Col % 2 == 0 ? "⚪" : "⚫", move.From);
            else
                newGameState.Board.BoardChess[move.From.Row, move.From.Col] = new Piece(PieceType.None, PlayerColor.None, 0, move.From.Col % 2 == 0 ? "⚫" : "⚪", move.From);

            // En Passant

            // Switch turn
            newHash ^= Zobrist.BlackToMoveKey;
            newGameState.CurrentPlayer = CurrentPlayer == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;

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
