using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.BLL
{
    public partial class AddTestSectionForm : Form
    {
        public AddTestSectionForm()
        {
            InitializeComponent();
        }

        private void LoadQuestionTypes()
        {
            cboQuestionType.Items.AddRange(new string[]
            {
        "MCQ",
        "FILL_BLANK",
        "MATCHING",
        "ESSAY",
        "SPEAKING"
            });

                cboQuestionType.SelectedIndexChanged += CboQuestionType_SelectedIndexChanged;
        }

        private void CboQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlDynamic.Controls.Clear();

            switch (cboQuestionType.SelectedItem.ToString())
            {
                case "MCQ":
                    LoadUI_MCQ();
                    break;

                case "FILL_BLANK":
                    LoadUI_FillBlank();
                    break;

                case "MATCHING":
                    LoadUI_Matching();
                    break;

                case "ESSAY":
                    LoadUI_Essay();
                    break;

                case "SPEAKING":
                    LoadUI_Speaking();
                    break;
            }
        }


        private FlowLayoutPanel flpChoices;

        private void LoadUI_MCQ()
        {
            flpChoices = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            Button btnAddChoice = new Button()
            {
                Text = "Add Choice",
                Width = 120
            };
            btnAddChoice.Click += (s, e) => AddMCQChoice();

            pnlDynamic.Controls.Add(btnAddChoice);
            pnlDynamic.Controls.Add(flpChoices);

            AddMCQChoice();
        }

        private void AddMCQChoice()
        {
            Panel p = new Panel() { Width = 400, Height = 30 };

            RadioButton rd = new RadioButton() { Left = 0, Top = 5 };
            TextBox txt = new TextBox() { Left = 25, Width = 300 };

            p.Controls.Add(rd);
            p.Controls.Add(txt);
            flpChoices.Controls.Add(p);
        }

        private void LoadUI_FillBlank()
        {
            Label lbl = new Label()
            {
                Text = "Correct Answer:",
                Top = 10
            };

            TextBox txt = new TextBox()
            {
                Top = 35,
                Width = 300,
                Name = "txtFillAnswer"
            };

            pnlDynamic.Controls.Add(lbl);
            pnlDynamic.Controls.Add(txt);
        }

        private DataGridView dgvMatching;

        private void LoadUI_Matching()
        {
            dgvMatching = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false
            };

            dgvMatching.Columns.Add("LeftItem", "Left");
            dgvMatching.Columns.Add("RightItem", "Right");

            pnlDynamic.Controls.Add(dgvMatching);
        }

        private void LoadUI_Essay()
        {
            TextBox txtEssay = new TextBox()
            {
                Multiline = true,
                Dock = DockStyle.Fill
            };

            pnlDynamic.Controls.Add(txtEssay);
        }

        private void LoadUI_Speaking()
        {
            Label lbl = new Label()
            {
                Text = "Prompt Text:",
                Top = 10
            };

            TextBox txt = new TextBox()
            {
                Top = 35,
                Width = 400,
                Height = 100,
                Multiline = true,
                Name = "txtSpeakingPrompt"
            };

            pnlDynamic.Controls.Add(lbl);
            pnlDynamic.Controls.Add(txt);
        }


    }
}
