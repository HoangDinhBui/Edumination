using Sunny.UI; // Giữ lại Sunny.UI cho các controls khác như UILabel, UITabControlMenu, v.v.
using System.Windows.Forms; // Cần thiết cho Panel và PictureBox

namespace Edumination.WinForms.UI.Admin.TestManager
{
    partial class pnlTestManager
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            uiLabelTitle = new UILabel();
            uiTextBoxSearch = new UITextBox();
            uiComboBoxSort = new UIComboBox();
            pnlCardQ4Set1 = new Panel();
            panel1 = new Panel();
            uiButton4 = new UIButton();
            uiButton3 = new UIButton();
            uiButton2 = new UIButton();
            uiButton1 = new UIButton();
            btnAllSkills = new UIButton();
            pnlMain = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // uiLabelTitle
            // 
            uiLabelTitle.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            uiLabelTitle.ForeColor = Color.FromArgb(48, 48, 48);
            uiLabelTitle.Location = new Point(3, 10);
            uiLabelTitle.Name = "uiLabelTitle";
            uiLabelTitle.Size = new Size(1664, 50);
            uiLabelTitle.Style = UIStyle.Custom;
            uiLabelTitle.TabIndex = 0;
            uiLabelTitle.Text = "IELTS Test Papers Library";
            uiLabelTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // uiTextBoxSearch
            // 
            uiTextBoxSearch.ButtonSymbol = 61453;
            uiTextBoxSearch.ButtonSymbolSize = 20;
            uiTextBoxSearch.Cursor = Cursors.IBeam;
            uiTextBoxSearch.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiTextBoxSearch.Location = new Point(120, 200);
            uiTextBoxSearch.Margin = new Padding(4, 5, 4, 5);
            uiTextBoxSearch.MinimumSize = new Size(1, 16);
            uiTextBoxSearch.Name = "uiTextBoxSearch";
            uiTextBoxSearch.Padding = new Padding(5);
            uiTextBoxSearch.RectColor = Color.LightGray;
            uiTextBoxSearch.ShowText = false;
            uiTextBoxSearch.Size = new Size(1207, 40);
            uiTextBoxSearch.Style = UIStyle.Custom;
            uiTextBoxSearch.TabIndex = 2;
            uiTextBoxSearch.TextAlignment = ContentAlignment.MiddleLeft;
            uiTextBoxSearch.Watermark = "Search...";
            // 
            // uiComboBoxSort
            // 
            uiComboBoxSort.DataSource = null;
            uiComboBoxSort.FillColor = Color.White;
            uiComboBoxSort.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiComboBoxSort.ItemHeight = 24;
            uiComboBoxSort.ItemHoverColor = Color.FromArgb(155, 200, 255);
            uiComboBoxSort.Items.AddRange(new object[] { "Latest", "Oldest", "Popular" });
            uiComboBoxSort.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            uiComboBoxSort.Location = new Point(1400, 200);
            uiComboBoxSort.Margin = new Padding(4, 5, 4, 5);
            uiComboBoxSort.MinimumSize = new Size(63, 0);
            uiComboBoxSort.Name = "uiComboBoxSort";
            uiComboBoxSort.Padding = new Padding(0, 0, 30, 2);
            uiComboBoxSort.Size = new Size(150, 40);
            uiComboBoxSort.Style = UIStyle.Custom;
            uiComboBoxSort.SymbolSize = 24;
            uiComboBoxSort.TabIndex = 3;
            uiComboBoxSort.Text = "Latest";
            uiComboBoxSort.TextAlignment = ContentAlignment.MiddleLeft;
            uiComboBoxSort.Watermark = "";
            // 
            // pnlCardQ4Set1
            // 
            pnlCardQ4Set1.Location = new Point(0, 0);
            pnlCardQ4Set1.Name = "pnlCardQ4Set1";
            pnlCardQ4Set1.Size = new Size(200, 100);
            pnlCardQ4Set1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(uiButton4);
            panel1.Controls.Add(uiButton3);
            panel1.Controls.Add(uiButton2);
            panel1.Controls.Add(uiButton1);
            panel1.Controls.Add(btnAllSkills);
            panel1.Location = new Point(120, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(1430, 105);
            panel1.TabIndex = 6;
            // 
            // uiButton4
            // 
            uiButton4.BackColor = SystemColors.Control;
            uiButton4.Cursor = Cursors.Hand;
            uiButton4.FillColor = Color.White;
            uiButton4.FillHoverColor = Color.FromArgb(230, 240, 255);
            uiButton4.FillPressColor = Color.DodgerBlue;
            uiButton4.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiButton4.ForeColor = Color.Black;
            uiButton4.Location = new Point(1167, 21);
            uiButton4.MinimumSize = new Size(1, 1);
            uiButton4.Name = "uiButton4";
            uiButton4.Radius = 20;
            uiButton4.RectColor = Color.LightGray;
            uiButton4.Size = new Size(230, 69);
            uiButton4.TabIndex = 4;
            uiButton4.Text = "🗣️  Speaking";
            uiButton4.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiButton4.TipsForeColor = Color.Azure;
            // 
            // uiButton3
            // 
            uiButton3.BackColor = SystemColors.Control;
            uiButton3.Cursor = Cursors.Hand;
            uiButton3.FillColor = Color.White;
            uiButton3.FillHoverColor = Color.FromArgb(230, 240, 255);
            uiButton3.FillPressColor = Color.DodgerBlue;
            uiButton3.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiButton3.ForeColor = Color.Black;
            uiButton3.Location = new Point(882, 21);
            uiButton3.MinimumSize = new Size(1, 1);
            uiButton3.Name = "uiButton3";
            uiButton3.Radius = 20;
            uiButton3.RectColor = Color.LightGray;
            uiButton3.Size = new Size(230, 69);
            uiButton3.TabIndex = 3;
            uiButton3.Text = "✍️  Writing";
            uiButton3.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiButton3.TipsForeColor = Color.Azure;
            // 
            // uiButton2
            // 
            uiButton2.BackColor = SystemColors.Control;
            uiButton2.Cursor = Cursors.Hand;
            uiButton2.FillColor = Color.White;
            uiButton2.FillHoverColor = Color.FromArgb(230, 240, 255);
            uiButton2.FillPressColor = Color.DodgerBlue;
            uiButton2.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiButton2.ForeColor = Color.Black;
            uiButton2.Location = new Point(590, 21);
            uiButton2.MinimumSize = new Size(1, 1);
            uiButton2.Name = "uiButton2";
            uiButton2.Radius = 20;
            uiButton2.RectColor = Color.LightGray;
            uiButton2.Size = new Size(230, 69);
            uiButton2.TabIndex = 2;
            uiButton2.Text = "📖  Reading";
            uiButton2.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiButton2.TipsForeColor = Color.Azure;
            // 
            // uiButton1
            // 
            uiButton1.BackColor = SystemColors.Control;
            uiButton1.Cursor = Cursors.Hand;
            uiButton1.FillColor = Color.White;
            uiButton1.FillHoverColor = Color.FromArgb(230, 240, 255);
            uiButton1.FillPressColor = Color.DodgerBlue;
            uiButton1.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uiButton1.ForeColor = Color.Black;
            uiButton1.Location = new Point(312, 21);
            uiButton1.MinimumSize = new Size(1, 1);
            uiButton1.Name = "uiButton1";
            uiButton1.Radius = 20;
            uiButton1.RectColor = Color.LightGray;
            uiButton1.Size = new Size(230, 69);
            uiButton1.TabIndex = 1;
            uiButton1.Text = "🎧  Listening";
            uiButton1.TipsFont = new Font("Microsoft Sans Serif", 9F);
            uiButton1.TipsForeColor = Color.Azure;
            // 
            // btnAllSkills
            // 
            btnAllSkills.BackColor = SystemColors.Control;
            btnAllSkills.Cursor = Cursors.Hand;
            btnAllSkills.FillColor = Color.White;
            btnAllSkills.FillHoverColor = Color.FromArgb(230, 240, 255);
            btnAllSkills.FillPressColor = Color.DodgerBlue;
            btnAllSkills.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAllSkills.ForeColor = Color.Black;
            btnAllSkills.Location = new Point(35, 21);
            btnAllSkills.MinimumSize = new Size(1, 1);
            btnAllSkills.Name = "btnAllSkills";
            btnAllSkills.Radius = 20;
            btnAllSkills.RectColor = Color.LightGray;
            btnAllSkills.Size = new Size(230, 69);
            btnAllSkills.TabIndex = 0;
            btnAllSkills.Text = "✺ All Skills";
            btnAllSkills.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnAllSkills.TipsForeColor = Color.Azure;
            btnAllSkills.Click += btnAllSkills_Click;
            // 
            // pnlMain
            // 
            pnlMain.Location = new Point(120, 269);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(1430, 748);
            pnlMain.TabIndex = 8;
            // 
            // pnlTestManager
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(pnlMain);
            Controls.Add(panel1);
            Controls.Add(uiComboBoxSort);
            Controls.Add(uiTextBoxSearch);
            Controls.Add(uiLabelTitle);
            Name = "pnlTestManager";
            Size = new Size(1670, 1020);
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UILabel uiLabelTitle;
        private Sunny.UI.UITextBox uiTextBoxSearch;
        private Sunny.UI.UIComboBox uiComboBoxSort;
        private System.Windows.Forms.Panel pnlMockTestContainer; // THAY UICard BẰNG PANEL
        private Sunny.UI.UILabel uiLabelMockTestTitle;
        private Sunny.UI.UIFlowLayoutPanel uiFlowLayoutPanelTestSets;
        private System.Windows.Forms.Panel pnlCardQ1Set1; // THAY UICard BẰNG PANEL
        private System.Windows.Forms.Panel pnlCardQ2Set1; // THAY UICard BẰNG PANEL
        private System.Windows.Forms.Panel pnlCardQ3Set1; // THAY UICard BẰNG PANEL
        private System.Windows.Forms.Panel pnlCardQ4Set1; // THAY UICard BẰNG PANEL
        private Sunny.UI.UILinkLabel uiLinkViewMore;
        private Panel panel1;
        private UIButton btnAllSkills;
        private UIButton uiButton4;
        private UIButton uiButton3;
        private UIButton uiButton2;
        private UIButton uiButton1;
        private Panel pnlMain;
    }
}