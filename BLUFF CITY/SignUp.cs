namespace BLUFF_CITY
{
    public partial class SignUp : Form
    {
        private Network network;
        private bool signupSuccessful = false;
        private Start startForm = null;
        public SignUp()
        {
            InitializeComponent();

            // 버튼 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();

            Network.b_newInstance = true;
            network = Network.Instance;//.Instance;
            Network.b_newInstance = false;

            network.MessageReceived += OnMessageReceived;
        }

        private async void signup_ok_Click(object sender, EventArgs e)
        {
            signupSuccessful = false;

            network.SendSignupInfo(signup_id.Text, signup_pw.Text, signup_name.Text);
            signup_id.Text = "";
            signup_pw.Text = "";
            signup_name.Text = "";
            await WaitForSignupResponse();

        }

        private async Task WaitForSignupResponse()
        {
            while (!signupSuccessful)
            {
                await Task.Delay(100);
            }

            if (signupSuccessful)
            {
                // ChooseGame 폼이 이미 열려 있는지 확인
                if (startForm == null || startForm.IsDisposed)
                {
                    startForm = new Start();
                    startForm.Show();
                    this.Close();
                }
                else
                {
                    // 폼이 이미 열려 있는 경우 포커스를 맞춤
                    startForm.Focus();
                }
            }
        }


        // chat 텍스트 박스 입력 메시지 전송
        private async void signupName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                signupSuccessful = false;

                network.SendSignupInfo(signup_id.Text, signup_pw.Text, signup_name.Text);
                await WaitForSignupResponse();
            }

        }

        private void OnMessageReceived(string message)
        {
            Console.WriteLine(message);
            if (message == "0")
            {
                CHECK.Text = "회원가입을 성공하였습니다.";
                signupSuccessful = true;
            }
            else if (message == "1")
            {
                CHECK.Text = "중복된 아이디입니다.";
            }
            else if (message == "2")
            {
                CHECK.Text = "중복된 닉네임입니다.";
            }

        }



        private void SignUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            network.MessageReceived -= OnMessageReceived;
            Application.Exit();
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // SignUpButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in SignUpButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // SignUpTextBox 배열에 대해 배경을 투명하게 설정
            foreach (var textBox in SignUpTextBox)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }

            // SignUpLabel 배열에 대해 배경을 투명하게 설정
            foreach (var Label in SignUpLabel)
            {
                Label.BorderStyle = BorderStyle.None; // Label 테두리 숨기기
            }
        }
    }
}
