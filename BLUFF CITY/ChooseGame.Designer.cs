using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System;

namespace BLUFF_CITY
{
    partial class ChooseGame
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
            LIAR_GAME = new Button();
            MAFIA_GAME = new Button();
            LogOut = new Button();
            SuspendLayout();
            // 
            // LIAR_GAME
            // 
            LIAR_GAME.BackColor = Color.Gainsboro;
            LIAR_GAME.Font = new Font("Pyunji R", 28F, FontStyle.Bold);
            LIAR_GAME.Location = new Point(272, 339);
            LIAR_GAME.Name = "LIAR_GAME";
            LIAR_GAME.Size = new Size(308, 75);
            LIAR_GAME.TabIndex = 0;
            LIAR_GAME.Text = "LIAR GAME";
            LIAR_GAME.UseVisualStyleBackColor = false;
            LIAR_GAME.Click += LIAR_GAME_Click;
            // 
            // MAFIA_GAME
            // 
            MAFIA_GAME.BackColor = Color.Gainsboro;
            MAFIA_GAME.Font = new Font("Pyunji R", 28F, FontStyle.Bold);
            MAFIA_GAME.Location = new Point(272, 139);
            MAFIA_GAME.Name = "MAFIA_GAME";
            MAFIA_GAME.Size = new Size(359, 75);
            MAFIA_GAME.TabIndex = 1;
            MAFIA_GAME.Text = "MAFIA_GAME";
            MAFIA_GAME.UseVisualStyleBackColor = false;
            MAFIA_GAME.Click += MAFIA_GAME_Click;
            // 
            // LogOut
            // 
            LogOut.BackColor = Color.Gainsboro;
            LogOut.Font = new Font("Pyunji R", 15F, FontStyle.Bold);
            LogOut.Location = new Point(764, 486);
            LogOut.Name = "LogOut";
            LogOut.Size = new Size(150, 46);
            LogOut.TabIndex = 2;
            LogOut.Text = "LogOut";
            LogOut.UseVisualStyleBackColor = false;
            LogOut.Click += LogOut_Click;
            // 
            // ChooseGame
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.choose_game;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(939, 551);
            Controls.Add(LogOut);
            Controls.Add(MAFIA_GAME);
            Controls.Add(LIAR_GAME);
            DoubleBuffered = true;
            Name = "ChooseGame";
            Text = "ChooseGame";
            ResumeLayout(false);
        }

        #endregion

        private Button LIAR_GAME;
        private Button MAFIA_GAME;
        private Button LogOut;

        private Button[] GameButtons; // LIAR_GAME,MAFIA_GAME 버튼 배열

        private void InitializeArrays()
        {
            // GameButtons 배열 초기화
            GameButtons = new Button[] { LIAR_GAME, MAFIA_GAME, LogOut };
        }

    }
}