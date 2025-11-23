using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IELTS.BLL;
using IELTS.DTO;

namespace IELTS.UI.User.Home
{
    public partial class Home : Form
    {
        int slideIndex = 0;
        List<Image> slides;

        List<IeltsTestCardPanel> testCards;
        List<LessonCardPanel> lessonCards;

        int testPageIndex = 0;
        int lessonPageIndex = 0;

        const int CardsPerPage = 4;

        // BLL
        private readonly TestPaperBLL _paperBLL = new TestPaperBLL();
        private readonly CourseBLL _courseBLL = new CourseBLL();

        public Home()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            // NAVBAR
            UserNavbarPanel nav = new UserNavbarPanel(this);
            nav.Dock = DockStyle.Top;
            this.Controls.Add(nav);
            nav.BringToFront();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            // =====================================
            // SLIDE SHOW
            // =====================================
            slides = new List<Image>
            {
                Image.FromFile(Path.Combine(Application.StartupPath, "assets/img/slide1.jpg")),
                Image.FromFile(Path.Combine(Application.StartupPath, "assets/img/slide2.png")),
                Image.FromFile(Path.Combine(Application.StartupPath, "assets/img/slide3.jpg"))
            };

            slideIndex = 0;
            picSlide.Image = slides[slideIndex];
            timerSlide.Start();


            // =====================================
            // LOAD MOCK TESTS
            // =====================================
            LoadMockTestsFromDatabase();
            ShowTestPage(0);


            // =====================================
            // LOAD COURSES
            // =====================================
            LoadCoursesFromDatabase();
            ShowLessonPage(0);
        }



        // =====================================
        // LOAD MOCK TESTS FROM DB
        // =====================================
        private void LoadMockTestsFromDatabase()
        {
            testCards = new List<IeltsTestCardPanel>();

            DataTable dt = _paperBLL.GetAllPublishedPapers();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có mock test nào được publish!", "Info");
                return;
            }

            string thumbnailPath = "assets/img/mock1.png";

            foreach (DataRow row in dt.Rows)
            {
                IeltsTestCardPanel card = new IeltsTestCardPanel();

                if (File.Exists(thumbnailPath))
                    card.Thumbnail = Image.FromFile(thumbnailPath);

                card.Title = row["Title"].ToString();

                Random rd = new Random();
                double rating = 4.5 + rd.NextDouble() * 0.4;
                int votes = rd.Next(120, 650);

                card.Rating = $"⭐ {rating:0.0} ({votes} votes)";

                testCards.Add(card);
            }
        }



        // =====================================
        // LOAD COURSES FROM DB
        // =====================================
        private void LoadCoursesFromDatabase()
        {
            lessonCards = new List<LessonCardPanel>();

            DataTable dt = _courseBLL.GetAllPublishedCourses();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có khóa học nào được publish!", "Info");
                return;
            }

            // Bộ ảnh random
            string[] thumbs =
            {
        "assets/img/imageGroupStudy1.png",
        "assets/img/imageGroupStudy2.png",
        "assets/img/imageGroupStudy3.png",
        "assets/img/lesson_writing.png"
    };

            Random rnd = new Random();

            foreach (DataRow row in dt.Rows)
            {
                LessonCardPanel card = new LessonCardPanel();

                // Random 1 ảnh
                string thumbPath = thumbs[rnd.Next(thumbs.Length)];

                if (File.Exists(thumbPath))
                    card.Thumbnail = Image.FromFile(thumbPath);

                // Mapping dữ liệu DB
                card.Category = row["Level"].ToString();
                card.TitleText = row["Title"].ToString();
                card.TimeText = $"{Convert.ToInt32(row["PriceVND"]):N0} VNĐ";

                string desc = row["Description"]?.ToString() ?? "";
                card.Attending = desc.Length > 80 ? desc.Substring(0, 80) + "..." : desc;

                lessonCards.Add(card);
            }
        }




        // =====================================
        // PAGING
        // =====================================
        private void ShowTestPage(int page)
        {
            panelTests.Controls.Clear();

            int start = page * CardsPerPage;
            int end = Math.Min(start + CardsPerPage, testCards.Count);

            for (int i = start; i < end; i++)
                panelTests.Controls.Add(testCards[i]);

            testPageIndex = page;
        }

        private void ShowLessonPage(int page)
        {
            panelLessons.Controls.Clear();

            int start = page * CardsPerPage;
            int end = Math.Min(start + CardsPerPage, lessonCards.Count);

            for (int i = start; i < end; i++)
                panelLessons.Controls.Add(lessonCards[i]);

            lessonPageIndex = page;
        }


        private void btnPrevTest_Click(object sender, EventArgs e)
        {
            if (testPageIndex > 0)
                ShowTestPage(testPageIndex - 1);
        }

        private void btnNextTest_Click(object sender, EventArgs e)
        {
            if ((testPageIndex + 1) * CardsPerPage < testCards.Count)
                ShowTestPage(testPageIndex + 1);
        }


        private void btnPrevLesson_Click(object sender, EventArgs e)
        {
            if (lessonPageIndex > 0)
                ShowLessonPage(lessonPageIndex - 1);
        }

        private void btnNextLesson_Click(object sender, EventArgs e)
        {
            if ((lessonPageIndex + 1) * CardsPerPage < lessonCards.Count)
                ShowLessonPage(lessonPageIndex + 1);
        }


        private void timerSlide_Tick(object sender, EventArgs e)
        {
            slideIndex++;

            if (slideIndex >= slides.Count)
                slideIndex = 0;

            picSlide.Image = slides[slideIndex];
        }

        private void lblTest_Click(object sender, EventArgs e)
        {

        }
    }
}
