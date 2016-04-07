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
            grid[x, y].Player = this.getCurrentPlayer();
            this.setNextPlayer();

            List<Piece> al = new List<Piece>();
            return al;
        }

        private Player getCurrentPlayer()
        {
            return players[Player.CurrentPlayer - 1];
        }

        private void setNextPlayer()
        {
            if (Player.CurrentPlayer >= players.Count)
            {
                Player.CurrentPlayer = Player.HUMAN;
            }
            else
            {
                Player.CurrentPlayer++;
            }

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

            this.play(3, 4);
            this.play(4, 4);
            this.play(4, 3);
            this.play(3, 3);

            /**
            grid[3, 3].Player = players[0]; // HUMAN
            grid[3, 4].Player = players[1]; // COMPUTER
            grid[4, 3].Player = players[1]; // HUMAN
            grid[4, 4].Player = players[0]; // COMPUTER
             */
        }

    }
}
