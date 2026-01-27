namespace IELTS.UI.Admin.CourseStudents
{
    partial class CourseStudentViewerPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Sunny.UI.UIPanel pnlTop;
        private Sunny.UI.UILabel lblTitle;
        private Sunny.UI.UITextBox txtSearch;
        private Sunny.UI.UISymbolButton btnApply;
        private Sunny.UI.UISymbolButton btnBack;

        private Sunny.UI.UISplitContainer splitMain;
        private Sunny.UI.UIDataGridView dgvMain;

        // Detail
        private Sunny.UI.UIPanel pnlDetail;
        private Sunny.UI.UILabel lblDetailTitle;
        private Sunny.UI.UILabel lblStudentName;
        private Sunny.UI.UILabel lblStudentEmail;
        private Sunny.UI.UILabel lblStudentStatus;
        private Sunny.UI.UIProcessBar progressStudy;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            pnlTop = new Sunny.UI.UIPanel();
            lblTitle = new Sunny.UI.UILabel();
            txtSearch = new Sunny.UI.UITextBox();
            btnApply = new Sunny.UI.UISymbolButton();
            btnBack = new Sunny.UI.UISymbolButton();
            splitMain = new Sunny.UI.UISplitContainer();
            dgvMain = new Sunny.UI.UIDataGridView();
            pnlDetail = new Sunny.UI.UIPanel();
            lblDetailTitle = new Sunny.UI.UILabel();
            lblStudentName = new Sunny.UI.UILabel();
            lblStudentEmail = new Sunny.UI.UILabel();
            lblStudentStatus = new Sunny.UI.UILabel();
            progressStudy = new Sunny.UI.UIProcessBar();
            pnlTop.SuspendLayout();
            (splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
            pnlDetail.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(txtSearch);
            pnlTop.Controls.Add(btnApply);
            pnlTop.Controls.Add(btnBack);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.FillColor = Color.White;
            pnlTop.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4, 5, 4, 5);
            pnlTop.MinimumSize = new Size(1, 1);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(20);
            pnlTop.RectColor = Color.FromArgb(229, 231, 235);
            pnlTop.Size = new Size(1300, 119);
            pnlTop.TabIndex = 1;
            pnlTop.Text = null;
            pnlTop.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(621, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Danh sách khóa học";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 11F);
            txtSearch.Location = new Point(20, 70);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.MinimumSize = new Size(1, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(5);
            txtSearch.Radius = 15;
            txtSearch.ShowText = false;
            txtSearch.Size = new Size(280, 41);
            txtSearch.TabIndex = 1;
            txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearch.Watermark = "Tìm kiếm...";
            // 
            // btnApply
            // 
            btnApply.FillColor = Color.FromArgb(79, 124, 255);
            btnApply.FillHoverColor = Color.FromArgb(99, 144, 255);
            btnApply.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnApply.Location = new Point(307, 70);
            btnApply.MinimumSize = new Size(1, 1);
            btnApply.Name = "btnApply";
            btnApply.Radius = 15;
            btnApply.Size = new Size(110, 41);
            btnApply.Symbol = 61442;
            btnApply.TabIndex = 2;
            btnApply.Text = "Tìm";
            btnApply.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnApply.Click += btnApply_Click;
            // 
            // btnBack
            // 
            btnBack.FillColor = Color.FromArgb(243, 244, 246);
            btnBack.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnBack.ForeColor = Color.FromArgb(55, 65, 81);
            btnBack.Location = new Point(423, 70);
            btnBack.MinimumSize = new Size(1, 1);
            btnBack.Name = "btnBack";
            btnBack.Radius = 15;
            btnBack.Size = new Size(120, 41);
            btnBack.Symbol = 61536;
            btnBack.TabIndex = 3;
            btnBack.Text = "Quay lại";
            btnBack.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnBack.Visible = false;
            btnBack.Click += btnBack_Click;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 119);
            splitMain.MinimumSize = new Size(20, 20);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(dgvMain);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(pnlDetail);
            splitMain.Size = new Size(1300, 631);
            splitMain.SplitterDistance = 987;
            splitMain.SplitterWidth = 11;
            splitMain.TabIndex = 0;
            // 
            // dgvMain
            // 
            dgvMain.AllowUserToAddRows = false;
            dgvMain.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(249, 250, 251);
            dgvMain.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvMain.BackgroundColor = Color.White;
            dgvMain.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(55, 65, 81);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvMain.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvMain.ColumnHeadersHeight = 48;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(55, 65, 81);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(235, 240, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvMain.DefaultCellStyle = dataGridViewCellStyle3;
            dgvMain.Dock = DockStyle.Fill;
            dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dgvMain.GridColor = Color.FromArgb(235, 238, 245);
            dgvMain.Location = new Point(0, 0);
            dgvMain.Name = "dgvMain";
            dgvMain.ReadOnly = true;
            dgvMain.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvMain.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvMain.RowHeadersVisible = false;
            dgvMain.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dgvMain.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvMain.RowTemplate.Height = 46;
            dgvMain.SelectedIndex = -1;
            dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMain.Size = new Size(987, 631);
            dgvMain.StripeOddColor = Color.FromArgb(249, 250, 251);
            dgvMain.TabIndex = 0;
            dgvMain.CellDoubleClick += dgvMain_CellDoubleClick;
            // 
            // pnlDetail
            // 
            pnlDetail.Controls.Add(lblDetailTitle);
            pnlDetail.Controls.Add(lblStudentName);
            pnlDetail.Controls.Add(lblStudentEmail);
            pnlDetail.Controls.Add(lblStudentStatus);
            pnlDetail.Controls.Add(progressStudy);
            pnlDetail.Dock = DockStyle.Fill;
            pnlDetail.FillColor = Color.White;
            pnlDetail.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlDetail.Location = new Point(0, 0);
            pnlDetail.Margin = new Padding(4, 5, 4, 5);
            pnlDetail.MinimumSize = new Size(1, 1);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Padding = new Padding(20);
            pnlDetail.RectColor = Color.FromArgb(229, 231, 235);
            pnlDetail.Size = new Size(302, 631);
            pnlDetail.TabIndex = 0;
            pnlDetail.Text = null;
            pnlDetail.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblDetailTitle
            // 
            lblDetailTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblDetailTitle.ForeColor = Color.FromArgb(79, 124, 255);
            lblDetailTitle.Location = new Point(20, 20);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new Size(100, 29);
            lblDetailTitle.TabIndex = 0;
            lblDetailTitle.Text = "Chi tiết học viên";
            // 
            // lblStudentName
            // 
            lblStudentName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStudentName.ForeColor = Color.FromArgb(48, 48, 48);
            lblStudentName.Location = new Point(20, 65);
            lblStudentName.Name = "lblStudentName";
            lblStudentName.Size = new Size(259, 23);
            lblStudentName.TabIndex = 1;
            // 
            // lblStudentEmail
            // 
            lblStudentEmail.Font = new Font("Segoe UI", 10F);
            lblStudentEmail.ForeColor = Color.FromArgb(48, 48, 48);
            lblStudentEmail.Location = new Point(20, 95);
            lblStudentEmail.Name = "lblStudentEmail";
            lblStudentEmail.Size = new Size(259, 23);
            lblStudentEmail.TabIndex = 2;
            // 
            // lblStudentStatus
            // 
            lblStudentStatus.Font = new Font("Segoe UI", 10F);
            lblStudentStatus.ForeColor = Color.FromArgb(48, 48, 48);
            lblStudentStatus.Location = new Point(20, 125);
            lblStudentStatus.Name = "lblStudentStatus";
            lblStudentStatus.Size = new Size(259, 23);
            lblStudentStatus.TabIndex = 3;
            // 
            // progressStudy
            // 
            progressStudy.FillColor = Color.FromArgb(235, 243, 255);
            progressStudy.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            progressStudy.Location = new Point(20, 165);
            progressStudy.MinimumSize = new Size(3, 3);
            progressStudy.Name = "progressStudy";
            progressStudy.Radius = 6;
            progressStudy.Size = new Size(260, 19);
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
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            (splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
            pnlDetail.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
