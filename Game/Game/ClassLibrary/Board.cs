using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{

    class Board
    {
        private Piece[,] grid;

        internal Piece[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        public Board()
        {
            this.grid = new Piece[8, 8];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1) ; j++)
                {
                    grid[i, j] = new Piece(i, j);
                }
            }

            grid[3, 3].Player = new Player(Player.HUMAN);
            grid[3, 4].Player = new Player(Player.COMPUTER);
            grid[4, 3].Player = new Player(Player.COMPUTER);
            grid[4, 4].Player = new Player(Player.HUMAN);       
        }

    }
}
