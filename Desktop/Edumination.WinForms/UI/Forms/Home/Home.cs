using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.Home
{
    public partial class Home : Form
    {

        int slideIndex = 0;
        string slidePath;
        List<Image> slides;

        List<IeltsTestCardPanel> testCards;
        List<LessonCardPanel> lessonCards;

        int testPageIndex = 0;
        int lessonPageIndex = 0;

        const int CardsPerPage = 4;

        //public class PaperDto
        //{
        //    public long Id { get; set; }
        //    public string Title { get; set; }
        //    public string Status { get; set; }
        //    public DateTime CreatedAt { get; set; }
        //    public long? PdfAssetId { get; set; }
        //}

        public Home()
        {
            InitializeComponent();

            UserNavbarPanel nav = new UserNavbarPanel();
            nav.Dock = DockStyle.Top;

            this.Controls.Add(nav);
            nav.BringToFront();
        }

        private void Home_Load(object sender, EventArgs e)
        {


            slides = new List<Image>
            {
                Image.FromFile(Path.Combine(Application.StartupPath, "assets/img/slide1.jpg")),
                Image.FromFile(Path.Combine(Application.StartupPath, "assets/img/slide2.png")),
                Image.FromFile(Path.Combine(Application.StartupPath, "assets/img/slide3.jpg"))
            };

            slideIndex = 0;
            picSlide.Image = slides[slideIndex];

            timerSlide.Start();

            // ========================
            // BUILD TEST CARD LIST
            // ========================
            testCards = new List<IeltsTestCardPanel>
            {
                new IeltsTestCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/mock1.png"),
                    Title = "IELTS Mock Test 2025 January",
                    Rating = "⭐ 4.7 (125 votes)"
                },
                new IeltsTestCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/mock2.png"),
                    Title = "IELTS Mock Test 2024 October",
                    Rating = "⭐ 4.8 (175 votes)"
                },
                new IeltsTestCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/mock2.png"),
                    Title = "IELTS Mock Test 2024 October",
                    Rating = "⭐ 4.8 (175 votes)"
                },
                new IeltsTestCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/mock2.png"),
                    Title = "IELTS Mock Test 2024 October",
                    Rating = "⭐ 4.8 (175 votes)"
                },
                new IeltsTestCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/mock2.png"),
                    Title = "IELTS Mock Test 2024 October",
                    Rating = "⭐ 4.8 (175 votes)"
                },
                new IeltsTestCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/mock2.png"),
                    Title = "IELTS Mock Test 2024 October",
                    Rating = "⭐ 4.8 (175 votes)"
                },
                new IeltsTestCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/mock2.png"),
                    Title = "IELTS Mock Test 2024 October",
                    Rating = "⭐ 4.8 (175 votes)"
                }
            };

            ShowTestPage(0);


            // ========================
            // BUILD LESSON CARD LIST
            // ========================
            lessonCards = new List<LessonCardPanel>
            {
                new LessonCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/lesson_writing.png"),
                    Category = "Writing",
                    TitleText = "Academic Writing Task 1 – Describing Maps",
                    TimeText = "19:00 – 20:00 (GMT+7)",
                    Attending = "1,800+ attending"
                },
                new LessonCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/lesson_writing.png"),
                    Category = "Writing",
                    TitleText = "Academic Writing Task 1 – Describing Maps",
                    TimeText = "19:00 – 20:00 (GMT+7)",
                    Attending = "1,800+ attending"
                },
                new LessonCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/lesson_writing.png"),
                    Category = "Writing",
                    TitleText = "Academic Writing Task 1 – Describing Maps",
                    TimeText = "19:00 – 20:00 (GMT+7)",
                    Attending = "1,800+ attending"
                },
                new LessonCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/lesson_writing.png"),
                    Category = "Writing",
                    TitleText = "Academic Writing Task 1 – Describing Maps",
                    TimeText = "19:00 – 20:00 (GMT+7)",
                    Attending = "1,800+ attending"
                },
                new LessonCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/lesson_writing.png"),
                    Category = "Writing",
                    TitleText = "Academic Writing Task 1 – Describing Maps",
                    TimeText = "19:00 – 20:00 (GMT+7)",
                    Attending = "1,800+ attending"
                },
                new LessonCardPanel
                {
                    Thumbnail = Image.FromFile("assets/img/lesson_writing.png"),
                    Category = "Writing",
                    TitleText = "Academic Writing Task 1 – Describing Maps",
                    TimeText = "19:00 – 20:00 (GMT+7)",
                    Attending = "1,800+ attending"
                }
            };

            ShowLessonPage(0);
        }

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
