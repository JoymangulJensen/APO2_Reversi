using Game.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Game
{
    public partial class Reversi : Form
    {

        #region Attributes

        /// <summary>
        /// Used to mark the "virtual pieces" indicating the possible moves of the current player
        /// </summary>
        private const string POSSIBLE_TAG = "possible";

        /// <summary>
        /// Number of seconds before stopping the IA
        /// </summary>
        private const int IA_TIMEOUT = 3;

        /// <summary>
        /// Boolean used to stop the IA calculation if it takes too long
        /// </summary>
        public static bool stopIA = false;

        /// <summary>
        /// Piece used to record the last play
        /// </summary>
        private Piece previousPlay;

        /// <summary>
        /// Flag used to indicate if the game is finished
        /// </summary>
        private bool gameFinished = false;

        /// <summary>
        /// Dictionnary : a label is assigned to each player
        /// </summary>
        private Dictionary<Player, Label> labels;

        /// <summary>
        /// Board
        /// </summary>
        private Board board;

        #endregion

        public Reversi()
        {
            InitializeComponent();
            initGUI();
            init();
        }

        #region Initialization

        /// <summary>
        /// Initialize the GUI
        /// </summary>
        private void initGUI()
        {
            for (int col = 0; col < boardGUI.ColumnCount; col++)
            {
                for (int row = 0; row < boardGUI.RowCount; row++)
                {
                    PictureBox p = initPictureBox();
                    boardGUI.Controls.Add(p, col, row);
                }
            }
        }

        /// <summary>
        /// Return a picturebox properly set & initialized
        /// </summary>
        /// <returns></returns>
        private PictureBox initPictureBox()
        {
            PictureBox p = new PictureBox();
            p.Visible = true;
            p.SizeMode = PictureBoxSizeMode.StretchImage;
            p.Click += new EventHandler(this.pictureBox_Click);
            p.MouseEnter += new EventHandler(this.pictureBox_HoverIn);
            p.MouseLeave += new EventHandler(this.pictureBox_HoverOut);
            p.BackColor = Color.Transparent;
            p.Image = null;
            return p;
        }

        /// <summary>
        /// Initialize the board
        /// </summary>
        private void init()
        {
            gameFinished = false;
            previousPlay = null;

            Player.CurrentPlayer = Player.HUMAN;
            this.board = new Board();
            Board.IA_ON = this.pveItem.Checked;
            this.updateDifficulty();

            // Disable the events of the first 4 pieces
            foreach (Piece p in this.board.InitPieces)
            {
                this.disableEvents(p);
            }

            this.labels = new Dictionary<Player, Label>();
            this.labels.Add(board.getPlayer(Player.HUMAN), this.labelScore1);
            this.labels.Add(board.getPlayer(Player.COMPUTER), this.labelScore2);

            this.refreshBoard();
            this.refreshScore();
        }

        #endregion

        #region Refresh of display

        /// <summary>
        /// Refresh the grid (boardGUI) according to the content of the board
        /// </summary>
        private void refreshBoard()
        {
            Piece piece;
            for (int col = 0; col < boardGUI.ColumnCount; col++)
            {
                for (int row = 0; row < boardGUI.RowCount; row++)
                {
                    PictureBox p = (PictureBox)boardGUI.GetControlFromPosition(col, row);

                    if ((piece = this.board.Grid[col, row]) != null)
                    {
                        // If there is a piece

                        if (previousPlay != null && piece.X == previousPlay.X && piece.Y == previousPlay.Y)
                        {
                            this.setImageAccordingToPlayer(p, piece.Player, true);
                        }
                        else
                        {
                            this.setImageAccordingToPlayer(p, piece.Player, false);
                        }
                    }
                    else if(p.Image != null && (String) p.Tag != POSSIBLE_TAG)
                    {
                        p.Click += new EventHandler(this.pictureBox_Click);
                        p.MouseEnter += new EventHandler(this.pictureBox_HoverIn);
                        p.MouseLeave += new EventHandler(this.pictureBox_HoverOut);
                        p.BackColor = Color.Transparent;
                        p.Image = null;
                        p.Tag = null;
                    }
                    else if((String) p.Tag == POSSIBLE_TAG)
                    {
                        p.Image = null;
                        p.Tag = null;
                    }
                }
            }
            this.displayNextPossibleMoves();
            this.but_Undo.Enabled = !(this.board.SavePieces.Count == 0);
        }

        /// <summary>
        /// Display the possible moves as little pieces
        /// </summary>
        private void displayNextPossibleMoves()
        {
            PictureBox p;
            List<Piece> currentMovePossible = this.board.getAllLegalMoves();
            foreach (Piece piece in currentMovePossible)
            {
                p = (PictureBox)this.boardGUI.GetControlFromPosition(piece.X, piece.Y);
                p.Tag = POSSIBLE_TAG;
                this.setImageAccordingToPlayer(p, this.board.getCurrentPlayer(), false, true);
            }
        }

        /// <summary>
        /// Set the image properly according to the params given
        /// </summary>
        /// <param name="image"></param>
        /// <param name="player"></param>
        /// <param name="triggered"></param>
        private void setImageAccordingToPlayer(PictureBox image, Player player, bool triggered = false, bool mignature = false)
        {
            if (player == this.board.getPlayer(Player.HUMAN))
            {
                if (mignature && !triggered)
                {
                    image.Image = Image.FromFile("../../Resources/small_black.png");
                }
                else if (triggered)
                {
                    image.Image = Image.FromFile("../../Resources/black_triggered.png");
                }
                else
                {
                    image.Image = Image.FromFile("../../Resources/black.png");
                }
            }
            else
            {
                if (mignature && !triggered)
                {
                    image.Image = Image.FromFile("../../Resources/small_white.png");
                }
                else if (triggered)
                {
                    image.Image = Image.FromFile("../../Resources/white_triggered.png");
                }
                else
                {
                    image.Image = Image.FromFile("../../Resources/white.png");
                }
            }
        }

        /// <summary>
        /// Update the labels containing the scores
        /// </summary>
        private void refreshScore()
        {
            foreach (KeyValuePair<Player, Label> item in labels)
            {
                item.Value.Text = item.Key.Name + " : " + item.Key.Score;
            }
            this.refreshCurrentPlayer();
        }

        /// <summary>
        /// Update the labels according to the current player
        /// Set in Bold the current player
        /// </summary>
        private void refreshCurrentPlayer()
        {
            Font bold = new Font(this.labelScore1.Font, FontStyle.Bold);
            Font regular = new Font(this.labelScore1.Font, FontStyle.Regular);
            foreach (Player p in board.Players)
            {
                this.labels[p].Font = (board.getCurrentPlayer() == p) ? bold : regular;
                this.labels[p].BackColor = (board.getCurrentPlayer() == p) ? Color.OrangeRed : SystemColors.Control;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Click on PictureBox : play the piece on the position selected if possible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            TableLayoutPanelCellPosition position = boardGUI.GetPositionFromControl((Control) sender);
            int x = position.Column, y = position.Row;
            Piece pieceToPlay = new Piece(x, y);

            if (this.board.play(pieceToPlay, true))
            {
                // If the move was played
                this.disableEvents(pieceToPlay);
                previousPlay = this.board.Grid[x, y];
                if (Board.IA_ON)
                {
                    playIA();
                }
                else
                {
                    this.but_Undo.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Vous ne pouvez pas jouer ici !");
            }

            if (!this.board.canPlay())
            {
                if (this.board.gameFinished())
                    gameFinished = true;
                if (!Board.IA_ON && ! gameFinished)
                    this.skipRoundOfCurrentPlayer();
            }

            this.refreshBoard();
            this.refreshScore();

            if (gameFinished)
            {
                this.manageEnd();
            }

        }

        /// <summary>
        /// The player has played, it's the IA round now
        /// </summary>
        private void playIA()
        {
            
            bool iACanPlay = true;
            if (this.board.canPlay())
            {
                do
                {
                    if (!runWithTimeout(setBestMoveIA, TimeSpan.FromSeconds(IA_TIMEOUT)))
                    {
                        MessageBox.Show("IA trop longue");
                        stopIA = false;
                    }
                    if (this.board.play(this.board.BestMove, true))
                    {
                        this.previousPlay = this.board.BestMove;
                        this.disableEvents(this.board.BestMove);
                    }
                    else
                    {
                        this.board.setNextPlayer();
                    }
                    
                    // this.board.setNextPlayer();
                } while (! this.board.canPlay(this.board.getPlayer(Player.HUMAN)) && (iACanPlay = this.board.canPlay(this.board.getPlayer(Player.COMPUTER))));

                if (!iACanPlay)
                    gameFinished = true;
            }
            else
            {
                this.skipRoundOfCurrentPlayer();
            }
        }


        /// <summary>
        /// Hover on PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void pictureBox_HoverIn(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            TableLayoutPanelCellPosition position = boardGUI.GetPositionFromControl(p);
            int x = position.Column, y = position.Row;

            Piece pieceSim = new Piece(x, y);
            if (board.canMove(pieceSim))
            {
                List<Piece> pieces = board.listeMove(pieceSim);
                foreach (Piece piece in pieces)
                {
                    PictureBox pic = (PictureBox)this.boardGUI.GetControlFromPosition(piece.X, piece.Y);
                    if (piece.X == x && piece.Y == y)
                    {
                        if (this.board.getCurrentPlayer() == this.board.getPlayer(Player.HUMAN))
                        {
                            pic.Image = Image.FromFile("../../Resources/black.png");
                        }
                        else
                        {
                            pic.Image = Image.FromFile("../../Resources/white.png");
                        }
                    }
                    else
                    {
                        pic.Image = Image.FromFile("../../Resources/dark_grey_small.png");
                    }
                }
            }
        }

        /// <summary>
        /// Hover out PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_HoverOut(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.Image = null;
            p.Tag = null;
            this.refreshBoard();
        }

        /// <summary>
        /// Disable all the events linked to the picture of the piece
        /// </summary>
        /// <param name="p"></param>
        private void disableEvents(Piece p)
        {
            try
            {
                PictureBox picture = (PictureBox)this.boardGUI.GetControlFromPosition(p.X, p.Y);
                picture.Click -= pictureBox_Click;
                picture.MouseEnter -= pictureBox_HoverIn;
                picture.MouseLeave -= pictureBox_HoverOut;
                picture.Tag = null;
            }
            catch (Exception)
            {
                MessageBox.Show("Disable events on " + p.ToString() + " failed");
            }
        }

        #endregion

        #region Threads

        /// <summary>
        /// Run the method given in param, stopIA is set to true once the timeout is over
        /// </summary>
        /// <param name="threadStart">Thread to be started</param>
        /// <param name="timeout">Time to let the thread live</param>
        /// <returns>true if the thread finished by itself, false if it didn't have time</returns>
        private static bool runWithTimeout(ThreadStart threadStart, TimeSpan timeout)
        {
            Thread workerThread = new Thread(threadStart);

            workerThread.Start();

            bool finished = workerThread.Join(timeout);
            if (!finished)
                stopIA = true;

            return finished;
        }

        /// <summary>
        /// Call the IA algorithm (alphabeta)
        /// This needs to be in a separated method because we need to call it in a different thread (to stop it after X seconds)
        /// </summary>
        private void setBestMoveIA()
        {
            this.board.aplhaBeta(this.board.IA_LEVEL, double.PositiveInfinity, double.NegativeInfinity);
        }

        #endregion

        #region Utils

        /// <summary>
        /// Manage the display of the end of the game
        /// </summary>
        private void manageEnd()
        {
            List<Player> winners = this.board.getWinners();
            if (winners.Count > 1)
            {
                MessageBox.Show("Egalité");
            }
            else if (winners.Count == 1)
            {
                MessageBox.Show(winners[0].Name + " a gagné !");
            }
            else
            {
                MessageBox.Show("Pas de gagnant");
            }
        }

        /// <summary>
        /// Skip the round of the current player because this one can't play !
        /// </summary>
        private void skipRoundOfCurrentPlayer()
        {
            MessageBox.Show(this.board.getCurrentPlayer().Name + " ne peut pas jouer");
            this.board.setNextPlayer();
        }

        #endregion

        #region Buttons
        /// <summary>
        /// Button Undo Click Event : Undo the last move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_Undo_Click(object sender, EventArgs e)
        {
            board.undoMove(this.board.SavePieces.Pop(), this.board.SaveTurnover.Pop());
            if (Board.IA_ON)
                board.undoMove(this.board.SavePieces.Pop(), this.board.SaveTurnover.Pop());

            previousPlay = this.board.SavePieces.Count > 0 ? this.board.SavePieces.Peek() : null;
            this.refreshBoard();
            this.refreshScore();
            this.gameFinished = false;
        }

        /// <summary>
        /// Button restart : Restart the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_restart_Click(object sender, EventArgs e)
        {
            this.init();
        }
        #endregion

        #region Menu Items
        /// <summary>
        /// Click on an item of the difficulty menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemDifficulty_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            this.manageCheck(tsmi, difficultyItem);
            this.updateDifficulty();
        }

        /// <summary>
        /// Update the difficulty according to the menu item checked
        /// </summary>
        private void updateDifficulty()
        {
            if (easyItem.Checked)
            {
                this.board.IA_LEVEL = 1;
            }
            else if (mediumItem.Checked)
            {
                this.board.IA_LEVEL = 3;  
            }
            else
            {
                this.board.IA_LEVEL = 5; 
            }
        }

        /// <summary>
        /// Click on an item of the mode menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemMode_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            this.manageCheck(tsmi, modeItem);
            this.difficultyItem.Enabled = (Board.IA_ON = !this.pvpItem.Checked);
            if (Board.IA_ON)
            {
                this.board.Players[0].Name = "Joueur";
                this.board.Players[1].Name = "Ordinateur";
            }
            else
            {
                this.board.Players[0].Name = "Joueur 1";
                this.board.Players[1].Name = "Joueur 2";
            }
            this.refreshScore();
            // this.init();
        }

        /// <summary>
        /// Manage the check of the items
        /// </summary>
        /// <param name="clicked">Item cliecked</param>
        /// <param name="parent">Parent of the item clicked (Mode or Difficulty)</param>
        private void manageCheck(ToolStripMenuItem clicked, ToolStripMenuItem parent)
        {
            foreach (ToolStripMenuItem ts in parent.DropDownItems)
            {
                if (clicked == ts) clicked.Checked = true; else ts.Checked = false;
            }
        }
        #endregion
    }
}
