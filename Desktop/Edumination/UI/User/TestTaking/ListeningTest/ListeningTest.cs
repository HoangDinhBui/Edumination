using IELTS.DAL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using IELTS.UI.User.TestTaking.Controls;

namespace IELTS.UI.User.TestTaking.ListeningTest
{
    public partial class ListeningTest : Form
    {
        private readonly long _paperId;
        private readonly long _sectionId;

        private int _remainingSeconds;
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        private string _audioPath = null;
        private string _pdfPath = null;

        public ListeningTest(long paperId, long sectionId)
        {
            _paperId = paperId;
            _sectionId = sectionId;

            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        private void ListeningTest_Load(object sender, EventArgs e)
        {
            testNavBar.OnExitRequested += TestNavBar_OnExitRequested;
            testNavBar.OnSubmitRequested += TestNavBar_OnSubmitRequested;

            if (_sectionId <= 0)
            {
                MessageBox.Show(
                    "No Listening section specified. Please open this test from Test Library.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
                return;
            }

            if (!LoadSectionFromDatabase())
            {
                Close();
                return;
            }

            audioPanel.LoadAudio(_audioPath);

            pdfViewer.ShowPdf(
                _pdfPath,
                "Listening Section",
                "No PDF configured for this Listening test."
            );

            UpdateTimeLabel();
            _timer.Start();
        }

        private void TestNavBar_OnExitRequested()
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to exit this Listening test?",
                "Exit Test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                _timer.Stop();
                Hide();
                new IELTS.UI.User.TestLibrary.TestLibrary().Show();
            }
        }

        private void TestNavBar_OnSubmitRequested()
        {
            var confirm = MessageBox.Show(
                "Do you want to submit your answers now?",
                "Submit Test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                _timer.Stop();
                Hide();
                new IELTS.UI.User.TestLibrary.TestLibrary().Show();
            }
        }

        private bool LoadSectionFromDatabase()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT TimeLimitMinutes, AudioFilePath, PdfFilePath
                        FROM TestSections
                        WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Id", _sectionId);
                    conn.Open();

                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                        {
                            MessageBox.Show("Listening section not found in database.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        int limit = r["TimeLimitMinutes"] != DBNull.Value
                            ? Convert.ToInt32(r["TimeLimitMinutes"])
                            : 30;

                        _remainingSeconds = limit * 60;

                        if (r["AudioFilePath"] != DBNull.Value)
                            _audioPath = r["AudioFilePath"].ToString();

                        if (r["PdfFilePath"] != DBNull.Value)
                            _pdfPath = r["PdfFilePath"].ToString();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error:\r\n" + ex.Message,
                    "DB Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;

            if (_remainingSeconds <= 0)
            {
                _timer.Stop();
                UpdateTimeLabel();
                MessageBox.Show("Time is up!", "Time up",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
                return;
            }

            UpdateTimeLabel();
        }

        private void UpdateTimeLabel()
        {
            int min = _remainingSeconds / 60;
            int sec = _remainingSeconds % 60;

            testNavBar.SetTimeText($"{min:D2}:{sec:D2} minutes remaining");
        }
    }
}
