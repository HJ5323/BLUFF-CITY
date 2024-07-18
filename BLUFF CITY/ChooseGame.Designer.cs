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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseGame));
            LIAR_GAME = new Button();
            LogOut = new Button();
            CHECK = new Label();
            SuspendLayout();
            // 
            // LIAR_GAME
            // 
            LIAR_GAME.BackColor = Color.Gainsboro;
            LIAR_GAME.Font = new Font("Pyunji R", 28F, FontStyle.Bold);
            LIAR_GAME.Location = new Point(290, 155);
            LIAR_GAME.Margin = new Padding(3, 4, 3, 4);
            LIAR_GAME.Name = "LIAR_GAME";
            LIAR_GAME.Size = new Size(308, 75);
            LIAR_GAME.TabIndex = 0;
            LIAR_GAME.Text = "LIAR GAME";
            LIAR_GAME.UseVisualStyleBackColor = false;
            LIAR_GAME.Click += LIAR_GAME_Click;
            // 
            // LogOut
            // 
            LogOut.BackColor = Color.Gainsboro;
            LogOut.Font = new Font("Pyunji R", 15F, FontStyle.Bold);
            LogOut.Location = new Point(667, 423);
            LogOut.Margin = new Padding(3, 4, 3, 4);
            LogOut.Name = "LogOut";
            LogOut.Size = new Size(150, 46);
            LogOut.TabIndex = 2;
            LogOut.Text = "LogOut";
            LogOut.UseVisualStyleBackColor = false;
            LogOut.Click += LogOut_Click;
            // 
            // CHECK
            // 
            CHECK.BackColor = Color.Gainsboro;
            CHECK.Font = new Font("Pyunji R", 10F);
            CHECK.Location = new Point(142, 343);
            CHECK.Margin = new Padding(3, 2, 3, 2);
            CHECK.Name = "CHECK";
            CHECK.Size = new Size(584, 31);
            CHECK.TabIndex = 16;
            CHECK.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ChooseGame
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.choose_game;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(884, 551);
            ControlBox = false;
            Controls.Add(CHECK);
            Controls.Add(LogOut);
            Controls.Add(LIAR_GAME);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "ChooseGame";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ChooseGame";
            ResumeLayout(false);
        }

        #endregion

        private Button LIAR_GAME;
        private Button LogOut;

        private Button[] GameButtons; // LIAR_GAME,MAFIA_GAME 버튼 배열

        private void InitializeArrays()
        {
            // GameButtons 배열 초기화
            GameButtons = new Button[] { LIAR_GAME, LogOut };
        }

        private Label CHECK;

    }
}