namespace BLUFF_CITY
{
    public partial class Topic_item : Form
    {
        private Network network;
        private string keyword;

        public Topic_item(string keyword)
        {
            Console.WriteLine($"Topic_item : { keyword}");

            InitializeComponent();
            this.keyword = keyword; // keyword를 폼 내부에 저장
        }

        private void CheckLiarGuess(string buttonText)
        {
            Console.WriteLine("CheckLiarGuess");

            if (buttonText == keyword)
            {
                // 라이어가 맞춤
                string result = "right";
                network.SendGuessMessage(result);
            }
            else
            {
                // 라이어가 틀림
                string result = "wrong";
                network.SendGuessMessage(result);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("button1_Click");

            CheckLiarGuess(button1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button4.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button5.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button6.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button7.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button8.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CheckLiarGuess(button9.Text);
        }
    }
}
