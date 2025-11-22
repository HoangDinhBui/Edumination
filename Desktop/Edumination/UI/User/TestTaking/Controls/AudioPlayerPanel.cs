using IELTS.UI.User.TestTaking.ReadingTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class AudioPlayerPanel : UserControl
    {
        private SoundPlayer _player;
        private string _audioPath;

        public AudioPlayerPanel()
        {
            InitializeComponent();
        }

        // Mock: hiển thị data từ ReadingPart
        public void DisplayPart(ReadingPart part)
        {
            lblTitle.Text = part.PassageTitle;
            lblDescription.Text = part.PassageText;

            // AUDIO MOCK – bạn có thể đổi sang path thật
            _audioPath = Application.StartupPath + @"\assets\audio\mock_listening.wav";
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (_player == null && System.IO.File.Exists(_audioPath))
                    _player = new SoundPlayer(_audioPath);

                _player?.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot play audio (mock): " + ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _player?.Stop();
        }
    }
}
