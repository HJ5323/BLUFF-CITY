using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BLUFF_CITY
{
    public partial class liar : Form
    {
        Network network = new Network(1);
        private string playerID;
        private string playerNickname;
        private List<string> playerInfo;  // 플레이어 정보를 저장할 리스트
        private object obj = new object();

        public liar(string id, string nickname)
        {
            InitializeComponent();

            // 버튼과 텍스트 박스 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();

            playerInfo = new List<string>(); // 플레이어 정보 리스트 초기화
            playerID = id;
            playerNickname = nickname;
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
            lock (obj)
            {
                // 메시지에 콜론이 포함되어 있는지 확인
                if (message.Contains(":"))
                {
                    // 메시지를 콜론을 기준으로 파싱
                    // mafia_game:ME: aaa
                    string[] parts = message.Split(':');
                    string messageType = parts[0]; // 메시지의 유형
                    // 마피아 게임 채팅 메시지인 경우
                    if (messageType == "liar_game" && parts.Length == 3)
                    {
                        if (messageType == "player_info")
                        {
                            // parts 배열에서 parts[0]을 제외한 나머지 부분들을 추출
                            string playersMessage = string.Join(":", parts.Skip(1));
                            UpdatePlayerInfo(playersMessage); // 플레이어 정보를 업데이트
                        }

                        string nickname = parts[1]; // 닉네임
                        string actualMessage = parts[2]; // 실제 메시지 내용

                        // UI 스레드에서 players_chat에 메시지를 추가
                        players_chat.Invoke(new Action(() =>
                        {
                            // 이전 내용을 모두 지우고 새로운 메시지를 추가
                            //players_chat.Clear();
                            if (actualMessage[actualMessage.Length - 1] == '*')
                            {
                                players_chat.Clear();
                                players_chat.AppendText($"\n[{nickname}] {actualMessage}" + Environment.NewLine);
                            }
                            else
                            {
                                players_chat.AppendText($"\n[{nickname}] {actualMessage}" + Environment.NewLine);
                            }
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
                e.SuppressKeyPress = true;

                chat.Clear();
            }

        }
        /*
        // 폼이 닫히면 연결 종료
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (client != null)
            {
                client.Close();
                receiveThread.Abort();
            }
        }
        */
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
        }

    }
}

