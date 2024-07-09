using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace BLUFF_CITY
{
    public partial class ChooseGame : Form
    {
        private string playerID;
        private string playerNickname;

        public ChooseGame(string id, string nickname)
        {
            InitializeComponent();

            // 버튼 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();

            playerID = id;
            playerNickname = nickname;
        }

        private void ApplyTransparentBackgroundAndHideBorder()
        {
            // maButtons 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            foreach (var button in GameButtons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = Color.Transparent;
                button.FlatAppearance.BorderSize = 0;
            }
        }

        private void MAFIA_GAME_Click(object sender, EventArgs e)
        {
            mafia mafiaForm = new mafia(playerID, playerNickname);
            mafiaForm.Show();

            this.Hide();
        }

        private void LIAR_GAME_Click(object sender, EventArgs e)
        {
            liar liarForm = new liar();
            liarForm.Show();

            this.Hide();
        }

        private void LogOut_Click(object sender, EventArgs e)
        {
            start startForm = new start();
            startForm.Show();

            this.Hide();
        }
        private void ChooseGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
