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

            try
            {
                MySqlConnection connection = new MySqlConnection("Server = localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;");
                // SQL 서버와 연결.
                // Server = localhost : 로컬 호스트 (내 컴퓨터) 서버와 연결
                // Database = 스키마 이름
                // Uid = DB 로그인 아이디
                // Pwd = DB 로그인 비밀번호

                connection.Open();
                // SQL 서버 연결

                int login_status = 0;
                // 로그인 상태 변수 선언, 비로그인 상태는 0

                string loginid = login_id.Text;
                // 문자열 loginid 변수는 login_id 의 텍스트값
                string loginpwd = login_pw.Text;
                // 문자열 loginpwd 변수는 login_pw 의 텍스트값

                string selectQuery = "SELECT * FROM user WHERE ID = \'" + loginid + "\' ";
                // 문자열 selectQuery 변수 선언.
                // MySQL에 전송할 명령어를 입력한다.
                // 실제로 MySQL에 전송될 명령어는 "" 사이의 값.
                // dbtest 스키마의 user 테이블 값을 읽기 위해 변수 선언.

                MySqlCommand Selectcommand = new MySqlCommand(selectQuery, connection);
                // MySqlCommand는 MySQL로 명령어를 전송하기 위한 클래스.
                // MySQL에 selectQuery 값을 보내고, connection 값을 보내 연결을 시도한다.
                // 위 정보를 Selectcommand 변수에 저장한다.

                MySqlDataReader userAccount = Selectcommand.ExecuteReader();
                // MySqlDataReader은 입력값을 받기 위함.
                // Selectcommand 변수에 ExecuteReader() 객체를 통해 입력값을 받고,
                // 해당 정보를 userAccount 변수에 저장한다.

                while (userAccount.Read())
                // userAccount가 Read 되고 있을 동안
                {
                    if (loginid == (string)userAccount["ID"] && loginpwd == (string)userAccount["PW"])
                    // 만약 loginid변수의 값이 user 테이블 값의 ID 정보와,
                    // loginpwd변수의 값이 user 테이블 값의 PW 정보와 일치한다면
                    {
                        login_status = 1;
                        // 해당 변수 상태를 1로 바꾼다.
                    }
                }
                connection.Close();
                // MySQL과 연결을 끊는다.

                if (login_status == 1)
                // 만약 해당 변수 상태가 1이라면,
                {
                    //MessageBox.Show("로그인 완료");
                    // 로그인 완료 메시지박스를 띄운다.

                    ChooseGame ChooseGameForm = new ChooseGame();
                    ChooseGameForm.Show();

                    // 현재 폼 숨김
                    this.Hide();
                }
                else
                // 아니라면,
                {
                    CHECK.Text = "잘못된 입력입니다.";
                    // CHECK 텍스트 박스에 오류 메세지 출력.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // 예외값 발생 시 해당 정보와 관련된 메시지박스를 띄운다.
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
