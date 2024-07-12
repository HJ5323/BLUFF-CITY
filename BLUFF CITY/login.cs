namespace BLUFF_CITY
{

    public partial class Login : Form
    {

        private Network network;
        private bool loginSuccessful = false;
        private string receivedNickname;
        public Login()
        {
            InitializeComponent();

            InitializeArrays();

            ApplyTransparentBackgroundAndHideBorder();

            network = Network.Instance;

            network.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(string message)
        {
            if (message.StartsWith("login_success"))
            {
                receivedNickname = message.Split(':')[1];
                loginSuccessful = true;
            }
            else if (message.StartsWith("login_failure"))
            {
                CHECK.Text = "ID와 PW를 확인해 주세요.";
            }
        }

        private async void Login_ok_Click(object sender, EventArgs e)
        {
            network.SendLoginInfo(login_id.Text, login_pw.Text);
            await WaitForLoginResponse();
        }

        private async Task WaitForLoginResponse()
        {
            while (!loginSuccessful)
            {
                await Task.Delay(100);
            }

            if (loginSuccessful)
            {
                ChooseGame chooseGameForm = new ChooseGame(login_id.Text, receivedNickname);
                chooseGameForm.Show();
                this.Hide();
            }
        }


        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // LoginButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in LoginButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // LoginTextBox 배열에 대해 배경을 투명하게 설정
            foreach (var textBox in LoginTextBox)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }
        }
    }
}
