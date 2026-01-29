using IELTS.DAL;
using Microsoft.Data.SqlClient;
using System;
using System.Windows.Forms;

namespace IELTS.UI.Admin.TestManager
{
    public partial class AddPassageForm : Form
    {
        private long _sectionId;
        private ShowPassageControl showPassageCotrol;

        public AddPassageForm(long sectionId, ShowPassageControl showPassageControl)
        {
            InitializeComponent();
            _sectionId = sectionId;
            this.showPassageCotrol = showPassageControl; // ✅ FIX
            LoadPassagePositions();
        }

        // ================= LOAD POSITION =================
        private void LoadPassagePositions()
        {
            string skill = GetSkillBySectionId(_sectionId);

            int max = skill switch
            {
                "READING" => 3,
                "SPEAKING" => 3,
                "WRITING" => 2,
                "LISTENING" => 4,
                _ => 0
            };

            cboPassagePosition.Items.Clear();
            for (int i = 1; i <= max; i++)
                cboPassagePosition.Items.Add(i);

            if (cboPassagePosition.Items.Count > 0)
                cboPassagePosition.SelectedIndex = 0;
        }

        // ================= CREATE =================
        private void btnCreate_Click(object sender, EventArgs e)
        {
            // 1️⃣ Validate
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Chưa nhập tiêu đề passage");
                return;
            }

            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("Chưa nhập nội dung passage");
                return;
            }

            if (cboPassagePosition.SelectedItem == null)
            {
                MessageBox.Show("Chưa chọn vị trí passage");
                return;
            }

            int position = (int)cboPassagePosition.SelectedItem;

            // 2️⃣ Check số lượng passage tối đa
            int current = CountPassages(_sectionId);
            int max = cboPassagePosition.Items.Count;

            if (current >= max)
            {
                MessageBox.Show("Section này đã đủ số passage");
                return;
            }

            // 3️⃣ Check trùng position
            if (IsPositionExist(_sectionId, position))
            {
                MessageBox.Show("Vị trí passage này đã tồn tại");
                return;
            }

            // 4️⃣ Insert DB
            InsertPassage(
                _sectionId,
                txtTitle.Text.Trim(),
                richTextBox1.Text.Trim(),
                position
            );

            MessageBox.Show("✅ Thêm passage thành công");

            showPassageCotrol?.LoadPassages(); // reload UI
            Close();
        }

        // ================= DB FUNCTIONS =================

        private string GetSkillBySectionId(long sectionId)
        {
            using var conn = DatabaseConnection.GetConnection();
            string sql = "SELECT Skill FROM TestSections WHERE Id = @Id";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", sectionId);
            conn.Open();
            return cmd.ExecuteScalar()?.ToString() ?? "";
        }

        private int CountPassages(long sectionId)
        {
            using var conn = DatabaseConnection.GetConnection();
            string sql = "SELECT COUNT(*) FROM Passages WHERE SectionId = @Id";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", sectionId);
            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        private bool IsPositionExist(long sectionId, int position)
        {
            using var conn = DatabaseConnection.GetConnection();
            string sql = @"
                SELECT COUNT(*) 
                FROM Passages 
                WHERE SectionId = @Id AND Position = @Pos";

            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", sectionId);
            cmd.Parameters.AddWithValue("@Pos", position);
            conn.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }

        private void InsertPassage(long sectionId, string title, string content, int position)
        {
            using var conn = DatabaseConnection.GetConnection();
            string sql = @"
                INSERT INTO Passages (SectionId, Title, ContentText, Position)
                VALUES (@SectionId, @Title, @ContentText, @Position)";

            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SectionId", sectionId);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@ContentText", content);
            cmd.Parameters.AddWithValue("@Position", position);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
