using Game.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public partial class Reversi : Form
    {

        #region Attributes

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
            this.board.IA_ON = this.pveItem.Checked;

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
            for (int col = 0; col < boardGUI.ColumnCount; col++)
            {
                for (int row = 0; row < boardGUI.RowCount; row++)
                {
                    PictureBox p = (PictureBox)boardGUI.GetControlFromPosition(col, row);

                    if (this.board.Grid[col, row] != null)
                    {
                        // If there is a piece

                        if (this.board.Grid[col, row].Player.Owner == Player.HUMAN)
                        {
                            // Human : Display Black
                            p.Image = Image.FromFile("../../Resources/black.png");
                        }
                        else
                        {
                            // AI : Display White
                            p.Image = Image.FromFile("../../Resources/white.png");
                        }
                    }
                    else if(p.Image != null)
                    {
                        p.Click += new EventHandler(this.pictureBox_Click);
                        p.MouseEnter += new EventHandler(this.pictureBox_HoverIn);
                        p.MouseLeave += new EventHandler(this.pictureBox_HoverOut);
                        p.BackColor = Color.Transparent;
                        p.Image = null;
                    }
                }
            }
            if(this.board.SavePieces.Count == 0)
            {
                this.but_Undo.Enabled = false;
        }
            else
            {
                this.but_Undo.Enabled = true;
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
                if (this.board.IA_ON)
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
                if (!board.IA_ON && ! gameFinished)
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
                    this.board.aplhaBeta(4, double.PositiveInfinity, double.NegativeInfinity); // TODO : Manage players here
                    this.board.play(this.board.BestMove, true);
                    this.disableEvents(this.board.BestMove);
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
            }
            catch (Exception)
            {
                MessageBox.Show("Disable events on " + p.ToString() + " failed");
            }
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
            if (this.board.IA_ON)
                board.undoMove(this.board.SavePieces.Pop(), this.board.SaveTurnover.Pop());
            this.refreshBoard();
            this.refreshScore();
            previousPlay = null;
            this.gameFinished = false;
            //this.but_Undo.Enabled = false;
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
            if (tsmi == easyItem)
            {
                MessageBox.Show("Easy !");
            }
            else if (tsmi == mediumItem)
            {
                MessageBox.Show("Medium !");
            }
            else
            {
                MessageBox.Show("Hard !");
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
            this.difficultyItem.Enabled = (this.board.IA_ON = !this.pvpItem.Checked);
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
