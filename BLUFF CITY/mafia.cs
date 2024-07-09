using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySql.Data.MySqlClient;
using System.Threading;

namespace BLUFF_CITY
{
    public partial class mafia : Form
    {
        private static TcpClient client; // 서버와 TCP 연결을 나타내는 TcpCient 객체
        private static NetworkStream stream; // 네트워크 스트림
        private Thread receiveThread; // 메시지 수신 스레드
        private string playerID;
        private string playerNickname;
        private List<string> playerInfo;  // 플레이어 정보를 저장할 리스트

        public mafia(string id, string nickname)
        {
            InitializeComponent();

            // 버튼과 텍스트 박스 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();

            playerInfo = new List<string>(); // 플레이어 정보 리스트 초기화
            playerID = id;
            playerNickname = nickname;
            Join();
        }

        private void mafia_Load(object sender, EventArgs e)
        {
            // BackgroundImage 크기에 맞춰 폼 크기 조정
            if (this.BackgroundImage != null)
            {
                this.ClientSize = this.BackgroundImage.Size;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            //ChooseGame ChooseGameForm = new ChooseGame();
            //ChooseGameForm.Show();

            // 현재 폼 숨김
            this.Hide();
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // MafiaButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in MafiaButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // MafiaNames 배열에 대해 배경을 투명하게 설정
            foreach (var textBox in MafiaNames)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }

            // MafiaOtherButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in MafiaOtherButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // MafiaOtherTextBox 대해 배경을 투명하게 설정
            foreach (var textBox in MafiaOtherTextBox)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }
        }

        // 서버 연결
        public static void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 13000); // 서버 연결
                stream = client.GetStream(); // 네트워크 스트림 설정
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        
        // 서버에 로그인 정보 전송
        public static void SendLoginInfo(string id, string nickname)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    ConnectToServer();
                }

                string message = $"login:{id}:{nickname}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        // gameRoom==mafia_game 참여
        private void Join()
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    ConnectToServer();
                }

                byte[] data = Encoding.UTF8.GetBytes($"join:{playerID}:{playerNickname}:mafia_game");
                stream.Write(data, 0, data.Length);

                receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 서버로부터 메시지 수신
        private void ReceiveMessages()
        {
            try
            {
                byte[] buffer = new byte[256];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    DisplayMessage(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 수신된 메시지 폼에 표시
        private void DisplayMessage(string message)
        {
            if (InvokeRequired)
            {
                // 다른 스레드에서 호출된 경우, UI 스레드에서 재귀적으로 메서드를 호출합니다.
                this.Invoke(new Action<string>(DisplayMessage), new object[] { message });
                return;
            }

            // 메시지에 콜론이 포함되어 있는지 확인합니다.
            if (message.Contains(":"))
            {
                // 메시지를 콜론을 기준으로 파싱합니다.
                string[] parts = message.Split(new[] { ':' }, 2);
                string messageType = parts[0]; // 메시지의 유형
                string chatMessage = parts[1]; // 실제 채팅 내용

                // 플레이어 정보 메시지인 경우
                if (messageType == "player_info")
                {
                    UpdatePlayerInfo(chatMessage); // 플레이어 정보를 업데이트합니다.
                }
                // 마피아 게임 채팅 메시지인 경우
                else if (messageType == "mafia_game")
                {
                    // 채팅 메시지를 다시 콜론을 기준으로 파싱합니다.
                    string[] chatParts = chatMessage.Split(new[] { ':' }, 2);
                    if (chatParts.Length < 2)
                    {
                        Console.WriteLine("잘못된 채팅 메시지 형식");
                        return;
                    }

                    string nickname = chatParts[0]; // 닉네임
                    string actualMessage = chatParts[1]; // 실제 메시지 내용

                    // UI 스레드에서 players_chat에 메시지를 추가합니다.
                    players_chat.Invoke(new Action(() =>
                    {
                        // 이전 내용을 모두 지우고 새로운 메시지를 추가합니다.
                        players_chat.Clear();
                        players_chat.AppendText($"\n[{nickname}] {actualMessage}" + Environment.NewLine);
                    }));
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
                SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        // 메시지 서버로 전송
        private void SendMessage()
        {
            try
            {
                string message = $"chat:{playerNickname}:{chat.Text}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                chat.Clear();
                MessageBox.Show("메시지 전송: " + message);  // 메시지 박스 출력
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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

        // 플레이어 목록 로드
        private void LoadPlayers()
        {
            for (int i = 0; i < MafiaNames.Length; i++)
            {
                if (i < playerInfo.Count)
                {
                    MafiaNames[i].Text = playerInfo[i].Split(':')[1];
                }
                else
                {
                    MafiaNames[i].Text = "";
                }
            }
        }

        private void READY_Click(object sender, EventArgs e)
        {

        }
    }
}

