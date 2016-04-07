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
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Piece(i, j);
                }
            }

            grid[3, 3].Player = new Player(Player.HUMAN);
            grid[3, 4].Player = new Player(Player.COMPUTER);
            grid[4, 3].Player = new Player(Player.COMPUTER);
            grid[4, 4].Player = new Player(Player.HUMAN);
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
                if(grid[indX, i].Player != p.Player)
                {
                    return true;
                }
                if(grid[grid.GetLength(0) - i, indY].Player != p.Player)
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
                if (grid[indX - 1, indY - 1].Player != p.Player && indX != 0 && indY != 0)
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
                if (grid[indX - 1, indY + 1].Player != p.Player && indX != 0 && indY != grid.GetLength(1) - 1)
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
                if (grid[indX + 1, indY + 1].Player != p.Player && indX != grid.GetLength(0) - 1 && indY != grid.GetLength(1) - 1)
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
                if (grid[indX + 1, indY - 1].Player != p.Player && indX != grid.GetLength(0) - 1 && indY != 0)
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
                if (grid[indX - 1, indY].Player != p.Player && indX != 0)
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
                if (grid[indX, indY + 1].Player != p.Player && indY != grid.GetLength(1) - 1)
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
                if (grid[indX + 1, indY].Player != p.Player && indX != grid.GetLength(0) - 1)
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
                if (grid[indX, indY - 1].Player != p.Player && indY != 0)
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
