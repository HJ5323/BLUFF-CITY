using System.Windows.Forms;

namespace BLUFF_CITY
{
    partial class tutorial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tutorial));
            next = new Button();
            Previous = new Button();
            SuspendLayout();
            // 
            // next
            // 
            next.BackColor = Color.Transparent;
            next.FlatStyle = FlatStyle.Flat;
            next.Image = Properties.Resources.NEXT;
            next.Location = new Point(828, 412);
            next.Margin = new Padding(3, 2, 3, 2);
            next.Name = "next";
            next.Size = new Size(57, 56);
            next.TabIndex = 32;
            next.UseVisualStyleBackColor = false;
            next.Click += next_Click;
            // 
            // Previous
            // 
            Previous.BackColor = Color.Transparent;
            Previous.FlatStyle = FlatStyle.Flat;
            Previous.Image = Properties.Resources.Previous;
            Previous.Location = new Point(12, 412);
            Previous.Margin = new Padding(3, 2, 3, 2);
            Previous.Name = "Previous";
            Previous.Size = new Size(57, 56);
            Previous.TabIndex = 33;
            Previous.UseVisualStyleBackColor = false;
            Previous.Click += Previous_Click;
            // 
            // tutorial
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(217, 217, 217);
            ClientSize = new Size(906, 924);
            Controls.Add(Previous);
            Controls.Add(next);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "tutorial";
            Text = "tutorial";
            ResumeLayout(false);
        }

        #endregion

        private Button next;
        private PictureBox tutorial_pic;

        private void InitializePictureBox()
        {
            tutorial_pic = new PictureBox();
            tutorial_pic.Size = new Size(747, 894); // 적절한 크기로 설정
            // 폼의 중앙에 맞추기
            int x = (this.ClientSize.Width - tutorial_pic.Width) / 2;
            int y = (this.ClientSize.Height - tutorial_pic.Height) / 2;
            tutorial_pic.Location = new Point(x, y);
            tutorial_pic.BorderStyle = BorderStyle.FixedSingle;
            tutorial_pic.SizeMode = PictureBoxSizeMode.Zoom;
            Controls.Add(tutorial_pic);
        }

        private Button Previous;
    }
}