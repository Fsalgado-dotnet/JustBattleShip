using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JustBattleShip
{
    public class Game
    {
        private Player human;
        private ComputerPlayer computer;

        public Game()
        {
            human = new Player("You");
            computer = new ComputerPlayer("Computer");
        }

        public void Start()
        {
            Console.Clear();
            ConsoleColors.WriteLineColor("=== Welcome to Battleship Game! ===", ConsoleColor.Magenta);
            human.PlaceAllShips();
            computer.PlaceShipsRandomly();

            bool gameOver = false;
            while (!gameOver)
            {
                Console.Clear();
                ConsoleColors.WriteLineColor("--- Your Board ---", ConsoleColor.Cyan);
                human.Board.Display(true);
                ConsoleColors.WriteLineColor("--- Opponent's Board ---", ConsoleColor.Cyan);
                computer.Board.Display(false);

                if (human.TakeShot(computer))
                {
                    gameOver = true;
                    ConsoleColors.WriteLineColor("Congratulations! You won!", ConsoleColor.Green);
                    break;
                }

                if (computer.TakeTurn(human))
                {
                    gameOver = true;
                    ConsoleColors.WriteLineColor("Sorry, you lost. Computer won.", ConsoleColor.Red);
                    break;
                }
            }
            ConsoleColors.WriteLineColor("Game over. Press any key to exit.", ConsoleColor.White);
            Console.ReadKey(true);
        }
    }
}

