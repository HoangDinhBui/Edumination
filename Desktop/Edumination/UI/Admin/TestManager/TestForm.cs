using Microsoft.VisualBasic;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin.TestManager
{
    public class QuestionCardGenerator
    {
        public const int CARD_WIDTH = 700;
        public const int CARD_PADDING = 20;

        #region Main Generator Method

        /// <summary>
        /// Tạo card câu hỏi dựa theo type
        /// </summary>
        public static UIPanel CreateQuestionCard(int questionNumber, string questionType,
            string questionText, object optionsData = null, string savedAnswer = null)
        {
            UIPanel card = CreateBaseCard(questionNumber, questionText);

            ReaLTaiizor.Controls.Panel answerPanel = new ReaLTaiizor.Controls.Panel
            {
                Location = new Point(CARD_PADDING, 100 + 25),
                Width = CARD_WIDTH - CARD_PADDING * 2,
            };
            //int variableOfMultiChoise;
            switch (questionType)
            {
                case "MCQ":
                    CreateMCQAnswer(answerPanel, questionNumber, savedAnswer);
                    break;

                case "MULTI_SELECT":

                    int optionCount = optionsData != null ? (int)optionsData : 4;
                    CreateMultiSelectAnswer(answerPanel, questionNumber, optionCount, savedAnswer);
                    break;

                case "FILL_BLANK":
                    CreateFillBlankAnswer(answerPanel, questionNumber, savedAnswer);
                    break;

                case "MATCHING":
                    var matchData = optionsData as Dictionary<string, List<string>>;
                    CreateMatchingAnswer(answerPanel, questionNumber, matchData, savedAnswer);
                    break;

                case "ORDERING":
                    var orderItems = optionsData as List<string>;
                    CreateOrderingAnswer(answerPanel, questionNumber, orderItems, savedAnswer);
                    break;

                case "SHORT_ANSWER":
                    CreateShortAnswerAnswer(answerPanel, questionNumber, savedAnswer);
                    break;

                case "ESSAY":
                    CreateEssayAnswer(answerPanel, questionNumber, savedAnswer);
                    break;

                case "SPEAK_PROMPT":
                    CreateSpeakPromptAnswer(answerPanel, questionNumber, savedAnswer);
                    break;
            }

            card.Controls.Add(answerPanel);
            card.Height = answerPanel.Bottom + CARD_PADDING + 10;

            return card;
        }

        #endregion

        #region Base Card Creation

        public static UIPanel CreateBaseCard(int questionNumber, string questionText)
        {
            UIPanel card = new UIPanel
            {
                Width = CARD_WIDTH,
                FillColor = Color.White,
                RectColor = Color.FromArgb(220, 220, 220),
                Radius = 10,
                Margin = new Padding(10),
                Padding = new Padding(CARD_PADDING),
                Font = new Font("Segoe UI", 10F),
                RadiusSides = UICornerRadiusSides.All,
                RectSize = 2
            };

            UIPanel badge = new UIPanel
            {
                Size = new Size(50, 30),
                FillColor = Color.FromArgb(80, 160, 255),
                Radius = 5,
                RadiusSides = UICornerRadiusSides.All,
                Location = new Point(CARD_PADDING, CARD_PADDING)
            };

            UILabel lblNumber = new UILabel
            {
                Text = $"Q{questionNumber}",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };

            badge.Controls.Add(lblNumber);
            card.Controls.Add(badge);

            UIRichTextBox txtQuestion = new UIRichTextBox
            {
                Location = new Point(CARD_PADDING, badge.Bottom + 10),
                Width = CARD_WIDTH - CARD_PADDING * 2,
                Height = 60,
                Text = questionText,
                Font = new Font("Segoe UI", 10F),
                ReadOnly = true,
                BackColor = Color.White,
                FillColor = Color.White
            };

            card.Controls.Add(txtQuestion);
            card.Height = txtQuestion.Bottom + CARD_PADDING;

            return card;
        }

        #endregion

        #region MCQ - Multiple Choice (A, B, C, D)

        public static void CreateMCQAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber, string savedAnswer = null)
        {
            string[] options = { "A", "B", "C", "D" };
            int yPos = 10;
            int panelWidth = panel.Width;
            int optionHeight = 35;
            int spacing = 10;

            panel.BackColor = Color.White;

            foreach (string opt in options)
            {
                UIRadioButton radio = new UIRadioButton
                {
                    Text = $"Option {opt}",
                    Location = new Point(10, yPos),
                    Width = panelWidth - 20,
                    Height = optionHeight,
                    Font = new Font("Segoe UI", 10F),
                    Name = $"radio_Q{questionNumber}_{opt}",
                    Tag = opt,
                    Cursor = Cursors.Hand,
                    AutoSize = false,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Padding = new Padding(5),
                    // ⭐ Set saved answer
                    Checked = !string.IsNullOrEmpty(savedAnswer) && savedAnswer.Trim().ToUpper() == opt
                };

                radio.MouseEnter += (s, e) => { radio.BackColor = Color.FromArgb(240, 248, 255); };
                radio.MouseLeave += (s, e) => { radio.BackColor = Color.White; };

                panel.Controls.Add(radio);
                yPos += optionHeight + spacing;
            }

            panel.Height = yPos + 5;
        }

        #endregion

        #region MULTI_SELECT - Multiple Choice with Checkboxes

        private static void CreateMultiSelectAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber, int optionCount, string savedAnswer = null)
        {
            // savedAnswer format: "A,C,E"
            List<string> selectedOptions = new List<string>();
            if (!string.IsNullOrEmpty(savedAnswer))
            {
                selectedOptions = savedAnswer.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                             .Select(x => x.Trim().ToUpper())
                                             .ToList();
            }

            int yPos = 10;
            int panelWidth = panel.Width;
            int optionHeight = 35;
            int spacing = 10;

            panel.BackColor = Color.White;

            for (int i = 0; i < optionCount; i++)
            {
                char letter = (char)('A' + i);
                string letterStr = letter.ToString();

                UICheckBox checkbox = new UICheckBox
                {
                    Text = $"Option {letter}",
                    Location = new Point(10, yPos),
                    Width = panelWidth - 20,
                    Height = optionHeight,
                    Font = new Font("Segoe UI", 10F),
                    Name = $"chk_Q{questionNumber}_{letter}",
                    Tag = letterStr,
                    AutoSize = false,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                    Padding = new Padding(5),
                    Cursor = Cursors.Hand,
                    // ⭐ Set saved answer
                    Checked = selectedOptions.Contains(letterStr)
                };

                checkbox.MouseEnter += (s, e) => { checkbox.BackColor = Color.FromArgb(240, 248, 255); };
                checkbox.MouseLeave += (s, e) => { checkbox.BackColor = Color.White; };

                panel.Controls.Add(checkbox);
                yPos += optionHeight + spacing;
            }

            panel.Height = yPos + 5;
        }

        #endregion

        #region FILL_BLANK - Text Input

        private static void CreateFillBlankAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber, string savedAnswer = null)
        {
            int yPos = 10;
            int panelWidth = panel.Width;
            int txtHeight = 35;

            panel.BackColor = Color.White;

            UITextBox txtAnswer = new UITextBox
            {
                Location = new Point(10, yPos),
                Width = panelWidth - 20,
                Height = txtHeight,
                Font = new Font("Segoe UI", 10F),
                Name = $"txt_Q{questionNumber}_answer",
                Watermark = "Type your answer here...",
                Radius = 5,
                Cursor = Cursors.IBeam,
                BackColor = Color.White,
                ForeColor = Color.Black,
                // ⭐ Set saved answer
                Text = savedAnswer ?? ""
            };

            txtAnswer.MouseEnter += (s, e) => { txtAnswer.BackColor = Color.FromArgb(240, 248, 255); };
            txtAnswer.MouseLeave += (s, e) => { txtAnswer.BackColor = Color.White; };

            panel.Controls.Add(txtAnswer);
            panel.Height = yPos + txtHeight + 10;
        }

        #endregion

        #region MATCHING - Drag & Drop Style

        private static void CreateMatchingAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber,
            Dictionary<string, List<string>> matchData, string savedAnswer = null)
        {
            // savedAnswer format: "Dog:Bark;Cat:Meow"
            Dictionary<string, string> savedMatches = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(savedAnswer))
            {
                foreach (var pair in savedAnswer.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var parts = pair.Split(':');
                    if (parts.Length == 2)
                    {
                        savedMatches[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }

            if (matchData == null || !matchData.Any())
            {
                matchData = new Dictionary<string, List<string>>
                {
                    { "Item 1", new List<string> { "A", "B", "C" } },
                    { "Item 2", new List<string> { "A", "B", "C" } },
                    { "Item 3", new List<string> { "A", "B", "C" } }
                };
            }

            int yPos = 10;
            int itemLabelWidth = 250;
            int arrowWidth = 30;
            int comboWidth = 150;
            int spacing = 10;

            panel.BackColor = Color.White;

            foreach (var item in matchData)
            {
                UILabel lblItem = new UILabel
                {
                    Text = item.Key,
                    Location = new Point(0, yPos),
                    Size = new Size(itemLabelWidth, 30),
                    Font = new Font("Segoe UI", 10F),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                };

                UILabel lblArrow = new UILabel
                {
                    Text = "→",
                    Location = new Point(itemLabelWidth + 10, yPos),
                    Size = new Size(arrowWidth, 30),
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(80, 160, 255),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                UIComboBox cmbMatch = new UIComboBox
                {
                    Location = new Point(itemLabelWidth + arrowWidth + 20, yPos),
                    Width = comboWidth,
                    Height = 35,
                    Font = new Font("Segoe UI", 10F),
                    Name = $"cmb_Q{questionNumber}_{item.Key.Replace(" ", "_")}",
                    DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList,
                    Radius = 5,
                    Cursor = Cursors.Hand,
                    FillColor = Color.White,
                    ForeColor = Color.Black
                };

                cmbMatch.Items.Add("-- Select --");
                foreach (var option in item.Value)
                    cmbMatch.Items.Add(option);

                // ⭐ Set saved answer
                string itemKey = item.Key.Replace(" ", "_");
                if (savedMatches.ContainsKey(itemKey) || savedMatches.ContainsKey(item.Key))
                {
                    string savedValue = savedMatches.ContainsKey(itemKey)
                        ? savedMatches[itemKey]
                        : savedMatches[item.Key];

                    int index = cmbMatch.Items.IndexOf(savedValue);
                    if (index >= 0)
                    {
                        cmbMatch.SelectedIndex = index;
                    }
                    else
                    {
                        cmbMatch.SelectedIndex = 0;
                    }
                }
                else
                {
                    cmbMatch.SelectedIndex = 0;
                }

                panel.Controls.Add(lblItem);
                panel.Controls.Add(lblArrow);
                panel.Controls.Add(cmbMatch);

                yPos += 45;
            }

            panel.Height = yPos + spacing;
        }

        #endregion

        #region ORDERING - Sortable List

        private static void CreateOrderingAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber, List<string> items, string savedAnswer = null)
        {
            // savedAnswer format: "Bark|Meow|C|D|E|F"
            List<string> orderedItems = new List<string>();

            if (!string.IsNullOrEmpty(savedAnswer))
            {
                // ⭐ Use saved order
                orderedItems = savedAnswer.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(x => x.Trim())
                                          .ToList();
            }
            else if (items != null && items.Any())
            {
                orderedItems = new List<string>(items);
            }
            else
            {
                orderedItems = new List<string> { "First item", "Second item", "Third item", "Fourth item" };
            }

            int panelWidth = panel.Width;
            int listHeight = orderedItems.Count * 30 + 10;
            int spacing = 10;

            panel.BackColor = Color.White;

            ListBox listBox = new ListBox
            {
                Location = new Point(10, 10),
                Width = panelWidth - 70,
                Height = listHeight,
                Font = new Font("Segoe UI", 10F),
                Name = $"lst_Q{questionNumber}_order",
                BorderStyle = BorderStyle.FixedSingle
            };

            foreach (var item in orderedItems)
                listBox.Items.Add(item);

            UISymbolButton btnUp = new UISymbolButton
            {
                Location = new Point(listBox.Right + 10, listBox.Top),
                Size = new Size(40, 40),
                Symbol = 61514,
                Radius = 5,
                Cursor = Cursors.Hand
            };

            UISymbolButton btnDown = new UISymbolButton
            {
                Location = new Point(listBox.Right + 10, listBox.Top + 50),
                Size = new Size(40, 40),
                Symbol = 61515,
                Radius = 5,
                Cursor = Cursors.Hand
            };

            btnUp.MouseEnter += (s, e) => btnUp.FillColor = Color.FromArgb(240, 248, 255);
            btnUp.MouseLeave += (s, e) => btnUp.FillColor = Color.White;
            btnDown.MouseEnter += (s, e) => btnDown.FillColor = Color.FromArgb(240, 248, 255);
            btnDown.MouseLeave += (s, e) => btnDown.FillColor = Color.White;

            btnUp.Click += (s, e) => MoveListBoxItem(listBox, -1);
            btnDown.Click += (s, e) => MoveListBoxItem(listBox, 1);

            panel.Controls.Add(listBox);
            panel.Controls.Add(btnUp);
            panel.Controls.Add(btnDown);

            panel.Height = listBox.Bottom + spacing;
        }

        private static void MoveListBoxItem(ListBox listBox, int direction)
        {
            if (listBox.SelectedItem == null || listBox.SelectedIndex < 0)
                return;

            int newIndex = listBox.SelectedIndex + direction;
            if (newIndex < 0 || newIndex >= listBox.Items.Count)
                return;

            object selected = listBox.SelectedItem;
            listBox.Items.RemoveAt(listBox.SelectedIndex);
            listBox.Items.Insert(newIndex, selected);
            listBox.SelectedIndex = newIndex;
        }

        #endregion

        #region SHORT_ANSWER - Single Line Text

        private static void CreateShortAnswerAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber, string savedAnswer = null)
        {
            int panelWidth = panel.Width;
            int txtHeight = 35;
            int spacing = 10;

            panel.BackColor = Color.White;

            UITextBox txtAnswer = new UITextBox
            {
                Location = new Point(10, 10),
                Width = panelWidth - 20,
                Height = txtHeight,
                Font = new Font("Segoe UI", 10F),
                Name = $"txt_Q{questionNumber}_short",
                Watermark = "Enter short answer (1-2 sentences)...",
                MaxLength = 200,
                Radius = 5,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Cursor = Cursors.IBeam,
                // ⭐ Set saved answer
                Text = savedAnswer ?? ""
            };

            txtAnswer.MouseEnter += (s, e) => { txtAnswer.BackColor = Color.FromArgb(240, 248, 255); };
            txtAnswer.MouseLeave += (s, e) => { txtAnswer.BackColor = Color.White; };

            UILabel lblCount = new UILabel
            {
                Location = new Point(10, txtAnswer.Bottom + 5),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.Gray,
                Text = $"{txtAnswer.Text.Length} / 200 characters"
            };

            txtAnswer.TextChanged += (s, e) =>
            {
                lblCount.Text = $"{txtAnswer.Text.Length} / 200 characters";
            };

            panel.Controls.Add(txtAnswer);
            panel.Controls.Add(lblCount);

            panel.Height = txtAnswer.Height + lblCount.Height + spacing * 2;
        }

        #endregion

        #region ESSAY - Rich Text Area

        private static void CreateEssayAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber, string savedAnswer = null)
        {
            int panelWidth = panel.Width;
            int txtHeight = 200;
            int spacing = 10;

            panel.BackColor = Color.White;

            UIRichTextBox txtEssay = new UIRichTextBox
            {
                Location = new Point(10, 10),
                Width = panelWidth - 20,
                Height = txtHeight,
                Font = new Font("Segoe UI", 10F),
                Name = $"rtb_Q{questionNumber}_essay",
                FillColor = Color.White,
                ForeColor = Color.Black,
                Cursor = Cursors.IBeam,
                // ⭐ Set saved answer
                Text = savedAnswer ?? ""
            };

            txtEssay.MouseEnter += (s, e) => txtEssay.FillColor = Color.FromArgb(240, 248, 255);
            txtEssay.MouseLeave += (s, e) => txtEssay.FillColor = Color.White;

            int initialWordCount = 0;
            if (!string.IsNullOrEmpty(savedAnswer))
            {
                initialWordCount = savedAnswer.Split(new[] { ' ', '\n', '\r' },
                    StringSplitOptions.RemoveEmptyEntries).Length;
            }

            UILabel lblWordCount = new UILabel
            {
                Location = new Point(10, txtEssay.Bottom + 5),
                Size = new Size(250, 20),
                Font = new Font("Segoe UI", 8F),
                ForeColor = initialWordCount >= 250 ? Color.Green : Color.Gray,
                Text = $"Words: {initialWordCount} | Minimum: 250"
            };

            txtEssay.TextChanged += (s, e) =>
            {
                int wordCount = txtEssay.Text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                lblWordCount.Text = $"Words: {wordCount} | Minimum: 250";
                lblWordCount.ForeColor = wordCount >= 250 ? Color.Green : Color.Gray;
            };

            panel.Controls.Add(txtEssay);
            panel.Controls.Add(lblWordCount);

            panel.Height = txtEssay.Height + lblWordCount.Height + spacing * 2;
        }

        #endregion

        #region SPEAK_PROMPT - Audio Recording

        private static void CreateSpeakPromptAnswer(ReaLTaiizor.Controls.Panel panel, int questionNumber, string savedAnswer = null)
        {
            // savedAnswer = path to audio file
            bool hasRecording = !string.IsNullOrEmpty(savedAnswer) && System.IO.File.Exists(savedAnswer);

            UILabel lblStatus = new UILabel
            {
                Location = new Point(0, 0),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = hasRecording ? Color.Green : Color.FromArgb(80, 160, 255),
                Text = hasRecording ? "✓ Recording available" : "⏺ Click 'Record' to start speaking",
                TextAlign = ContentAlignment.MiddleLeft
            };

            UISymbolButton btnRecord = new UISymbolButton
            {
                Location = new Point(0, 40),
                Size = new Size(120, 45),
                Symbol = 61511,
                Text = "Record",
                Radius = 8,
                Font = new Font("Segoe UI", 10F)
            };

            UISymbolButton btnStop = new UISymbolButton
            {
                Location = new Point(130, 40),
                Size = new Size(120, 45),
                Symbol = 61516,
                Text = "Stop",
                Radius = 8,
                Enabled = false,
                Font = new Font("Segoe UI", 10F)
            };

            UISymbolButton btnPlay = new UISymbolButton
            {
                Location = new Point(260, 40),
                Size = new Size(120, 45),
                Symbol = 61515,
                Text = "Play",
                Radius = 8,
                Enabled = hasRecording,
                Font = new Font("Segoe UI", 10F)
            };

            UILabel lblTimer = new UILabel
            {
                Location = new Point(390, 45),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.Red,
                Text = "00:00",
                TextAlign = ContentAlignment.MiddleCenter
            };

            TextBox txtAudioPath = new TextBox
            {
                Name = $"txt_Q{questionNumber}_audio",
                Visible = false,
                // ⭐ Set saved answer
                Text = savedAnswer ?? ""
            };

            btnRecord.Click += (s, e) =>
            {
                lblStatus.Text = "🔴 Recording...";
                lblStatus.ForeColor = Color.Red;
                btnRecord.Enabled = false;
                btnStop.Enabled = true;
                btnPlay.Enabled = false;
                // TODO: Start audio recording logic
            };

            btnStop.Click += (s, e) =>
            {
                lblStatus.Text = "✓ Recording saved";
                lblStatus.ForeColor = Color.Green;
                btnRecord.Enabled = true;
                btnStop.Enabled = false;
                btnPlay.Enabled = true;
                // TODO: Stop recording and save file
            };

            btnPlay.Click += (s, e) =>
            {
                lblStatus.Text = "▶ Playing...";
                lblStatus.ForeColor = Color.Blue;
                // TODO: Play recorded audio from txtAudioPath.Text
            };

            panel.Controls.Add(lblStatus);
            panel.Controls.Add(btnRecord);
            panel.Controls.Add(btnStop);
            panel.Controls.Add(btnPlay);
            panel.Controls.Add(lblTimer);
            panel.Controls.Add(txtAudioPath);

            panel.Height = 100;
        }

        #endregion

        #region Get Student Answer Methods

        public static string GetStudentAnswer(UIPanel card, string questionType)
        {
            switch (questionType)
            {
                case "MCQ":
                    return GetMCQAnswer(card);
                case "MULTI_SELECT":
                    return GetMultiSelectAnswer(card);
                case "FILL_BLANK":
                case "SHORT_ANSWER":
                    return GetTextAnswer(card);
                case "MATCHING":
                    return GetMatchingAnswer(card);
                case "ORDERING":
                    return GetOrderingAnswer(card);
                case "ESSAY":
                    return GetEssayAnswer(card);
                case "SPEAK_PROMPT":
                    return GetSpeakPromptAnswer(card);
                default:
                    return string.Empty;
            }
        }

        private static string GetMCQAnswer(UIPanel card)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is ReaLTaiizor.Controls.Panel panel)
                {
                    foreach (Control innerCtrl in panel.Controls)
                    {
                        if (innerCtrl is UIRadioButton radio && radio.Checked)
                        {
                            return radio.Tag?.ToString() ?? "";
                        }
                    }
                }
            }
            return "";
        }

        private static string GetMultiSelectAnswer(UIPanel card)
        {
            List<string> answers = new List<string>();
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is ReaLTaiizor.Controls.Panel panel)
                {
                    foreach (Control innerCtrl in panel.Controls)
                    {
                        if (innerCtrl is UICheckBox checkbox && checkbox.Checked)
                        {
                            answers.Add(checkbox.Tag?.ToString() ?? "");
                        }
                    }
                }
            }
            return string.Join(",", answers);
        }

        private static string GetTextAnswer(UIPanel card)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is ReaLTaiizor.Controls.Panel panel)
                {
                    foreach (Control innerCtrl in panel.Controls)
                    {
                        if (innerCtrl is UITextBox textBox)
                        {
                            return textBox.Text;
                        }
                    }
                }
            }
            return "";
        }

        private static string GetMatchingAnswer(UIPanel card)
        {
            Dictionary<string, string> matches = new Dictionary<string, string>();
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is ReaLTaiizor.Controls.Panel panel)
                {
                    foreach (Control innerCtrl in panel.Controls)
                    {
                        if (innerCtrl is UIComboBox cmb && cmb.SelectedIndex > 0)
                        {
                            string key = cmb.Name.Replace("cmb_Q", "").Split('_')[1];
                            matches[key] = cmb.SelectedItem.ToString();
                        }
                    }
                }
            }
            return string.Join(";", matches.Select(x => $"{x.Key}:{x.Value}"));
        }

        private static string GetOrderingAnswer(UIPanel card)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is ReaLTaiizor.Controls.Panel panel)
                {
                    foreach (Control innerCtrl in panel.Controls)
                    {
                        if (innerCtrl is ListBox listBox)
                        {
                            return string.Join("|", listBox.Items.Cast<string>());
                        }
                    }
                }
            }
            return "";
        }

        private static string GetEssayAnswer(UIPanel card)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is ReaLTaiizor.Controls.Panel panel)
                {
                    foreach (Control innerCtrl in panel.Controls)
                    {
                        if (innerCtrl is UIRichTextBox richText)
                        {
                            return richText.Text;
                        }
                    }
                }
            }
            return "";
        }

        private static string GetSpeakPromptAnswer(UIPanel card)
        {
            foreach (Control ctrl in card.Controls)
            {
                if (ctrl is ReaLTaiizor.Controls.Panel panel)
                {
                    foreach (Control innerCtrl in panel.Controls)
                    {
                        if (innerCtrl is TextBox txt && innerCtrl.Name.Contains("audio"))
                        {
                            return txt.Text;
                        }
                    }
                }
            }
            return "";
        }

        #endregion
    }

    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            this.Width = 800;
            this.Height = 600;
            this.BackColor = Color.White;
            this.Text = "Test Form - Review Mode";

            Panel scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };
            this.Controls.Add(scrollPanel);

            int yPos = 10;

            //// ✅ Test MCQ với saved answer
            //UIPanel mcqCard = QuestionCardGenerator.CreateQuestionCard(
            //    1,
            //    "MCQ",
            //    "What is the capital of France?",
            //    null,
            //    "B"  // ⭐ Saved answer - Radio B sẽ được chọn sẵn
            //);
            //mcqCard.Location = new Point(10, yPos);
            //scrollPanel.Controls.Add(mcqCard);
            //yPos += mcqCard.Height + 20;

            // ✅ Test MULTI_SELECT với saved answer
            UIPanel multiSelectCard = QuestionCardGenerator.CreateQuestionCard(
                2,
                "MULTI_SELECT",
                "Select all prime numbers.",
                5,
                "A,C,E"  // ⭐ Saved answer - Checkbox A, C, E sẽ được chọn sẵn
            );
            multiSelectCard.Location = new Point(10, yPos);
            scrollPanel.Controls.Add(multiSelectCard);
            yPos += multiSelectCard.Height + 20;

            // ✅ Test Fill-in-the-Blank với saved answer
            //UIPanel fillBlankCard = QuestionCardGenerator.CreateQuestionCard(
            //    3,
            //    "FILL_BLANK",
            //    "Fill in the blank: What is the capital of France?",
            //    null,
            //    "Paris"  // ⭐ Saved answer - TextBox sẽ có sẵn "Paris"
            //);
            //fillBlankCard.Location = new Point(10, yPos);
            //scrollPanel.Controls.Add(fillBlankCard);
            //yPos += fillBlankCard.Height + 20;

            //// ✅ Test MATCHING với saved answer
            //var matchData = new Dictionary<string, List<string>>
            //{
            //    { "Dog", new List<string> { "Bark", "Meow", "Roar" } },
            //    { "Cat", new List<string> { "Bark", "Meow", "Roar" } },
            //    { "Lion", new List<string> { "Bark", "Meow", "Roar" } }
            //};

            //            UIPanel matchingCard = QuestionCardGenerator.CreateQuestionCard(
            //                4,
            //                "MATCHING",
            //                "Match the animals with their sounds.",
            //                matchData,
            //                "Dog:Bark;Cat:Meow;Lion:Roar"  // ⭐ Saved answer - ComboBox sẽ chọn sẵn
            //            );
            //            matchingCard.Location = new Point(10, yPos);
            //            scrollPanel.Controls.Add(matchingCard);
            //            yPos += matchingCard.Height + 20;

            //            // ✅ Test ORDERING với saved answer
            //            var orderItems = new List<string> { "First", "Second", "Third", "Fourth", "Fifth", "Sixth" };

            //            UIPanel orderingCard = QuestionCardGenerator.CreateQuestionCard(
            //                5,
            //                "ORDERING",
            //                "Arrange these items in alphabetical order.",
            //                orderItems,
            //                "Fifth|First|Fourth|Second|Sixth|Third"  // ⭐ Saved answer - ListBox sắp theo thứ tự này
            //            );
            //            orderingCard.Location = new Point(10, yPos);
            //            scrollPanel.Controls.Add(orderingCard);
            //            yPos += orderingCard.Height + 20;

            //            // ✅ Test SHORT_ANSWER với saved answer
            //            UIPanel shortAnswerCard = QuestionCardGenerator.CreateQuestionCard(
            //                6,
            //                "SHORT_ANSWER",
            //                "Write a short answer about your favorite color.",
            //                null,
            //                "My favorite color is blue because it reminds me of the ocean."  // ⭐ Saved answer
            //            );
            //            shortAnswerCard.Location = new Point(10, yPos);
            //            scrollPanel.Controls.Add(shortAnswerCard);
            //            yPos += shortAnswerCard.Height + 20;

            //            // ✅ Test ESSAY với saved answer
            //            string savedEssay = @"This is my essay about my favorite hobby. 
            //I love reading books because it helps me learn new things and escape to different worlds. 
            //Reading has been a passion of mine since childhood, and I enjoy various genres including fiction, non-fiction, and poetry. 
            //Through reading, I have developed better vocabulary, improved my writing skills, and gained knowledge about different cultures and perspectives. 
            //I believe that reading is one of the most valuable habits anyone can develop. It opens doors to endless possibilities and enriches our understanding of the world around us. 
            //Whether it's a thrilling mystery, an inspiring biography, or a thought-provoking philosophical work, each book offers something unique. 
            //In my free time, I often visit libraries and bookstores to discover new titles. 
            //I also participate in book clubs where I can discuss my favorite reads with like-minded individuals. 
            //Reading is not just a hobby for me; it's a lifelong journey of learning and personal growth. 
            //I encourage everyone to pick up a book and experience the magic of reading for themselves.";

            //            UIPanel essayCard = QuestionCardGenerator.CreateQuestionCard(
            //                7,
            //                "ESSAY",
            //                "Write an essay about your favorite hobby.",
            //                null,
            //                savedEssay  // ⭐ Saved answer - RichTextBox có sẵn toàn bộ essay
            //            );
            //            essayCard.Location = new Point(10, yPos);
            //            scrollPanel.Controls.Add(essayCard);
            //            yPos += essayCard.Height + 20;

            //            // ✅ Test SPEAK_PROMPT với saved answer
            //            UIPanel speakCard = QuestionCardGenerator.CreateQuestionCard(
            //                8,
            //                "SPEAK_PROMPT",
            //                "Speak about your hometown for 2 minutes.",
            //                null,
            //                @"C:\recordings\question8_audio.mp3"  // ⭐ Saved answer - Nút Play sẽ được enable
            //            );
            //            speakCard.Location = new Point(10, yPos);
            //            scrollPanel.Controls.Add(speakCard);
            //            yPos += speakCard.Height + 20;

            // Button để lấy tất cả câu trả lời
            UIButton btnGetAnswers = new UIButton
            {
                Text = "Get All Answers",
                Location = new Point(10, yPos),
                Size = new Size(200, 45),
                Radius = 8,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            btnGetAnswers.Click += (s, e) =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=== STUDENT ANSWERS ===\n");

                //sb.AppendLine($"Q1 (MCQ): {QuestionCardGenerator.GetStudentAnswer(mcqCard, "MCQ")}");
                sb.AppendLine($"Q2 (MULTI_SELECT): {QuestionCardGenerator.GetStudentAnswer(multiSelectCard, "MULTI_SELECT")}");
                //sb.AppendLine($"Q3 (FILL_BLANK): {QuestionCardGenerator.GetStudentAnswer(fillBlankCard, "FILL_BLANK")}");
                //sb.AppendLine($"Q4 (MATCHING): {QuestionCardGenerator.GetStudentAnswer(matchingCard, "MATCHING")}");
                //sb.AppendLine($"Q5 (ORDERING): {QuestionCardGenerator.GetStudentAnswer(orderingCard, "ORDERING")}");
                //sb.AppendLine($"Q6 (SHORT_ANSWER): {QuestionCardGenerator.GetStudentAnswer(shortAnswerCard, "SHORT_ANSWER")}");
                //sb.AppendLine($"Q7 (ESSAY): {QuestionCardGenerator.GetStudentAnswer(essayCard, "ESSAY").Substring(0, Math.Min(100, QuestionCardGenerator.GetStudentAnswer(essayCard, "ESSAY").Length))}...");
                //sb.AppendLine($"Q8 (SPEAK_PROMPT): {QuestionCardGenerator.GetStudentAnswer(speakCard, "SPEAK_PROMPT")}");

                MessageBox.Show(sb.ToString(), "All Answers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            scrollPanel.Controls.Add(btnGetAnswers);
        }
    }
}