using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.Results
{
    public partial class AnswerRowPanel : UserControl
    {
        public AnswerRowPanel()
        {
            InitializeComponent();
        }

        public void Bind(QuestionReview q)
        {
            lblNumber.Text = q.Number.ToString();

            string user = string.IsNullOrWhiteSpace(q.UserAnswer)
                ? "(no answer)"
                : q.UserAnswer;

            lblUserAnswer.Text = $"Your answer: {user}";
            lblCorrectAnswer.Text = $"Correct: {q.CorrectAnswer}";

            if (q.IsCorrect)
            {
                lblIcon.Text = "✓";
                lblIcon.ForeColor = Color.FromArgb(0, 160, 80);
            }
            else
            {
                lblIcon.Text = "✗";
                lblIcon.ForeColor = Color.FromArgb(220, 70, 70);
            }
        }
    }
}
