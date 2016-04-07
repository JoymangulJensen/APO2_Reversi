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

        public Piece canPlay(Piece p)
        {

            return getNext(Direction.NORTHEAST, p);
            /*bool legal = false; //By default a move is illegal

            if(p.Player != null)    //If there id already a piece
            {
                return legal = false;
            }*/
        }

        public Piece getNext(int direction, Piece p)
        {
            Piece res;
            if(direction == Direction.NORTHEAST)
            {
                if (p.X - 1  < 0 && p.Y - 1 < 0)
                {
                    return res = new Piece(0,0);
                } else 
                    return res = new Piece(--p.X, --p.Y);
            }
            return res = new Piece(0, 0);
        }
        
#region Not Working Test
        /*


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
                if (grid[indX - 1, indY + 1].Player != null && grid[indX - 1, indY + 1].Player != this.getCurrentPlayer() && indX != 0 && indY != grid.GetLength(1) - 1)
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
                if (grid[indX + 1, indY + 1].Player != null && grid[indX + 1, indY + 1].Player != this.getCurrentPlayer() && indX != grid.GetLength(0) - 1 && indY != grid.GetLength(1) - 1)
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
                if (grid[indX + 1, indY - 1].Player != null  && grid[indX + 1, indY - 1].Player != this.getCurrentPlayer() && indX != grid.GetLength(0) - 1 && indY != 0)
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
                if (grid[indX - 1, indY].Player != null && (grid[indX - 1, indY].Player != this.getCurrentPlayer() && indX != 0))
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
                if (grid[indX, indY + 1].Player != null && grid[indX, indY + 1].Player != this.getCurrentPlayer() && indY != grid.GetLength(1) - 1)
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
                if (grid[indX + 1, indY].Player != null && grid[indX + 1, indY].Player != this.getCurrentPlayer() && indX != grid.GetLength(0) - 1)
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
                if (grid[indX, indY - 1].Player != null && grid[indX, indY - 1].Player != this.getCurrentPlayer() && indY != 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

*/
#endregion

    }
}
