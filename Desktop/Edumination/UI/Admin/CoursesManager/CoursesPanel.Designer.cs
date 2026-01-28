namespace IELTS.UI.Admin.CoursesManager
{
	partial class CoursesPanel
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) components.Dispose();
			base.Dispose(disposing);
		}

		#region Component Designer generated code

<<<<<<< HEAD
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            pnlLeft = new Sunny.UI.UIPanel();
            dgvCourses = new Sunny.UI.UIDataGridView();
            pnlSearch = new Sunny.UI.UIPanel();
            btnSearch = new Sunny.UI.UISymbolButton();
            txtSearch = new Sunny.UI.UITextBox();
            pnlRight = new Sunny.UI.UITitlePanel();
            btnRefresh = new Sunny.UI.UISymbolButton();
            btnDelete = new Sunny.UI.UISymbolButton();
            btnSave = new Sunny.UI.UISymbolButton();
            swPublished = new Sunny.UI.UISwitch();
            lblPublished = new Sunny.UI.UILabel();
            txtPrice = new Sunny.UI.UITextBox();
            lblPrice = new Sunny.UI.UILabel();
            cboLevel = new Sunny.UI.UIComboBox();
            lblLevel = new Sunny.UI.UILabel();
            txtDesc = new Sunny.UI.UITextBox();
            lblDesc = new Sunny.UI.UILabel();
            txtTitle = new Sunny.UI.UITextBox();
            lblTitle = new Sunny.UI.UILabel();
            txtId = new Sunny.UI.UITextBox();
            lblId = new Sunny.UI.UILabel();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCourses).BeginInit();
            pnlSearch.SuspendLayout();
            pnlRight.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(dgvCourses);
            pnlLeft.Controls.Add(pnlSearch);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Margin = new Padding(4, 5, 4, 5);
            pnlLeft.MinimumSize = new Size(1, 1);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.RectColor = Color.Transparent;
            pnlLeft.Size = new Size(1193, 1020);
            pnlLeft.TabIndex = 0;
            pnlLeft.Text = null;
            pnlLeft.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // dgvCourses
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            dgvCourses.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCourses.BackgroundColor = Color.White;
            dgvCourses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dgvCourses.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvCourses.ColumnHeadersHeight = 32;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvCourses.DefaultCellStyle = dataGridViewCellStyle3;
            dgvCourses.Dock = DockStyle.Fill;
            dgvCourses.EnableHeadersVisualStyles = false;
            dgvCourses.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dgvCourses.GridColor = Color.FromArgb(80, 160, 255);
            dgvCourses.Location = new Point(0, 60);
            dgvCourses.Name = "dgvCourses";
            dgvCourses.ReadOnly = true;
            dgvCourses.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvCourses.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvCourses.RowHeadersVisible = false;
            dgvCourses.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = Color.White;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 12F);
            dgvCourses.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvCourses.SelectedIndex = -1;
            dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourses.Size = new Size(1193, 960);
            dgvCourses.StripeOddColor = Color.FromArgb(235, 243, 255);
            dgvCourses.TabIndex = 1;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlSearch.Location = new Point(0, 0);
            pnlSearch.Margin = new Padding(4, 5, 4, 5);
            pnlSearch.MinimumSize = new Size(1, 1);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(1193, 60);
            pnlSearch.TabIndex = 0;
            pnlSearch.Text = null;
            pnlSearch.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(0, 0, 0, 0);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Font = new Font("Microsoft Sans Serif", 12F);
            btnSearch.Location = new Point(375, 5);
            btnSearch.MinimumSize = new Size(1, 1);
            btnSearch.Name = "btnSearch";
            btnSearch.Radius = 15;
            btnSearch.Size = new Size(131, 49);
            btnSearch.Symbol = 61442;
            btnSearch.TabIndex = 0;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(0, 0, 0, 0);
            txtSearch.Cursor = Cursors.IBeam;
            txtSearch.Font = new Font("Microsoft Sans Serif", 12F);
            txtSearch.Location = new Point(13, 5);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.MinimumSize = new Size(1, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(5);
            txtSearch.Radius = 15;
            txtSearch.ShowText = false;
            txtSearch.Size = new Size(350, 50);
            txtSearch.Symbol = 61442;
            txtSearch.TabIndex = 1;
            txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearch.Watermark = "Tìm khóa học...";
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.FromArgb(0, 0, 0, 0);
            pnlRight.Controls.Add(btnRefresh);
            pnlRight.Controls.Add(btnDelete);
            pnlRight.Controls.Add(btnSave);
            pnlRight.Controls.Add(swPublished);
            pnlRight.Controls.Add(lblPublished);
            pnlRight.Controls.Add(txtPrice);
            pnlRight.Controls.Add(lblPrice);
            pnlRight.Controls.Add(cboLevel);
            pnlRight.Controls.Add(lblLevel);
            pnlRight.Controls.Add(txtDesc);
            pnlRight.Controls.Add(lblDesc);
            pnlRight.Controls.Add(txtTitle);
            pnlRight.Controls.Add(lblTitle);
            pnlRight.Controls.Add(txtId);
            pnlRight.Controls.Add(lblId);
            pnlRight.Dock = DockStyle.Right;
            pnlRight.FillColor = Color.White;
            pnlRight.FillColor2 = Color.FromArgb(0, 0, 0, 0);
            pnlRight.Font = new Font("Microsoft Sans Serif", 12F);
            pnlRight.Location = new Point(1193, 0);
            pnlRight.Margin = new Padding(4, 5, 4, 5);
            pnlRight.MinimumSize = new Size(1, 1);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(10, 35, 10, 10);
            pnlRight.ShowText = false;
            pnlRight.Size = new Size(537, 1020);
            pnlRight.TabIndex = 1;
            pnlRight.Text = "CHI TIẾT KHÓA HỌC";
            pnlRight.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            btnRefresh.FillColor = Color.Gray;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnRefresh.Location = new Point(282, 582);
            btnRefresh.MinimumSize = new Size(1, 1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 40);
            btnRefresh.Symbol = 61473;
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Làm mới";
            btnRefresh.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // btnDelete
            // 
            btnDelete.Enabled = false;
            btnDelete.FillColor = Color.Red;
            btnDelete.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnDelete.Location = new Point(172, 582);
            btnDelete.MinimumSize = new Size(1, 1);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 40);
            btnDelete.Symbol = 61453;
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Xóa";
            btnDelete.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // btnSave
            // 
            btnSave.FillColor = Color.FromArgb(110, 190, 40);
            btnSave.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnSave.Location = new Point(32, 582);
            btnSave.MinimumSize = new Size(1, 1);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 40);
            btnSave.Symbol = 61639;
            btnSave.TabIndex = 2;
            btnSave.Text = "Lưu";
            btnSave.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // swPublished
            // 
            swPublished.Active = true;
            swPublished.ActiveText = "Có";
            swPublished.BackColor = Color.FromArgb(0, 0, 0, 0);
            swPublished.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            swPublished.InActiveText = "Không";
            swPublished.Location = new Point(202, 529);
            swPublished.MinimumSize = new Size(1, 1);
            swPublished.Name = "swPublished";
            swPublished.Size = new Size(75, 29);
            swPublished.TabIndex = 3;
            // 
            // lblPublished
            // 
            lblPublished.AutoSize = true;
            lblPublished.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblPublished.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblPublished.ForeColor = Color.FromArgb(48, 48, 48);
            lblPublished.Location = new Point(32, 531);
            lblPublished.Name = "lblPublished";
            lblPublished.Size = new Size(97, 25);
            lblPublished.TabIndex = 4;
            lblPublished.Text = "Xuất bản:";
            // 
            // txtPrice
            // 
            txtPrice.BackColor = Color.FromArgb(0, 0, 0, 0);
            txtPrice.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtPrice.Location = new Point(30, 467);
            txtPrice.Margin = new Padding(4, 5, 4, 5);
            txtPrice.MinimumSize = new Size(1, 16);
            txtPrice.Name = "txtPrice";
            txtPrice.Padding = new Padding(5);
            txtPrice.ShowText = false;
            txtPrice.Size = new Size(440, 40);
            txtPrice.TabIndex = 5;
            txtPrice.Text = "0";
            txtPrice.TextAlignment = ContentAlignment.MiddleLeft;
            txtPrice.Type = Sunny.UI.UITextBox.UIEditType.Integer;
            txtPrice.Watermark = "";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblPrice.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblPrice.ForeColor = Color.FromArgb(48, 48, 48);
            lblPrice.Location = new Point(30, 437);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(109, 25);
            lblPrice.TabIndex = 6;
            lblPrice.Text = "Giá (VNĐ):";
            // 
            // cboLevel
            // 
            cboLevel.BackColor = Color.FromArgb(0, 0, 0, 0);
            cboLevel.DataSource = null;
            cboLevel.FillColor = Color.White;
            cboLevel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            cboLevel.ItemHoverColor = Color.FromArgb(155, 200, 255);
            cboLevel.Items.AddRange(new object[] { "BEGINNER", "INTERMEDIATE", "ADVANCED" });
            cboLevel.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            cboLevel.Location = new Point(30, 383);
            cboLevel.Margin = new Padding(4, 5, 4, 5);
            cboLevel.MinimumSize = new Size(63, 0);
            cboLevel.Name = "cboLevel";
            cboLevel.Padding = new Padding(0, 0, 30, 2);
            cboLevel.Size = new Size(440, 40);
            cboLevel.SymbolSize = 24;
            cboLevel.TabIndex = 7;
            cboLevel.TextAlignment = ContentAlignment.MiddleLeft;
            cboLevel.Watermark = "";
            // 
            // lblLevel
            // 
            lblLevel.AutoSize = true;
            lblLevel.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblLevel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblLevel.ForeColor = Color.FromArgb(48, 48, 48);
            lblLevel.Location = new Point(30, 353);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(90, 25);
            lblLevel.TabIndex = 8;
            lblLevel.Text = "Trình độ:";
            // 
            // txtDesc
            // 
            txtDesc.BackColor = Color.FromArgb(0, 0, 0, 0);
            txtDesc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtDesc.Location = new Point(30, 236);
            txtDesc.Margin = new Padding(4, 5, 4, 5);
            txtDesc.MinimumSize = new Size(1, 16);
            txtDesc.Multiline = true;
            txtDesc.Name = "txtDesc";
            txtDesc.Padding = new Padding(5);
            txtDesc.ShowText = false;
            txtDesc.Size = new Size(440, 100);
            txtDesc.TabIndex = 9;
            txtDesc.TextAlignment = ContentAlignment.MiddleLeft;
            txtDesc.Watermark = "";
            // 
            // lblDesc
            // 
            lblDesc.AutoSize = true;
            lblDesc.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblDesc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblDesc.ForeColor = Color.FromArgb(48, 48, 48);
            lblDesc.Location = new Point(30, 211);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(67, 25);
            lblDesc.TabIndex = 10;
            lblDesc.Text = "Mô tả:";
            // 
            // txtTitle
            // 
            txtTitle.BackColor = Color.FromArgb(0, 0, 0, 0);
            txtTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtTitle.Location = new Point(30, 166);
            txtTitle.Margin = new Padding(4, 5, 4, 5);
            txtTitle.MinimumSize = new Size(1, 16);
            txtTitle.Name = "txtTitle";
            txtTitle.Padding = new Padding(5);
            txtTitle.ShowText = false;
            txtTitle.Size = new Size(440, 40);
            txtTitle.TabIndex = 11;
            txtTitle.TextAlignment = ContentAlignment.MiddleLeft;
            txtTitle.Watermark = "";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblTitle.ForeColor = Color.FromArgb(48, 48, 48);
            lblTitle.Location = new Point(30, 141);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(138, 25);
            lblTitle.TabIndex = 12;
            lblTitle.Text = "Tên khóa học:";
            // 
            // txtId
            // 
            txtId.BackColor = Color.FromArgb(0, 0, 0, 0);
            txtId.Enabled = false;
            txtId.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtId.Location = new Point(30, 85);
            txtId.Margin = new Padding(4, 5, 4, 5);
            txtId.MinimumSize = new Size(1, 16);
            txtId.Name = "txtId";
            txtId.Padding = new Padding(5);
            txtId.ShowText = false;
            txtId.Size = new Size(440, 38);
            txtId.TabIndex = 13;
            txtId.TextAlignment = ContentAlignment.MiddleLeft;
            txtId.Watermark = "";
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblId.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblId.ForeColor = Color.FromArgb(48, 48, 48);
            lblId.Location = new Point(30, 60);
            lblId.Name = "lblId";
            lblId.Size = new Size(79, 25);
            lblId.TabIndex = 14;
            lblId.Text = "Mã KH:";
            // 
            // CoursesPanel
            // 
            Controls.Add(pnlLeft);
            Controls.Add(pnlRight);
            Name = "CoursesPanel";
            Size = new Size(1730, 1020);
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCourses).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlRight.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel pnlLeft;
        private Sunny.UI.UITitlePanel pnlRight;
        private Sunny.UI.UIPanel pnlSearch;
        private Sunny.UI.UIDataGridView dgvCourses;
        private Sunny.UI.UITextBox txtSearch;
        private Sunny.UI.UISymbolButton btnSearch;

        // Input Controls
        private Sunny.UI.UILabel lblId, lblTitle, lblDesc, lblLevel, lblPrice, lblPublished;
        private Sunny.UI.UITextBox txtId, txtTitle, txtDesc, txtPrice;
        private Sunny.UI.UIComboBox cboLevel;
        private Sunny.UI.UISwitch swPublished;
        private Sunny.UI.UISymbolButton btnSave, btnDelete, btnRefresh;
    }
}
=======
		private void InitializeComponent()
		{
			DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
			pnlLeft = new Sunny.UI.UIPanel();
			dgvCourses = new Sunny.UI.UIDataGridView();
			pnlSearch = new Sunny.UI.UIPanel();
			btnSearch = new Sunny.UI.UISymbolButton();
			txtSearch = new Sunny.UI.UITextBox();
			pnlRight = new Sunny.UI.UITitlePanel();
			tabControlDetail = new Sunny.UI.UITabControl();
			tpCourseInfo = new TabPage();
			lblId = new Sunny.UI.UILabel();
			txtId = new Sunny.UI.UITextBox();
			lblTitle = new Sunny.UI.UILabel();
			txtTitle = new Sunny.UI.UITextBox();
			lblDesc = new Sunny.UI.UILabel();
			txtDesc = new Sunny.UI.UITextBox();
			lblLevel = new Sunny.UI.UILabel();
			cboLevel = new Sunny.UI.UIComboBox();
			lblPrice = new Sunny.UI.UILabel();
			txtPrice = new Sunny.UI.UITextBox();
			lblPublished = new Sunny.UI.UILabel();
			swPublished = new Sunny.UI.UISwitch();
			btnSave = new Sunny.UI.UISymbolButton();
			btnDelete = new Sunny.UI.UISymbolButton();
			btnRefresh = new Sunny.UI.UISymbolButton();
			tpLessons = new TabPage();
			dgvLessons = new Sunny.UI.UIDataGridView();
			pnlLessonTool = new Sunny.UI.UIPanel();
			btnAddLesson = new Sunny.UI.UISymbolButton();
			lblLessonHint = new Sunny.UI.UILabel();
			pnlLeft.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvCourses).BeginInit();
			pnlSearch.SuspendLayout();
			pnlRight.SuspendLayout();
			tabControlDetail.SuspendLayout();
			tpCourseInfo.SuspendLayout();
			tpLessons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvLessons).BeginInit();
			pnlLessonTool.SuspendLayout();
			SuspendLayout();
			// 
			// pnlLeft
			// 
			pnlLeft.Controls.Add(dgvCourses);
			pnlLeft.Controls.Add(pnlSearch);
			pnlLeft.Dock = DockStyle.Fill;
			pnlLeft.Font = new Font("Microsoft Sans Serif", 12F);
			pnlLeft.Location = new Point(0, 0);
			pnlLeft.Margin = new Padding(4, 5, 4, 5);
			pnlLeft.MinimumSize = new Size(1, 1);
			pnlLeft.Name = "pnlLeft";
			pnlLeft.Size = new Size(1100, 1020);
			pnlLeft.TabIndex = 0;
			pnlLeft.Text = null;
			pnlLeft.TextAlignment = ContentAlignment.MiddleCenter;
			// 
			// dgvCourses
			// 
			dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
			dgvCourses.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			dgvCourses.BackgroundColor = Color.White;
			dgvCourses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
			dgvCourses.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			dgvCourses.ColumnHeadersHeight = 32;
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Window;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 12F);
			dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
			dgvCourses.DefaultCellStyle = dataGridViewCellStyle3;
			dgvCourses.Dock = DockStyle.Fill;
			dgvCourses.EnableHeadersVisualStyles = false;
			dgvCourses.Font = new Font("Microsoft Sans Serif", 12F);
			dgvCourses.GridColor = Color.FromArgb(80, 160, 255);
			dgvCourses.Location = new Point(0, 60);
			dgvCourses.Name = "dgvCourses";
			dgvCourses.ReadOnly = true;
			dgvCourses.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
			dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 12F);
			dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
			dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
			dataGridViewCellStyle4.SelectionForeColor = Color.White;
			dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
			dgvCourses.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			dgvCourses.RowHeadersVisible = false;
			dgvCourses.RowHeadersWidth = 51;
			dataGridViewCellStyle5.BackColor = Color.White;
			dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 12F);
			dgvCourses.RowsDefaultCellStyle = dataGridViewCellStyle5;
			dgvCourses.SelectedIndex = -1;
			dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvCourses.Size = new Size(1100, 960);
			dgvCourses.StripeOddColor = Color.FromArgb(235, 243, 255);
			dgvCourses.TabIndex = 0;
			// 
			// pnlSearch
			// 
			pnlSearch.Controls.Add(btnSearch);
			pnlSearch.Controls.Add(txtSearch);
			pnlSearch.Dock = DockStyle.Top;
			pnlSearch.Font = new Font("Microsoft Sans Serif", 12F);
			pnlSearch.Location = new Point(0, 0);
			pnlSearch.Margin = new Padding(4, 5, 4, 5);
			pnlSearch.MinimumSize = new Size(1, 1);
			pnlSearch.Name = "pnlSearch";
			pnlSearch.Size = new Size(1100, 60);
			pnlSearch.TabIndex = 1;
			pnlSearch.Text = null;
			pnlSearch.TextAlignment = ContentAlignment.MiddleCenter;
			// 
			// btnSearch
			// 
			btnSearch.Font = new Font("Microsoft Sans Serif", 12F);
			btnSearch.Location = new Point(0, 0);
			btnSearch.MinimumSize = new Size(1, 1);
			btnSearch.Name = "btnSearch";
			btnSearch.Size = new Size(100, 35);
			btnSearch.TabIndex = 0;
			btnSearch.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// txtSearch
			// 
			txtSearch.Font = new Font("Microsoft Sans Serif", 12F);
			txtSearch.Location = new Point(0, 0);
			txtSearch.Margin = new Padding(4, 5, 4, 5);
			txtSearch.MinimumSize = new Size(1, 16);
			txtSearch.Name = "txtSearch";
			txtSearch.Padding = new Padding(5);
			txtSearch.ShowText = false;
			txtSearch.Size = new Size(150, 29);
			txtSearch.TabIndex = 1;
			txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
			txtSearch.Watermark = "";
			// 
			// pnlRight
			// 
			pnlRight.Controls.Add(tabControlDetail);
			pnlRight.Dock = DockStyle.Right;
			pnlRight.Font = new Font("Microsoft Sans Serif", 12F);
			pnlRight.Location = new Point(1100, 0);
			pnlRight.Margin = new Padding(4, 5, 4, 5);
			pnlRight.MinimumSize = new Size(1, 1);
			pnlRight.Name = "pnlRight";
			pnlRight.Padding = new Padding(1, 35, 1, 1);
			pnlRight.ShowText = false;
			pnlRight.Size = new Size(630, 1020);
			pnlRight.TabIndex = 1;
			pnlRight.Text = "QUẢN LÝ NỘI DUNG";
			pnlRight.TextAlignment = ContentAlignment.MiddleCenter;
			// 
			// tabControlDetail
			// 
			tabControlDetail.Controls.Add(tpCourseInfo);
			tabControlDetail.Controls.Add(tpLessons);
			tabControlDetail.Dock = DockStyle.Fill;
			tabControlDetail.DrawMode = TabDrawMode.OwnerDrawFixed;
			tabControlDetail.Font = new Font("Microsoft Sans Serif", 12F);
			tabControlDetail.ItemSize = new Size(150, 40);
			tabControlDetail.Location = new Point(1, 35);
			tabControlDetail.MainPage = "";
			tabControlDetail.Name = "tabControlDetail";
			tabControlDetail.SelectedIndex = 0;
			tabControlDetail.Size = new Size(628, 984);
			tabControlDetail.SizeMode = TabSizeMode.Fixed;
			tabControlDetail.TabIndex = 0;
			tabControlDetail.TabUnSelectedForeColor = Color.FromArgb(240, 240, 240);
			tabControlDetail.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// tpCourseInfo
			// 
			tpCourseInfo.Controls.Add(lblId);
			tpCourseInfo.Controls.Add(txtId);
			tpCourseInfo.Controls.Add(lblTitle);
			tpCourseInfo.Controls.Add(txtTitle);
			tpCourseInfo.Controls.Add(lblDesc);
			tpCourseInfo.Controls.Add(txtDesc);
			tpCourseInfo.Controls.Add(lblLevel);
			tpCourseInfo.Controls.Add(cboLevel);
			tpCourseInfo.Controls.Add(lblPrice);
			tpCourseInfo.Controls.Add(txtPrice);
			tpCourseInfo.Controls.Add(lblPublished);
			tpCourseInfo.Controls.Add(swPublished);
			tpCourseInfo.Controls.Add(btnSave);
			tpCourseInfo.Controls.Add(btnDelete);
			tpCourseInfo.Controls.Add(btnRefresh);
			tpCourseInfo.Location = new Point(0, 40);
			tpCourseInfo.Name = "tpCourseInfo";
			tpCourseInfo.Size = new Size(628, 944);
			tpCourseInfo.TabIndex = 0;
			tpCourseInfo.Text = "1. Thông tin khóa học";
			// 
			// lblId
			// 
			lblId.Font = new Font("Microsoft Sans Serif", 12F);
			lblId.ForeColor = Color.FromArgb(48, 48, 48);
			lblId.Location = new Point(20, 20);
			lblId.Name = "lblId";
			lblId.Size = new Size(100, 23);
			lblId.TabIndex = 0;
			lblId.Text = "Mã KH:";
			// 
			// txtId
			// 
			txtId.Enabled = false;
			txtId.Font = new Font("Microsoft Sans Serif", 12F);
			txtId.Location = new Point(20, 45);
			txtId.Margin = new Padding(4, 5, 4, 5);
			txtId.MinimumSize = new Size(1, 16);
			txtId.Name = "txtId";
			txtId.Padding = new Padding(5);
			txtId.ShowText = false;
			txtId.Size = new Size(580, 29);
			txtId.TabIndex = 1;
			txtId.TextAlignment = ContentAlignment.MiddleLeft;
			txtId.Watermark = "";
			// 
			// lblTitle
			// 
			lblTitle.Font = new Font("Microsoft Sans Serif", 12F);
			lblTitle.ForeColor = Color.FromArgb(48, 48, 48);
			lblTitle.Location = new Point(20, 90);
			lblTitle.Name = "lblTitle";
			lblTitle.Size = new Size(100, 23);
			lblTitle.TabIndex = 2;
			lblTitle.Text = "Tên khóa học:";
			// 
			// txtTitle
			// 
			txtTitle.Font = new Font("Microsoft Sans Serif", 12F);
			txtTitle.Location = new Point(20, 115);
			txtTitle.Margin = new Padding(4, 5, 4, 5);
			txtTitle.MinimumSize = new Size(1, 16);
			txtTitle.Name = "txtTitle";
			txtTitle.Padding = new Padding(5);
			txtTitle.ShowText = false;
			txtTitle.Size = new Size(580, 29);
			txtTitle.TabIndex = 3;
			txtTitle.TextAlignment = ContentAlignment.MiddleLeft;
			txtTitle.Watermark = "";
			// 
			// lblDesc
			// 
			lblDesc.Font = new Font("Microsoft Sans Serif", 12F);
			lblDesc.ForeColor = Color.FromArgb(48, 48, 48);
			lblDesc.Location = new Point(20, 160);
			lblDesc.Name = "lblDesc";
			lblDesc.Size = new Size(100, 23);
			lblDesc.TabIndex = 4;
			lblDesc.Text = "Mô tả:";
			// 
			// txtDesc
			// 
			txtDesc.Font = new Font("Microsoft Sans Serif", 12F);
			txtDesc.Location = new Point(20, 185);
			txtDesc.Margin = new Padding(4, 5, 4, 5);
			txtDesc.MinimumSize = new Size(1, 16);
			txtDesc.Multiline = true;
			txtDesc.Name = "txtDesc";
			txtDesc.Padding = new Padding(5);
			txtDesc.ShowText = false;
			txtDesc.Size = new Size(580, 120);
			txtDesc.TabIndex = 5;
			txtDesc.TextAlignment = ContentAlignment.MiddleLeft;
			txtDesc.Watermark = "";
			// 
			// lblLevel
			// 
			lblLevel.Font = new Font("Microsoft Sans Serif", 12F);
			lblLevel.ForeColor = Color.FromArgb(48, 48, 48);
			lblLevel.Location = new Point(20, 320);
			lblLevel.Name = "lblLevel";
			lblLevel.Size = new Size(100, 23);
			lblLevel.TabIndex = 6;
			lblLevel.Text = "Trình độ:";
			// 
			// cboLevel
			// 
			cboLevel.DataSource = null;
			cboLevel.FillColor = Color.White;
			cboLevel.Font = new Font("Microsoft Sans Serif", 12F);
			cboLevel.ItemHoverColor = Color.FromArgb(155, 200, 255);
			cboLevel.Items.AddRange(new object[] { "BEGINNER", "INTERMEDIATE", "ADVANCED" });
			cboLevel.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
			cboLevel.Location = new Point(20, 345);
			cboLevel.Margin = new Padding(4, 5, 4, 5);
			cboLevel.MinimumSize = new Size(63, 0);
			cboLevel.Name = "cboLevel";
			cboLevel.Padding = new Padding(0, 0, 30, 2);
			cboLevel.Size = new Size(580, 29);
			cboLevel.SymbolSize = 24;
			cboLevel.TabIndex = 7;
			cboLevel.TextAlignment = ContentAlignment.MiddleLeft;
			cboLevel.Watermark = "";
			// 
			// lblPrice
			// 
			lblPrice.Font = new Font("Microsoft Sans Serif", 12F);
			lblPrice.ForeColor = Color.FromArgb(48, 48, 48);
			lblPrice.Location = new Point(20, 390);
			lblPrice.Name = "lblPrice";
			lblPrice.Size = new Size(100, 23);
			lblPrice.TabIndex = 8;
			lblPrice.Text = "Giá (VNĐ):";
			// 
			// txtPrice
			// 
			txtPrice.Font = new Font("Microsoft Sans Serif", 12F);
			txtPrice.Location = new Point(20, 415);
			txtPrice.Margin = new Padding(4, 5, 4, 5);
			txtPrice.MinimumSize = new Size(1, 16);
			txtPrice.Name = "txtPrice";
			txtPrice.Padding = new Padding(5);
			txtPrice.ShowText = false;
			txtPrice.Size = new Size(580, 29);
			txtPrice.TabIndex = 9;
			txtPrice.Text = "0";
			txtPrice.TextAlignment = ContentAlignment.MiddleLeft;
			txtPrice.Type = Sunny.UI.UITextBox.UIEditType.Integer;
			txtPrice.Watermark = "";
			// 
			// lblPublished
			// 
			lblPublished.Font = new Font("Microsoft Sans Serif", 12F);
			lblPublished.ForeColor = Color.FromArgb(48, 48, 48);
			lblPublished.Location = new Point(20, 470);
			lblPublished.Name = "lblPublished";
			lblPublished.Size = new Size(100, 23);
			lblPublished.TabIndex = 10;
			lblPublished.Text = "Trạng thái công khai:";
			// 
			// swPublished
			// 
			swPublished.Font = new Font("Microsoft Sans Serif", 12F);
			swPublished.Location = new Point(200, 468);
			swPublished.MinimumSize = new Size(1, 1);
			swPublished.Name = "swPublished";
			swPublished.Size = new Size(75, 29);
			swPublished.TabIndex = 11;
			// 
			// btnSave
			// 
			btnSave.Font = new Font("Microsoft Sans Serif", 12F);
			btnSave.Location = new Point(20, 530);
			btnSave.MinimumSize = new Size(1, 1);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(174, 45);
			btnSave.TabIndex = 12;
			btnSave.Text = "Lưu khóa học";
			btnSave.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// btnDelete
			// 
			btnDelete.FillColor = Color.FromArgb(230, 80, 80);
			btnDelete.Font = new Font("Microsoft Sans Serif", 12F);
			btnDelete.Location = new Point(235, 530);
			btnDelete.MinimumSize = new Size(1, 1);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new Size(120, 45);
			btnDelete.Symbol = 61453;
			btnDelete.TabIndex = 13;
			btnDelete.Text = "Xóa";
			btnDelete.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// btnRefresh
			// 
			btnRefresh.Font = new Font("Microsoft Sans Serif", 12F);
			btnRefresh.Location = new Point(395, 530);
			btnRefresh.MinimumSize = new Size(1, 1);
			btnRefresh.Name = "btnRefresh";
			btnRefresh.Size = new Size(120, 45);
			btnRefresh.Symbol = 61473;
			btnRefresh.TabIndex = 14;
			btnRefresh.Text = "Làm mới";
			btnRefresh.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// tpLessons
			// 
			tpLessons.Controls.Add(dgvLessons);
			tpLessons.Controls.Add(pnlLessonTool);
			tpLessons.Location = new Point(0, 40);
			tpLessons.Name = "tpLessons";
			tpLessons.Size = new Size(200, 60);
			tpLessons.TabIndex = 1;
			tpLessons.Text = "2. Bài học & Video";
			// 
			// dgvLessons
			// 
			dataGridViewCellStyle6.BackColor = Color.FromArgb(235, 243, 255);
			dgvLessons.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
			dgvLessons.BackgroundColor = Color.White;
			dgvLessons.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle7.BackColor = Color.FromArgb(110, 190, 40);
			dgvLessons.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			dgvLessons.ColumnHeadersHeight = 32;
			dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle8.BackColor = SystemColors.Window;
			dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 12F);
			dataGridViewCellStyle8.ForeColor = Color.FromArgb(48, 48, 48);
			dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
			dgvLessons.DefaultCellStyle = dataGridViewCellStyle8;
			dgvLessons.Dock = DockStyle.Fill;
			dgvLessons.EnableHeadersVisualStyles = false;
			dgvLessons.Font = new Font("Microsoft Sans Serif", 12F);
			dgvLessons.GridColor = Color.FromArgb(80, 160, 255);
			dgvLessons.Location = new Point(0, 80);
			dgvLessons.Name = "dgvLessons";
			dgvLessons.ReadOnly = true;
			dgvLessons.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = Color.FromArgb(235, 243, 255);
			dataGridViewCellStyle9.Font = new Font("Microsoft Sans Serif", 12F);
			dataGridViewCellStyle9.ForeColor = Color.FromArgb(48, 48, 48);
			dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(80, 160, 255);
			dataGridViewCellStyle9.SelectionForeColor = Color.White;
			dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
			dgvLessons.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
			dgvLessons.RowHeadersVisible = false;
			dgvLessons.RowHeadersWidth = 51;
			dataGridViewCellStyle10.BackColor = Color.White;
			dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 12F);
			dgvLessons.RowsDefaultCellStyle = dataGridViewCellStyle10;
			dgvLessons.SelectedIndex = -1;
			dgvLessons.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvLessons.Size = new Size(200, 0);
			dgvLessons.StripeOddColor = Color.FromArgb(235, 243, 255);
			dgvLessons.TabIndex = 0;
			// 
			// pnlLessonTool
			// 
			pnlLessonTool.Controls.Add(btnAddLesson);
			pnlLessonTool.Controls.Add(lblLessonHint);
			pnlLessonTool.Dock = DockStyle.Top;
			pnlLessonTool.Font = new Font("Microsoft Sans Serif", 12F);
			pnlLessonTool.Location = new Point(0, 0);
			pnlLessonTool.Margin = new Padding(4, 5, 4, 5);
			pnlLessonTool.MinimumSize = new Size(1, 1);
			pnlLessonTool.Name = "pnlLessonTool";
			pnlLessonTool.Size = new Size(200, 80);
			pnlLessonTool.TabIndex = 1;
			pnlLessonTool.Text = null;
			pnlLessonTool.TextAlignment = ContentAlignment.MiddleCenter;
			// 
			// btnAddLesson
			// 
			btnAddLesson.FillColor = Color.SeaGreen;
			btnAddLesson.Font = new Font("Microsoft Sans Serif", 12F);
			btnAddLesson.Location = new Point(15, 15);
			btnAddLesson.MinimumSize = new Size(1, 1);
			btnAddLesson.Name = "btnAddLesson";
			btnAddLesson.Size = new Size(180, 45);
			btnAddLesson.Symbol = 61525;
			btnAddLesson.TabIndex = 0;
			btnAddLesson.Text = "Thêm buổi học";
			btnAddLesson.TipsFont = new Font("Microsoft Sans Serif", 9F);
			// 
			// lblLessonHint
			// 
			lblLessonHint.Font = new Font("Microsoft Sans Serif", 12F);
			lblLessonHint.ForeColor = Color.Gray;
			lblLessonHint.Location = new Point(210, 25);
			lblLessonHint.Name = "lblLessonHint";
			lblLessonHint.Size = new Size(350, 25);
			lblLessonHint.TabIndex = 1;
			lblLessonHint.Text = "(Nhấn đúp vào dòng để sửa Video/Câu hỏi)";
			// 
			// CoursesPanel
			// 
			Controls.Add(pnlLeft);
			Controls.Add(pnlRight);
			Name = "CoursesPanel";
			Size = new Size(1730, 1020);
			pnlLeft.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvCourses).EndInit();
			pnlSearch.ResumeLayout(false);
			pnlRight.ResumeLayout(false);
			tabControlDetail.ResumeLayout(false);
			tpCourseInfo.ResumeLayout(false);
			tpLessons.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvLessons).EndInit();
			pnlLessonTool.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Sunny.UI.UIPanel pnlLeft;
		private Sunny.UI.UITitlePanel pnlRight;
		private Sunny.UI.UIPanel pnlSearch;
		private Sunny.UI.UIDataGridView dgvCourses;
		private Sunny.UI.UITextBox txtSearch;
		private Sunny.UI.UISymbolButton btnSearch;

		// TabControl & Pages
		private Sunny.UI.UITabControl tabControlDetail;
		private System.Windows.Forms.TabPage tpCourseInfo;
		private System.Windows.Forms.TabPage tpLessons;

		// Tab 1 Controls
		private Sunny.UI.UILabel lblId, lblTitle, lblDesc, lblLevel, lblPrice, lblPublished;
		private Sunny.UI.UITextBox txtId, txtTitle, txtDesc, txtPrice;
		private Sunny.UI.UIComboBox cboLevel;
		private Sunny.UI.UISwitch swPublished;
		private Sunny.UI.UISymbolButton btnSave, btnDelete, btnRefresh;

		// Tab 2 Controls
		private Sunny.UI.UIDataGridView dgvLessons;
		private Sunny.UI.UIPanel pnlLessonTool;
		private Sunny.UI.UISymbolButton btnAddLesson;
		private Sunny.UI.UILabel lblLessonHint;
	}
}
>>>>>>> feat/Course
