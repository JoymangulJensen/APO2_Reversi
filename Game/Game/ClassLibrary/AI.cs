using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{
    /// <summary>
    /// This class contain all the methode required for the AI
    /// </summary>
    class AI
    {

        private Board board;

        public AI(Board b)
        {
            this.board = b;
            this.minmax(this.board.IA_LEVEL);
        }

        #region AI
        /// <summary>
        /// Copy the playing grid
        /// </summary>
        /// <param name="source">The grid to be copied</param>
        /// <param name="destination">Where the grid is stored</param>
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
                        destination[col, lig] = new Piece(col, lig, source[col, lig].Player);
                    }
                }
            }
        }

        public void minmax(int depth)
        {
            double max_value = Double.NegativeInfinity;
            List<Piece> listPiece = this.board.getAllLegalMoves();
            foreach (Piece p in listPiece)
            {
                p.Player = this.board.getCurrentPlayer();
                Piece[,] tempo = new Piece[8, 8];
                List<Player> tempoPlayers = new List<Player>();
                this.copyGrid(this.board.Grid, tempo);
                int scoreHumain = this.board.getPlayer(Player.HUMAN).Score;
                int scoreComputer = this.board.getPlayer(Player.COMPUTER).Score;

                this.board.play(p, false);

                double val = min(depth);

                if (val > max_value)
                {
                    max_value = val;
                    this.board.BestMove = p;
                }
                this.copyGrid(tempo, this.board.Grid);
                this.board.getPlayer(Player.HUMAN).Score = scoreHumain;
                this.board.getPlayer(Player.COMPUTER).Score = scoreComputer;
                this.board.setNextPlayer();
            }
        }

        public double min(int depth)
        {
            if (depth <= 0 || this.board.gameFinished())
            {
                return new Evaluation().evaluateGrid(this.board);
            }
            double min_value = Double.PositiveInfinity;
            List<Piece> listPiece = this.board.getAllLegalMoves();
            foreach (Piece p in listPiece)
            {
                p.Player = this.board.getCurrentPlayer();
                Piece[,] tempo = new Piece[8, 8];
                List<Player> tempoPlayers = new List<Player>();
                this.copyGrid(this.board.Grid, tempo);
                int scoreHumain = this.board.getPlayer(Player.HUMAN).Score;
                int scoreComputer = this.board.getPlayer(Player.COMPUTER).Score;

                this.board.play(p, false);

                double val = max(depth - 1);

                if (val < min_value)
                {
                    min_value = val;
                }
                this.copyGrid(tempo, this.board.Grid);
                this.board.getPlayer(Player.HUMAN).Score = scoreHumain;
                this.board.getPlayer(Player.COMPUTER).Score = scoreComputer;
                this.board.setNextPlayer();
            }
            return min_value;
        }


        public double max(int depth)
        {
            if (depth <= 0 || this.board.gameFinished())
            {
                return new Evaluation().evaluateGrid(this.board);
            }
            double max_value = Double.NegativeInfinity;
            List<Piece> listPiece = this.board.getAllLegalMoves();
            foreach (Piece p in listPiece)
            {
                p.Player = this.board.getCurrentPlayer();
                Piece[,] tempo = new Piece[8, 8];
                List<Player> tempoPlayers = new List<Player>();
                this.copyGrid(this.board.Grid, tempo);
                int scoreHumain = this.board.getPlayer(Player.HUMAN).Score;
                int scoreComputer = this.board.getPlayer(Player.COMPUTER).Score;

                this.board.play(p, false);

                double val = min(depth - 1);

                if (val > max_value)
                {
                    max_value = val;
                }
                this.copyGrid(tempo, this.board.Grid);
                this.board.getPlayer(Player.HUMAN).Score = scoreHumain;
                this.board.getPlayer(Player.COMPUTER).Score = scoreComputer;
                this.board.setNextPlayer();
            }
            return max_value;
        }

        #endregion

    }
}
