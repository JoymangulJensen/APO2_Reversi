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

        private Dictionary<Player, Label> labels = new Dictionary<Player, Label>();

        /// <summary>
        /// Board
        /// </summary>
        private Board board;

        public Reversi()
        {
            InitializeComponent();
            init();
        }

        #region Initialization

        /// <summary>
        /// Initialise la grille
        /// </summary>
        private void init()
        {
            this.board = new Board();

            for (int col = 0; col < boardGUI.ColumnCount; col++)
            {
                for (int row = 0; row < boardGUI.RowCount; row++)
                {
                    PictureBox p = initPictureBox();
                    boardGUI.Controls.Add(p, col, row);
                }
            }

            this.labels.Add(board.Players[0], this.labelScore1);
            this.labels.Add(board.Players[1], this.labelScore2);
            this.refreshBoard();
        }

        /// <summary>
        /// Renvoie une PictureBox correctement initialisée
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
                        p.MouseEnter -= pictureBox_HoverIn;
                        p.MouseLeave -= pictureBox_HoverOut;

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
                }
            }
            this.refreshScore();

        }

        /// <summary>
        /// Update the labels containing the scores
        /// </summary>
        private void refreshScore()
        {
            foreach (KeyValuePair<Player, Label> label in labels)
            {
                label.Value.Text = label.Key.Name + " : " + label.Key.Score;
            }
            this.refreshCurrentPlayer();
        }

        /// <summary>
        /// Update the labels according to the current player
        /// Set in Bold the next player
        /// </summary>
        private void refreshCurrentPlayer()
        {
            foreach (Player p in board.Players)
            {
                if (board.getCurrentPlayer() == p)
                {
                    this.labels[p].Font = new Font(this.labels[p].Font, FontStyle.Bold);
                }
                else
                {
                    this.labels[p].Font = new Font(this.labels[p].Font, FontStyle.Regular);
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
            }

            this.refreshBoard();
        }

        /// <summary>
        /// Hover on PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        pic.Image = Image.FromFile("../../Resources/grey.png");
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

    }
}
