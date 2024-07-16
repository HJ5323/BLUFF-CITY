namespace BLUFF_CITY
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();

            // Form ũ�� ����
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            InitializeArrays();

            ApplyTransparentBackgroundAndHideBorder();
        }

        private void LOGIN_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();

            this.Hide();
        }

        private void SIGN_UP_Click(object sender, EventArgs e)
        {
            SignUp SignUpForm = new SignUp();
            SignUpForm.Show();

            this.Hide();
        }

        private void start_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
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

        private void exit_Click(object sender, EventArgs e)
        {
  
        }
    }
}
