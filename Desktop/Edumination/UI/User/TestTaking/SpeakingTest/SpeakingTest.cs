using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.SpeakingTest
{
    public partial class SpeakingTest : Form
    {
        private readonly List<SpeakingPart> _parts;
        private int _currentPartIndex = 0;

        private readonly System.Windows.Forms.Timer _timer;
        private int _remainingSeconds;

        private int questionIndex = 0;

        public SpeakingTest()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;

            // Load mockdata
            _parts = SpeakingMockData.GetParts();
            _remainingSeconds = SpeakingMockData.TotalTimeSeconds;

            // Timer
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;

            // Khi phóng to – giữ audio panel ở giữa
            //this.Resize += (s, e) => CenterAudioPanel();
        }

        // =============================
        // TIME LABEL
        // =============================
        private void UpdateTimeLabel()
        {
            int m = _remainingSeconds / 60;
            int s = _remainingSeconds % 60;
            testNavBar.SetTimeText($"{m:D2}:{s:D2} minutes remaining");
        }

        private void SpeakingTest_Load(object sender, EventArgs e)
        {
            // NAV
            testNavBar.OnExitRequested += Exit_Click;
            testNavBar.OnSubmitRequested += Submit_Click;

            // FOOTER
            testFooter.OnPartSelected += Footer_OnPartClicked;
            testFooter.LoadParts(_parts.Select(p => p.PartName));

            // AUDIO PANEL
            audioPanel.OnNextPressed += NextQuestion;

            // SHOW FIRST PART
            ShowPart(0);

            UpdateTimeLabel();
            _timer.Start();

            //CenterAudioPanel();
        }

        // =============================
        // CENTER AUDIO PANEL
        // =============================
        //private void CenterAudioPanel()
        //{
        //    audioPanel.Left = (this.ClientSize.Width - audioPanel.Width) / 2;
        //    audioPanel.Top = 650; // giống design bạn đưa
        //}

        // =============================
        // TIMER
        // =============================
        private void Timer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;
            if (_remainingSeconds <= 0)
            {
                Submit();
                return;
            }

            UpdateTimeLabel();
        }

        // =============================
        // SHOW PART
        // =============================
        private void ShowPart(int index)
        {
            _currentPartIndex = index;
            var part = _parts[index];

            lblPart.Text = part.PartName;
            lblTitle.Text = part.Title;

            questionIndex = 0;
            lblQuestion.Text = part.Questions[0];

            testFooter.SetActivePart(part.PartName);
        }

        // =============================
        // NEXT QUESTION
        // =============================
        private void NextQuestion()
        {
            var list = _parts[_currentPartIndex].Questions;

            if (questionIndex < list.Count - 1)
            {
                questionIndex++;
                lblQuestion.Text = list[questionIndex];
            }
        }

        // =============================
        // FOOTER PART CLICK
        // =============================
        private void Footer_OnPartClicked(string partName)
        {
            int index = _parts.FindIndex(p => p.PartName == partName);
            if (index >= 0)
                ShowPart(index);
        }

        // =============================
        // EXIT
        // =============================
        private void Exit_Click()
        {
            _timer.Stop();
            Hide();
            new IELTS.UI.User.TestLibrary.TestLibrary().Show();
        }

        // =============================
        // SUBMIT
        // =============================
        private void Submit_Click()
        {
            Submit();
        }

        private void Submit()
        {
            _timer.Stop();
            MessageBox.Show("Speaking test completed! (Mock)", "Submit", MessageBoxButtons.OK);
            Hide();
            new IELTS.UI.User.TestLibrary.TestLibrary().Show();
        }
    }
}
