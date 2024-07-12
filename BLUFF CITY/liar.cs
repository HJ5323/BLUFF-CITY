namespace BLUFF_CITY
{
    public partial class Liar : Form
    {
        //Network network = new Network(1);
        private Network network;
        private string playerID;
        private string playerNickname;
        private List<string> playerInfo;  // 플레이어 정보를 저장할 리스트
        private object obj = new object();

        public Liar(string id, string nickname)
        {
            InitializeComponent();

            // 버튼과 텍스트 박스 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();

            // player 배경 Picture Box 이미지 설정
            InitializePictureBoxes();

            playerInfo = new List<string>(); // 플레이어 정보 리스트 초기화
            playerID = id;
            playerNickname = nickname;
            login_name.Text = playerNickname;
            network = Network.Instance;

            network.Join(playerID, playerNickname);

            network.MessageReceived += DisplayMessage;
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // LiarButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in LiarButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // LiarNames 배열에 대해 배경을 투명하게 설정
            foreach (var textBox in LiarNames)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }

            // LiarOtherButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in LiarOtherButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // LiarOtherTextBox 대해 배경을 투명하게 설정
            foreach (var textBox in LiarOtherTextBox)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            ChooseGame ChooseGameForm = new ChooseGame(playerID, playerNickname);
            ChooseGameForm.Show();

            // 현재 폼 숨김
            this.Hide();
        }
        // 수신된 메시지 폼에 표시
        public void DisplayMessage(string message)
        {
            Console.WriteLine($"{playerNickname}메시지 받음{message}");
            lock (obj)
            {
                // 메시지에 콜론이 포함되어 있는지 확인
                if (message.Contains(":"))
                {
                    // 메시지를 콜론을 기준으로 파싱
                    // mafia_game:ME: aaa
                    string[] parts = message.Split(':');
                    string messageType = parts[0]; // 메시지의 유형

                    // 플레이어 정보 메시지인 경우
                    if (messageType == "player_info")
                    {
                        Console.WriteLine($"{playerNickname}player_info 받음");

                        // parts 배열에서 parts[0]을 제외한 나머지 부분들을 추출
                        string playersMessage = string.Join(":", parts.Skip(1));
                        UpdatePlayerInfo(playersMessage); // 플레이어 정보를 업데이트
                    }
                    //// liar_game 채팅 메시지인 경우
                    //if (messageType == "liar_game" && parts.Length == 3)
                    //{
                    // 채팅 메시지를 다시 콜론을 기준으로 파싱
                    //string[] chatParts = chatMessage.Split(new[] { ':' }, 2);
                    //if (chatParts.Length < 2)
                    //{
                    //    Console.WriteLine("잘못된 채팅 메시지 형식");
                    //    return;
                    //}

                    else if (messageType == "topic_keyword")
                    {
                        string topic = parts[1]; // 주제
                        string keyword = parts[2]; // 키워드
                        Console.WriteLine($"{playerNickname}{topic} 받음");
                        Console.WriteLine($"{playerNickname}{keyword} 받음");

                        // UI 스레드에서 텍스트 박스에 주제와 키워드를 표시
                        category.Invoke(new Action(() =>
                        {
                            category.Text = topic;
                        }));
                        word.Invoke(new Action(() =>
                        {
                            word.Text = keyword;
                        }));
                        return;
                    }

                    else if (messageType == "chat")
                    {
                        Console.WriteLine($"{playerNickname}chat 받음");

                        string nickname = parts[1]; // 닉네임
                        string actualMessage = parts[2]; // 실제 메시지 내용

                        // UI 스레드에서 players_chat에 메시지를 추가
                        players_chat.Invoke(new Action(() =>
                        {
                            players_chat.AppendText($"\n[{nickname}] {actualMessage}" + Environment.NewLine);

                        }));
                    }

                    else if (messageType == "ready")
                    {
                        Console.WriteLine($"{playerNickname}ready 받음");

                        string id = parts[1]; // 닉네임
                        string nickname = parts[2]; // 실제 메시지 내용

                        // UI 스레드에서 players_chat에 메시지를 추가
                        players_chat.Invoke(new Action(() =>
                        {
                            players_chat.AppendText($"\n{nickname} : Ready" + Environment.NewLine);
                        }));
                    }
                }
            }
        }
        // 플레이어 정보 업데이트
        private void UpdatePlayerInfo(string info)
        {
            string[] playerDetails = info.Split(',');

            playerInfo.Clear();
            foreach (var player in playerDetails)
            {
                playerInfo.Add(player);
            }
            LoadPlayers();
        }

        // chat 텍스트 박스 입력 메시지 전송
        private void chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                network.SendMessage(playerNickname, chat.Text);
                Console.WriteLine($"{playerNickname}ENTER 채팅 보냄");
                e.SuppressKeyPress = true;
                chat.Clear();
            }

        }
        // 플레이어 목록 로드
        private void LoadPlayers()
        {
            for (int i = 0; i < LiarNames.Length; i++)
            {
                if (i < playerInfo.Count)
                {
                    LiarNames[i].Text = playerInfo[i].Split(':')[1];
                }
                else
                {
                    LiarNames[i].Text = "";
                }
            }

            LoadPlayerBtnImage();
        }

        private void LoadPlayerBtnImage()
        {
            string[] animals = { "otter", "bear", "dog", "elephant", "ferret", "killer_whale", "raccoon", "tiger" };
            ImageList[] imageLists = new ImageList[animals.Length];

            for (int i = 0; i < animals.Length; i++)
            {
                imageLists[i] = new ImageList();
                imageLists[i].ImageSize = new Size(100, 100);
                imageLists[i].Images.Add(Image.FromFile($"{animals[i]}.png"));
                imageLists[i].Images.Add(Image.FromFile($"{animals[i]}_check.png"));
                imageLists[i].Images.Add(Image.FromFile($"{animals[i]}_jail.png"));
                LiarButtons[i].Visible = false; // 버튼을 비활성화

                if (i < playerInfo.Count)
                {
                    LiarButtons[i].ImageList = imageLists[i];
                    LiarButtons[i].ImageIndex = 0; // 첫 번째 이미지 설정
                    LiarButtons[i].ImageAlign = ContentAlignment.MiddleCenter; // 이미지 정렬을 중앙으로 설정
                    Liar_player_images[i].Visible = true; // 배경 이미지 활성화
                    Liar_player_images[i].Cursor = Cursors.No; // 커서 변경
                }
                else
                {
                    Liar_player_images[i].Visible = false; // 배경 이미지 비활성화
                }
            }
        }

        private void ClickeButton(int btnNum)
        {
            if (LiarButtons[btnNum].ImageIndex == 0)
            {
                LiarButtons[btnNum].ImageIndex = 1; // 선택 이미지
            }
            else if (LiarButtons[btnNum].ImageIndex == 1)
            {
                LiarButtons[btnNum].ImageIndex = 0; // 선택 해제 이미지
            }
        }

        private void li_p1_Click(object sender, EventArgs e)
        {
            ClickeButton(0);
        }

        private void li_p2_Click(object sender, EventArgs e)
        {
            ClickeButton(1);
        }

        private void li_p3_Click(object sender, EventArgs e)
        {
            ClickeButton(2);
        }

        private void li_p4_Click(object sender, EventArgs e)
        {
            ClickeButton(3);
        }

        private void li_p5_Click(object sender, EventArgs e)
        {
            ClickeButton(4);
        }

        private void li_p6_Click(object sender, EventArgs e)
        {
            ClickeButton(5);
        }

        private void li_p7_Click(object sender, EventArgs e)
        {
            ClickeButton(6);
        }

        private void li_p8_Click(object sender, EventArgs e)
        {
            ClickeButton(7);
        }

        private void READY_Click(object sender, EventArgs e)
        {
            try
            {
                network.SendReady(playerID, playerNickname);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}

