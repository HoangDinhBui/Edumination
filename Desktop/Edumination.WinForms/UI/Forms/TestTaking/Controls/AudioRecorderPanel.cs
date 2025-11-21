using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestTaking.Controls
{
    public partial class AudioRecorderPanel : UserControl
    {
        // 🔥 EVENT — SpeakingTest sẽ subscribe vào đây
        public event Action OnNextPressed;

        private int _seconds = 0;
        private readonly System.Windows.Forms.Timer _timer;

        public AudioRecorderPanel()
        {
            InitializeComponent();

            // timer đếm giây ghi âm
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _seconds++;
            lblTime.Text = $"{_seconds / 60:D2}:{_seconds % 60:D2}";
        }

        public void StartRecording()
        {
            _seconds = 0;
            lblTime.Text = "00:00";
            _timer.Start();
        }

        public void StopRecording()
        {
            _timer.Stop();
        }

        // 🔥 Nút NEXT — gọi event
        private void btnNext_Click(object sender, EventArgs e)
        {
            StopRecording();
            OnNextPressed?.Invoke();   // GỌI EVENT
        }
    }
}
