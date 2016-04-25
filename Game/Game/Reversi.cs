using Game.ClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Reversi : Form
    {
        private Piece previousPlay;

        private Dictionary<Player, Label> labels;

        /// <summary>
        /// Board
        /// </summary>
        private Board board;

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
            Player.CurrentPlayer = Player.HUMAN;
            this.board = new Board();
            this.labels = new Dictionary<Player, Label>();
            this.labels.Add(board.Players[0], this.labelScore1);
            this.labels.Add(board.Players[1], this.labelScore2);
            this.refreshBoard();
        }

        #endregion

        #region Refresh of display

        /// <summary>
        /// Actualise l'affichage de la grille selon board
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
            this.refreshScore();

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
        /// Set in Bold the next player
        /// </summary>
        private void refreshCurrentPlayer()
        {
            Font bold = new Font(this.labelScore1.Font, FontStyle.Bold);
            Font regular = new Font(this.labelScore1.Font, FontStyle.Regular);
            foreach (Player p in board.Players)
            {
                if (board.getCurrentPlayer() == p)
                {
                    this.labels[p].Font = bold;
                }
                else
                {
                    this.labels[p].Font = regular;
                    // new Font(this.labels[p].Font, FontStyle.Regular);
                }
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Click on PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            TableLayoutPanelCellPosition position = boardGUI.GetPositionFromControl(p);
            int x = position.Column;
            int y = position.Row;

            if (!this.board.play(new Piece(x, y)))
            {
                MessageBox.Show("Vous ne pouvez pas jouer ici !");
            }
            else
            {
                p.Click -= pictureBox_Click;
                p.MouseEnter -= pictureBox_HoverIn;
                p.MouseLeave -= pictureBox_HoverOut;
                previousPlay = this.board.Grid[x, y];
            }
            
            this.but_Undo.Enabled = true;
            /*
            int score = this.board.getBestMove(2, 50, - 50, this.board.BestMove);
            this.board.play(this.board.BestMove);
            */
            if (!this.board.canPlay())
            {
                this.board.setNextPlayer();
                if (this.board.gameEnd())
                    MessageBox.Show("Jeux Terminé");

            }
            
            this.refreshBoard();
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
            int x = position.Column;
            int y = position.Row;
            // MessageBox.Show(board.testNeighbour(board.Grid[y, x]).ToString());


            Piece pi = new Piece(x, y);
            if (board.canMove(pi))
            {
                // p.Image = Image.FromFile("../../Resources/grey.png");
                List<Piece> pieces = board.listeMove(pi);
                foreach (Piece piece in pieces)
                {
                    PictureBox pic = (PictureBox)this.boardGUI.GetControlFromPosition(piece.X, piece.Y);
                    if (piece.X == x && piece.Y == y)
                    {
                        if (this.board.getCurrentPlayer() == this.board.Players[0])
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
            TableLayoutPanelCellPosition position = boardGUI.GetPositionFromControl(p);
            int x = position.Column;
            int y = position.Row;
            this.refreshBoard();
        }

        #endregion

        private void but_Undo_Click(object sender, EventArgs e)
        {
            board.undoMove(previousPlay);
            this.refreshBoard();
            previousPlay = null;
            this.but_Undo.Enabled = false;
        }

        private void but_restart_Click(object sender, EventArgs e)
        {
            this.init();
        }
    }
}
