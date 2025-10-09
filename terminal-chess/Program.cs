// See https://aka.ms/new-console-template for more information

using System.Text;
using terminal_chess.Core;
using terminal_chess.Core.Models;

// Encode to display chess pieces
Console.OutputEncoding = Encoding.UTF8;

// Play
bool isValid = true;
do
{
    Console.WriteLine("Choose your side (0 = White; 1 = Black; default = 0):");
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

            // Game loop
            while (game.Status == GameStatus.Ongoing)
            {
                game.RenderBoard(game);
                //if (game.CurrentPlayer == player)
                //{
                Console.WriteLine($"{playerStr} to move:");
                string moveInput = Console.ReadLine();
                Move parsedMove = game.Board.ParseMove(moveInput);
                if (parsedMove == null)
                {
                    Console.WriteLine("Invalid move");
                    continue;
                }

                game = game.MakeMove(parsedMove);
                //}
            }
        }
        else
        {
            Console.WriteLine("Invalid input");
            isValid = false;
        }
    }
    else
    {
        Console.WriteLine("Invalid input");
        isValid = false;
    }
} while (!isValid);
