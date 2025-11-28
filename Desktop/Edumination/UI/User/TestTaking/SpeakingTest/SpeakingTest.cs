using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.SpeakingTest
{
    public partial class SpeakingTest : Form
    {
        private IELTS.BLL.AudioRecorder _audioRecorder;
        private Button btnRecord;
        private bool isRecording = false;
        private string _currentAudioFilePath = null;

        private List<SpeakingPart> _parts;
        private int _currentPartIndex = 0;

        private readonly System.Windows.Forms.Timer _timer;
        private int _remainingSeconds;

        private int questionIndex = 0;

        private readonly long _sectionId;

        // Thư mục lưu audio
        private readonly string _audioFolder;

        private ComboBox cboMicrophone;
        private ProgressBar audioLevelMeter;
        private int selectedDeviceNumber = 0;

        public SpeakingTest(long sectionId)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            _sectionId = sectionId;

            // Tạo thư mục lưu audio
            _audioFolder = Path.Combine(Application.StartupPath, "SpeakingRecordings");
            if (!Directory.Exists(_audioFolder))
            {
                Directory.CreateDirectory(_audioFolder);
            }

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            _remainingSeconds = 15 * 60;

            // ==== THÊM COMBOBOX CHỌN MICROPHONE ====
            var lblMic = new Label();
            lblMic.Text = "Chọn Microphone:";
            lblMic.Font = new Font("Segoe UI", 10);
            lblMic.Size = new Size(150, 25);
            lblMic.Location = new Point(50, 500);
            this.Controls.Add(lblMic);

            cboMicrophone = new ComboBox();
            cboMicrophone.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMicrophone.Font = new Font("Segoe UI", 10);
            cboMicrophone.Size = new Size(400, 30);
            cboMicrophone.Location = new Point(200, 500);
            cboMicrophone.SelectedIndexChanged += CboMicrophone_SelectedIndexChanged;
            this.Controls.Add(cboMicrophone);

            // Load danh sách microphones
            LoadMicrophones();

            // ==== THÊM AUDIO LEVEL METER ====
            audioLevelMeter = new ProgressBar();
            audioLevelMeter.Size = new Size(400, 20);
            audioLevelMeter.Location = new Point(200, 535);
            audioLevelMeter.Maximum = 100;
            audioLevelMeter.Value = 0;
            audioLevelMeter.Style = ProgressBarStyle.Continuous;
            this.Controls.Add(audioLevelMeter);

            // ==== NÚT GHI ÂM ====
            btnRecord = new Button();
            btnRecord.Text = "🎤 Bắt đầu ghi âm";
            btnRecord.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnRecord.Size = new Size(200, 45);
            btnRecord.Location = new Point(50, 565);
            btnRecord.BackColor = Color.FromArgb(46, 125, 50);
            btnRecord.ForeColor = Color.White;
            btnRecord.FlatStyle = FlatStyle.Flat;
            btnRecord.Click += BtnRecord_Click;
            this.Controls.Add(btnRecord);

            _audioRecorder = new IELTS.BLL.AudioRecorder();
            _audioRecorder.AudioLevelChanged += AudioRecorder_AudioLevelChanged;
        }

        private void LoadMicrophones()
        {
            try
            {
                var devices = IELTS.BLL.AudioRecorder.GetRecordingDevices();
                
                if (devices.Length == 0)
                {
                    MessageBox.Show("⚠️ Không tìm thấy microphone nào!\n\nVui lòng kiểm tra:\n" +
                                "- Microphone đã được cắm chưa\n" +
                                "- Driver âm thanh đã cài đặt chưa\n" +
                                "- Quyền truy cập microphone trong Windows Settings",
                                "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnRecord.Enabled = false;
                    return;
                }

                cboMicrophone.Items.Clear();
                foreach (var device in devices)
                {
                    cboMicrophone.Items.Add(device);
                }
                
                cboMicrophone.SelectedIndex = 0;
                selectedDeviceNumber = 0;

                System.Diagnostics.Debug.WriteLine($"Loaded {devices.Length} recording devices");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách microphone:\n{ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboMicrophone_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDeviceNumber = cboMicrophone.SelectedIndex;
            System.Diagnostics.Debug.WriteLine($"Selected device: {selectedDeviceNumber}");
        }

        private void LoadPartsFromDatabase()
        {
            var questionBLL = new IELTS.BLL.QuestionBLL();
            var dt = questionBLL.GetQuestionsBySectionId(_sectionId);
            if (dt == null || dt.Rows.Count == 0)
            {
                _parts = new List<SpeakingPart>();
                return;
            }

            _parts = new List<SpeakingPart>();
            int pos = 1;
            foreach (System.Data.DataRow row in dt.Rows)
            {
                var part = new SpeakingPart
                {
                    PartName = $"Part {pos}",
                    Title = "Speaking Test",
                    Questions = new List<string> { row["QuestionText"].ToString() }
                };
                _parts.Add(part);
                pos++;
            }
        }

        private void AudioRecorder_AudioLevelChanged(object sender, float level)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, float>(AudioRecorder_AudioLevelChanged), sender, level);
                return;
            }

            // Cập nhật progress bar (0-100)
            int percentage = (int)(level * 100);
            audioLevelMeter.Value = Math.Min(percentage, 100);

            // Đổi màu theo mức độ
            if (percentage > 70)
                audioLevelMeter.ForeColor = Color.Red;
            else if (percentage > 40)
                audioLevelMeter.ForeColor = Color.Green;
            else
                audioLevelMeter.ForeColor = Color.Yellow;
        }

        // =============================
        // TIME LABEL
        // =============================
        private void UpdateTimeLabel()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateTimeLabel));
                return;
            }

            int m = _remainingSeconds / 60;
            int s = _remainingSeconds % 60;
            testNavBar.SetTimeText($"{m:D2}:{s:D2} minutes remaining");
        }

        private void SpeakingTest_Load(object sender, EventArgs e)
        {
            LoadPartsFromDatabase();
            
            if (testNavBar == null || testFooter == null || audioPanel == null)
            {
                MessageBox.Show("UI controls are not initialized properly.", "Error");
                return;
            }

            if (_parts == null || _parts.Count == 0)
            {
                MessageBox.Show("Không tìm thấy câu hỏi cho phần Speaking này!\nVui lòng kiểm tra lại dữ liệu trong database.", 
                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
        }

        // =============================
        // TIMER
        // =============================
        private void Timer_Tick(object sender, EventArgs e)
        {
            _remainingSeconds--;
            if (_remainingSeconds <= 0)
            {
                _ = SubmitAsync(); // Fire and forget
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
            if (isRecording)
            {
                _audioRecorder.StopRecording();
                isRecording = false;
            }
            Hide();
            new IELTS.UI.User.TestLibrary.TestLibrary().Show();
        }

        // =============================
        // SUBMIT
        // =============================
        private void Submit_Click()
        {
            _ = SubmitAsync(); // Fire and forget
        }

        private async Task SubmitAsync()
        {
            _timer.Stop();

            // Dừng recording nếu đang ghi
            if (isRecording)
            {
                _audioRecorder.StopRecording();
                isRecording = false;
                btnRecord.Text = "Bắt đầu ghi âm";
            }

            // Kiểm tra có file audio không
            if (string.IsNullOrEmpty(_currentAudioFilePath) || !File.Exists(_currentAudioFilePath))
            {
                var result = MessageBox.Show(
                    "Bạn chưa ghi âm bài nói của mình!\nBạn có muốn nộp bài không?",
                    "Cảnh báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    _timer.Start(); // Tiếp tục làm bài
                    return;
                }

                // Nộp không có audio
                MessageBox.Show("Bài thi đã được nộp (không có audio)!", "Hoàn thành", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Hide();
                return;
            }

            // Hiển thị loading
            var loadingForm = new Form
            {
                Text = "Đang chấm bài...",
                Size = new Size(400, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblLoading = new Label
            {
                Text = "Đang gửi bài lên hệ thống AI để chấm điểm...\nVui lòng đợi trong giây lát.",
                AutoSize = false,
                Size = new Size(360, 80),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10)
            };

            var progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                Size = new Size(360, 20),
                Location = new Point(20, 70)
            };

            loadingForm.Controls.Add(lblLoading);
            loadingForm.Controls.Add(progressBar);
            loadingForm.Show(this);

            try
            {
                // Lấy câu hỏi đầu tiên làm task prompt
                string taskPrompt = _parts.Count > 0 && _parts[0].Questions.Count > 0 
                    ? _parts[0].Questions[0] 
                    : "Speaking test";

                // Cập nhật loading message
                lblLoading.Text = "🎤 Bước 1/2: Đang chuyển giọng nói thành văn bản...";
                Application.DoEvents();

                // Gọi Groq API để chấm điểm (async)
                var groqService = new IELTS.BLL.GroqService();
                var gradeResult = await groqService.GradeSpeakingAsync(taskPrompt, _currentAudioFilePath);

                loadingForm.Close();

                // Kiểm tra kết quả
                if (gradeResult == null)
                {
                    MessageBox.Show("Không nhận được kết quả chấm điểm từ hệ thống.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hiển thị kết quả
                string resultMessage = $"=== KẾT QUẢ CHẤM SPEAKING ===\n\n" +
                                     $"📊 Band Score: {gradeResult.BandScore}\n\n" +
                                     $"📝 Transcript:\n{gradeResult.Transcript}\n\n" +
                                     $"💬 Feedback:\n{gradeResult.Feedback}\n\n" +
                                     $"📁 File audio: {Path.GetFileName(_currentAudioFilePath)}";

                MessageBox.Show(resultMessage, "Kết quả Speaking Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                loadingForm.Close();
                
                // Hiển thị lỗi chi tiết để debug
                string errorMessage = $"❌ Lỗi khi chấm bài:\n\n" +
                                    $"Lỗi: {ex.Message}\n\n";
                
                if (ex.InnerException != null)
                {
                    errorMessage += $"Chi tiết: {ex.InnerException.Message}\n\n";
                }
                
                errorMessage += $"📁 File audio đã được lưu tại:\n{_currentAudioFilePath}\n\n" +
                              $"💡 Bạn có thể thử:\n" +
                              $"- Kiểm tra kết nối internet\n" +
                              $"- Kiểm tra GROQ_API_KEY trong file .env\n" +
                              $"- Kiểm tra file audio có âm thanh không (mở bằng Windows Media Player)";
                
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Hide();
        }

        // =============================
        // AUDIO RECORDING
        // =============================
        private void BtnRecord_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                try
                {
                    // Kiểm tra có microphone không
                    if (cboMicrophone.Items.Count == 0)
                    {
                        MessageBox.Show("Không có microphone nào được phát hiện!", 
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string fileName = $"speaking_{DateTime.Now:yyyyMMdd_HHmmss}.wav";
                    _currentAudioFilePath = Path.Combine(_audioFolder, fileName);

                    // Bắt đầu ghi với device được chọn
                    _audioRecorder.StartRecording(_currentAudioFilePath, selectedDeviceNumber);
                    isRecording = true;
                    
                    btnRecord.Text = "⏹ Dừng ghi âm";
                    btnRecord.BackColor = Color.FromArgb(198, 40, 40);
                    
                    // Disable combobox khi đang recording
                    cboMicrophone.Enabled = false;

                    System.Diagnostics.Debug.WriteLine($"Recording started: {_currentAudioFilePath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Lỗi khi bắt đầu ghi âm:\n\n{ex.Message}\n\n" +
                                $"Vui lòng thử:\n" +
                                $"- Chọn microphone khác\n" +
                                $"- Kiểm tra quyền truy cập microphone\n" +
                                $"- Khởi động lại ứng dụng",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                try
                {
                    _audioRecorder.StopRecording();
                    isRecording = false;
                    
                    btnRecord.Text = "🎤 Bắt đầu ghi âm";
                    btnRecord.BackColor = Color.FromArgb(46, 125, 50);
                    
                    // Enable lại combobox
                    cboMicrophone.Enabled = true;
                    
                    // Reset level meter
                    audioLevelMeter.Value = 0;

                    // Kiểm tra file
                    if (File.Exists(_currentAudioFilePath))
                    {
                        FileInfo fileInfo = new FileInfo(_currentAudioFilePath);
                        long fileSizeKB = fileInfo.Length / 1024;
                        
                        System.Diagnostics.Debug.WriteLine($"File size: {fileSizeKB} KB");

                        if (fileSizeKB < 5)
                        {
                            MessageBox.Show($"⚠️ Cảnh báo: File ghi âm rất nhỏ ({fileSizeKB} KB)\n\n" +
                                        $"Có thể microphone không hoạt động hoặc không có âm thanh.\n\n" +
                                        $"Vui lòng:\n" +
                                        $"1. Kiểm tra microphone trong Windows Sound Settings\n" +
                                        $"2. Thử nói to hơn\n" +
                                        $"3. Chọn microphone khác",
                                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show($"✅ Đã lưu file ghi âm!\n\n" +
                                        $"📁 Đường dẫn:\n{_currentAudioFilePath}\n\n" +
                                        $"📊 Kích thước: {fileSizeKB} KB\n\n" +
                                        $"💡 Bạn có thể mở file này bằng Windows Media Player để kiểm tra âm thanh.",
                                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi dừng ghi âm:\n{ex.Message}", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            // Dọn dẹp resources
            _timer?.Stop();
            if (isRecording)
            {
                _audioRecorder?.StopRecording();
            }
        }
    }
}