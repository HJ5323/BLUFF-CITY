using System.Xml.Linq;

namespace BLUFF_CITY
{
    partial class start
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
            LOGIN = new Button();
            SIGN_UP = new Button();
            title = new TextBox();
            exit = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            SuspendLayout();
            // 
            // LOGIN
            // 
            LOGIN.BackColor = Color.Gainsboro;
            LOGIN.Font = new Font("Pyunji R", 25F, FontStyle.Bold);
            LOGIN.Location = new Point(149, 277);
            LOGIN.Margin = new Padding(3, 2, 3, 2);
            LOGIN.Name = "LOGIN";
            LOGIN.Size = new Size(183, 64);
            LOGIN.TabIndex = 0;
            LOGIN.Text = "LOGIN";
            LOGIN.UseVisualStyleBackColor = false;
            LOGIN.Click += LOGIN_Click;
            // 
            // SIGN_UP
            // 
            SIGN_UP.BackColor = Color.Gainsboro;
            SIGN_UP.Font = new Font("Pyunji R", 25F, FontStyle.Bold);
            SIGN_UP.Location = new Point(511, 277);
            SIGN_UP.Margin = new Padding(3, 2, 3, 2);
            SIGN_UP.Name = "SIGN_UP";
            SIGN_UP.Size = new Size(202, 64);
            SIGN_UP.TabIndex = 0;
            SIGN_UP.Text = "SIGN UP";
            SIGN_UP.UseVisualStyleBackColor = false;
            SIGN_UP.Click += SIGN_UP_Click;
            // 
            // title
            // 
            title.BackColor = Color.Gainsboro;
            title.Font = new Font("Pyunji R", 30F, FontStyle.Bold);
            title.Location = new Point(283, 87);
            title.Margin = new Padding(3, 2, 3, 2);
            title.Name = "title";
            title.Size = new Size(288, 66);
            title.TabIndex = 1;
            title.Text = "BLUFF CITY";
            // 
            // exit
            // 
            exit.Location = new Point(779, 17);
            exit.Margin = new Padding(3, 2, 3, 2);
            exit.Name = "exit";
            exit.Size = new Size(43, 44);
            exit.TabIndex = 2;
            exit.Text = "button1";
            exit.UseVisualStyleBackColor = true;
            exit.Click += exit_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(648, 17);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 3;
            textBox1.Text = "127.0.0.1";
            // 
            // start
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImage = Properties.Resources.start;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(846, 441);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(exit);
            Controls.Add(title);
            Controls.Add(SIGN_UP);
            Controls.Add(LOGIN);
            DoubleBuffered = true;
            Margin = new Padding(3, 2, 3, 2);
            Name = "start";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LOGIN;
        private Button SIGN_UP;
        private TextBox title;

        private Button[] StartButtons;
        private TextBox[] StartTextBox;

        private void InitializeArrays()
        {
            // StartButtons 배열 초기화
            StartButtons = new Button[] { LOGIN, SIGN_UP };

            // StartTextBox 배열 초기화
            StartTextBox = new TextBox[] { title };


        }

        private Button exit;
        private TextBox textBox1;
        private TextBox textBox2;
    }
}
