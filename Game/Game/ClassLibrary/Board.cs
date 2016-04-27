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
        internal List<Player> Players
        {
            get { return players; }
            set { players = value; }
        }

        /// <summary>
        /// Store the current turonver, use to preview a move
        /// </summary>
        private Turnover currentTurnover;
        /// <summary>
        /// Save all the turnovers of all played piece
        /// </summary>
        private Stack<Turnover> saveTurnover = new Stack<Turnover>();
        /// <summary>
        /// Save all the playes piece
        /// </summary>
        private Stack<Piece> savePieces = new Stack<Piece>();


        /// <summary>
        /// The number od value in each direction
        /// Key : direction [1-8]
        /// Value : the number of value
        /// </summary>
        //public Dictionary<int, int> turnover = new Dictionary<int, int>();
        /// <summary>
        /// Save the value table for the last played piece
        /// </summary>
        //public Dictionary<int, int> saveTurnover = new Dictionary<int, int>();


        /// <summary>
        /// Grid of the game : 2d Array of Pieces
        /// </summary>
        private Piece[,] grid;
        internal Piece[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }

        private int[,] gridWeightBis = new int[8, 8] { 
                                                { 4 , -3, 2 , 2 , 2 , 2 , -3, 4  },
                                                { -3, -4, -1, -1, -1, -1, -4, -3 },
                                                { 2 , -1, 1 , 0 , 0 , 1 , -1, 2  },
                                                { 2 , -1, 0 , 1 , 1 , 0 , -1, 2  },
                                                { 2 , -1, 0 , 1 , 1 , 0 , -1, 2  },
                                                { 2 , -1, 1 , 0 , 0 , 1 , -1, 2  },
                                                { -3, -4, -1, -1, -1, -1, -4, -3 },
                                                { 4 , -3, 2 , 2 , 2 , 2 , -3, 4  }
                                                    };

        private int[,] gridWeight = new int[8, 8] {
                                                { 1000 , -250, 10 , 20 , 20 , 10 , -250, 1000  },
                                                { -250, -400, -20, -10, -10, -20, -400, -250 },
                                                { 10 , -20, -5 , 0 , 0 , -5 , -20, 10  },
                                                { 20 , -10, 0 , 0 , 0 , 0 , -10, 20  },
                                                { 20 , -10, 0 , 0 , 0 , 0 , -10, 2  },
                                                { 10 , -20, -5 , 0 , 0 , -5 , -20, 2  },
                                                { -250, -400, -20, -10, -10, -20, -400, -250 },
                                                { 1000 , -250, 10 , 20 , 20 , 10 , -250, 1000  }
                                                    };

        private Piece bestMove;
        internal Piece BestMove
        {
            get
            {
                return bestMove;
            }
            }

        private List<Piece> initPieces = new List<Piece>();
        internal List<Piece> InitPieces
        {
            get
            {
                return initPieces;
            }
        }

        internal Stack<Piece> SavePieces
        {
            get
            {
                return savePieces;
            }

            set
            {
                savePieces = value;
            }
        }

        internal Stack<Turnover> SaveTurnover
        {
            get
            {
                return saveTurnover;
        }

            set
            {
                saveTurnover = value;
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
            this.bestMove = new Piece(0, 0, this.getPlayer(Player.COMPUTER));

            this.currentTurnover = new Turnover();

            this.InitPieces.Add(this.grid[3, 3] = new Piece(3, 3, this.getPlayer(Player.COMPUTER)));
            this.InitPieces.Add(this.grid[3, 4] = new Piece(3, 4, this.getPlayer(Player.HUMAN)));
            this.InitPieces.Add(this.grid[4, 3] = new Piece(4, 3, this.getPlayer(Player.HUMAN)));
            this.InitPieces.Add(this.grid[4, 4] = new Piece(4, 4, this.getPlayer(Player.COMPUTER)));

            this.updateScoresByCounting();
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
        /// Get the player matching the num given
        /// </summary>
        /// <param name="numPlayer"></param>
        /// <returns>The player matching the given number</returns>
        public Player getPlayer(int numPlayer)
        {
            foreach (Player p in players)
            {
                if (p.Owner == numPlayer)
                    return p;
            }
            // Shouldn't reach this point
            return players[0];
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

        #endregion

        #region Scores Management

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

        #region Manage end of the game & winners

        /// <summary>
        /// If none of the players can play
        /// </summary>
        /// <returns>true if none of the players can play</returns>
        public Boolean gameFinished()
        {
            return (!this.canPlay(this.getPlayer(Player.COMPUTER)) && !this.canPlay(this.getPlayer(Player.HUMAN)));
        }

        /// <summary>
        /// Return the winner(s) : players with the maximum score
        /// </summary>
        /// <returns>The winners (players with the maximum score)</returns>
        public List<Player> getWinners()
        {
            List<Player> winners = new List<Player>();
            Player currentWinner = players[0];
            foreach (Player p in players)
            {

                if (p.Score > currentWinner.Score)
                {
                    currentWinner = p;
                    winners.Clear();
                    winners.Add(p);
                }
                else if (p.Score == currentWinner.Score)
                {
                    winners.Add(p);
                }
            }
            return winners;
        }

        #endregion

        #region Play

        /// <summary>
        /// Try to play the given Piece
        /// </summary>
        /// <param name="p">Piece to play</param>
        /// <returns>True if the move is legal</returns>
        public bool play(Piece p, Boolean save)
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
                if(save)
                {
                    this.SavePieces.Push(p);
                    Turnover temp = new Turnover();
                    temp.clone(this.currentTurnover);
                    this.SaveTurnover.Push(temp);
                }


                // Respect the orders of the call of the two next methods
                this.updateScoresOp(changedPieces);
                this.setNextPlayer();

                return true;
            }
            else
                return false;
        }

        #endregion

        #region Test if a player have a legal move remaining

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
                int retournement = this.currentTurnover.Value[direction];
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
        public void undoMove(Piece p, Turnover p_turonver)
        {
            this.grid[p.X, p.Y] = null;  //delete the piece
            this.canMove(p);

            //Go through all directions
            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p); //get next move of the current piece
                int turnover = p_turonver.Value[direction];  //get the turnonver for a specific direction

                while (turnover > 0)
                {
                    //make the piece return to its previos state
                    this.grid[next.X, next.Y] = new Piece(next.X, next.Y, this.getCurrentPlayer());
                    next = this.getNext(direction, next);
                    turnover = turnover - 1;
                }
            }
            this.setNextPlayer();        //Make the player that wanted an undo play again
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
        /// this method also update the turonver table to get the number of value possible in a specific direction
        /// </summary>
        /// <param name="p">Piece which the player want to play</param>
        /// <returns>true: can play piece</returns>
        public Boolean canMove(Piece p, Player player)
        {
            Boolean res = false;    //By default a move is illegal
            this.currentTurnover.reinitialiase();  //reinitialiase all the turnovers

            if (grid[p.X, p.Y] != null) //If the current compartiment already contain a player
                return res = false;

            //Go through all directions
            for (int direction = 1; direction < 9; direction++)
            {
                Piece next = this.getNext(direction, p); //get next move of the current piece
                Piece tempo;    //temporary variable to check if next piece is not outbound
                Boolean stop = false;    //Variable is set true when outbound
                int turnover = 0;   //the current number of turonver for the specific direction

                //Loop untill the next compartiment is different from the current player and not outbound
                while (!stop && (this.possiblePlace(next) && Grid[next.X, next.Y] != null && Grid[next.X, next.Y].Player.Owner != player.Owner))
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
            
                //If the turonver is not zero and the last piece to be tested is the same as the current player
                if (this.possiblePlace(next) && Grid[next.X, next.Y] != null && turnover != 0 && Grid[next.X, next.Y].Player.Owner == player.Owner)
                {
                    res = true;
                    //update the turonver table for that specific direction
                    this.currentTurnover.Value[direction] = turnover;

                }
                else
                {
                    this.currentTurnover.Value[direction] = 0;
                }
            }
            return res;
        }

        public Boolean possiblePlace(Piece p)
        {
            return (p.X >= 0 && p.Y >= 0 && p.X < 8 && p.Y < 8);
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
        //Prog ou Adver
        public double aplhaBeta(int depth, double alpha, double beta, int noeud)
        {
            if(depth <= 0 || this.gameFinished())
            {
                return this.evaluateGrid();
            }
            List<Piece> listPiece = this.getAllLegalMoves();
            if (noeud == 1)//Appeller a 1 commence programme
            {
                foreach (Piece p in listPiece)
                {
                    p.Player = this.getCurrentPlayer();
                    Piece[,] tempo = new Piece[8, 8];
                    List<Player> tempoPlayers = new List<Player>();
                    this.copyGrid(this.Grid, tempo);
                    int scoreHumain = this.getPlayer(Player.HUMAN).Score;
                    int scoreComputer = this.getPlayer(Player.COMPUTER).Score;

                    this.play(p,false);

                    double score = aplhaBeta(depth - 1, alpha, beta, 2);
                    this.copyGrid(tempo, this.Grid);
                    this.getPlayer(Player.HUMAN).Score = scoreHumain;
                    this.getPlayer(Player.COMPUTER).Score = scoreComputer;
                    this.setNextPlayer();
                    if(score> alpha)
                    {
                        alpha = score;
                        this.bestMove = p;
                        if (alpha >= beta)
                        {
                            break;
                        }
                    }
                }
                return alpha;
            }
            else
            {//Adversaire
                foreach (Piece p in listPiece)
                {
                    p.Player = this.getCurrentPlayer();
                    Piece[,] tempo = new Piece[8, 8];
                    List<Player> tempoPlayers = new List<Player>();
                    this.copyGrid(this.Grid, tempo);
                    int scoreHumain = this.getPlayer(Player.HUMAN).Score;
                    int scoreComputer = this.getPlayer(Player.COMPUTER).Score;
                    this.play(p, false);

                    double score = aplhaBeta(depth - 1, alpha, beta, 2);
                    this.copyGrid(tempo, this.Grid);
                    this.getPlayer(Player.HUMAN).Score = scoreHumain;
                    this.getPlayer(Player.COMPUTER).Score = scoreComputer;
                    this.setNextPlayer();
                    if (score < beta)
                    {
                        beta = score;
                        this.bestMove = p;
                        if (alpha >= beta)
                        {
                            break;
                        }
                    }
                }
                return beta;
            }
        }
   

        public void copyGrid(Piece[,] source, Piece[,] destination)
        {
            for (int col = 0; col < 8; col++)
            {
                for (int lig = 0; lig < 8; lig++)
                {
                    if (source[col, lig] == null)
                        destination[col, lig] = null;
                    else
                    {

                        /*
                        int playnum = source[col, lig].Player.Owner;
                        Player player = this.getPlayer(playnum);
                        */
                        destination[col, lig] = new Piece(col, lig, source[col, lig].Player);
                    }
                }
            }
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
            int nbCurrent, nbNext, mobility;
            int res = 0;
            nbCurrent = getCurrentPlayer().Score;
            this.setNextPlayer();
            nbNext = getCurrentPlayer().Score;
            this.setNextPlayer();
            mobility = getAllLegalMoves().Capacity;
            int score, scoreIA = 0;
            score = 0;
            for (int col = 0; col < 8; col++)
            {
                for (int lig = 0; lig < 8; lig++)
                {
                    if (this.grid[col, lig] != null && this.grid[col, lig].Player.Owner == 1)
                    { // Si c'est la machine alors on calcule combien de point il a
                        scoreIA += this.gridWeight[col, lig];
                    }else if (this.grid[col, lig] != null && this.grid[col, lig].Player.Owner == 2)
                    {
                        score += this.gridWeight[col, lig];
                    }
                }
            }
            if(nbCurrent+ nbNext <= 15)//Begining
            {
                res = -3 * (nbCurrent-nbNext) + (scoreIA - score) * 1 + mobility * (-2);
            }
            else if (nbCurrent + nbNext <= 40)//middle
            {
                res = 1 * (nbCurrent - nbNext) + (scoreIA - score) * 2 + mobility * 2;
            }
            else//end
            {
                res = 1 * (nbCurrent - nbNext) + (scoreIA - score) * 1 + mobility * 2;
            }
            return res;
        }

        /// <summary>
        /// Methode to evaluate the grid with the corresponding weight
        /// </summary>
        /// <returns></returns>
        public int evaluateGridBis()
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
