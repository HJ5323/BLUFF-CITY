using Microsoft.VisualBasic.Devices;
using System.Windows.Forms;

namespace BLUFF_CITY
{
    public partial class Liar : Form
    {
        //Network network = new Network(1);
        private Network network;
        private string playerID;
        private string playerNickname;
        private List<string> entryPlayer;  // 플레이어 정보를 저장할 리스트
        private bool isReady = false;
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

            entryPlayer = new List<string>(); // 플레이어 정보 리스트 초기화
            playerID = id;
            playerNickname = nickname;
            login_name.Text = $"{playerNickname}";
            network = Network.Instance;
            Console.WriteLine("Liar 생성 : ");
            Console.WriteLine(playerID);
            network.Join(playerID, playerNickname);

            //이벤트 핸들러 중복 방지
            network.MessageReceived -= DisplayMessage;
            network.MessageReceived += DisplayMessage;

            // TextChanged 이벤트 핸들러 추가
            players_chat.TextChanged += players_chat_TextChanged;
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
            foreach (var label in LiarNames)
            {
                label.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }

            // LiarOtherButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in LiarOtherButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // LiarOtherLabel 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var label in LiarOtherLabel)
            {
                label.BorderStyle = BorderStyle.None; // Label 테두리 숨기기
            }


            chat.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기

            players_chat.BorderStyle = BorderStyle.None; // RichTextBox 테두리 숨기기

        }
        private void players_chat_MouseDown(object sender, MouseEventArgs e)
        {
            players_chat.SelectionLength = 0; // 선택 길이를 0으로 설정하여 선택을 취소합니다.
        }
        private void players_chat_TextChanged(object sender, EventArgs e)
        {
            // 커서 위치를 텍스트 끝으로 이동
            players_chat.SelectionStart = players_chat.Text.Length;

            // 커서 위치로 스크롤
            players_chat.ScrollToCaret();
        }

        private string PadMessageToCenter(string message, int richTextBoxWidth, Font font)
        {
            // 메시지의 크기를 측정
            using (Graphics g = CreateGraphics())
            {
                SizeF messageSize = g.MeasureString(message, font);

                // 양쪽에 추가할 공백의 개수를 계산
                int totalPadding = (int)((richTextBoxWidth - messageSize.Width) / (2 * font.Size));
                if (totalPadding > 0)
                {
                    return new string(' ', totalPadding) + message;
                }
                else
                {
                    return message;
                }
            }
        }

        public void DisplayMessage(string receivedMessage)
        {
            Console.WriteLine($"{playerNickname}메시지 받음{receivedMessage}");

            // 메시지를 세미콜론으로 구분하여 배열로 나눔
            string[] messages = receivedMessage.Split(';');

            foreach (var message in messages)
            {
                // 각 메시지를 처리하기 위해 잠금을 걸어 동기화
                lock (obj)
                {
                    // 메시지에 콜론이 있는지 확인하여 유효한 메시지인지 판단
                    if (message.Contains(":"))
                    {
                        // 메시지를 콜론을 기준으로 파싱
                        string[] parts = message.Split(':');
                        string messageType = parts[0]; // 메시지 타입

                        // 각 메시지 타입에 따라 처리
                        switch (messageType)
                        {
                            case "entry_player_info":
                                // 플레이어 정보 메시지 처리
                                EntryPlayerInfo(parts);
                                break;

                            case "topic_keyword":
                                // 주제와 키워드 메시지 처리
                                TopicKeyword(parts);
                                break;

                            case "chat":
                                // 채팅 메시지 처리
                                ChatMessage(parts);
                                break;

                            case "ready":
                            case "cancel_ready":
                                // 준비 상태 메시지 처리
                                ReadyMessage(parts);
                                break;

                            case "disable_chat":
                                // 채팅 비활성화 메시지 처리
                                DisableChat();
                                break;

                            case "enable_chat":
                                // 채팅 활성화 메시지 처리
                                EnableChat(parts);
                                break;

                            case "timer":
                                // 타이머 메시지 처리
                                Timer(parts);
                                break;

                            case "start_voting":
                                // 투표 메시지 처리
                                VoteMode();
                                break;

                            case "maxVotes":
                                // 투표 메시지 처리
                                ResultVote(parts);
                                break;

                            case "liar":
                                // liar 최다득표
                                // 토픽에 맞는 폼 열어주기
                                maxVotee_liar(parts);
                                break;

                            case "noliar":
                                // noliar 최다득표
                                maxVotee_noliar(parts);
                                break;

                            case "close":
                                // 게임 종료
                                Formclose(parts);
                                break;

                            default:
                                Console.WriteLine($"Unknown message type: {messageType}");
                                break;
                        }
                    }
                }
            }
        }

        // 플레이어 정보 처리 메서드
        private void EntryPlayerInfo(string[] parts)
        {
            Console.WriteLine($"{playerNickname}player_info 받음");

            // parts 배열에서 parts[0]을 제외한 나머지 부분들을 추출
            string playersMessage = string.Join(":", parts.Skip(1));
            UpdateEntryPlayerInfo(playersMessage); // 플레이어 정보 업데이트
        }

        // 주제와 키워드 처리 메서드
        private void TopicKeyword(string[] parts)
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

            READY.Visible = false;
            exit.Visible = false;
        }

        // 채팅 메시지 처리 메서드
        private void ChatMessage(string[] parts)
        {
            Console.WriteLine($"{playerNickname}chat 받음");

            string nickname = parts[1]; // 닉네임
            string actualMessage = parts[2]; // 실제 메시지 내용

            // UI 스레드에서 players_chat에 메시지를 추가
            players_chat.Invoke(new Action(() =>
            {
                if (nickname == "server")
                {
                    players_chat.SelectionColor = Color.DarkBlue; // 텍스트 색상을 짙은 파란색으로 설정
                }
                else
                {
                    players_chat.SelectionColor = Color.Black;
                }
                string message = $"[{nickname}] {actualMessage}";

                // 수동으로 가운데 정렬을 위한 패딩 추가
                string paddedMessage = PadMessageToCenter(message, players_chat.Width, players_chat.Font);
                players_chat.AppendText(paddedMessage + Environment.NewLine);
            }));
        }

        // 준비 완료 메시지 처리 메서드
        private void ReadyMessage(string[] parts)
        {
            Console.WriteLine($"{playerNickname}ready 받음");
            string id = parts[1]; // id
            string nickname = parts[2]; //nickname 

            string message;
            if (parts[0] == "ready")
            {
                message = $"{nickname} : Ready";
            }
            else if (parts[0] == "cancel_ready")
            {
                message = $"{nickname} : cancel_ready";
            }
            else
            {
                return; // 예상되지 않은 메시지 타입이면 처리 종료
            }

            // UI 스레드에서 players_chat에 메시지를 추가
            players_chat.Invoke(new Action(() =>
            {
                players_chat.SelectionColor = Color.DarkBlue; // 텍스트 색상 설정
                string paddedMessage = PadMessageToCenter(message, players_chat.Width, players_chat.Font);
                players_chat.AppendText(paddedMessage + Environment.NewLine);
            }));
        }

        // 채팅 비활성화 처리 메서드
        private void DisableChat()
        {
            Console.WriteLine($"{playerNickname}채팅 비활성화 받음");

            chat.Invoke(new Action(() =>
            {
                chat.Enabled = false;
                chat.KeyDown -= chat_KeyDown; // 채팅 입력 이벤트 해제
            }));
        }

        // 채팅 활성화 처리 메서드
        private void EnableChat(string[] parts)
        {
            Console.WriteLine($"{playerNickname}채팅 활성화 받음");

            chat.Invoke(new Action(() =>
            {
                chat.Enabled = true;
                chat.KeyDown += chat_KeyDown; // 채팅 입력 이벤트 설정
                Console.WriteLine($"{playerNickname}채팅 활성화 되었습니다.");
            }));
        }

        // 타이머 메시지 처리 메서드
        private void Timer(string[] parts)
        {
            string nickname = parts[1];
            int timeLeft = int.Parse(parts[2]);

            timer.Invoke(new Action(() =>
            {
                timer.Text = $"{timeLeft}S";
            }));
        }

        private void UpdateEntryPlayerInfo(string info)
        {
            string[] playerDetails = info.Split(',');

            entryPlayer.Clear();
            foreach (var player in playerDetails)
            {
                entryPlayer.Add(player);
            }
            LoadPlayers();
        }

        // chat 텍스트 박스 입력 메시지 전송
        private void chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (chat.Enabled) // 채팅이 활성화된 경우에만 메시지 전송 가능
                {
                    network.SendMessage(playerNickname, chat.Text);
                    Console.WriteLine($"{playerNickname}ENTER 채팅 보냄");
                }
                e.SuppressKeyPress = true;
                chat.Clear();
            }

        }
        // 플레이어 목록 로드
        private void LoadPlayers()
        {
            for (int i = 0; i < LiarNames.Length; i++)
            {
                if (i < entryPlayer.Count)
                {
                    LiarNames[i].Text = entryPlayer[i].Split(':')[1];
                }
                else
                {
                    LiarNames[i].Text = "";
                }
            }

            LoadPlayerBtnImage();
        }

        private void ResultVote(string[] parts)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string[]>(ResultVote), new object[] { parts });
                return;
            }

            Console.WriteLine($"{playerNickname}ResultVote 받음");

            string server = parts[1]; // 서버
            string maxvote = parts[2]; // 최다득표자
            string actualMessage = parts[3]; // 실제 메시지 내용

            // 일치하는 player의 인덱스를 가져옴
            int playerIndex = -1;
            for (int i = 0; i < entryPlayer.Count; i++)
            {
                if (entryPlayer[i].Split(':')[1].Equals(maxvote))
                {
                    playerIndex = i;
                    break;
                }
            }

            Console.WriteLine($"{playerIndex}playerIndex 받음");

            if (playerIndex != -1)
            {
                Console.WriteLine($"playerIndex 들어옴");

                // 모든 버튼의 ImageIndex를 0으로 설정
                for (int i = 0; i < LiarButtons.Length; i++)
                {
                    if (i != playerIndex)
                    {
                        LiarButtons[i].ImageIndex = 0;
                    }
                }

                // 해당 인덱스의 버튼만 ImageIndex를 2로 설정
                LiarButtons[playerIndex].ImageIndex = 2; // 선택 이미지 설정
            }

            players_chat.SelectionColor = Color.DarkBlue; // 텍스트 색상을 짙은 파란색으로 설정

            string message = $"[{server}] {actualMessage}";

            // 수동으로 가운데 정렬을 위한 패딩 추가
            string paddedMessage = PadMessageToCenter(message, players_chat.Width, players_chat.Font);
            players_chat.AppendText(paddedMessage + Environment.NewLine);
        }

        private void maxVotee_liar(string[] parts)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string[]>(maxVotee_liar), new object[] { parts });
                return;
            }

            Console.WriteLine($"{string.Join(", ", parts)} - liar");
            string server = parts[1]; // server
            string keyword = parts[2]; // keyword

            if (category.Text == "동물")
            {
                Topic_animal Topic_animalForm = new Topic_animal(keyword);
                Topic_animalForm.Show();
            }
            else if (category.Text == "도시")
            {
                Topic_city Topic_cityForm = new Topic_city(keyword);
                Topic_cityForm.Show();
            }
            else if (category.Text == "과일")
            {
                Topic_fruit Topic_fruitForm = new Topic_fruit(keyword);
                Topic_fruitForm.Show();
            }
            else
            {
                Topic_item Topic_itemForm = new Topic_item(keyword);
                Topic_itemForm.Show();
            }
        }

        private void maxVotee_noliar(string[] parts)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string[]>(maxVotee_noliar), new object[] { parts });
                return;
            }

            Console.WriteLine($"{parts} - noliar 받음");
            string server = parts[1]; // server
            string actualMessage = parts[2]; // 실제 메시지 내용

            players_chat.SelectionColor = Color.DarkBlue; // textBox1의 텍스트 색상을 짙은 파란색으로 설정

            string message = $"\n[{server}] {actualMessage}";

            // 수동으로 가운데 정렬을 위한 패딩 추가
            string paddedMessage = PadMessageToCenter(message, players_chat.Width, players_chat.Font);
            players_chat.AppendText(paddedMessage + Environment.NewLine);
        }

        private void LoadPlayerBtnImage()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LoadPlayerBtnImage));
                return;
            }

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

                if (i < entryPlayer.Count)
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

        private void VoteMode()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(VoteMode));
                return;
            }
            LoadPlayerBtnImage();

            for (int i = 0; i < entryPlayer.Count; i++)
            {
                LiarButtons[i].Visible = true;
                Console.WriteLine("버튼 활성화 됨");
            }
        }

        private void ClickeButton(int btnNum)
        {
            // 현재 선택된 버튼이 있는지 확인
            bool isAnyButtonSelected = LiarButtons.Any(btn => btn.ImageIndex == 1);

            // 선택된 버튼이 있고, 현재 클릭한 버튼이 선택되지 않은 상태이면 리턴
            if (isAnyButtonSelected && LiarButtons[btnNum].ImageIndex == 0)
            {
                return;
            }

            if (LiarButtons[btnNum].ImageIndex == 0)
            {
                // 다른 버튼을 모두 선택 해제 상태로 변경
                foreach (var btn in LiarButtons)
                {
                    btn.ImageIndex = 0;
                    btn.Tag = 0;
                }
                LiarButtons[btnNum].ImageIndex = 1; // 선택 이미지
            }
            else if (LiarButtons[btnNum].ImageIndex == 1)
            {
                LiarButtons[btnNum].ImageIndex = 0; // 선택 해제 이미지
            }

            string selectedPlayer = entryPlayer[btnNum].Split(':')[1];// 닉네임 추출

            // 선택 상태를 전환합니다. 1: 선택됨, 0: 선택 해제됨
            int voteState = (LiarButtons[btnNum].Tag != null && (int)LiarButtons[btnNum].Tag == 1) ? 0 : 1;
            LiarButtons[btnNum].Tag = voteState;

            // 서버로 선택 전송
            network.SendVote(playerID, selectedPlayer, voteState);
            string message = $"{playerID}:{selectedPlayer}:{voteState}";
            network_VoteReceived(message);
        }

        private void network_VoteReceived(string message)
        {
            string[] parts = message.Split(':');
            string selectedPlayer = parts[1];
            int voteState = int.Parse(parts[2]);

            if (voteState == 1)
            {
                // 선택됨
                Console.WriteLine($"{selectedPlayer}이(가) 지목되었습니다.");
            }
            else
            {
                // 선택 해제됨
                Console.WriteLine($"{selectedPlayer} 지목이 해제되었습니다.");
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
                if (!isReady)
                {
                    // Ready 상태로 전환
                    network.SendReady(playerID, playerNickname);
                    isReady = true;
                    READY.Text = "Cancel"; // 버튼 텍스트 변경
                }
                else
                {
                    // Ready 상태 취소
                    network.SendReady(playerID, playerNickname);
                    isReady = false;
                    READY.Text = "Ready"; // 버튼 텍스트 변경
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            network.ExitGameroom(playerID, playerNickname);
            network.MessageReceived -= DisplayMessage;

            ChooseGame ChooseGameForm = new ChooseGame(playerID, playerNickname);
            ChooseGameForm.Show();

            // 현재 폼 숨김
            this.Hide();
        }

        private async void Formclose(string[] parts)
        {
            //Console.WriteLine($"{playerNickname}chat 받음");

            string server = parts[1]; // server
            string closeMessage = parts[2]; // close message

            string message = $"\n[{server}] {closeMessage}";

            // UI 스레드에서 players_chat에 메시지를 추가
            players_chat.Invoke(new Action(() =>
            {
                players_chat.SelectionColor = Color.DarkRed; // 텍스트 색상 설정
                string paddedMessage = PadMessageToCenter(message, players_chat.Width, players_chat.Font);
                players_chat.AppendText(paddedMessage + Environment.NewLine);
            }));

            await Task.Delay(3000); // 3초 대기

            players_chat.Clear();

            LoadPlayerBtnImage();

            READY.Visible = true;

            exit.Visible = true;

            EnableChat(parts);

            isReady = false;
            READY.Text = "Ready"; // 버튼 텍스트 변경
            timer.Text = "";
            category.Invoke(new Action(() =>
            {
                category.Text = "";
            }));
            word.Invoke(new Action(() =>
            {
                word.Text = "";
            }));

        }

        private void tutorial_Click(object sender, EventArgs e)
        {
            tutorial tutorialForm = new tutorial();
            tutorialForm.Show();
        }
    }
}