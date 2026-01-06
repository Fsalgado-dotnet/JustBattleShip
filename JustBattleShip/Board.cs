using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBattleShip
{
    public class Board
    {
        private const int BoardSize = 10;
        private readonly char[,] grid;
        private readonly Ship[,] shipGrid;
        private readonly string[] rowLabels = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        public Board()
        {
            grid = new char[BoardSize, BoardSize];
            shipGrid = new Ship[BoardSize, BoardSize];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < BoardSize; r++)
                for (int c = 0; c < BoardSize; c++)
                    grid[r, c] = '~'; // Water symbol
        }

        public bool PlaceShip(Ship ship, int row, int col, Orientation orientation)
        {
            if (!CanPlaceShip(ship, row, col, orientation)) return false;
            for (int i = 0; i < ship.Size; i++)
            {
                int r = orientation == Orientation.Horizontal ? row : row + i;
                int c = orientation == Orientation.Horizontal ? col + i : col;
                grid[r, c] = ship.Symbol;
                shipGrid[r, c] = ship;
            }
            return true;
        }

        private bool CanPlaceShip(Ship ship, int row, int col, Orientation orientation)
        {
            if (orientation == Orientation.Horizontal)
            {
                if (col + ship.Size > BoardSize) return false;
                for (int i = 0; i < ship.Size; i++)
                    if (shipGrid[row, col + i] != null)
                        return false;
            }
            else // Vertical
            {
                if (row + ship.Size > BoardSize) return false;
                for (int i = 0; i < ship.Size; i++)
                    if (shipGrid[row + i, col] != null)
                        return false;
            }
            return true;
        }

        public bool ReceiveShot(int row, int col, out bool isHit, out bool isSunk, out string sunkShipName)
        {
            isHit = false;
            isSunk = false;
            sunkShipName = null;

            if (grid[row, col] == 'X' || grid[row, col] == 'O')
            {
                // Position already shot here
                return false;
            }

            if (shipGrid[row, col] != null)
            {
                isHit = true;
                Ship ship = shipGrid[row, col];
                ship.Hit();
                grid[row, col] = 'X'; // Mark hit
                if (ship.IsSunk())
                {
                    isSunk = true;
                    sunkShipName = ship.Name;
                }
            }
            else
            {
                grid[row, col] = 'O'; // Mark miss
            }
            return true;
        }

        public void Display(bool showShips)
        {
            // Header
            ConsoleColors.WriteColor("   ", ConsoleColor.White);
            for (int c = 0; c < BoardSize; c++)
            {
                ConsoleColors.WriteColor((c + 1).ToString().PadLeft(2) + " ", ConsoleColor.White);
            }
            Console.WriteLine();

            for (int r = 0; r < BoardSize; r++)
            {
                ConsoleColors.WriteColor(rowLabels[r] + "  ", ConsoleColor.White);
                for (int c = 0; c < BoardSize; c++)
                {
                    char symbol = grid[r, c];
                    ConsoleColor color = ConsoleColor.White;

                    if (symbol == '~')
                    {
                        color = ConsoleColor.Blue;
                    }
                    else if (symbol == 'O')
                    {
                        color = ConsoleColor.Yellow;
                    }
                    else if (symbol == 'X')
                    {
                        color = ConsoleColor.Red;
                    }
                    else if (showShips)
                    {
                        color = ConsoleColor.Green;
                    }
                    else
                    {
                        symbol = '~';
                        color = ConsoleColor.Blue;
                    }
                    ConsoleColors.WriteColor(SymbolToString(symbol) + " ", color);
                }
                Console.WriteLine();
            }
        }

        private string SymbolToString(char symbol)
        {
            // Use unicode symbols or text for better visuals
            switch (symbol)
            {
                case '~': return "•"; // water dot
                case 'O': return "○"; // miss
                case 'X': return "×"; // hit
                default: return symbol.ToString(); // ship symbol
            }
        }

        public bool AllShipsSunk()
        {
            foreach (var ship in shipGrid)
            {
                if (ship != null && !ship.IsSunk()) return false;
            }
            return true;
        }
    }

}

