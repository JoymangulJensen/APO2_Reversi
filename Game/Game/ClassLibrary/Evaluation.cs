using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ClassLibrary
{
    /// <summary>
    /// This methode containthe methodes to evaluate a grid
    /// </summary>
    class Evaluation
    {
        private int[,] gridWeight = new int[8, 8] {
                                                { 1000 , 50, 200 , 200 , 200 , 200 , 50, 1000  },
                                                { 50, 0, 100, 100, 100, 100, 0, 50 },
                                                { 200 , 100, 150 , 150 , 150 , 150 , 100, 200  },
                                                { 200 , 100, 150 , 150 , 150 , 150 , 100, 200  },
                                                { 200 , 100, 150 , 150 , 150 , 150 , 100, 200  },
                                                { 200 , 100, 150 , 150 , 150 , 150 , 100, 200  },
                                                { 50, 0, 100, 100, 100, 100, 0, 50 },
                                                { 1000 , 50, 200 , 200 , 200 , 200 , 50, 1000  },
                                                    };

        /// <summary>
        /// Methode to evaluate the grid with the corresponding weight
        /// </summary>
        /// <returns></returns>
        public int evaluateGrid(Board board)
        {
            int nbCurrent, nbNext, mobilityCurrent, mobilityNext;
            int res = 0;
            nbCurrent = board.getCurrentPlayer().Score;
            board.setNextPlayer();
            nbNext = board.getCurrentPlayer().Score;
            mobilityNext = board.getAllLegalMoves().Capacity;
            board.setNextPlayer();
            int scoreCurrent, scoreNext = 0;
            scoreCurrent = 0;
            mobilityCurrent = board.getAllLegalMoves().Capacity;

            for (int col = 0; col < 8; col++)
            {
                for (int lig = 0; lig < 8; lig++)
                {
                    if (board.Grid[col, lig] != null && board.Grid[col, lig].Player == board.getCurrentPlayer())
                    { // Si c'est la machine alors on calcule combien de point il a
                        scoreCurrent += this.gridWeight[col, lig];
                    }
                    else if (board.Grid[col, lig] != null)
                    {
                        scoreNext += this.gridWeight[col, lig];
                    }
                }
            }
            if (nbCurrent + nbNext <= 12)//Begining
            {
                res = -2 * (nbCurrent - nbNext) + (scoreCurrent - scoreNext) * 2 + (mobilityCurrent - mobilityNext) * 3;
            }
            else if (nbCurrent + nbNext <= 60)//middle
            {
                res = 1 * (nbCurrent - nbNext) + (scoreCurrent - scoreNext) * 10 + (mobilityCurrent - mobilityNext) * 5;
            }
            else//end
            {
                res = 200 * (nbCurrent - nbNext) + (scoreCurrent - scoreNext) * 1 + (mobilityCurrent - mobilityNext) * 1;
            }
            return res;
        }
    }
}
