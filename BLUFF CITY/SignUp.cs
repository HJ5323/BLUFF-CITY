using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using MySql.Data.MySqlClient;

namespace BLUFF_CITY
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();

            // 버튼 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();
        }

        private void signup_ok_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=localhost; Database=bluff_city; Uid=bluff_city; Pwd=bluff_city;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if ID exists
                    string checkIdQuery = "SELECT * FROM user WHERE ID = @ID";
                    MySqlCommand checkIdCmd = new MySqlCommand(checkIdQuery, conn);
                    checkIdCmd.Parameters.AddWithValue("@ID", signup_id.Text);
                    MySqlDataReader idReader = checkIdCmd.ExecuteReader();
                    if (idReader.Read())
                    {
                        idReader.Close();
                        MessageBox.Show("중복된 ID입니다.");
                        return;
                    }
                    idReader.Close();

                    // Check if Nickname exists
                    string checkNicknameQuery = "SELECT * FROM user WHERE NICKNAME = @NICKNAME";
                    MySqlCommand checkNicknameCmd = new MySqlCommand(checkNicknameQuery, conn);
                    checkNicknameCmd.Parameters.AddWithValue("@NICKNAME", signup_name.Text);
                    MySqlDataReader nicknameReader = checkNicknameCmd.ExecuteReader();
                    if (nicknameReader.Read())
                    {
                        nicknameReader.Close();
                        MessageBox.Show("중복된 닉네임입니다.");
                        return;
                    }
                    nicknameReader.Close();

                    // Insert new user
                    string insertQuery = "INSERT INTO user (ID, PW, NICKNAME) VALUES (@ID, @PW, @NICKNAME)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@ID", signup_id.Text);
                    insertCmd.Parameters.AddWithValue("@PW", signup_pw.Text);
                    insertCmd.Parameters.AddWithValue("@NICKNAME", signup_name.Text);
                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("회원가입이 완료되었습니다.");
                    //start startForm = new start();
                    //startForm.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //// 현재 폼 닫음
            //this.Hide();

        }

        private void SignUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // SignUpButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in SignUpButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }

            // SignUpTextBox 배열에 대해 배경을 투명하게 설정
            foreach (var textBox in SignUpTextBox)
            {
                textBox.BorderStyle = BorderStyle.None; // 텍스트 박스 테두리 숨기기
            }
        }
    }
}
