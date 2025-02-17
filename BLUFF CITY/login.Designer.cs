﻿namespace BLUFF_CITY
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            CHECK = new Label();
            PW = new Label();
            ID = new Label();
            login_pw = new TextBox();
            login_id = new TextBox();
            Login_ok = new Button();
            exit = new Button();
            SuspendLayout();
            // 
            // CHECK
            // 
            CHECK.BackColor = Color.Gainsboro;
            CHECK.Font = new Font("Pyunji R", 10F);
            CHECK.Location = new Point(213, 332);
            CHECK.Margin = new Padding(3, 2, 3, 2);
            CHECK.Name = "CHECK";
            CHECK.Size = new Size(258, 31);
            CHECK.TabIndex = 15;
            CHECK.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PW
            // 
            PW.BackColor = Color.Gainsboro;
            PW.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            PW.Location = new Point(209, 205);
            PW.Margin = new Padding(3, 2, 3, 2);
            PW.Name = "PW";
            PW.Size = new Size(78, 35);
            PW.TabIndex = 13;
            PW.Text = "PW";
            PW.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ID
            // 
            ID.BackColor = Color.Gainsboro;
            ID.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            ID.Location = new Point(209, 138);
            ID.Margin = new Padding(3, 2, 3, 2);
            ID.Name = "ID";
            ID.Size = new Size(78, 35);
            ID.TabIndex = 12;
            ID.Text = "ID";
            ID.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // login_pw
            // 
            login_pw.Font = new Font("Pyunji R", 10F);
            login_pw.ImeMode = ImeMode.NoControl;
            login_pw.Location = new Point(321, 205);
            login_pw.Margin = new Padding(3, 2, 3, 2);
            login_pw.Name = "login_pw";
            login_pw.PasswordChar = '*';
            login_pw.PlaceholderText = "한글 입력 불가";
            login_pw.Size = new Size(150, 31);
            login_pw.TabIndex = 10;
            login_pw.KeyDown += loginPW_KeyDown;
            // 
            // login_id
            // 
            login_id.Font = new Font("Pyunji R", 10F, FontStyle.Regular, GraphicsUnit.Point, 129);
            login_id.ImeMode = ImeMode.Disable;
            login_id.Location = new Point(321, 138);
            login_id.Margin = new Padding(3, 2, 3, 2);
            login_id.Name = "login_id";
            login_id.PlaceholderText = "한글 입력 불가";
            login_id.Size = new Size(150, 31);
            login_id.TabIndex = 9;
            // 
            // Login_ok
            // 
            Login_ok.BackColor = Color.Transparent;
            Login_ok.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            Login_ok.Location = new Point(495, 324);
            Login_ok.Margin = new Padding(3, 2, 3, 2);
            Login_ok.Name = "Login_ok";
            Login_ok.Size = new Size(147, 42);
            Login_ok.TabIndex = 8;
            Login_ok.Text = "Login";
            Login_ok.UseVisualStyleBackColor = false;
            Login_ok.Click += Login_ok_Click;
            // 
            // exit
            // 
            exit.BackColor = Color.FromArgb(217, 217, 217);
            exit.BackgroundImage = Properties.Resources.icons8_출구_64;
            exit.BackgroundImageLayout = ImageLayout.Zoom;
            exit.FlatStyle = FlatStyle.Flat;
            exit.ForeColor = Color.Black;
            exit.Location = new Point(731, 11);
            exit.Margin = new Padding(3, 2, 3, 2);
            exit.Name = "exit";
            exit.Size = new Size(57, 48);
            exit.TabIndex = 42;
            exit.UseVisualStyleBackColor = false;
            exit.Click += exit_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(exit);
            Controls.Add(CHECK);
            Controls.Add(PW);
            Controls.Add(ID);
            Controls.Add(login_pw);
            Controls.Add(login_id);
            Controls.Add(Login_ok);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LOGIN";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox login_id;
        private TextBox login_pw;
        private Label CHECK;
        private Label PW;
        private Label ID;
        private Button Login_ok;

        private Button[] LoginButtons;
        private TextBox[] LoginTextBox;
        private Label[] LoginLabel;

        private const int MaxSize = 1024;
        //private static Socket clientSocket;
        private static string loginid;

        private void InitializeArrays()
        {
            // LoginButtons 배열 초기화
            LoginButtons = new Button[] { Login_ok, exit };

            // LoginTextBox 배열 초기화
            LoginTextBox = new TextBox[] { login_id, login_pw};

            // LoginLabel 배열 초기화
            LoginLabel = new Label[] { CHECK, ID, PW };
        }

        private Button exit;
    }
}