using System.Net.Sockets;
using System.Xml.Linq;

namespace BLUFF_CITY
{
    partial class login
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
            CHECK = new TextBox();
            PW = new TextBox();
            ID = new TextBox();
            login_pw = new TextBox();
            login_id = new TextBox();
            Login_ok = new Button();
            SuspendLayout();
            // 
            // CHECK
            // 
            CHECK.BackColor = Color.Gainsboro;
            CHECK.Font = new Font("Pyunji R", 10F);
            CHECK.Location = new Point(213, 332);
            CHECK.Name = "CHECK";
            CHECK.Size = new Size(258, 31);
            CHECK.TabIndex = 15;
            CHECK.Text = "잘못입력";
            // 
            // PW
            // 
            PW.BackColor = Color.Gainsboro;
            PW.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            PW.Location = new Point(209, 205);
            PW.Name = "PW";
            PW.ReadOnly = true;
            PW.Size = new Size(78, 35);
            PW.TabIndex = 13;
            PW.Text = "PW";
            // 
            // ID
            // 
            ID.BackColor = Color.Gainsboro;
            ID.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            ID.Location = new Point(209, 138);
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Size = new Size(78, 35);
            ID.TabIndex = 12;
            ID.Text = "ID";
            // 
            // login_pw
            // 
            login_pw.Font = new Font("Pyunji R", 10F);
            login_pw.Location = new Point(321, 205);
            login_pw.Name = "login_pw";
            login_pw.Size = new Size(150, 31);
            login_pw.TabIndex = 10;
            login_pw.Text = "비밀번호";
            // 
            // login_id
            // 
            login_id.Font = new Font("Pyunji R", 10F, FontStyle.Regular, GraphicsUnit.Point, 129);
            login_id.Location = new Point(321, 138);
            login_id.Name = "login_id";
            login_id.Size = new Size(150, 31);
            login_id.TabIndex = 9;
            login_id.Text = "아이디";
            // 
            // Login_ok
            // 
            Login_ok.BackColor = Color.Transparent;
            Login_ok.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            Login_ok.Location = new Point(495, 324);
            Login_ok.Name = "Login_ok";
            Login_ok.Size = new Size(147, 42);
            Login_ok.TabIndex = 8;
            Login_ok.Text = "Login";
            Login_ok.UseVisualStyleBackColor = false;
            Login_ok.Click += Login_ok_Click;
            // 
            // login
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(800, 450);
            Controls.Add(CHECK);
            Controls.Add(PW);
            Controls.Add(ID);
            Controls.Add(login_pw);
            Controls.Add(login_id);
            Controls.Add(Login_ok);
            Name = "login";
            Text = "LOGIN";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox login_id;
        private TextBox login_pw;
        private TextBox CHECK;
        private TextBox PW;
        private TextBox ID;
        private Button Login_ok;

        private Button[] LoginButtons;
        private TextBox[] LoginTextBox;

        private const int MaxSize = 1024;
        private static Socket clientSocket;
        private static string loginid;

        private void InitializeArrays()
        {
            // LoginButtons 배열 초기화
            LoginButtons = new Button[] { Login_ok };

            // LoginTextBox 배열 초기화
            LoginTextBox = new TextBox[] { login_id, login_pw, CHECK, ID, PW};
        }
    }
}