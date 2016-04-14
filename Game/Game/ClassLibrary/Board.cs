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
            /*for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = new Piece(i, j);
                }
            }*/

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
        public Player getCurrentPlayer()
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
                    if (grid[i, j] != null && grid[i, j].Player == p)
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
                Piece p = new Piece(x, y);
                p.Player = this.getCurrentPlayer();
                grid[x, y] = p;
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

        #region Test if move is legal

        public Boolean canMove(Piece p)
        {
            Boolean res = false;

            if (grid[p.X, p.Y] != null)
                return res = false;

            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p);
                Piece tempo;
                Boolean stop = true;
                int turnover = 0;

                try
                {
                    while (Grid[next.X, next.Y].Player != null && Grid[next.X, next.Y].Player.Owner == Player.COMPUTER && stop)
                    {
                        tempo = this.getNext(direction, next);
                        if (tempo.X == -1)
                        {
                            stop = false;
                        }
                        else
                        {
                            turnover++;
                            next = tempo;
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
                if (Grid[next.X, next.Y].Player != null && turnover != 0 && Grid[next.X, next.Y].Player.Owner == Player.HUMAN)
                {
                    res = true;
                }
            }
            return res;
        }

        /// <summary>
        /// Lethode to get next piece
        /// </summary>
        /// <param name="direction">Direction to move</param>
        /// <param name="p">Current piece</param>    
        /// <returns>true: move legal </returns>
        public Piece getNext(int direction, Piece p)
        {
            Piece res;
            if (direction == Direction.NORTHWEST)
            {
                if (p.X - 1 < 0 && p.Y - 1 < 0)
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X - 1, p.Y - 1);
            }
            if (direction == Direction.NORTH)
            {
                if (p.X - 1 < 0)
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X - 1, p.Y);
            }
            if (direction == Direction.NORTHEAST)
            {
                if (p.X - 1 < 0 && p.Y + 1 >= this.Grid.GetLength(1))
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X - 1, p.Y + 1);
            }
            if (direction == Direction.EAST)
            {
                if (p.Y + 1 > this.Grid.GetLength(1))
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X, p.Y + 1);
            }
            if (direction == Direction.SOUTHEAST)
            {
                if (p.X + 1 >= this.Grid.GetLength(0) && p.Y + 1 >= this.Grid.GetLength(1))
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X + 1, p.Y + 1);
            }
            if (direction == Direction.SOUTH)
            {
                if (p.X + 1 >= this.Grid.GetLength(0))
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X + 1, p.Y);
            }
            if (direction == Direction.SOUTHWEST)
            {
                if (p.X - 1 > this.Grid.GetLength(0) && p.Y - 1 < 0)
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X + 1, p.Y - 1);
            }
            if (direction == Direction.WEST)
            {
                if (p.Y - 1 < 0)
                    return res = new Piece(-1, -1);
                else
                    return res = new Piece(p.X, p.Y - 1);
            }
            return res = new Piece(-1, -1);
        }

    #endregion
        
        #region return all legal move
        /*
        public List<Piece> listeMove(Piece p)
        {
            List<Piece> listePiece = new List<Piece>();
            listePiece.Add(p);
            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p);
                int retournement = this.turnover[direction];

                while (retournement > 0)
                {

                    next = this.getNext(direction, next);
                    retournement--;
                }
            }
            return listePiece;
        }*/
        #endregion

    }
}
