using System;
using System.Net.Sockets;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Net;
using System.Text;
using System.Threading;

namespace BLUFF_CITY
{
    public partial class start : Form
    {
        private Socket socket; //소켓
        private TcpClient client;
        private Thread receiveThread;

        public start()
        {
            InitializeComponent();

            InitializeArrays();

            ApplyTransparentBackgroundAndHideBorder();

        }

        private void LOGIN_Click(object sender, EventArgs e)
        {
            login loginForm = new login();
            loginForm.Show();

            this.Hide();
        }

        private void SIGN_UP_Click(object sender, EventArgs e)
        {
            SignUp SignUpForm = new SignUp();
            SignUpForm.Show();
        }

        private void start_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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
            try
            {
                // 서버 IP 주소와 포트 번호를 설정합니다.
                IPAddress serverIP = IPAddress.Parse("127.0.0.1");
                int port = 5252;
                // TCP/IP 소켓을 생성하고 서버에 연결합니다.
                client = new TcpClient();
                client.Connect(serverIP, port);
                // 서버와 연결된 경우, 수신 스레드를 시작합니다.
                //receiveThread = new Thread(new ThreadStart(Receive));
                //receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
