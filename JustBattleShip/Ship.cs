using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustBattleShip
{
    public class Ship
    {
        public string Name { get; }
        public int Size { get; }
        public char Symbol { get; }
        public int Hits { get; private set; }

        public Ship(string name, int size, char symbol)
        {
            Name = name;
            Size = size;
            Symbol = symbol;
            Hits = 0;
        }

        public void Hit()
        {
            Hits++;
        }

        public bool IsSunk()
        {
            return Hits >= Size;
        }
    }

}

