namespace BLUFF_CITY
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();

            // Form 크기 고정
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
            // StartButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in StartButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // StartTextBox 배열에 대해 배경을 투명하게 설정
            foreach (var textBox in StartTextBox)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
  
        }
    }
}
