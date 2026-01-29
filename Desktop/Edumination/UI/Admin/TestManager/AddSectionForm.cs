using IELTS.DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class AddSectionForm : Form
    {
        private long _paperId;
        public long PaperId
        {
            get => _paperId;
            set => _paperId = value;
        }
        private ShowSectionControl showSectionControl;
        public ShowSectionControl ShowSectionControl
        {
            get => showSectionControl; set => showSectionControl = value;
        }

        private string selectedPdfPath = "";
        private string selectedAudioPath = "";

        public class ComboItem<T>
        {
            public string Text { get; set; }
            public T Value { get; set; }

            public override string ToString() => Text;
        }

        public AddSectionForm(ShowSectionControl showSectionControl,long paperId)
        {
            InitializeComponent();
            this.showSectionControl = showSectionControl;
            _paperId = paperId;

            LoadSkillCombo();
            LoadTimeLimitCombo();
        }

        private void LoadSkillCombo()
        {
            var skills = new List<ComboItem<string>>
    {
        new ComboItem<string> { Text = "Listening", Value = "LISTENING" },
        new ComboItem<string> { Text = "Reading",   Value = "READING" },
        new ComboItem<string> { Text = "Writing",   Value = "WRITING" },
        new ComboItem<string> { Text = "Speaking",  Value = "SPEAKING" }
    };

            cboSkill.DataSource = skills;
            cboSkill.DisplayMember = "Text";
            cboSkill.ValueMember = "Value";
            cboSkill.SelectedIndex = -1;
        }

        private void LoadTimeLimitCombo()
        {
            var times = new List<ComboItem<int>>
    {
        new ComboItem<int> { Text = "15 phút", Value = 15 },
        new ComboItem<int> { Text = "30 phút", Value = 30 },
        new ComboItem<int> { Text = "60 phút", Value = 40 },
        new ComboItem<int> { Text = "60 phút", Value = 60 }
    };

            cboTimeLimit.DataSource = times;
            cboTimeLimit.DisplayMember = "Text";
            cboTimeLimit.ValueMember = "Value";
            cboTimeLimit.SelectedIndex = -1;
        }

        private void cboSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSkill.SelectedValue == null) return;

            switch (cboSkill.SelectedValue.ToString())
            {
                case "LISTENING":
                    cboTimeLimit.SelectedValue = 40;
                    break;
                case "READING":
                    cboTimeLimit.SelectedValue = 60;
                    break;
                case "WRITING":
                    cboTimeLimit.SelectedValue = 60;
                    break;
                case "SPEAKING":
                    cboTimeLimit.SelectedValue = 15;
                    break;
            }
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files|*.pdf";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedPdfPath = ofd.FileName;
                    txtFileName.Text = Path.GetFileName(selectedPdfPath);
                    LoadPdf(selectedPdfPath);
                }
            }
        }

        private void LoadPdf(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                bool ok = axAcroPDFViewer.LoadFile(path);

                if (ok)
                {
                    axAcroPDFViewer.setShowToolbar(true);
                    axAcroPDFViewer.setView("Fit");
                    MessageBox.Show("PDF loaded successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to load PDF!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading PDF: {ex.Message}");
            }
        }

        private void ClearPdf()
        {
            axAcroPDFViewer.src = "about:blank";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (cboSkill.SelectedIndex == -1 || cboTimeLimit.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn kỹ năng và thời gian");
                return;
            }

            string skill = cboSkill.SelectedValue.ToString();
            int timeLimit = (int)cboTimeLimit.SelectedValue;

            if (skill == "LISTENING" && string.IsNullOrWhiteSpace(selectedAudioPath))
            {
                MessageBox.Show("Listening bắt buộc phải có file audio!");
                return;
            }

            // ✅ Copy PDF
            string pdfPathInAssets = SaveFileToAssets(selectedPdfPath);

            // ✅ Copy Audio (nếu có)
            string audioPathInAssets = "";
            if (skill == "LISTENING")
            {
                audioPathInAssets = SaveFileToAssets(selectedAudioPath);
            }

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // 1️⃣ Check section tồn tại
                string checkSql = @"
            SELECT COUNT(*) 
            FROM TestSections 
            WHERE PaperId = @PaperId AND Skill = @Skill";

                using (var checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@PaperId", _paperId);
                    checkCmd.Parameters.AddWithValue("@Skill", skill);

                    int exists = (int)checkCmd.ExecuteScalar();
                    if (exists > 0)
                    {
                        MessageBox.Show($"Kỹ năng {skill} đã tồn tại!");
                        return;
                    }
                }

                // 2️⃣ Insert section
                string insertSql = @"
            INSERT INTO TestSections
            (PaperId, Skill, TimeLimitMinutes, PdfFilePath, AudioFilePath)
            VALUES
            (@PaperId, @Skill, @Time, @Pdf, @Audio)";

                using (var cmd = new SqlCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("@PaperId", _paperId);
                    cmd.Parameters.AddWithValue("@Skill", skill);
                    cmd.Parameters.AddWithValue("@Time", timeLimit);
                    cmd.Parameters.AddWithValue("@Pdf", pdfPathInAssets);
                    cmd.Parameters.AddWithValue("@Audio", audioPathInAssets);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("✅ Thêm section thành công!");
            DialogResult = DialogResult.OK;
            Close();

            showSectionControl.LoadSections();
        }


        private string SaveFileToAssets(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                return "";

            string assetsPath = Path.Combine(
                Application.StartupPath,
                "..", "..", "UI", "assets"
            );

            Directory.CreateDirectory(assetsPath);

            string ext = Path.GetExtension(sourcePath);
            string newFileName = $"{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid():N}{ext}";
            string destPath = Path.Combine(assetsPath, newFileName);

            File.Copy(sourcePath, destPath, true);

            // ✅ Lưu đường dẫn tương đối
            return Path.Combine("UI", "assets", newFileName);
        }

        private void btnChooseAudio_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Audio Files|*.mp3;*.wav;*.m4a;*.flac;*.aac";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedAudioPath = ofd.FileName;
                    MessageBox.Show($"Chọn file audio: {Path.GetFileName(selectedAudioPath)}");
                }
            }
        }

        

    }
}
