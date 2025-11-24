using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using IELTS.DAL;
using IELTS.UI.User.TestTaking.Controls;

namespace IELTS.UI.User.TestTaking.ReadingTest
{
    public partial class ReadingTest : Form
    {
        // ====== FIELDS ======
        private readonly long _paperId;
        private readonly long _sectionId;

        private int _remainingSeconds;
        private readonly System.Windows.Forms.Timer _timer;

        // ====== CONSTRUCTOR ======
        public ReadingTest(long paperId, long sectionId)
        {
            _paperId = paperId;
            _sectionId = sectionId;

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ⭐ KHỞI TẠO TIMER — bắt buộc
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        // Designer cần constructor rỗng → gọi về constructor chính
        public ReadingTest() : this(0, 0) { }


        private void ReadingTest_Load(object sender, EventArgs e)
        {
            if (pdfViewer == null)
            {
                MessageBox.Show("UI controls are not initialized properly.", "Error");
                Close();
                return;
            }

            if (_sectionId <= 0)
            {
                MessageBox.Show(
                    "No Reading section specified. Please open this test from Test Library.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Close();
                return;
            }

            // Load từ DB
            if (!LoadSectionFromDatabase())
            {
                Close();
                return;
            }

            UpdateTimeLabel();
            _timer.Start();
        }

        // ====== LOAD DB ======
        private bool LoadSectionFromDatabase()
        {
            string skill = "READING";
            int? timeLimitMinutes = null;
            string pdfPath = null;

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Skill, TimeLimitMinutes, PdfFilePath
                                        FROM TestSections
                                        WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", _sectionId);

                    conn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                        {
                            MessageBox.Show(
                                "Reading section not found in database.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            return false;
                        }

                        skill = r["Skill"]?.ToString() ?? "READING";

                        if (r["TimeLimitMinutes"] != DBNull.Value)
                            timeLimitMinutes = Convert.ToInt32(r["TimeLimitMinutes"]);

                        if (r["PdfFilePath"] != DBNull.Value)
                            pdfPath = r["PdfFilePath"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading section from database:\r\n" + ex.Message,
                    "DB Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            // Default time
            if (!timeLimitMinutes.HasValue || timeLimitMinutes.Value <= 0)
                timeLimitMinutes = 60;

            _remainingSeconds = timeLimitMinutes.Value * 60;

            this.Text = $"IELTS Reading – {skill}";

            // Show PDF
            pdfViewer.ShowPdf(
                pdfPath,
                title: $"{skill} Section",
                fallbackText: "No PDF is configured for this section in the database."
            );

            return true;
        }

        // ====== TIMER ======
        private void Timer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;

            if (_remainingSeconds <= 0)
            {
                _timer.Stop();
                UpdateTimeLabel();

                MessageBox.Show(
                    "Time is up! The test will be submitted.",
                    "Time up",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                SubmitTest();
                return;
            }

            UpdateTimeLabel();
        }

        private void UpdateTimeLabel()
        {
            int minutes = _remainingSeconds / 60;
            int seconds = _remainingSeconds % 60;

            string t = $"{minutes:D2}:{seconds:D2} minutes remaining";

            this.Text = $"IELTS Reading – {t}";
        }

        // ====== SUBMIT ======
        private void SubmitTest()
        {
            MessageBox.Show(
                "Submit test (grading & saving result will be implemented later).",
                "Submit",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            Close();
        }
    }
}
