// See https://aka.ms/new-console-template for more information

using System.Text;
using terminal_chess.Core;
using terminal_chess.Core.Models;

// Encode to display chess pieces
Console.OutputEncoding = Encoding.UTF8;

// Play
Console.WriteLine(@" _____________  __  ________  _____   __     _______ ______________
/_  __/ __/ _ \/  |/  /  _/ |/ / _ | / /    / ___/ // / __/ __/ __/
 / / / _// , _/ /|_/ // //    / __ |/ /__  / /__/ _  / _/_\ \_\ \  
/_/ /___/_/|_/_/  /_/___/_/|_/_/ |_/____/  \___/_//_/___/___/___/  
                                                                   ");
bool isValid = true;
do
{
    Console.WriteLine("Choose your side (0 = White; 1 = Black; default = White):");
    string? side = Console.ReadLine();
    if (string.IsNullOrEmpty(side))
        side = "0";

    if (int.TryParse(side, out int n))
    {
        if (n == 0 || n == 1)
        {
            isValid = true;
            // Init game state
            PlayerColor player = (PlayerColor)n;
            string playerStr = player == PlayerColor.White ? "White" : "Black";
            GameState game = new GameState(player);
            bool render = true;

            // Game loop
            while (game.Status == GameStatus.Ongoing)
            {
                if (render)
                    game.RenderBoard(game);
                //if (game.CurrentPlayer == player)
                //{
                Console.WriteLine($"{playerStr} to move:");
                string moveInput = Console.ReadLine();
                Move parsedMove = game.Board.ParseMove(moveInput, game.CastlingRights);
                if (parsedMove == null)
                {
                    Console.WriteLine("INVALID MOVE!");
                    render = false;
                    continue;
                }
                else render = true;

                game = game.MakeMove(parsedMove);
                //}
            }
        }
        else
        {
            Console.WriteLine("INVALID INPUT!");
            isValid = false;
        }
    }
    else
    {
        Console.WriteLine("INVALID INPUT!");
        isValid = false;
    }
} while (!isValid);
