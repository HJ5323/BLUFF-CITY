using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BLUFF_CITY
{
    public partial class mafia : Form
    {
        public mafia()
        {
            InitializeComponent();

            // 버튼과 텍스트 박스 배열 초기화
            InitializeArrays();

            // 배경을 투명하게 설정하고 윤곽선을 숨기는 메서드 호출
            ApplyTransparentBackgroundAndHideBorder();

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
            ChooseGame ChooseGameForm = new ChooseGame();
            ChooseGameForm.Show();

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
    }
}
