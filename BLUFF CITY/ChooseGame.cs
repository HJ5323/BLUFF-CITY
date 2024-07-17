namespace BLUFF_CITY
{
    public partial class ChooseGame : Form
    {
        private string playerID;
        private string playerNickname;
        private Network network;
        bool ShowLiargameForm;

        Liar liarForm;

        public ChooseGame(string id, string nickname)
        {
            InitializeComponent();

            // 버튼 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();

            playerID = id;
            playerNickname = nickname;
            Console.WriteLine("Choose Game");
            Console.WriteLine(playerID);

            network = Network.Instance;
            network.MessageReceived += OnMessageReceived;
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // maButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in GameButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            CHECK.BorderStyle = BorderStyle.None; // Label 테두리 숨기기
        }

        private void OnMessageReceived(string message)
        {
            // 메시지를 콜론을 기준으로 파싱
            string[] parts = message.Split(':');
            string messageType = parts[0]; // 메시지 타입

            if (messageType == "createRoom")
            {
                string SuccessFailure = parts[1]; // 게임 실행 여부
                string actualMessage = parts[2]; // 메시지 타입

                if (SuccessFailure == "success")
                {
                    CHECK.Text = actualMessage;
                    ShowLiargameForm = true;
                }
                else if (SuccessFailure == "failure")
                {
                    CHECK.Text = actualMessage;
                    ShowLiargameForm = false;
                }
            }
        }

        private async Task WaitForShowLiargameForm()
        {
            while (!ShowLiargameForm)
            {

                await Task.Delay(100);
            }

            if (ShowLiargameForm)
            {
                // ChooseGame 폼이 이미 열려 있는지 확인
                if (liarForm == null || liarForm.IsDisposed)
                {
                    liarForm = new Liar(playerID, playerNickname);
                    liarForm.Show();
                    this.Close();
                    ShowLiargameForm = false;
                }
                else
                {
                    // 폼이 이미 열려 있는 경우 포커스를 맞춤
                    liarForm.Focus();
                }
            }
        }

        private async void LIAR_GAME_Click(object sender, EventArgs e)
        {
            ShowLiargameForm = false;
            network.CreatRoom(playerID, playerNickname);
            await WaitForShowLiargameForm();
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            network.Sendlogout(playerID, playerNickname);
            Start startForm = new Start();
            startForm.Show();
            if (network != null)
            {
                network.cancelTokenSource.Cancel();
                network.Dispose();
            }
            this.Close();
        }

        private void ChooseGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            network.MessageReceived -= OnMessageReceived;
            Application.Exit();
        }

    }
}