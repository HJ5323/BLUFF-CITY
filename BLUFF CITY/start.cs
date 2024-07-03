using System;
using System.Windows.Forms;

namespace BLUFF_CITY
{
    public partial class start : Form
    {
        public start()
        {
            InitializeComponent();

            InitializeArrays();

            ApplyTransparentBackgroundAndHideBorder();
        }

        private void LOGIN_Click(object sender, EventArgs e)
        {
            login loginForm = new login();
            loginForm.Show();

            this.Hide();
        }

        private void SIGN_UP_Click(object sender, EventArgs e)
        {
            SignUp SignUpForm = new SignUp();
            SignUpForm.Show();
        }

        private void start_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // StartButtons �迭�� ���� ����� �����ϰ� �����ϰ� �������� ����
            foreach (var button in StartButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // StartTextBox �迭�� ���� ����� �����ϰ� ����
            foreach (var textBox in StartTextBox)
            {
                textBox.BorderStyle = BorderStyle.None; // �ؽ�Ʈ �ڽ� �׵θ� �����
            }
        }
    }
}
