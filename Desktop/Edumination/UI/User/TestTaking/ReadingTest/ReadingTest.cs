using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Edumination.Api.Domain.Entities;
using IELTS.DAL;
using IELTS.UI.User.Results;
using IELTS.UI.User.TestTaking.Controls;
using System.IO;

namespace IELTS.UI.User.TestTaking.ReadingTest
{
	public partial class ReadingTest : Form
	{
		private List<Question> _questions = new List<Question>();
		private Dictionary<long, string> _userAnswers = new Dictionary<long, string>();
		private readonly long _paperId;
		private readonly long _sectionId;

		private int _remainingSeconds;
		private readonly System.Windows.Forms.Timer _timer;

		private FlowLayoutPanel questionsPanel;

		public ReadingTest(long paperId, long sectionId)
		{
			_paperId = paperId;
			_sectionId = sectionId;

			InitializeComponent();

			WindowState = FormWindowState.Maximized;
			StartPosition = FormStartPosition.CenterScreen;

			_timer = new System.Windows.Forms.Timer();
			_timer.Interval = 1000;
			_timer.Tick += Timer_Tick;
		}

		public ReadingTest() : this(0, 0) { }

		private void ReadingTest_Load(object sender, EventArgs e)
		{
			if (pdfViewer == null || answerPanel == null || testNavBar == null)
			{
				MessageBox.Show("UI controls are not initialized properly.", "Error");
				Close();
				return;
			}

			testNavBar.OnExitRequested += TestNavBar_OnExitRequested;
			testNavBar.OnSubmitRequested += TestNavBar_OnSubmitRequested;

			if (_sectionId <= 0)
			{
				MessageBox.Show("No Reading section specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
				return;
			}

			if (!LoadSectionFromDatabase())
			{
				Close();
				return;
			}

			LoadQuestionsFromDatabase();
			DisplayQuestions();

			UpdateTimeLabel();
			_timer.Start();
		}

		private void TestNavBar_OnExitRequested()
		{
			var confirm = MessageBox.Show("Are you sure you want to exit?", "Exit Test", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (confirm == DialogResult.Yes)
			{
				_timer.Stop();
				Hide();
				new IELTS.UI.User.TestLibrary.TestLibrary().Show();
			}
		}

		private void TestNavBar_OnSubmitRequested()
		{
			var confirm = MessageBox.Show("Do you want to submit your answers now?", "Submit Test", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (confirm == DialogResult.Yes)
			{
				_timer.Stop();
				SubmitTest();
			}
		}

		private bool LoadSectionFromDatabase()
		{
			string skill = "READING";
			int? timeLimitMinutes = null;
			string pdfPath = null;

			try
			{
				using (var conn = DatabaseConnection.GetConnection())
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"SELECT Skill, TimeLimitMinutes, PdfFilePath FROM TestSections WHERE Id = @Id";
					var p = cmd.CreateParameter();
					p.ParameterName = "@Id";
					p.Value = _sectionId;
					cmd.Parameters.Add(p);

					conn.Open();
					using (var r = cmd.ExecuteReader())
					{
						if (!r.Read()) return false;
						if (r["Skill"] != DBNull.Value) skill = r["Skill"].ToString();
						if (r["TimeLimitMinutes"] != DBNull.Value) timeLimitMinutes = Convert.ToInt32(r["TimeLimitMinutes"]);
						if (r["PdfFilePath"] != DBNull.Value) pdfPath = r["PdfFilePath"].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading section: " + ex.Message, "DB Error");
				return false;
			}

			_remainingSeconds = (timeLimitMinutes ?? 60) * 60;
			this.Text = $"IELTS Reading – {skill}";

			// HIỂN THỊ TRỰC TIẾP FILE PDF GỐC (KHÔNG CẮT)
			try
			{
				if (!string.IsNullOrEmpty(pdfPath))
				{
					pdfViewer.ShowPdf(pdfPath, $"{skill} Section", "No PDF available.");
				}
			}
			catch { /* Viewer Error Handle */ }

			return true;
		}

		private void LoadQuestionsFromDatabase()
		{
			_questions.Clear();
			try
			{
				using (var conn = DatabaseConnection.GetConnection())
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = @"SELECT Id, QuestionText, QuestionType FROM Questions WHERE SectionId = @SectionId ORDER BY Position";
					var p = cmd.CreateParameter();
					p.ParameterName = "@SectionId";
					p.Value = _sectionId;
					cmd.Parameters.Add(p);

					conn.Open();
					using (var r = cmd.ExecuteReader())
					{
						while (r.Read())
						{
							_questions.Add(new Question
							{
								Id = Convert.ToInt64(r["Id"]),
								Text = r["QuestionText"]?.ToString() ?? "",
								Type = r["QuestionType"]?.ToString() ?? ""
							});
						}
					}
				}
			}
			catch (Exception ex) { MessageBox.Show("Error loading questions: " + ex.Message); }
		}

		private void DisplayQuestions()
		{
			if (questionsPanel == null)
			{
				questionsPanel = new FlowLayoutPanel
				{
					Dock = DockStyle.Fill,
					AutoScroll = true,
					FlowDirection = FlowDirection.TopDown,
					WrapContents = false,
					Padding = new Padding(10)
				};
				answerPanel.Controls.Clear();
				answerPanel.Controls.Add(questionsPanel);
			}

			questionsPanel.Controls.Clear();
			int qNum = 1;
			foreach (var q in _questions)
			{
				var table = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, Width = questionsPanel.Width - 30, Margin = new Padding(0, 0, 0, 20) };
				table.Controls.Add(new Label { Text = $"Question {qNum}", Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.Blue, AutoSize = true }, 0, 0);
				table.Controls.Add(new Label { Text = q.Text, Font = new Font("Segoe UI", 10), AutoSize = true, MaximumSize = new Size(table.Width, 0) }, 0, 1);

				var txt = new TextBox { Width = 250, Tag = q.Id, Font = new Font("Segoe UI", 10) };
				txt.TextChanged += (s, e) => { _userAnswers[q.Id] = ((TextBox)s).Text; };
				table.Controls.Add(txt, 0, 2);

				questionsPanel.Controls.Add(table);
				qNum++;
			}
		}

		private void SubmitTest()
		{
			string currentName = IELTS.UI.User.SessionManager.FullName;
			if (currentName == "Student") currentName = "Trần Tú";

			var finalResult = new ExamResult
			{
				Skill = "Reading",
				UserName = currentName,
				TimeTakenSeconds = 3600 - _remainingSeconds,
				TotalQuestions = _questions.Count,
				Parts = new List<PartReview>()
			};

			for (int p = 1; p <= 4; p++)
			{
				var part = new PartReview { PartName = $"Part {p}", Questions = new List<QuestionReview>() };
				var partQs = _questions.Skip((p - 1) * 10).Take(10).ToList();

				foreach (var q in partQs)
				{
					_userAnswers.TryGetValue(q.Id, out string userAns);
					string correctAns = GetCorrectAnswerFromDB(q.Id);
					bool isCorrect = string.Equals(userAns?.Trim(), correctAns?.Trim(), StringComparison.OrdinalIgnoreCase);
					if (isCorrect) finalResult.CorrectCount++;

					part.Questions.Add(new QuestionReview
					{
						Number = _questions.IndexOf(q) + 1,
						UserAnswer = userAns ?? "",
						CorrectAnswer = correctAns,
						IsCorrect = isCorrect
					});
				}
				finalResult.Parts.Add(part);
			}

			finalResult.Band = CalculateBandScore(finalResult.CorrectCount);
			new AnswerResultForm(finalResult).Show();
			this.Close();
		}

		private double CalculateBandScore(int correctAnswers)
		{
			if (correctAnswers >= 39) return 9.0;
			if (correctAnswers >= 30) return 7.0;
			if (correctAnswers >= 23) return 6.0;
			if (correctAnswers >= 15) return 5.0;
			return 0.0;
		}

		private string GetCorrectAnswerFromDB(long questionId)
		{
			string rawData = string.Empty;
			try
			{
				using (var conn = DatabaseConnection.GetConnection())
				{
					conn.Open();
					using (var cmd = conn.CreateCommand())
					{
						cmd.CommandText = @"SELECT ChoiceText FROM QuestionChoices WHERE QuestionId = @QId AND IsCorrect = 1";
						var p = cmd.CreateParameter(); p.ParameterName = "@QId"; p.Value = questionId;
						cmd.Parameters.Add(p);
						var res = cmd.ExecuteScalar();
						if (res != null) rawData = res.ToString();
					}
					if (string.IsNullOrEmpty(rawData))
					{
						using (var cmd = conn.CreateCommand())
						{
							cmd.CommandText = @"SELECT AnswerData FROM QuestionAnswerKeys WHERE QuestionId = @QId";
							var p = cmd.CreateParameter(); p.ParameterName = "@QId"; p.Value = questionId;
							cmd.Parameters.Add(p);
							var res = cmd.ExecuteScalar();
							if (res != null) rawData = res.ToString();
						}
					}
				}
			}
			catch { }
			return ParseJsonAnswer(rawData);
		}

		private string ParseJsonAnswer(string json)
		{
			if (string.IsNullOrWhiteSpace(json)) return "N/A";
			// Xử lý làm sạch JSON từ AI
			if (json.Contains(":"))
			{
				try
				{
					int colonIndex = json.IndexOf(':');
					return json.Substring(colonIndex + 1).Replace("}", "").Replace("{", "").Replace("\"", "").Trim();
				}
				catch { return json; }
			}
			return json;
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			_remainingSeconds--;
			if (_remainingSeconds <= 0) { _timer.Stop(); SubmitTest(); }
			UpdateTimeLabel();
		}

		private void UpdateTimeLabel()
		{
			int min = Math.Max(0, _remainingSeconds / 60);
			int sec = Math.Max(0, _remainingSeconds % 60);
			this.Text = $"IELTS Reading – {min:D2}:{sec:D2} remaining";
		}
	}
}