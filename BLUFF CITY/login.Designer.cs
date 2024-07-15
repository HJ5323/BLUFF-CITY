using System.Net.Sockets;
using System.Xml.Linq;

namespace BLUFF_CITY
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
            CHECK = new Label();
            PW = new Label();
            ID = new Label();
            login_pw = new TextBox();
            login_id = new TextBox();
            Login_ok = new Button();
            SuspendLayout();
            // 
            // CHECK
            // 
            CHECK.BackColor = Color.Gainsboro;
            CHECK.Font = new Font("휴먼편지체", 10F);
            CHECK.Location = new Point(192, 266);
            CHECK.Margin = new Padding(3, 2, 3, 2);
            CHECK.Name = "CHECK";
            CHECK.Size = new Size(233, 27);
            CHECK.TabIndex = 15;
            CHECK.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PW
            // 
            PW.BackColor = Color.Gainsboro;
            PW.Font = new Font("휴먼편지체", 12F, FontStyle.Bold);
            PW.Location = new Point(188, 164);
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
            ID.Font = new Font("휴먼편지체", 12F, FontStyle.Bold);
            ID.Location = new Point(188, 110);
            ID.Margin = new Padding(3, 2, 3, 2);
            ID.Name = "ID";
            ID.Size = new Size(78, 35);
            ID.TabIndex = 12;
            ID.Text = "ID";
            ID.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // login_pw
            // 
            login_pw.Font = new Font("휴먼편지체", 10F);
            login_pw.Location = new Point(289, 164);
            login_pw.Margin = new Padding(3, 2, 3, 2);
            login_pw.Name = "login_pw";
            login_pw.PasswordChar = '*';
            login_pw.Size = new Size(135, 27);
            login_pw.TabIndex = 10;
            login_pw.KeyDown += loginPW_KeyDown;
            // 
            // login_id
            // 
            login_id.Font = new Font("휴먼편지체", 10F, FontStyle.Regular, GraphicsUnit.Point, 129);
            login_id.Location = new Point(289, 110);
            login_id.Margin = new Padding(3, 2, 3, 2);
            login_id.Name = "login_id";
            login_id.Size = new Size(135, 27);
            login_id.TabIndex = 9;
            // 
            // Login_ok
            // 
            Login_ok.BackColor = Color.Transparent;
            Login_ok.Font = new Font("휴먼편지체", 12F, FontStyle.Bold);
            Login_ok.Location = new Point(446, 259);
            Login_ok.Margin = new Padding(3, 2, 3, 2);
            Login_ok.Name = "Login_ok";
            Login_ok.Size = new Size(132, 34);
            Login_ok.TabIndex = 8;
            Login_ok.Text = "Login";
            Login_ok.UseVisualStyleBackColor = false;
            Login_ok.Click += Login_ok_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(720, 360);
            Controls.Add(CHECK);
            Controls.Add(PW);
            Controls.Add(ID);
            Controls.Add(login_pw);
            Controls.Add(login_id);
            Controls.Add(Login_ok);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Login";
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
            LoginButtons = new Button[] { Login_ok };

            // LoginTextBox 배열 초기화
            LoginTextBox = new TextBox[] { login_id, login_pw};

            // LoginLabel 배열 초기화
            LoginLabel = new Label[] { CHECK, ID, PW };
        }
    }
}