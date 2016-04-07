using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{

    class Board
    {
        List<Player> players;

        private Piece[,] grid;

        internal Piece[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        public List<Piece> play(int x, int y)
        {
            grid[x, y].Player = getCurrentPlayer();
            List<Piece> al = new List<Piece>();
            return al;
        }

        private Player getCurrentPlayer()
        {
            foreach (Player p in players)
            {
                if (p.Owner == Player.currentPlayer)
                    return p;
            }
            return players[0];
        }

        public int numberOfPieceByPlayer(int p)
        {
            int count = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].Player != null && grid[i, j].Player.Owner == p)
                        count++;
                }
            }
            return count;
        }

        public Board()
        {

            players = new List<Player>();
            players.Add(new Player(Player.HUMAN));
            players.Add(new Player(Player.COMPUTER));

            this.grid = new Piece[8, 8];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Piece(i, j);
                }
            }

            grid[3, 3].Player = players[0]; // HUMAN
            grid[3, 4].Player = players[1]; // COMPUTER
            grid[4, 3].Player = players[1]; // HUMAN
            grid[4, 4].Player = players[0]; // COMPUTER
        }

    }
}
