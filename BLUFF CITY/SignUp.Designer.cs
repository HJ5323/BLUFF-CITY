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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUp));
            Sign_Up = new Button();
            signup_id = new TextBox();
            signup_pw = new TextBox();
            signup_name = new TextBox();
            ID = new Label();
            PW = new Label();
            NAME = new Label();
            CHECK = new Label();
            SuspendLayout();
            // 
            // Sign_Up
            // 
            Sign_Up.BackColor = Color.Transparent;
            Sign_Up.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            Sign_Up.Location = new Point(549, 345);
            Sign_Up.Margin = new Padding(3, 2, 3, 2);
            Sign_Up.Name = "Sign_Up";
            Sign_Up.Size = new Size(147, 42);
            Sign_Up.TabIndex = 0;
            Sign_Up.Text = "Sign Up";
            Sign_Up.UseVisualStyleBackColor = false;
            Sign_Up.Click += signup_ok_Click;
            // 
            // signup_id
            // 
            signup_id.Font = new Font("Pyunji R", 10F, FontStyle.Regular, GraphicsUnit.Point, 129);
            signup_id.ImeMode = ImeMode.Disable;
            signup_id.Location = new Point(324, 106);
            signup_id.Margin = new Padding(3, 2, 3, 2);
            signup_id.Name = "signup_id";
            signup_id.PlaceholderText = "한글 입력 불가";
            signup_id.Size = new Size(201, 31);
            signup_id.TabIndex = 1;
            // 
            // signup_pw
            // 
            signup_pw.Font = new Font("Pyunji R", 10F);
            signup_pw.ImeMode = ImeMode.Disable;
            signup_pw.Location = new Point(324, 173);
            signup_pw.Margin = new Padding(3, 2, 3, 2);
            signup_pw.Name = "signup_pw";
            signup_pw.PlaceholderText = "한글 입력 불가";
            signup_pw.Size = new Size(201, 31);
            signup_pw.TabIndex = 2;
            // 
            // signup_name
            // 
            signup_name.Font = new Font("Pyunji R", 10F);
            signup_name.Location = new Point(324, 245);
            signup_name.Margin = new Padding(3, 2, 3, 2);
            signup_name.Name = "signup_name";
            signup_name.Size = new Size(201, 31);
            signup_name.TabIndex = 3;
            signup_name.KeyDown += signupName_KeyDown;
            // 
            // ID
            // 
            ID.BackColor = Color.Gainsboro;
            ID.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            ID.Location = new Point(212, 106);
            ID.Margin = new Padding(3, 2, 3, 2);
            ID.Name = "ID";
            ID.Size = new Size(78, 35);
            ID.TabIndex = 4;
            ID.Text = "ID";
            ID.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PW
            // 
            PW.BackColor = Color.Gainsboro;
            PW.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            PW.Location = new Point(212, 173);
            PW.Margin = new Padding(3, 2, 3, 2);
            PW.Name = "PW";
            PW.Size = new Size(78, 35);
            PW.TabIndex = 5;
            PW.Text = "PW";
            PW.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // NAME
            // 
            NAME.BackColor = Color.Gainsboro;
            NAME.Font = new Font("Pyunji R", 12F, FontStyle.Bold);
            NAME.Location = new Point(212, 245);
            NAME.Margin = new Padding(3, 2, 3, 2);
            NAME.Name = "NAME";
            NAME.Size = new Size(78, 35);
            NAME.TabIndex = 6;
            NAME.Text = "NAME";
            NAME.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // CHECK
            // 
            CHECK.BackColor = Color.Gainsboro;
            CHECK.Font = new Font("Pyunji R", 10F);
            CHECK.Location = new Point(267, 353);
            CHECK.Margin = new Padding(3, 2, 3, 2);
            CHECK.Name = "CHECK";
            CHECK.Size = new Size(258, 31);
            CHECK.TabIndex = 7;
            CHECK.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gainsboro;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(CHECK);
            Controls.Add(NAME);
            Controls.Add(PW);
            Controls.Add(ID);
            Controls.Add(signup_name);
            Controls.Add(signup_pw);
            Controls.Add(signup_id);
            Controls.Add(Sign_Up);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "SignUp";
            Text = "Sign Up";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Sign_Up;
        private TextBox signup_id;
        private TextBox signup_pw;
        private TextBox signup_name;
        private Label ID;
        private Label PW;
        private Label NAME;
        private Label CHECK;

        private Button[] SignUpButtons; 
        private TextBox[] SignUpTextBox;
        private Label[] SignUpLabel;

        private void InitializeArrays()
        {
            // SignUpButtons 배열 초기화
            SignUpButtons = new Button[] { Sign_Up };

            // SignUpTextBox 배열 초기화
            SignUpTextBox = new TextBox[] { signup_id, signup_pw, signup_name};

            // SignUpLabel 배열 초기화
            SignUpLabel = new Label[] { ID, PW, NAME, CHECK};
        }
    }
}