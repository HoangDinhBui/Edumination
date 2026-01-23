namespace IELTS.UI.Admin.CourseStudents
{
    partial class CourseStudentViewerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnBack;

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView dgvMain;

        // Detail panel
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Label lblDetailTitle;
        private System.Windows.Forms.Label lblStudentName;
        private System.Windows.Forms.Label lblStudentEmail;
        private System.Windows.Forms.Label lblStudentStatus;
        private System.Windows.Forms.ProgressBar progressStudy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            pnlTop = new Panel();
            lblTitle = new Label();
            txtSearch = new TextBox();
            btnApply = new Button();
            btnBack = new Button();
            splitMain = new SplitContainer();
            dgvMain = new DataGridView();
            pnlDetail = new Panel();
            lblDetailTitle = new Label();
            lblStudentName = new Label();
            lblStudentEmail = new Label();
            lblStudentStatus = new Label();
            progressStudy = new ProgressBar();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
            pnlDetail.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(0, 120, 215);
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Controls.Add(btnApply);
            pnlTop.Controls.Add(btnBack);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(15);
            pnlTop.Size = new Size(1300, 90);
            pnlTop.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(15, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(296, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Danh sách khóa học";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 12F);
            txtSearch.Location = new Point(20, 50);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(373, 34);
            txtSearch.TabIndex = 1;
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.White;
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.FlatStyle = FlatStyle.Flat;
            btnApply.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnApply.ForeColor = Color.FromArgb(0, 120, 215);
            btnApply.Location = new Point(413, 50);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(156, 34);
            btnApply.TabIndex = 2;
            btnApply.Text = "🔍 Áp dụng";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(220, 235, 250);
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnBack.ForeColor = Color.FromArgb(0, 120, 215);
            btnBack.Location = new Point(590, 50);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(120, 34);
            btnBack.TabIndex = 3;
            btnBack.Text = "← Quay lại";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Visible = false;
            btnBack.Click += btnBack_Click;
            // 
            // splitMain
            // 
            splitMain.BackColor = Color.LightGray;
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 90);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(dgvMain);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(pnlDetail);
            splitMain.Size = new Size(1300, 660);
            splitMain.SplitterDistance = 1048;
            splitMain.TabIndex = 0;
            // 
            // dgvMain
            // 
            dgvMain.AllowUserToAddRows = false;
            dgvMain.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 245, 255);
            dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMain.BackgroundColor = Color.White;
            dgvMain.BorderStyle = BorderStyle.None;
            dgvMain.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(0, 120, 215);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvMain.ColumnHeadersHeight = 45;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 11F);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvMain.DefaultCellStyle = dataGridViewCellStyle6;
            dgvMain.Dock = DockStyle.Fill;
            dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.Location = new Point(0, 0);
            dgvMain.MultiSelect = false;
            dgvMain.Name = "dgvMain";
            dgvMain.ReadOnly = true;
            dgvMain.RowHeadersVisible = false;
            dgvMain.RowHeadersWidth = 51;
            dgvMain.RowTemplate.Height = 40;
            dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMain.Size = new Size(1048, 660);
            dgvMain.TabIndex = 0;
            dgvMain.CellDoubleClick += dgvMain_CellDoubleClick;
            // 
            // pnlDetail
            // 
            pnlDetail.BackColor = Color.White;
            pnlDetail.Controls.Add(lblDetailTitle);
            pnlDetail.Controls.Add(lblStudentName);
            pnlDetail.Controls.Add(lblStudentEmail);
            pnlDetail.Controls.Add(lblStudentStatus);
            pnlDetail.Controls.Add(progressStudy);
            pnlDetail.Dock = DockStyle.Fill;
            pnlDetail.Location = new Point(0, 0);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Padding = new Padding(25);
            pnlDetail.Size = new Size(248, 660);
            pnlDetail.TabIndex = 0;
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.AutoSize = true;
            lblDetailTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblDetailTitle.ForeColor = Color.FromArgb(0, 120, 215);
            lblDetailTitle.Location = new Point(25, 20);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new Size(228, 32);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "Thông tin học viên";
            // 
            // lblStudentName
            // 
            lblStudentName.AutoSize = true;
            lblStudentName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStudentName.Location = new Point(25, 70);
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Size = new Size(295, 28);
            lblStudentName.TabIndex = 1;
            lblStudentName.Text = "Chọn học viên để xem chi tiết";
            // 
            // lblStudentEmail
            // 
            lblStudentEmail.AutoSize = true;
            lblStudentEmail.Font = new Font("Segoe UI", 11F);
            lblStudentEmail.Location = new Point(25, 110);
            lblStudentEmail.Name = "lblStudentEmail";
            lblStudentEmail.Size = new Size(0, 25);
            lblStudentEmail.TabIndex = 2;
            // 
            // lblStudentStatus
            // 
            lblStudentStatus.AutoSize = true;
            lblStudentStatus.Font = new Font("Segoe UI", 11F);
            lblStudentStatus.Location = new Point(25, 145);
            lblStudentStatus.Name = "lblStudentStatus";
            lblStudentStatus.Size = new Size(0, 25);
            lblStudentStatus.TabIndex = 3;
            // 
            // progressStudy
            // 
            progressStudy.Location = new Point(25, 190);
            progressStudy.Name = "progressStudy";
            progressStudy.Size = new Size(300, 24);
            progressStudy.TabIndex = 4;
            // 
            // CourseStudentViewerPanel
            // 
            BackColor = Color.White;
            Controls.Add(splitMain);
            Controls.Add(pnlTop);
            Font = new Font("Segoe UI", 10F);
            Name = "CourseStudentViewerPanel";
            Size = new Size(1300, 750);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
            pnlDetail.ResumeLayout(false);
            pnlDetail.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}
