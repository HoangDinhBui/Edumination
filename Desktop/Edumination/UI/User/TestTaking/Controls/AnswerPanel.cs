using IELTS.UI.User.TestTaking.ReadingTest;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IELTS.UI.User.TestTaking.Controls
{
    public partial class AnswerPanel : UserControl
    {
        public AnswerPanel()
        {
            InitializeComponent();
        }

        // Load 1 Part vào panel, bind lại câu đã trả lời (nếu có)
        public void LoadPart(ReadingPart part, Dictionary<int, string> userAnswers)
        {
            flowAnswers.Controls.Clear();

            foreach (var q in part.Questions)
            {
                flowAnswers.Controls.Add(BuildQuestionCard(q, userAnswers));
            }
        }

        private Control BuildQuestionCard(ReadingQuestion q, Dictionary<int, string> userAnswers)
        {
            var card = new UIPanel
            {
                Size = new Size(880, 110),
                Padding = new Padding(15),
                Radius = 20,
                FillColor = Color.White,
                RectColor = Color.FromArgb(39, 56, 146),
                RectSize = 2
            };

            var lbl = new UILabel
            {
                Text = $"{q.Number}. {q.Prompt}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 56, 146),
                Location = new Point(10, 10),
                AutoSize = true
            };
            card.Controls.Add(lbl);

            // --- FIX CS0165 ---
            string prev = "";
            if (userAnswers != null && userAnswers.ContainsKey(q.Number))
                prev = userAnswers[q.Number] ?? "";

            Control inputControl;

            if (q.Type == QuestionType.TrueFalse)
            {
                var cb = new UIComboBox
                {
                    Font = new Font("Segoe UI", 11F),
                    Size = new Size(200, 40),
                    Location = new Point(10, 55),
                    Radius = 20
                };
                cb.Items.Add("True");
                cb.Items.Add("False");

                var p = prev.Trim().ToLower();
                if (p.StartsWith("t")) cb.SelectedItem = "True";
                else if (p.StartsWith("f")) cb.SelectedItem = "False";

                inputControl = cb;
            }
            else
            {
                var txt = new UITextBox
                {
                    Font = new Font("Segoe UI", 11F),
                    Size = new Size(400, 40),
                    Location = new Point(10, 55),
                    Radius = 20,
                    Watermark = "Type your answer...",
                    Text = prev
                };
                inputControl = txt;
            }

            inputControl.Tag = q.Number;
            card.Controls.Add(inputControl);

            return card;
        }

        public Dictionary<int, string> CollectAnswers()
        {
            var dict = new Dictionary<int, string>();

            foreach (Control card in flowAnswers.Controls)
            {
                var input = card.Controls
                                .Cast<Control>()
                                .FirstOrDefault(c => c.Tag is int);

                if (input == null) continue;

                int number = (int)input.Tag;
                string value = "";

                if (input is UITextBox tb)
                    value = tb.Text.Trim();
                else if (input is UIComboBox cb)
                    value = cb.SelectedItem?.ToString().Trim() ?? "";

                dict[number] = value;
            }

            return dict;
        }
    }
}
