﻿namespace Game
{
    partial class Reversi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.boardGUI = new System.Windows.Forms.TableLayoutPanel();
            this.labelScore1 = new System.Windows.Forms.Label();
            this.labelScore2 = new System.Windows.Forms.Label();
            this.but_Undo = new System.Windows.Forms.Button();
            this.but_restart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // boardGUI
            // 
            this.boardGUI.BackgroundImage = global::Game.Properties.Resources.background_grid;
            this.boardGUI.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.boardGUI.ColumnCount = 8;
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.Location = new System.Drawing.Point(85, 54);
            this.boardGUI.Name = "boardGUI";
            this.boardGUI.RowCount = 8;
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.boardGUI.Size = new System.Drawing.Size(451, 432);
            this.boardGUI.TabIndex = 0;
            // 
            // labelScore1
            // 
            this.labelScore1.AutoSize = true;
            this.labelScore1.Location = new System.Drawing.Point(227, 29);
            this.labelScore1.Name = "labelScore1";
            this.labelScore1.Size = new System.Drawing.Size(63, 13);
            this.labelScore1.TabIndex = 1;
            this.labelScore1.Text = "Joueur 1 : 0";
            // 
            // labelScore2
            // 
            this.labelScore2.AutoSize = true;
            this.labelScore2.Location = new System.Drawing.Point(339, 29);
            this.labelScore2.Name = "labelScore2";
            this.labelScore2.Size = new System.Drawing.Size(63, 13);
            this.labelScore2.TabIndex = 2;
            this.labelScore2.Text = "Joueur 2 : 0";
            // 
            // but_Undo
            // 
            this.but_Undo.Location = new System.Drawing.Point(215, 492);
            this.but_Undo.Name = "but_Undo";
            this.but_Undo.Size = new System.Drawing.Size(75, 23);
            this.but_Undo.TabIndex = 3;
            this.but_Undo.Text = "Annuler";
            this.but_Undo.UseVisualStyleBackColor = true;
            this.but_Undo.Click += new System.EventHandler(this.but_Undo_Click);
            // 
            // but_restart
            // 
            this.but_restart.Location = new System.Drawing.Point(325, 492);
            this.but_restart.Name = "but_restart";
            this.but_restart.Size = new System.Drawing.Size(92, 23);
            this.but_restart.TabIndex = 4;
            this.but_restart.Text = "Recommencer";
            this.but_restart.UseVisualStyleBackColor = true;
            this.but_restart.Click += new System.EventHandler(this.but_restart_Click);
            // 
            // Reversi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Game.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(616, 548);
            this.Controls.Add(this.but_restart);
            this.Controls.Add(this.but_Undo);
            this.Controls.Add(this.labelScore2);
            this.Controls.Add(this.labelScore1);
            this.Controls.Add(this.boardGUI);
            this.Name = "Reversi";
            this.Text = "Reversi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel boardGUI;
        private System.Windows.Forms.Label labelScore1;
        private System.Windows.Forms.Label labelScore2;
        private System.Windows.Forms.Button but_Undo;
        private System.Windows.Forms.Button but_restart;
    }
}