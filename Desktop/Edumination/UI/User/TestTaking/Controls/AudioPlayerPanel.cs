using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class AudioPlayerPanel : UserControl
    {
        public AudioPlayerPanel()
        {
            InitializeComponent();
        }

        public void LoadAudio(string audioPath)
        {
            if (string.IsNullOrWhiteSpace(audioPath))
            {
                MessageBox.Show("No audio file available for this Listening test.",
                    "Missing Audio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                axWindowsMediaPlayer.URL = audioPath;
                axWindowsMediaPlayer.Ctlcontrols.stop();
            }
            catch
            {
                MessageBox.Show("Failed to load audio file.",
                    "Audio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
