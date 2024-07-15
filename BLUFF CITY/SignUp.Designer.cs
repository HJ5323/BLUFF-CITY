using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System;

namespace BLUFF_CITY
{
    partial class SignUp
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
            Sign_Up = new Button();
            signup_id = new TextBox();
            signup_pw = new TextBox();
            signup_name = new TextBox();
            ID = new TextBox();
            PW = new TextBox();
            NAME = new TextBox();
            CHECK = new TextBox();
            SuspendLayout();
            // 
            // Sign_Up
            // 
            Sign_Up.BackColor = Color.Transparent;
            Sign_Up.Font = new Font("휴먼편지체", 12F, FontStyle.Bold);
            Sign_Up.Location = new Point(494, 276);
            Sign_Up.Margin = new Padding(3, 2, 3, 2);
            Sign_Up.Name = "Sign_Up";
            Sign_Up.Size = new Size(132, 34);
            Sign_Up.TabIndex = 0;
            Sign_Up.Text = "Sign Up";
            Sign_Up.UseVisualStyleBackColor = false;
            Sign_Up.Click += signup_ok_Click;
            // 
            // signup_id
            // 
            signup_id.Font = new Font("휴먼편지체", 10F, FontStyle.Regular, GraphicsUnit.Point, 129);
            signup_id.Location = new Point(292, 85);
            signup_id.Margin = new Padding(3, 2, 3, 2);
            signup_id.Name = "signup_id";
            signup_id.Size = new Size(135, 27);
            signup_id.TabIndex = 1;
            // 
            // signup_pw
            // 
            signup_pw.Font = new Font("휴먼편지체", 10F);
            signup_pw.Location = new Point(292, 138);
            signup_pw.Margin = new Padding(3, 2, 3, 2);
            signup_pw.Name = "signup_pw";
            signup_pw.Size = new Size(135, 27);
            signup_pw.TabIndex = 2;
            // 
            // signup_name
            // 
            signup_name.Font = new Font("휴먼편지체", 10F);
            signup_name.Location = new Point(292, 196);
            signup_name.Margin = new Padding(3, 2, 3, 2);
            signup_name.Name = "signup_name";
            signup_name.Size = new Size(135, 27);
            signup_name.TabIndex = 3;
            signup_name.KeyDown += signupName_KeyDown;
            // 
            // ID
            // 
            ID.BackColor = Color.Gainsboro;
            ID.Font = new Font("휴먼편지체", 12F, FontStyle.Bold);
            ID.Location = new Point(191, 85);
            ID.Margin = new Padding(3, 2, 3, 2);
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Size = new Size(71, 31);
            ID.TabIndex = 4;
            ID.Text = "ID";
            // 
            // PW
            // 
            PW.BackColor = Color.Gainsboro;
            PW.Font = new Font("휴먼편지체", 12F, FontStyle.Bold);
            PW.Location = new Point(191, 138);
            PW.Margin = new Padding(3, 2, 3, 2);
            PW.Name = "PW";
            PW.ReadOnly = true;
            PW.Size = new Size(71, 31);
            PW.TabIndex = 5;
            PW.Text = "PW";
            // 
            // NAME
            // 
            NAME.BackColor = Color.Gainsboro;
            NAME.Font = new Font("휴먼편지체", 12F, FontStyle.Bold);
            NAME.Location = new Point(191, 196);
            NAME.Margin = new Padding(3, 2, 3, 2);
            NAME.Name = "NAME";
            NAME.ReadOnly = true;
            NAME.Size = new Size(71, 31);
            NAME.TabIndex = 6;
            NAME.Text = "NAME";
            // 
            // CHECK
            // 
            CHECK.BackColor = Color.Gainsboro;
            CHECK.Font = new Font("휴먼편지체", 10F);
            CHECK.Location = new Point(240, 282);
            CHECK.Margin = new Padding(3, 2, 3, 2);
            CHECK.Name = "CHECK";
            CHECK.Size = new Size(233, 27);
            CHECK.TabIndex = 7;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(720, 360);
            Controls.Add(CHECK);
            Controls.Add(NAME);
            Controls.Add(PW);
            Controls.Add(ID);
            Controls.Add(signup_name);
            Controls.Add(signup_pw);
            Controls.Add(signup_id);
            Controls.Add(Sign_Up);
            Margin = new Padding(3, 2, 3, 2);
            Name = "SignUp";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Sign_Up;
        private TextBox signup_id;
        private TextBox signup_pw;
        private TextBox signup_name;
        private TextBox ID;
        private TextBox PW;
        private TextBox NAME;
        private TextBox CHECK;

        private Button[] SignUpButtons; 
        private TextBox[] SignUpTextBox;

        private void InitializeArrays()
        {
            // SignUpButtons 배열 초기화
            SignUpButtons = new Button[] { Sign_Up };

            // SignUpTextBox 배열 초기화
            SignUpTextBox = new TextBox[] { signup_id, signup_pw, signup_name, 
                ID, PW, NAME, CHECK};
        }
    }
}