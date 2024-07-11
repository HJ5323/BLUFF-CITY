using System;
using System.Net.Sockets;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Net;
using System.Text;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Mysqlx.Crud;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace BLUFF_CITY
{
    public partial class start : Form
    {
        private Socket socket; //소켓
        //private TcpClient client;
        private Thread receiveThread;

        public start()
        {
            InitializeComponent();

            InitializeArrays();

            ApplyTransparentBackgroundAndHideBorder();

            // 서버 연결
            liar.ConnectToServer();
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
