namespace IELTS.UI.Admin.TestManager
{
    partial class ShowQuestionControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnDelete = new Button();
            btnEdit = new Button();
            lblTitle = new Label();
            pnlMain = new Panel();
            pnlQuestionContent = new Panel();
            flpQuestion = new FlowLayoutPanel();
            lblNoQuestion = new Label();
            pnlQuestionNav = new Panel();
            flpQuestionButtons = new FlowLayoutPanel();
            lblQuestionNav = new Label();
            pnlHeader.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlQuestionContent.SuspendLayout();
            flpQuestion.SuspendLayout();
            pnlQuestionNav.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(41, 128, 185);
            pnlHeader.Controls.Add(btnDelete);
            pnlHeader.Controls.Add(btnEdit);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(3, 4, 3, 4);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1634, 107);
            pnlHeader.TabIndex = 0;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(1457, 23);
            btnDelete.Margin = new Padding(3, 4, 3, 4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(149, 60);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "🗑️ Delete";
            btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnEdit
            // 
            btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEdit.BackColor = Color.FromArgb(46, 204, 113);
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(1291, 23);
            btnEdit.Margin = new Padding(3, 4, 3, 4);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(149, 60);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "✏️ Edit";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEditQuestion_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(23, 31);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(329, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📝 Question Manager";
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(240, 240, 240);
            pnlMain.Controls.Add(pnlQuestionContent);
            pnlMain.Controls.Add(pnlQuestionNav);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 107);
            pnlMain.Margin = new Padding(3, 4, 3, 4);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(23, 27, 23, 27);
            pnlMain.Size = new Size(1634, 840);
            pnlMain.TabIndex = 1;
            // 
            // pnlQuestionContent
            // 
            pnlQuestionContent.BackColor = Color.White;
            pnlQuestionContent.BorderStyle = BorderStyle.FixedSingle;
            pnlQuestionContent.Controls.Add(flpQuestion);
            pnlQuestionContent.Dock = DockStyle.Fill;
            pnlQuestionContent.Location = new Point(23, 186);
            pnlQuestionContent.Margin = new Padding(3, 4, 3, 4);
            pnlQuestionContent.Name = "pnlQuestionContent";
            pnlQuestionContent.Padding = new Padding(23, 27, 23, 27);
            pnlQuestionContent.Size = new Size(1588, 627);
            pnlQuestionContent.TabIndex = 1;
            // 
            // flpQuestion
            // 
            flpQuestion.AutoScroll = true;
            flpQuestion.Controls.Add(lblNoQuestion);
            flpQuestion.Dock = DockStyle.Fill;
            flpQuestion.FlowDirection = FlowDirection.TopDown;
            flpQuestion.Location = new Point(23, 27);
            flpQuestion.Margin = new Padding(3, 4, 3, 4);
            flpQuestion.Name = "flpQuestion";
            flpQuestion.Size = new Size(1540, 571);
            flpQuestion.TabIndex = 0;
            flpQuestion.WrapContents = false;
            // 
            // lblNoQuestion
            // 
            lblNoQuestion.AutoSize = true;
            lblNoQuestion.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
            lblNoQuestion.ForeColor = Color.Gray;
            lblNoQuestion.Location = new Point(3, 13);
            lblNoQuestion.Margin = new Padding(3, 13, 3, 0);
            lblNoQuestion.Name = "lblNoQuestion";
            lblNoQuestion.Size = new Size(511, 28);
            lblNoQuestion.TabIndex = 0;
            lblNoQuestion.Text = "👆 Please select a question number above to view details";
            // 
            // pnlQuestionNav
            // 
            pnlQuestionNav.BackColor = Color.White;
            pnlQuestionNav.BorderStyle = BorderStyle.FixedSingle;
            pnlQuestionNav.Controls.Add(flpQuestionButtons);
            pnlQuestionNav.Controls.Add(lblQuestionNav);
            pnlQuestionNav.Dock = DockStyle.Top;
            pnlQuestionNav.Location = new Point(23, 27);
            pnlQuestionNav.Margin = new Padding(3, 4, 3, 4);
            pnlQuestionNav.Name = "pnlQuestionNav";
            pnlQuestionNav.Padding = new Padding(17, 20, 17, 20);
            pnlQuestionNav.Size = new Size(1588, 159);
            pnlQuestionNav.TabIndex = 0;
            // 
            // flpQuestionButtons
            // 
            flpQuestionButtons.AutoScroll = true;
            flpQuestionButtons.Dock = DockStyle.Fill;
            flpQuestionButtons.Location = new Point(17, 61);
            flpQuestionButtons.Margin = new Padding(3, 4, 3, 4);
            flpQuestionButtons.Name = "flpQuestionButtons";
            flpQuestionButtons.Padding = new Padding(6, 7, 6, 7);
            flpQuestionButtons.Size = new Size(1552, 76);
            flpQuestionButtons.TabIndex = 1;
            // 
            // lblQuestionNav
            // 
            lblQuestionNav.AutoSize = true;
            lblQuestionNav.Dock = DockStyle.Top;
            lblQuestionNav.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblQuestionNav.ForeColor = Color.FromArgb(52, 73, 94);
            lblQuestionNav.Location = new Point(17, 20);
            lblQuestionNav.Name = "lblQuestionNav";
            lblQuestionNav.Padding = new Padding(0, 0, 0, 13);
            lblQuestionNav.Size = new Size(406, 41);
            lblQuestionNav.TabIndex = 0;
            lblQuestionNav.Text = "📊 Select Question Number to View/Edit:";
            // 
            // ShowQuestionControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlMain);
            Controls.Add(pnlHeader);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ShowQuestionControl";
            Size = new Size(1634, 947);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlQuestionContent.ResumeLayout(false);
            flpQuestion.ResumeLayout(false);
            flpQuestion.PerformLayout();
            pnlQuestionNav.ResumeLayout(false);
            pnlQuestionNav.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlQuestionNav;
        private System.Windows.Forms.Label lblQuestionNav;
        private System.Windows.Forms.FlowLayoutPanel flpQuestionButtons;
        private System.Windows.Forms.Panel pnlQuestionContent;
        private System.Windows.Forms.FlowLayoutPanel flpQuestion;
        private System.Windows.Forms.Label lblNoQuestion;
    }
}