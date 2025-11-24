using System;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class AudioPlayerPanel : UserControl
    {
        public AudioPlayerPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tải file âm thanh vào trình phát
        /// </summary>
        /// <param name="audioPath">Đường dẫn file mp3/wav</param>
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
                // Gán URL cho Windows Media Player
                axWindowsMediaPlayer.URL = audioPath;

                // Dừng ngay lập tức để người dùng tự bấm Play khi sẵn sàng
                axWindowsMediaPlayer.Ctlcontrols.stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load audio file.\nError: " + ex.Message,
                    "Audio Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Dừng phát nhạc (Dùng khi thoát bài thi hoặc hết giờ)
        /// </summary>
        public void StopAudio()
        {
            try
            {
                // Kiểm tra null để tránh lỗi nếu control chưa khởi tạo xong hoặc đã bị hủy
                if (axWindowsMediaPlayer != null)
                {
                    axWindowsMediaPlayer.Ctlcontrols.stop();
                }
            }
            catch
            {
                // Bỏ qua lỗi nếu quá trình dừng gặp trục trặc (không quan trọng)
            }
        }
    }
}