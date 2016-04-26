using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.ClassLibrary
{

    class Board
    {
        #region Attributes

        /// <summary>
        /// Indicates if the IA is activated or not
        /// </summary>
        public bool IA_ON = false;

        /// <summary>
        /// Indicates the level of the IA : 
        /// 1 = Easy
        /// 2 = Medium
        /// 3 = Hard
        /// </summary>
        public int IA_LEVEL = 1;

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
        /// <summary>
        /// Save the turnover table for the last played piece
        /// </summary>
        public Dictionary<int, int> saveTurnover = new Dictionary<int, int>();
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

        private int[,] gridWeight = new int[8, 8] { 
                                                { 500 , -150, 30 , 10 , 10 , 30 , -150, 500  },
                                                { -150, -250, 0, 0, 0, 0, -250, -150 },
                                                { 30 , 0, 1 , 2 , 2 , 1 , 0, 30  },
                                                { 10 , 0, 2 , 16 , 16 , 2 , 0, 10  },
                                                { 10 , 0, 2 , 16 , 16 , 2 , 0, 10  },
                                                { 30 , 0, 1 , 2 , 2 , 1 , 0, 30  },
                                                { -150, -250, 0, 0, 0, 0, -250, -150 },
                                                { 500 , -150, 30 , 10 , 10 , 30 , -150, 500  },
                                                    };


        private Piece bestMove;
        internal Piece BestMove
        {
            get
            {
                return bestMove;
            }

            set
            {
                bestMove = value;
            }
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

            //Initialise the turonver table
            for (int i = 1; i < 9; i++)
                this.turnover.Add(i, 0);

            this.grid[3, 3] = new Piece(3, 3, players[1]);
            this.grid[3, 4] = new Piece(3, 4, players[0]);
            this.grid[4, 3] = new Piece(4, 3, players[0]);
            this.grid[4, 4] = new Piece(4, 4, players[1]);

            this.updateScoresByCounting();
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
        public void setNextPlayer()
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
        private void updateScoresByCounting()
        {
            foreach (Player player in players)
            {
                player.Score = numberOfPieceByPlayer(player);
            }
        }

        /// <summary>
        /// Update the score of the players according of list of the changed pieces and not by browsing the grid
        /// </summary>
        /// <param name="list"></param>
        private void updateScoresOp(List<Piece> list)
        {
            int pieceChanged = list.Count;

            foreach (Player player in players)
            {
                if (this.getCurrentPlayer() == player)
                {
                    player.Score += pieceChanged;
                }
                else if (pieceChanged > 0)
                {
                    player.Score -= pieceChanged - 1;
                }
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
        /// Try to play the given Piece
        /// </summary>
        /// <param name="p">Piece to play</param>
        /// <returns>True if the move is legal</returns>
        public bool play(Piece p)
        {
            if (this.canMove(p)) // Can play
            {
                // p.Player = this.getCurrentPlayer();
                // grid[p.X, p.Y] = p;

                List<Piece> changedPieces = this.listeMove(p);

                foreach (Piece piece in changedPieces)
                {
                    piece.Player = this.getCurrentPlayer();
                    this.grid[piece.X, piece.Y] = piece;
                }
                //Copy the turnover table for the played piece(used for undoing a move)
                for (int i = 1; i < 9; i++)
                    saveTurnover[i] = turnover[i];
                    
                // Respect the orders of the call of the two next methods
                this.updateScoresOp(changedPieces);
                this.setNextPlayer();

                // playIA
                /*if (IA_ON)
                {
                    this.getMoveWithBadIa();
                    this.play(this.BestMove);
                }*/

                return true;
            }
            else
            return false;
        }

        #endregion

        #region Test if a player have a legal move remaining and if a game ends                  

        public Boolean gameEnd()
        {
            if (!this.canPlay(new Player(Player.COMPUTER)) && !this.canPlay(new Player(Player.HUMAN)))
                return true;
            return false;
        }

        public Boolean canPlay()
        {
            return this.canPlay(this.getCurrentPlayer());
        }

        public Boolean canPlay(Player player)
        {
            Boolean res = false; //by default we cannot play
            for (int col = 0; col < this.grid.GetLength(0); col++)
            {
                for (int row = 0; row < this.grid.GetLength(1); row++)
                {
                    if (grid[col, row] == null)
                    {
                        if (this.canMove(new Piece(col, row), player))
                            return res = true;
                    }
                }
            }
            return res;
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
            if (listePiece.Count != 0)
                listePiece.Add(p);
            return listePiece;
        }
        #endregion

        #region undo a move
        /// <summary>
        /// Methode to undo a previous move
        /// The grid return to its previous state
        /// </summary>
        /// <param name="p">The piece to be undone</param>
        public void undoMove(Piece p)
        {
            this.grid[p.X, p.Y] = null ;  //delete the piece
            this.canMove(p);

            //Go through all directions
            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p); //get next move of the current piece
                int turnover = this.saveTurnover[direction];  //get the turnonver for a specific direction

                while (turnover > 0)
                {
                    //make the piece return to its previos state
                    this.grid[next.X, next.Y] = new Piece(next.X, next.Y, this.getCurrentPlayer());
                    next = this.getNext(direction, next);
                    turnover = turnover - 1;
                }
            }
            this.setNextPlayer();        //Make the player that wanted an undo plat again
            this.updateScoresByCounting();
        }

        #endregion

        #region Test if move is legal

        public Boolean canMove(Piece p)
        {
            return this.canMove(p, this.getCurrentPlayer());
        }
        /// <summary>
        /// Methode to test if a player can play in a specific compartiment
        /// this method also update the turonver table to get the number of turnover possible in a specific direction
        /// </summary>
        /// <param name="p">Piece which the player want to play</param>
        /// <returns>true: can play piece</returns>
        public Boolean canMove(Piece p, Player player)
        {
            Boolean res = false;    //By default a move is illegal
            this.reInitTurnover();  //reinitiliase all the turnovers

            if (grid[p.X, p.Y] != null) //If the current compartiment already contain a player
                return res = false;

            //Go through all directions
            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p); //get next move of the current piece
                Piece tempo;    //temporary variable to check if next piece is not outbound
                Boolean stop = false;    //Variable is set true when outbound
                int turnover = 0;   //the current number of turonver for the specific direction
                try
                {
                    //Loop untill the next compartiment is different from the current player and not outbound
                    while (!stop && (Grid[next.X, next.Y] != null && Grid[next.X, next.Y].Player.Owner != player.Owner))
                    {
                        tempo = this.getNext(direction, next);
                        if (tempo.X < 0 || tempo.Y < 0 || tempo.X > 7 || tempo.Y > 7)  //if outbound stop the loop
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
                if (Grid[next.X, next.Y] != null && turnover != 0 && Grid[next.X, next.Y].Player.Owner == player.Owner)
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
            if (direction == Direction.NORTHEAST)
            {
                    return new Piece(p.X + 1, p.Y - 1);
            }
            if (direction == Direction.NORTH)
            {
                    return new Piece(p.X, p.Y - 1);
            }
            if (direction == Direction.NORTHWEST)
            {
                    return new Piece(p.X - 1, p.Y - 1);
            }
            
            if (direction == Direction.EAST)
            {
                    return new Piece(p.X + 1, p.Y);
            }
            if (direction == Direction.SOUTHEAST)
            {
                    return new Piece(p.X + 1, p.Y + 1);
            }
            if (direction == Direction.SOUTH)
            {
                    return new Piece(p.X, p.Y + 1);
            }
            if (direction == Direction.SOUTHWEST)
            {
                    return new Piece(p.X - 1, p.Y + 1);
            }
            if (direction == Direction.WEST)
            {
                return new Piece(p.X - 1, p.Y);
            }
            return new Piece(0, 0);
        }

        #endregion

        #region AI
        public int getBestMove(int depth, int alpha, int beta, Piece bestMove)
        {
            int bestScore;
            if(depth == 0 || this.gameEnd())
            {
                return this.evaluateGrid();
            }
            if (this.canPlay())
            {
                List<Piece> listPiece = this.getAllLegalMoves();
                Piece firstPiece = listPiece[0];
                firstPiece.Player = this.getCurrentPlayer();
                if(bestMove != null)
                {
                    bestMove = firstPiece;
                }
                listPiece.RemoveAt(0);
                this.play(firstPiece);
                Piece[,] tempo = this.copyGrid(this.Grid);
                bestScore = -getBestMove(depth - 1, -beta, -alpha, null);
                this.Grid = this.copyGrid(tempo);
                this.setNextPlayer();

                if(bestScore >= alpha )
                {
                    alpha = bestScore;
                }
                if(bestScore < beta)
                {
                    foreach(Piece p in listPiece)
                    {
                        p.Player = this.getCurrentPlayer();
                        this.play(p);
                        tempo = this.copyGrid(this.Grid);
                        int score = getBestMove(depth - 1, -alpha -1, -alpha, null);
                        this.Grid = this.copyGrid(tempo);
                        this.setNextPlayer();
                        if(score > bestScore)
                        {
                            bestScore = score;
                            bestMove = p;
                            if(bestScore > alpha)
                            {
                                alpha = score;
                                if(bestScore > beta)
                                {
                                    return bestScore;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                this.setNextPlayer();
                bestScore = -getBestMove(depth - 1, -beta, -alpha, null);
                this.setNextPlayer();
            }
            return bestScore;
        }

        public Piece[,] copyGrid(Piece[,] grid)
        {
            Piece[,] res = new Piece[8,8];
            for(int col=0; col <8; col++)
            {
                for(int lig=0; lig<8;lig++)
                {
                    res[col, lig] = this.grid[col, lig];
                }
            }
            return res;
        }


        /// <summary>
        /// Methode tha return all the piece that can be placed on the grid
        /// </summary>
        /// <returns></returns>
        public List<Piece> getAllLegalMoves()
        {
            List<Piece> listPiece = new List<Piece>();
            for (int col = 0; col < 8; col++)
            {
                for (int lig = 0; lig < 8; lig++)
                {
                    Piece p = new Piece(col, lig);
                    if (this.canMove(p))
                        listPiece.Add(p);
                }
            }
            return listPiece;
        }

        
        /// <summary>
        /// Methode to evaluate the grid with the corresponding weight
        /// </summary>
        /// <returns></returns>
        public int evaluateGrid()
        {
            int score = 0;
            for (int col = 0; col < 8; col++)
            {
                for (int lig = 0; lig < 8; lig++)
                {
                    if (this.grid[col, lig] != null)
                    { // Attention je ne suis pas sur faudrai peut etre plutot vérifier si c'est a un joueur?
                        score += this.gridWeight[col, lig];
                        
                    }
                }
            }

            return score;
        }
        #endregion
    }
}
