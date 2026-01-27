using Edumination.WinForms.UI.Admin.TestManager;
using IELTS.BLL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IELTS.BLL.QuestionBLL;

namespace IELTS.UI.Admin.TestManager
{
    public partial class ShowQuestionControl : UserControl
    {
        private long _passageId;
        private readonly QuestionService _service = new QuestionService();
        private TestManagerControl _testManagerControl;

        // ===== PROPERTY =====
        public long PassageId
        {
            get => _passageId;
            set
            {
                _passageId = value;
                LoadQuestions();   // ⭐ QUAN TRỌNG
            }
        }

        // ===== CONSTRUCTOR =====
        public ShowQuestionControl()
        {
            InitializeComponent();
        }
        public ShowQuestionControl(TestManagerControl testManagerControl)
        {
            _testManagerControl = testManagerControl;
            InitializeComponent();
        }

        public ShowQuestionControl(long passageId) : this()
        {
            PassageId = passageId;
        }

        // ===== LOAD =====
        private void LoadQuestions()
        {
            if (flpQuestions == null) return;

            flpQuestions.SuspendLayout();
            flpQuestions.Controls.Clear();

            var questions = _service.GetQuestionsByPassage(_passageId);

            foreach (var q in questions)
            {
                flpQuestions.Controls.Add(RenderQuestion(q));
            }

            flpQuestions.ResumeLayout();
        }
        private Control RenderQuestion(QuestionDTO q)
        {
            FlowLayoutPanel pnl = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Width = flpQuestions.Width - 30,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            pnl.Controls.Add(new Label
            {
                Text = q.QuestionText,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = true
            });

            switch (q.QuestionType)
            {
                case "MCQ":
                    RenderMCQ(pnl, q);
                    break;

                case "MULTI_SELECT":
                    RenderMulti(pnl, q);
                    break;

                case "MATCHING":
                    RenderMatching(pnl, q);
                    break;

                case "ORDER":
                    RenderOrder(pnl, q);
                    break;
            }

            return pnl;
        }

        private void RenderMCQ(Control parent, QuestionDTO q)
        {
            foreach (var opt in q.Options)
            {
                parent.Controls.Add(new RadioButton
                {
                    Text = $"{opt.OptionKey}. {opt.OptionText}",
                    AutoSize = true
                });
            }
        }

        private void RenderMulti(Control parent, QuestionDTO q)
        {
            foreach (var opt in q.Options)
            {
                parent.Controls.Add(new CheckBox
                {
                    Text = $"{opt.OptionKey}. {opt.OptionText}",
                    AutoSize = true
                });
            }
        }

        private void RenderMatching(Control parent, QuestionDTO q)
        {
            

            // container mỗi câu hỏi
            Panel wrapper = new Panel
            {
                AutoSize = true,
                Dock = DockStyle.Top
            };

            // ===== CỘT PHẢI (OPTIONS) =====
            FlowLayoutPanel rightPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Width = 300
            };

            foreach (var opt in q.Options)
            {
                rightPanel.Controls.Add(new Label
                {
                    Text = $"{opt.OptionKey}. {opt.OptionText}",
                    AutoSize = true
                });
            }

            // ===== CỘT TRÁI (QUESTIONS) =====
            FlowLayoutPanel leftPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Width = 350
            };

            foreach (var pair in q.MatchPairs)
            {
                leftPanel.Controls.Add(new Label
                {
                    Text = $"{pair.LeftKey}. {pair.LeftText}",
                    AutoSize = true
                });
            }

            // ===== GHÉP 2 CỘT =====
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));

            table.Controls.Add(leftPanel, 0, 0);
            table.Controls.Add(rightPanel, 1, 0);

            wrapper.Controls.Add(table);
            parent.Controls.Add(wrapper);
        }



        private void RenderOrder(Control parent, QuestionDTO q)
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            int max = q.Options.Count;

            for (int i = 0; i < q.Options.Count; i++)
            {
                ComboBox cbo = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Width = 60
                };

                for (int n = 1; n <= max; n++)
                    cbo.Items.Add(n);

                table.Controls.Add(cbo, 0, i);

                table.Controls.Add(new Label
                {
                    Text = q.Options[i].OptionText,
                    AutoSize = true
                }, 1, i);
            }

            parent.Controls.Add(table);
        }

    }

}
