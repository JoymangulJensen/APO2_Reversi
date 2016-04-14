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
        /// <summary>
        /// The number od turnover in each direction
        /// Key : direction [1-8]
        /// Value : the number of turnover
        /// </summary>
        public Dictionary<int, int> turnover = new Dictionary<int, int>();

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

            //Initialise the turonver table
            for (int i = 1; i < 9; i++)
                this.turnover.Add(i, 0);
            
        }

        /// <summary>
        /// Reinitialise the table of turonvers
        /// </summary>
        public void reInitTurnover()
        {
            for (int i = 1; i < 9; i++)
                this.turnover[i] = 0;
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

        /// <summary>
        /// Methode to test if a player can play in a specific compartiment
        /// this method also update the turonver table to get the number of turnover possible in a specific direction
        /// </summary>
        /// <param name="p">Piece which the player want to play</param>
        /// <returns>true: can play piece</returns>
        public Boolean canMove(Piece p)
        {
            Boolean res = false;    //By default a move is illegal
            this.reInitTurnover();  //reinitilase all the turnovers
            if (grid[p.X, p.Y] != null) //If the current compartiment already contain a player
                return res = false;

            //Go through all directions
            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p); //get nex move of the current piece
                Piece tempo;    //temporary variable to check if next piece is not outbound
                Boolean stop = false;    //Variable is set true when outbound
                int turnover = 0;   //the current number of turonver for the specific direction
                try
                {
                    //Loop untill the next compartiment is different from the current player and not outbound
                    while (Grid[next.X, next.Y].Player != null && Grid[next.X, next.Y].Player.Owner != this.getCurrentPlayer().Owner && !stop)
                    {
                        tempo = this.getNext(direction, next);
                        if (tempo == null) //if outbound stop the loop
                        {
                            stop = true;
                        }
                        else
                        {
                            turnover++; //increment the number of turonver for that specific direction
                            next = tempo;   
                        }
                    }
                }
                catch (Exception e)
                {
                    continue;
                }
                //If the turonver is not zero and the last piece to be tested is the same as the current player
                if (Grid[next.X, next.Y].Player != null && turnover != 0 && Grid[next.X, next.Y].Player.Owner == this.getCurrentPlayer().Owner)
                {
                    res = true;
                    //update the turonver table for that specific direction
                    this.turnover[direction] = turnover;
                }
                else
                {
                    this.turnover[direction] = 0;
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
                    return null;
                else
                    return res = new Piece(p.X - 1, p.Y - 1);
            }
            if (direction == Direction.NORTH)
            {
                if (p.Y - 1 < 0)
                    return null;
                else
                    return res = new Piece(p.X, p.Y-1);
            }
            if (direction == Direction.NORTHEAST)
            {
                if (p.Y - 1 < 0 && p.X + 1 >= this.Grid.GetLength(1))
                    return null;
                else
                    return res = new Piece(p.X + 1, p.Y - 1);
            }
            if (direction == Direction.EAST)
            {
                if (p.X + 1 >= this.Grid.GetLength(1))
                    return null;
                else
                    return res = new Piece(p.X + 1, p.Y);
            }
            if (direction == Direction.SOUTHEAST)
            {
                if (p.Y + 1 >= this.Grid.GetLength(0) && p.X + 1 >= this.Grid.GetLength(1))
                    return null;
                else
                    return res = new Piece(p.Y + 1, p.X + 1);
            }
            if (direction == Direction.SOUTH)
            {
                if (p.Y + 1 >= this.Grid.GetLength(0))
                    return null;
                else
                    return res = new Piece(p.X, p.Y + 1);
            }
            if (direction == Direction.SOUTHWEST)
            {
                if (p.Y - 1 >= this.Grid.GetLength(0) && p.X - 1 < 0)
                    return null;
                else
                    return res = new Piece(p.X - 1, p.Y + 1);
            }
            if (direction == Direction.WEST)
            {
                if (p.X - 1 < 0)
                    return null;
                else
                    return res = new Piece(p.X - 1 , p.Y);
            }
            return null;
        }

    #endregion
        
        #region return all legal move
        
        public List<Piece> listeMove(Piece p)
        {
            List<Piece> listePiece = new List<Piece>();
            
            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p);
                int retournement = this.turnover[direction];
                while (retournement > 0)
                {
                    listePiece.Add(next);
                    next = this.getNext(direction, next);
                    retournement--;
                }
            }
            if(listePiece.Count!=0)
                listePiece.Add(p);
            return listePiece;
        }
        #endregion

    }
}
