using Microsoft.VisualBasic.Devices;

namespace BLUFF_CITY
{
    public partial class tutorial : Form
    {
        private List<string> imagePaths;
        private int currentIndex;

        public tutorial()
        {
            InitializeComponent();

            // Previous 배열에 대해 배경을 투명하게 설정하고 윤곽선을 숨김
            Previous.FlatStyle = FlatStyle.Flat;
            Previous.BackColor = Color.Transparent;
            Previous.FlatAppearance.BorderSize = 0;

            next.FlatStyle = FlatStyle.Flat;
            next.BackColor = Color.Transparent;
            next.FlatAppearance.BorderSize = 0;

            InitializePictureBox();

            LoadImages();

            DisplayCurrentImage();
        }

        private void LoadImages()
        {
            imagePaths = new List<string>
            {
                "tutorial_1.png",
                "tutorial_2.png",
                "tutorial_3.png",
                "tutorial_4.png",
                "tutorial_5.png",
                "tutorial_6.png"
            };
            currentIndex = 0;
        }

        private void DisplayCurrentImage()
        {
            if (imagePaths.Count > 0 && currentIndex >= 0 && currentIndex < imagePaths.Count)
            {
                tutorial_pic.Image = Image.FromFile(imagePaths[currentIndex]);
            }
        }

        private void next_Click(object sender, EventArgs e)
        {
            if (currentIndex < imagePaths.Count - 1)
            {
                currentIndex++;
                DisplayCurrentImage();
            }
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                DisplayCurrentImage();
            }
        }

        private void tutorial_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
