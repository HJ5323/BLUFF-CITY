namespace BLUFF_CITY
{
    public partial class ChooseGame : Form
    {
        private string playerID;
        private string playerNickname;
        private Network network;

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
        }

        private void MAFIA_GAME_Click(object sender, EventArgs e)
        {
            //mafia mafiaForm = new mafia(playerID, playerNickname);
            //mafiaForm.Show();

            this.Hide();
        }

        private void LIAR_GAME_Click(object sender, EventArgs e)
        {
            liarForm = new Liar(playerID, playerNickname);
            liarForm.Show();

            this.Hide();
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            Start startForm = new Start();
            startForm.Show();
            if (network != null)
            {
                network.cancelTokenSource.Cancel();
                network.Dispose();
            }
            this.Hide();
        }
        private void ChooseGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
