using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Results
{
    public partial class AnswerRowPanel : UserControl
    {
        public AnswerRowPanel() => InitializeComponent();

        public void Bind(QuestionReview q)
        {
            // 1. Số thứ tự
            btnNo.Text = q.Number.ToString();

            // 2. Câu trả lời của user
            string displayAnswer = string.IsNullOrWhiteSpace(q.UserAnswer)
                ? "no answer"
                : q.UserAnswer;

            lblUserAns.Text = $"{displayAnswer}";

            // 3. Icon + Correct Hint
            if (q.IsCorrect)
            {
                lblIcon.Text = "✓";
                lblIcon.ForeColor = Color.Green;
                lblCorrectHint.Visible = false; // ✅ Ẩn khi đúng
            }
            else
            {
                lblIcon.Text = "✕";
                lblIcon.ForeColor = Color.Red;

                // ✅ Hiển thị đầy đủ đáp án đúng
                string correctDisplay = string.IsNullOrWhiteSpace(q.CorrectAnswer)
                    ? "N/A"
                    : q.CorrectAnswer;

                lblCorrectHint.Text = $"Correct: {correctDisplay}";
                lblCorrectHint.Visible = true;
            }
        }
    }
}