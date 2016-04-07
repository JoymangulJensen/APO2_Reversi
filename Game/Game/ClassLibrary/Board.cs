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
        #region Attributes

        /// <summary>
        /// List of the players
        /// </summary>
        List<Player> players;

        internal List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }

        /// <summary>
        /// Grid of the game : 2d Array of Pieces
        /// </summary>
        private Piece[,] grid;



        internal Piece[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        #endregion

        #region Constructor and Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        public Board()
        {
            players = new List<Player>();
            players.Add(new Player(Player.HUMAN, "Joueur"));
            players.Add(new Player(Player.COMPUTER, "Ordinateur"));

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

        #endregion

        #region Manage the player rounds

        /// <summary>
        /// Return the object Player who will play the next move
        /// </summary>
        /// <returns></returns>
        private Player getCurrentPlayer()
        {
            return players[Player.CurrentPlayer - 1];
        }

        /// <summary>
        /// Update the next player
        /// </summary>
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



        /// <summary>
        /// Update the score of the players
        /// </summary>
        private void updateScores()
        {
            foreach (Player player in players)
            {
                player.Score = numberOfPieceByPlayer(player);
        }
        }

        /// <summary>
        /// Return the number of piece possessed by the player given
        /// </summary>
        /// <param name="p">Player</param>
        /// <returns>The number of pieces</returns>
        private int numberOfPieceByPlayer(Player p)
        {
            int count = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j].Player != null && grid[i, j].Player == p)
                        count++;
                }
            }
            return count;
        }

        #endregion

        #region Play


        /// <summary>
        /// Try to play on the given coordonates
        /// </summary>
        /// <param name="x">Column index</param>
        /// <param name="y">Row index</param>
        /// <returns></returns>
        public List<Piece> play(int x, int y)
            {
            List<Piece> changedPieces = new List<Piece>();
            if (true) // Can play
                {
                grid[x, y].Player = this.getCurrentPlayer();
                changedPieces.Add(grid[x, y]);
                this.setNextPlayer();
                this.updateScores();
                }
            else
            {
                // Can't play
            }

            return changedPieces;
        }

        #endregion


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
