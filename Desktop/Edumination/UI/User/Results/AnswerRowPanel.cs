using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
    public partial class AnswerRowPanel : UserControl
    {
        public AnswerRowPanel()
        {
            InitializeComponent();
        }

        public void Bind(QuestionReview q)
        {
            if (q == null) return;

            // số câu
            lblNumber.Text = q.Number.ToString();

            // text user answer (giống mẫu: "1   Keiko:")
            lblUserAnswer.Text = $"{q.Number}.  {q.UserAnswer}";

            // đáp án đúng
            lblCorrectAnswer.Text = q.CorrectAnswer;

            // icon & màu đúng/sai
            if (q.IsCorrect)
            {
                lblIcon.Text = "✓";
                lblIcon.ForeColor = Color.FromArgb(0, 160, 80);
            }
            else
            {
                lblIcon.Text = "✕";
                lblIcon.ForeColor = Color.FromArgb(235, 85, 85);
            }
        }
    }
}
