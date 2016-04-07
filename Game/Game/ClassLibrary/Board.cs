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

        public Boolean testDiagonal(Piece p)
        {

            return true;

        }

        public Boolean testLigne(Piece p)
        {
            int indX = p.X;
            int indY = p.Y;
            for(int i= indY ; i<grid.GetLength(1); i++)
            {
                if (grid[indX, i].Player != this.getCurrentPlayer())
                {
                    return true;
                }
                if (grid[grid.GetLength(0) - i, indY].Player != this.getCurrentPlayer())
                {
                    return true;
                }
            }
            
            return true;
        }

        public Boolean testNeighbour(Piece p)
        {
            int indX = p.X;
            int indY = p.Y;
            try
            {
                //Test Upper left
                if (grid[indX - 1, indY - 1].Player != null && grid[indX - 1, indY - 1].Player != this.getCurrentPlayer() && indX != 0 && indY != 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                //Test Upper right
                if (grid[indX - 1, indY + 1].Player !=   null && grid[indX - 1, indY + 1].Player != this.getCurrentPlayer() && indX != 0 && indY != grid.GetLength(1) - 1)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }

            try
            {
                //Test bottom right
                if (grid[indX - 1, indY + 1].Player != null && grid[indX + 1, indY + 1].Player != this.getCurrentPlayer() && indX != grid.GetLength(0) - 1 && indY != grid.GetLength(1) - 1)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                //Test bottom left
                if (grid[indX - 1, indY + 1].Player != null  && grid[indX + 1, indY - 1].Player != this.getCurrentPlayer() && indX != grid.GetLength(0) - 1 && indY != 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                //Test Upper middle
                if (grid[indX - 1, indY + 1].Player != null && (grid[indX - 1, indY].Player != this.getCurrentPlayer() && indX != 0))
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                //Test right
                if (grid[indX - 1, indY + 1].Player != null && grid[indX, indY + 1].Player != this.getCurrentPlayer() && indY != grid.GetLength(1) - 1)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                //Test bottom 
                if (grid[indX - 1, indY + 1].Player != null && grid[indX + 1, indY].Player != this.getCurrentPlayer() && indX != grid.GetLength(0) - 1)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                //Test left
                if (grid[indX - 1, indY + 1].Player != null && grid[indX, indY - 1].Player != this.getCurrentPlayer() && indY != 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }



    }
}
