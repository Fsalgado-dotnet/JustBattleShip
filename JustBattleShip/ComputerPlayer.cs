using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JustBattleShip
{
    public class ComputerPlayer : Player
    {
        private Random random;

        public ComputerPlayer(string name) : base(name)
        {
            random = new Random();
        }

        public void PlaceShipsRandomly()
        {
            foreach (var ship in new Ship[]
            {
                new Ship("Carrier", 5, 'C'),
                new Ship("Battleship", 4, 'B'),
                new Ship("Destroyer", 3, 'D'),
                new Ship("Submarine", 3, 'S'),
                new Ship("Patrol Boat", 2, 'P')
            })
            {
                bool placed = false;
                do
                {
                    int row = random.Next(0, 10);
                    int col = random.Next(0, 10);
                    Orientation orientation = (Orientation)random.Next(0, 2);
                    placed = Board.PlaceShip(ship, row, col, orientation);
                } while (!placed);
            }
            ConsoleColors.WriteLineColor($"{Name} has placed ships.", ConsoleColor.Green);
        }

        public bool TakeTurn(Player opponent)
        {
            int row, col;
            bool validShot = false;
            do
            {
                row = random.Next(0, 10);
                col = random.Next(0, 10);
                validShot = BoardPositionIsShootable(opponent.Board, row, col);
            } while (!validShot);

            bool isHit, isSunk;
            string sunkShipName;
            opponent.Board.ReceiveShot(row, col, out isHit, out isSunk, out sunkShipName);

            ConsoleColors.WriteLineColor($"{Name} fires on {Convert.ToChar(row + 'A')}{col + 1}", ConsoleColor.Cyan);
            if (isHit)
            {
                ConsoleColors.WriteLineColor("Hit!", ConsoleColor.Green);
                if (isSunk)
                {
                    ConsoleColors.WriteLineColor($"{Name} sunk your {sunkShipName}!", ConsoleColor.Magenta);
                }
            }
            else
            {
                ConsoleColors.WriteLineColor("Miss!", ConsoleColor.Yellow);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return opponent.Board.AllShipsSunk();
        }

        private bool BoardPositionIsShootable(Board board, int row, int col)
        {
            return true; // Simplified: just shoot any coordinate blindly. Real implementation can track shots.
        }
    }
}


