using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
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
    public partial class NewShowQuestionCotrol : UserControl
    {

        private long _passageId;

        private readonly QuestionBLL _bll;
        private TestManagerControl _testManagerControl;
        private long? _selectedQuestionId = null;
        private int _selectedPosition = 0;
        private int _passagePosition;
        private PassageBLL _passageBll = new PassageBLL();

        public TestManagerControl TestManagerControl
        {
            get { return _testManagerControl; }
            set { _testManagerControl = value; }
        }

        public long PassageId
        {
            get => _passageId;
            set
            {
                _passageId = value;
                _passagePosition = _passageBll.GetPassagePosition(_passageId);
                LoadQuestionButtons();
            }
        }
        private long sectionId;
        public long SectionId
        {
            get => sectionId;
            set { sectionId = value; }
        }
        public NewShowQuestionCotrol()
        {
            InitializeComponent();
        }

        public void LoadQuestionButtons()
        {
            flpQuestions.Controls.Clear();

            if (_passagePosition <= 0 || sectionId <= 0)
                return;

            // 1️⃣ Xác định skill từ SectionId (hard-code)
            string skill = GetSkillBySectionId(sectionId);


            int start = 0;
            int end = 0;

            // 2️⃣ Tính range câu hỏi
            if (skill == "READING")
            {
                if (_passagePosition == 1)
                {
                    start = 1;
                    end = 13;
                }
                else if (_passagePosition == 2)
                {
                    start = 14;
                    end = 26;
                }
                else if (_passagePosition == 3)
                {
                    start = 27;
                    end = 40;
                }
            }
            else if (skill == "LISTENING")
            {
                start = (_passagePosition - 1) * 10 + 1;
                end = start + 9;
            }
            else
            {
                // Writing / Speaking để sau
                return;
            }

            // 3️⃣ Render button
            for (int i = start; i <= end; i++)
            {
                Button btn = new Button
                {
                    Width = 50,
                    Height = 50,
                    Text = i.ToString(),
                    Tag = i,
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Margin = new Padding(6),
                    Cursor = Cursors.Hand
                };

                btn.FlatAppearance.BorderSize = 0;

                btn.Click += (s, e) =>
                {
                    _selectedPosition = (int)((Button)s).Tag;

                    // highlight
                    foreach (Button b in flpQuestions.Controls.OfType<Button>())
                        b.BackColor = Color.LightGray;

                    ((Button)s).BackColor = Color.FromArgb(52, 152, 219);
                };

                flpQuestions.Controls.Add(btn);
            }
        }
        private string GetSkillBySectionId(long sectionId)
        {
            string skill = "";

            string connStr = @"Server=.;Database=Edumination;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
            SELECT Skill
            FROM TestSections
            WHERE Id = @SectionId
        ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SectionId", sectionId);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        skill = result.ToString();
                }
            }

            return skill;
        }


    }
}
