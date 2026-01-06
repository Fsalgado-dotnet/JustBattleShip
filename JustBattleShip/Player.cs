using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBattleShip
{
    public class Player
    {
        public string Name { get; private set; }
        public Board Board { get; }
        private Ship[] ships;

        public Player(string name)
        {
            Name = name;
            Board = new Board();

            ships = new Ship[]
            {
                new Ship("Carrier", 5, 'C'),
                new Ship("Battleship", 4, 'B'),
                new Ship("Destroyer", 3, 'D'),
                new Ship("Submarine", 3, 'S'),
                new Ship("Patrol Boat", 2, 'P')
            };
        }

        public void PlaceAllShips()
        {
            ConsoleColors.WriteLineColor($"{Name}, place your ships.", ConsoleColor.Cyan);
            Board.Display(true);

            foreach (var ship in ships)
            {
                bool placed = false;
                do
                {
                    ConsoleColors.WriteLineColor($"Place ship '{ship.Name}' (size {ship.Size}) - Enter start position (e.g., A1) and orientation (H/V):", ConsoleColor.Yellow);
                    Console.Write("Your input: ");
                    string input = Console.ReadLine().Trim().ToUpper();
                    // Input expected format: e.g. "A1 H" or "B5 V"
                    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        ConsoleColors.WriteLineColor("Invalid input format. Please enter position and orientation separated by space.", ConsoleColor.Red);
                        continue;
                    }

                    string pos = parts[0];
                    char orientationChar = parts[1][0];

                    if (!TryParsePosition(pos, out int row, out int col))
                    {
                        ConsoleColors.WriteLineColor("Invalid position. Use format like A1 to J10.", ConsoleColor.Red);
                        continue;
                    }

                    Orientation orientation;
                    if (orientationChar == 'H')
                        orientation = Orientation.Horizontal;
                    else if (orientationChar == 'V')
                        orientation = Orientation.Vertical;
                    else
                    {
                        ConsoleColors.WriteLineColor("Invalid orientation. Enter H or V.", ConsoleColor.Red);
                        continue;
                    }

                    if (!Board.PlaceShip(ship, row, col, orientation))
                    {
                        ConsoleColors.WriteLineColor("Cannot place ship here. It either goes off board or collides with another ship.", ConsoleColor.Red);
                        continue;
                    }

                    placed = true;
                    ConsoleColors.WriteLineColor($"Placed {ship.Name} at {pos} facing {(orientation == Orientation.Horizontal ? "Horizontal" : "Vertical")}.", ConsoleColor.Green);
                    Board.Display(true);

                } while (!placed);
            }
        }

        private bool TryParsePosition(string input, out int row, out int col)
        {
            row = -1;
            col = -1;
            if (string.IsNullOrEmpty(input)) return false;
            input = input.ToUpper();

            if (input.Length < 2 || input.Length > 3) return false;

            char rowChar = input[0];
            if (rowChar < 'A' || rowChar > 'J') return false;

            string colStr = input.Substring(1);
            if (!int.TryParse(colStr, out int colNum)) return false;
            if (colNum < 1 || colNum > 10) return false;

            row = rowChar - 'A';
            col = colNum - 1;
            return true;
        }

        public bool TakeShot(Player opponent)
        {
            bool validShot = false;
            int row = -1, col = -1;
            do
            {
                ConsoleColors.WriteLineColor($"{Name}, enter coordinates to fire on (e.g., A1):", ConsoleColor.Cyan);
                Console.Write("Shot: ");
                string input = Console.ReadLine().Trim().ToUpper();

                if (!TryParsePosition(input, out row, out col))
                {
                    ConsoleColors.WriteLineColor("Invalid coordinates. Please try again.", ConsoleColor.Red);
                    continue;
                }
                if (!IsUnshotPosition(opponent.Board, row, col))
                {
                    ConsoleColors.WriteLineColor("Position already shot before. Choose different coordinates.", ConsoleColor.Red);
                    continue;
                }
                validShot = true;
            } while (!validShot);

            bool isHit, isSunk;
            string sunkShipName;
            opponent.Board.ReceiveShot(row, col, out isHit, out isSunk, out sunkShipName);

            if (isHit)
            {
                ConsoleColors.WriteLineColor("Hit!", ConsoleColor.Green);
                if (isSunk)
                {
                    ConsoleColors.WriteLineColor($"You sunk {opponent.Name}'s {sunkShipName}!", ConsoleColor.Magenta);
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

        private bool IsUnshotPosition(Board board, int row, int col)
        {
            // You can shoot only positions that are not marked with X or O
            return true; // Board class handles check inside ReceiveShot, so allow for now
        }
    }

}

