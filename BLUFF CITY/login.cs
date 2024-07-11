using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Net.Sockets;
using System.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySqlX.XDevAPI;
using System.IO;

namespace BLUFF_CITY
{

    public partial class login : Form
    {
        //private static TcpClient client; // 서버와 TCP 연결을 나타내는 TcpCient 객체
        //private static NetworkStream stream; // 네트워크 스트림

        Network network = new Network(0);

        public login()
        {
            InitializeComponent();

            InitializeArrays();

            ApplyTransparentBackgroundAndHideBorder();

        }
        private void Login_ok_Click(object sender, EventArgs e)
        {
            string nickname;
            MessageBox.Show("Login successful");

            while(true)
            {
                // 서버에 로그인 정보 전송
                network.SendLoginInfo(login_id.Text, login_pw.Text);
                nickname = network.ReceiveNickname();
                if (nickname != null) { break; }
            }
            ChooseGame chooseGameForm = new ChooseGame(login_id.Text, nickname);
            chooseGameForm.Show();
            this.Hide();
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
