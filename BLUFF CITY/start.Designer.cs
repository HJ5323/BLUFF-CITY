﻿using System.Xml.Linq;

namespace BLUFF_CITY
{
    partial class Start
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Start));
            LOGIN = new Button();
            SIGN_UP = new Button();
            title = new TextBox();
            SuspendLayout();
            // 
            // LOGIN
            // 
            LOGIN.BackColor = Color.Gainsboro;
            LOGIN.Font = new Font("Pyunji R", 25F, FontStyle.Bold);
            LOGIN.Location = new Point(166, 346);
            LOGIN.Margin = new Padding(3, 2, 3, 2);
            LOGIN.Name = "LOGIN";
            LOGIN.Size = new Size(203, 80);
            LOGIN.TabIndex = 0;
            LOGIN.Text = "LOGIN";
            LOGIN.UseVisualStyleBackColor = false;
            LOGIN.Click += LOGIN_Click;
            // 
            // SIGN_UP
            // 
            SIGN_UP.BackColor = Color.Gainsboro;
            SIGN_UP.Font = new Font("Pyunji R", 25F, FontStyle.Bold);
            SIGN_UP.Location = new Point(568, 346);
            SIGN_UP.Margin = new Padding(3, 2, 3, 2);
            SIGN_UP.Name = "SIGN_UP";
            SIGN_UP.Size = new Size(224, 80);
            SIGN_UP.TabIndex = 0;
            SIGN_UP.Text = "SIGN UP";
            SIGN_UP.UseVisualStyleBackColor = false;
            SIGN_UP.Click += SIGN_UP_Click;
            // 
            // title
            // 
            title.BackColor = Color.Gainsboro;
            title.Font = new Font("Pyunji R", 30F, FontStyle.Bold);
            title.Location = new Point(314, 109);
            title.Margin = new Padding(3, 2, 3, 2);
            title.Name = "title";
            title.Size = new Size(320, 77);
            title.TabIndex = 1;
            title.Text = "BLUFF CITY";
            // 
            // Start
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImage = Properties.Resources.start;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(940, 551);
            Controls.Add(title);
            Controls.Add(SIGN_UP);
            Controls.Add(LOGIN);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Start";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BLUFF CITY";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LOGIN;
        private Button SIGN_UP;
        private TextBox title;

        private Button[] StartButtons;

        private void InitializeArrays()
        {
            // StartButtons 배열 초기화
            StartButtons = new Button[] { LOGIN, SIGN_UP };
        }
    }
}
