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

namespace BLUFF_CITY
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();

            InitializeArrays();

            ApplyTransparentBackgroundAndHideBorder();

        }

        private void Login_ok_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open(); // DB 연결
                    string query = "SELECT NICKNAME FROM user WHERE ID = @ID AND PW = @PW";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", login_id.Text); // ID 매개변수 설정
                    cmd.Parameters.AddWithValue("@PW", login_pw.Text); // PW 매개변수 설정
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) // 로그인 성공 시
                    {
                        string nickname = reader["NICKNAME"].ToString();

                        // 서버에 로그인 정보 전송
                        liar.SendLoginInfo(login_id.Text, nickname);

                        ChooseGame chooseGameForm = new ChooseGame(login_id.Text, nickname);
                        chooseGameForm.Show();
                        this.Hide();
                    }
                    else // 로그인 실패 시
                    {
                        CHECK.Text = "ID 또는 PW를 다시 입력해 주세요.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
