using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BLUFF_CITY
{
    partial class liar
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
            chat = new TextBox();
            players_chat = new TextBox();
            word = new TextBox();
            READY = new Button();
            exit = new Button();
            time = new TextBox();
            LIAR_GAME = new TextBox();
            li_p8_name = new TextBox();
            li_p8 = new Button();
            li_p7_name = new TextBox();
            li_p7 = new Button();
            li_p6_name = new TextBox();
            li_p6 = new Button();
            li_p5_name = new TextBox();
            li_p5 = new Button();
            li_p4_name = new TextBox();
            li_p4 = new Button();
            li_p3_name = new TextBox();
            li_p3 = new Button();
            li_p2_name = new TextBox();
            li_p2 = new Button();
            li_p1_name = new TextBox();
            li_p1 = new Button();
            category = new TextBox();
            login_name = new TextBox();
            SuspendLayout();
            // 
            // chat
            // 
            chat.BackColor = Color.FromArgb(217, 217, 217);
            chat.Font = new Font("Pyunji R", 10F, FontStyle.Bold);
            chat.Location = new Point(250, 1006);
            chat.Name = "chat";
            chat.Size = new Size(500, 31);
            chat.TabIndex = 45;
            chat.Click += chat_KeyDown;
            // 
            // players_chat
            // 
            players_chat.BackColor = Color.FromArgb(217, 217, 217);
            players_chat.Font = new Font("Pyunji R", 10F, FontStyle.Bold);
            players_chat.Location = new Point(250, 128);
            players_chat.Multiline = true;
            players_chat.Name = "players_chat";
            players_chat.ScrollBars = ScrollBars.Horizontal;
            players_chat.Size = new Size(500, 855);
            players_chat.TabIndex = 44;
            players_chat.TextAlign = HorizontalAlignment.Center;
            // 
            // word
            // 
            word.BackColor = Color.FromArgb(217, 217, 217);
            word.BorderStyle = BorderStyle.None;
            word.Font = new Font("Pyunji R", 15F, FontStyle.Bold);
            word.Location = new Point(400, 18);
            word.Name = "word";
            word.Size = new Size(195, 35);
            word.TabIndex = 43;
            word.Text = "레몬";
            word.TextAlign = HorizontalAlignment.Center;
            // 
            // READY
            // 
            READY.BackColor = Color.FromArgb(217, 217, 217);
            READY.BackgroundImageLayout = ImageLayout.None;
            READY.FlatStyle = FlatStyle.Flat;
            READY.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            READY.ForeColor = Color.Black;
            READY.Location = new Point(777, 1003);
            READY.Name = "READY";
            READY.Size = new Size(160, 51);
            READY.TabIndex = 42;
            READY.Text = "READY";
            READY.UseVisualStyleBackColor = false;
            READY.Click += READY_Click;
            // 
            // exit
            // 
            exit.BackColor = Color.FromArgb(217, 217, 217);
            exit.BackgroundImage = Properties.Resources.icons8_출구_64;
            exit.BackgroundImageLayout = ImageLayout.Zoom;
            exit.FlatStyle = FlatStyle.Flat;
            exit.ForeColor = Color.Black;
            exit.Location = new Point(910, 12);
            exit.Name = "exit";
            exit.Size = new Size(57, 48);
            exit.TabIndex = 41;
            exit.UseVisualStyleBackColor = false;
            exit.Click += exit_Click;
            // 
            // time
            // 
            time.BackColor = Color.FromArgb(217, 217, 217);
            time.BorderStyle = BorderStyle.None;
            time.Font = new Font("Pyunji R", 14F, FontStyle.Bold, GraphicsUnit.Point, 129);
            time.Location = new Point(781, 19);
            time.Name = "time";
            time.Size = new Size(67, 33);
            time.TabIndex = 40;
            time.Text = "60 S";
            // 
            // LIAR_GAME
            // 
            LIAR_GAME.BackColor = Color.FromArgb(217, 217, 217);
            LIAR_GAME.BorderStyle = BorderStyle.None;
            LIAR_GAME.Font = new Font("Pyunji R", 18F, FontStyle.Bold, GraphicsUnit.Point, 129);
            LIAR_GAME.ForeColor = SystemColors.WindowText;
            LIAR_GAME.Location = new Point(21, 14);
            LIAR_GAME.Name = "LIAR_GAME";
            LIAR_GAME.Size = new Size(194, 42);
            LIAR_GAME.TabIndex = 39;
            LIAR_GAME.Text = "LIAR GAME";
            // 
            // li_p8_name
            // 
            li_p8_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            li_p8_name.Location = new Point(766, 954);
            li_p8_name.Name = "li_p8_name";
            li_p8_name.Size = new Size(172, 35);
            li_p8_name.TabIndex = 38;
            li_p8_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p8
            // 
            li_p8.BackColor = Color.Transparent;
            li_p8.FlatStyle = FlatStyle.Flat;
            li_p8.ForeColor = Color.Black;
            li_p8.Location = new Point(765, 776);
            li_p8.Name = "li_p8";
            li_p8.Size = new Size(174, 174);
            li_p8.TabIndex = 37;
            li_p8.UseVisualStyleBackColor = false;
            li_p8.Click += li_p8_Click;
            // 
            // li_p7_name
            // 
            li_p7_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            li_p7_name.Location = new Point(766, 738);
            li_p7_name.Name = "li_p7_name";
            li_p7_name.Size = new Size(172, 35);
            li_p7_name.TabIndex = 36;
            li_p7_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p7
            // 
            li_p7.BackColor = Color.Transparent;
            li_p7.FlatStyle = FlatStyle.Flat;
            li_p7.ForeColor = Color.Black;
            li_p7.Location = new Point(765, 560);
            li_p7.Name = "li_p7";
            li_p7.Size = new Size(174, 174);
            li_p7.TabIndex = 35;
            li_p7.UseVisualStyleBackColor = false;
            li_p7.Click += li_p7_Click;
            // 
            // li_p6_name
            // 
            li_p6_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            li_p6_name.Location = new Point(766, 522);
            li_p6_name.Name = "li_p6_name";
            li_p6_name.Size = new Size(172, 35);
            li_p6_name.TabIndex = 34;
            li_p6_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p6
            // 
            li_p6.BackColor = Color.Transparent;
            li_p6.FlatStyle = FlatStyle.Flat;
            li_p6.ForeColor = Color.Black;
            li_p6.Location = new Point(765, 344);
            li_p6.Name = "li_p6";
            li_p6.Size = new Size(174, 174);
            li_p6.TabIndex = 33;
            li_p6.UseVisualStyleBackColor = false;
            li_p6.Click += li_p6_Click;
            // 
            // li_p5_name
            // 
            li_p5_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            li_p5_name.Location = new Point(766, 306);
            li_p5_name.Name = "li_p5_name";
            li_p5_name.Size = new Size(172, 35);
            li_p5_name.TabIndex = 32;
            li_p5_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p5
            // 
            li_p5.BackColor = Color.Transparent;
            li_p5.FlatStyle = FlatStyle.Flat;
            li_p5.Location = new Point(765, 128);
            li_p5.Name = "li_p5";
            li_p5.Size = new Size(174, 174);
            li_p5.TabIndex = 31;
            li_p5.UseVisualStyleBackColor = false;
            li_p5.Click += li_p5_Click;
            // 
            // li_p4_name
            // 
            li_p4_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            li_p4_name.Location = new Point(62, 954);
            li_p4_name.Name = "li_p4_name";
            li_p4_name.Size = new Size(172, 35);
            li_p4_name.TabIndex = 30;
            li_p4_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p4
            // 
            li_p4.BackColor = Color.Transparent;
            li_p4.FlatStyle = FlatStyle.Flat;
            li_p4.ForeColor = Color.Black;
            li_p4.Location = new Point(61, 776);
            li_p4.Name = "li_p4";
            li_p4.Size = new Size(174, 174);
            li_p4.TabIndex = 29;
            li_p4.UseVisualStyleBackColor = false;
            li_p4.Click += li_p4_Click;
            // 
            // li_p3_name
            // 
            li_p3_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            li_p3_name.Location = new Point(62, 738);
            li_p3_name.Name = "li_p3_name";
            li_p3_name.Size = new Size(172, 35);
            li_p3_name.TabIndex = 28;
            li_p3_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p3
            // 
            li_p3.BackColor = Color.Transparent;
            li_p3.FlatStyle = FlatStyle.Flat;
            li_p3.ForeColor = Color.Black;
            li_p3.Location = new Point(61, 560);
            li_p3.Name = "li_p3";
            li_p3.Size = new Size(174, 174);
            li_p3.TabIndex = 27;
            li_p3.UseVisualStyleBackColor = false;
            li_p3.Click += li_p3_Click;
            // 
            // li_p2_name
            // 
            li_p2_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            li_p2_name.Location = new Point(62, 522);
            li_p2_name.Name = "li_p2_name";
            li_p2_name.Size = new Size(172, 35);
            li_p2_name.TabIndex = 26;
            li_p2_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p2
            // 
            li_p2.BackColor = Color.Transparent;
            li_p2.FlatStyle = FlatStyle.Flat;
            li_p2.ForeColor = Color.Black;
            li_p2.Location = new Point(61, 344);
            li_p2.Name = "li_p2";
            li_p2.Size = new Size(174, 174);
            li_p2.TabIndex = 25;
            li_p2.UseVisualStyleBackColor = false;
            li_p2.Click += li_p2_Click;
            // 
            // li_p1_name
            // 
            li_p1_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            li_p1_name.Location = new Point(62, 306);
            li_p1_name.Name = "li_p1_name";
            li_p1_name.Size = new Size(172, 35);
            li_p1_name.TabIndex = 24;
            li_p1_name.TextAlign = HorizontalAlignment.Center;
            // 
            // li_p1
            // 
            li_p1.BackColor = Color.Transparent;
            li_p1.FlatStyle = FlatStyle.Flat;
            li_p1.ForeColor = Color.Black;
            li_p1.Location = new Point(61, 128);
            li_p1.Name = "li_p1";
            li_p1.Size = new Size(174, 174);
            li_p1.TabIndex = 23;
            li_p1.UseVisualStyleBackColor = false;
            li_p1.Click += li_p1_Click;
            // 
            // category
            // 
            category.BackColor = Color.FromArgb(217, 217, 217);
            category.BorderStyle = BorderStyle.None;
            category.Font = new Font("Pyunji R", 15F, FontStyle.Bold);
            category.Location = new Point(221, 17);
            category.Name = "category";
            category.Size = new Size(128, 35);
            category.TabIndex = 46;
            category.Text = "과일";
            category.TextAlign = HorizontalAlignment.Center;
            // 
            // login_name
            // 
            login_name.BackColor = Color.FromArgb(217, 217, 217);
            login_name.BorderStyle = BorderStyle.None;
            login_name.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            login_name.Location = new Point(765, 79);
            login_name.Name = "login_name";
            login_name.Size = new Size(219, 28);
            login_name.TabIndex = 47;
            // 
            // liar
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(217, 217, 217);
            BackgroundImage = Properties.Resources.game_background;
            ClientSize = new Size(996, 1119);
            Controls.Add(login_name);
            Controls.Add(category);
            Controls.Add(chat);
            Controls.Add(players_chat);
            Controls.Add(word);
            Controls.Add(READY);
            Controls.Add(exit);
            Controls.Add(time);
            Controls.Add(LIAR_GAME);
            Controls.Add(li_p8_name);
            Controls.Add(li_p8);
            Controls.Add(li_p7_name);
            Controls.Add(li_p7);
            Controls.Add(li_p6_name);
            Controls.Add(li_p6);
            Controls.Add(li_p5_name);
            Controls.Add(li_p5);
            Controls.Add(li_p4_name);
            Controls.Add(li_p4);
            Controls.Add(li_p3_name);
            Controls.Add(li_p3);
            Controls.Add(li_p2_name);
            Controls.Add(li_p2);
            Controls.Add(li_p1_name);
            Controls.Add(li_p1);
            Name = "liar";
            Text = "liar";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox chat;
        private TextBox players_chat;
        private TextBox word;
        private Button READY;
        private Button exit;
        private TextBox time;
        private TextBox LIAR_GAME;
        private TextBox li_p8_name;
        private Button li_p8;
        private TextBox li_p7_name;
        private Button li_p7;
        private TextBox li_p6_name;
        private Button li_p6;
        private TextBox li_p5_name;
        private Button li_p5;
        private TextBox li_p4_name;
        private Button li_p4;
        private TextBox li_p3_name;
        private Button li_p3;
        private TextBox li_p2_name;
        private Button li_p2;
        private TextBox li_p1_name;
        private Button li_p1;
        private TextBox category;

        private Button[] LiarButtons; // li_p1부터 li_p8까지 버튼 배열
        private TextBox[] LiarNames; // li_p1_name부터 li_p18_name까지 텍스트 박스 배열
        private Button[] LiarOtherButtons; // READY, exit 버튼 배열
        private TextBox[] LiarOtherTextBox; // time, MAFIA_GAME, roll, players_chat, chat 텍스트 박스　배열
        private PictureBox[] Liar_player_images = new PictureBox[8]; // li_p1_image부터 li_p8_image까지 픽처박스 배열

        // 생성자나 초기화 메서드에서 호출하여 PictureBox 초기화
        private void InitializePictureBoxes()
        {
            string[] animals = { "otter", "bear", "dog", "elephant", "ferret", "killer_whale", "raccoon", "tiger" };
            int startX = 61;
            int startY = 128;
            int spacingY = 216;
            int columnX1 = startX;
            int columnX2 = 765;

            for (int i = 0; i < Liar_player_images.Length; i++)
            {
                Liar_player_images[i] = new PictureBox();
                Liar_player_images[i].BackColor = Color.Transparent;
                Liar_player_images[i].BackgroundImage = Properties.Resources.ResourceManager.GetObject(animals[i]) as Image;
                Liar_player_images[i].BackgroundImageLayout = ImageLayout.Center;
                Liar_player_images[i].ForeColor = Color.Black;
                Liar_player_images[i].Location = new Point(i < 4 ? columnX1 : columnX2, startY + (i % 4) * spacingY);
                Liar_player_images[i].Name = $"li_p{i + 1}_image";
                Liar_player_images[i].Size = new Size(174, 174);
                Liar_player_images[i].TabIndex = 47;

                this.Controls.Add(Liar_player_images[i]);
            }
        }

        private void InitializeArrays()
        {
            // LiarButtons 배열 초기화
            LiarButtons = new Button[] { li_p1, li_p2, li_p3, li_p4, li_p5, li_p6, li_p7, li_p8 };

            // LiarNames 배열 초기화
            LiarNames = new TextBox[] { li_p1_name, li_p2_name, li_p3_name, li_p4_name,
                            li_p5_name, li_p6_name, li_p7_name, li_p8_name };

            // LiarOtherButtons 배열 초기화
            LiarOtherButtons = new Button[] { READY, exit };

            // LiarOtherTextBox 배열 초기화
            LiarOtherTextBox = new TextBox[] { time, LIAR_GAME, word, category, players_chat, chat };
        }

        private TextBox login_name;
    }
}