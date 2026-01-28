namespace IELTS.UI.Admin.CoursesManager
{
    partial class CoursesPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

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
            lblHeader = new Sunny.UI.UILabel();
            txtSearch = new Sunny.UI.UITextBox();
            btnSearch = new Sunny.UI.UISymbolButton();
            pnlRight = new Sunny.UI.UITitlePanel();
            tabControlDetail = new Sunny.UI.UITabControl();
            tpCourseInfo = new TabPage();
            tpLessons = new TabPage();
            dgvLessons = new Sunny.UI.UIDataGridView();
            pnlLessonTool = new Sunny.UI.UIPanel();
            btnAddLesson = new Sunny.UI.UISymbolButton();
            lblLessonHint = new Sunny.UI.UILabel();
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
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCourses).BeginInit();
            pnlSearch.SuspendLayout();
            pnlRight.SuspendLayout();
            tabControlDetail.SuspendLayout();
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
            pnlLeft.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Margin = new Padding(4, 5, 4, 5);
            pnlLeft.MinimumSize = new Size(1, 1);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(12);
            pnlLeft.Size = new Size(943, 900);
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
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
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
            dgvCourses.Location = new Point(12, 135);
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
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dgvCourses.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvCourses.SelectedIndex = -1;
            dgvCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCourses.Size = new Size(919, 753);
            dgvCourses.StripeOddColor = Color.FromArgb(235, 243, 255);
            dgvCourses.TabIndex = 0;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(lblHeader);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlSearch.Location = new Point(12, 12);
            pnlSearch.Margin = new Padding(4, 5, 4, 5);
            pnlSearch.MinimumSize = new Size(1, 1);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(919, 123);
            pnlSearch.TabIndex = 1;
            pnlSearch.Text = null;
            pnlSearch.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblHeader
            // 
            lblHeader.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblHeader.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblHeader.ForeColor = Color.FromArgb(48, 48, 48);
            lblHeader.Location = new Point(10, 15);
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new Size(196, 40);
            lblHeader.TabIndex = 0;
            lblHeader.Text = "KHÓA HỌC";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtSearch.Location = new Point(10, 68);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.MinimumSize = new Size(1, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(5);
            txtSearch.Radius = 12;
            txtSearch.ShowText = false;
            txtSearch.Size = new Size(300, 44);
            txtSearch.TabIndex = 1;
            txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearch.Watermark = "Tìm khóa học...";
            // 
            // btnSearch
            // 
            btnSearch.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnSearch.Location = new Point(330, 68);
            btnSearch.MinimumSize = new Size(1, 1);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(100, 44);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Tìm";
            btnSearch.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(tabControlDetail);
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlRight.Location = new Point(943, 0);
            pnlRight.Margin = new Padding(4, 5, 4, 5);
            pnlRight.MinimumSize = new Size(1, 1);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(10, 40, 10, 10);
            pnlRight.ShowText = false;
            pnlRight.Size = new Size(757, 900);
            pnlRight.TabIndex = 1;
            pnlRight.Text = "CHI TIẾT KHÓA HỌC";
            pnlRight.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // tabControlDetail
            // 
            tabControlDetail.Controls.Add(tpCourseInfo);
            tabControlDetail.Controls.Add(tpLessons);
            tabControlDetail.Dock = DockStyle.Fill;
            tabControlDetail.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlDetail.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            tabControlDetail.ItemSize = new Size(150, 40);
            tabControlDetail.Location = new Point(10, 40);
            tabControlDetail.MainPage = "";
            tabControlDetail.Name = "tabControlDetail";
            tabControlDetail.SelectedIndex = 0;
            tabControlDetail.Size = new Size(737, 850);
            tabControlDetail.SizeMode = TabSizeMode.Fixed;
            tabControlDetail.TabIndex = 0;
            tabControlDetail.TabUnSelectedForeColor = Color.FromArgb(240, 240, 240);
            tabControlDetail.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // tpCourseInfo
            // 
            tpCourseInfo.Location = new Point(0, 40);
            tpCourseInfo.Name = "tpCourseInfo";
            tpCourseInfo.Size = new Size(737, 810);
            tpCourseInfo.TabIndex = 0;
            tpCourseInfo.Text = "1. Thông tin khóa học";
            // 
            // tpLessons
            // 
            tpLessons.Controls.Add(dgvLessons);
            tpLessons.Controls.Add(pnlLessonTool);
            tpLessons.Location = new Point(0, 40);
            tpLessons.Name = "tpLessons";
            tpLessons.Size = new Size(200, 60);
            tpLessons.TabIndex = 1;
            tpLessons.Text = "2. Bài học";
            // 
            // dgvLessons
            // 
            dataGridViewCellStyle6.BackColor = Color.FromArgb(235, 243, 255);
            dgvLessons.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dgvLessons.BackgroundColor = Color.White;
            dgvLessons.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle7.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvLessons.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvLessons.ColumnHeadersHeight = 32;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvLessons.DefaultCellStyle = dataGridViewCellStyle8;
            dgvLessons.Dock = DockStyle.Fill;
            dgvLessons.EnableHeadersVisualStyles = false;
            dgvLessons.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dgvLessons.GridColor = Color.FromArgb(80, 160, 255);
            dgvLessons.Location = new Point(0, 70);
            dgvLessons.Name = "dgvLessons";
            dgvLessons.ReadOnly = true;
            dgvLessons.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle9.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle9.SelectionForeColor = Color.White;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvLessons.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvLessons.RowHeadersVisible = false;
            dgvLessons.RowHeadersWidth = 51;
            dataGridViewCellStyle10.BackColor = Color.White;
            dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dgvLessons.RowsDefaultCellStyle = dataGridViewCellStyle10;
            dgvLessons.SelectedIndex = -1;
            dgvLessons.Size = new Size(200, 0);
            dgvLessons.StripeOddColor = Color.FromArgb(235, 243, 255);
            dgvLessons.TabIndex = 0;
            // 
            // pnlLessonTool
            // 
            pnlLessonTool.Controls.Add(btnAddLesson);
            pnlLessonTool.Controls.Add(lblLessonHint);
            pnlLessonTool.Dock = DockStyle.Top;
            pnlLessonTool.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            pnlLessonTool.Location = new Point(0, 0);
            pnlLessonTool.Margin = new Padding(4, 5, 4, 5);
            pnlLessonTool.MinimumSize = new Size(1, 1);
            pnlLessonTool.Name = "pnlLessonTool";
            pnlLessonTool.Size = new Size(200, 70);
            pnlLessonTool.TabIndex = 1;
            pnlLessonTool.Text = null;
            pnlLessonTool.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnAddLesson
            // 
            btnAddLesson.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnAddLesson.Location = new Point(10, 15);
            btnAddLesson.MinimumSize = new Size(1, 1);
            btnAddLesson.Name = "btnAddLesson";
            btnAddLesson.Size = new Size(190, 40);
            btnAddLesson.TabIndex = 0;
            btnAddLesson.Text = "Thêm buổi học";
            btnAddLesson.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // lblLessonHint
            // 
            lblLessonHint.BackColor = Color.FromArgb(0, 0, 0, 0);
            lblLessonHint.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblLessonHint.ForeColor = Color.FromArgb(48, 48, 48);
            lblLessonHint.Location = new Point(221, 25);
            lblLessonHint.Name = "lblLessonHint";
            lblLessonHint.Size = new Size(255, 23);
            lblLessonHint.TabIndex = 1;
            lblLessonHint.Text = "Double-click để sửa bài học";
            // 
            // lblId
            // 
            lblId.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblId.ForeColor = Color.FromArgb(48, 48, 48);
            lblId.Location = new Point(0, 0);
            lblId.Name = "lblId";
            lblId.Size = new Size(100, 23);
            lblId.TabIndex = 0;
            // 
            // txtId
            // 
            txtId.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtId.Location = new Point(0, 0);
            txtId.Margin = new Padding(4, 5, 4, 5);
            txtId.MinimumSize = new Size(1, 16);
            txtId.Name = "txtId";
            txtId.Padding = new Padding(5);
            txtId.ShowText = false;
            txtId.Size = new Size(150, 29);
            txtId.TabIndex = 0;
            txtId.TextAlignment = ContentAlignment.MiddleLeft;
            txtId.Watermark = "";
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblTitle.ForeColor = Color.FromArgb(48, 48, 48);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(100, 23);
            lblTitle.TabIndex = 0;
            // 
            // txtTitle
            // 
            txtTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtTitle.Location = new Point(0, 0);
            txtTitle.Margin = new Padding(4, 5, 4, 5);
            txtTitle.MinimumSize = new Size(1, 16);
            txtTitle.Name = "txtTitle";
            txtTitle.Padding = new Padding(5);
            txtTitle.ShowText = false;
            txtTitle.Size = new Size(150, 29);
            txtTitle.TabIndex = 0;
            txtTitle.TextAlignment = ContentAlignment.MiddleLeft;
            txtTitle.Watermark = "";
            // 
            // lblDesc
            // 
            lblDesc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblDesc.ForeColor = Color.FromArgb(48, 48, 48);
            lblDesc.Location = new Point(0, 0);
            lblDesc.Name = "lblDesc";
            lblDesc.Size = new Size(100, 23);
            lblDesc.TabIndex = 0;
            // 
            // txtDesc
            // 
            txtDesc.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtDesc.Location = new Point(0, 0);
            txtDesc.Margin = new Padding(4, 5, 4, 5);
            txtDesc.MinimumSize = new Size(1, 16);
            txtDesc.Name = "txtDesc";
            txtDesc.Padding = new Padding(5);
            txtDesc.ShowText = false;
            txtDesc.Size = new Size(150, 29);
            txtDesc.TabIndex = 0;
            txtDesc.TextAlignment = ContentAlignment.MiddleLeft;
            txtDesc.Watermark = "";
            // 
            // lblLevel
            // 
            lblLevel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblLevel.ForeColor = Color.FromArgb(48, 48, 48);
            lblLevel.Location = new Point(0, 0);
            lblLevel.Name = "lblLevel";
            lblLevel.Size = new Size(100, 23);
            lblLevel.TabIndex = 0;
            // 
            // cboLevel
            // 
            cboLevel.DataSource = null;
            cboLevel.FillColor = Color.White;
            cboLevel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            cboLevel.ItemHoverColor = Color.FromArgb(155, 200, 255);
            cboLevel.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            cboLevel.Location = new Point(0, 0);
            cboLevel.Margin = new Padding(4, 5, 4, 5);
            cboLevel.MinimumSize = new Size(63, 0);
            cboLevel.Name = "cboLevel";
            cboLevel.Padding = new Padding(0, 0, 30, 2);
            cboLevel.Size = new Size(150, 29);
            cboLevel.SymbolSize = 24;
            cboLevel.TabIndex = 0;
            cboLevel.TextAlignment = ContentAlignment.MiddleLeft;
            cboLevel.Watermark = "";
            // 
            // lblPrice
            // 
            lblPrice.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblPrice.ForeColor = Color.FromArgb(48, 48, 48);
            lblPrice.Location = new Point(0, 0);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(100, 23);
            lblPrice.TabIndex = 0;
            // 
            // txtPrice
            // 
            txtPrice.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtPrice.Location = new Point(0, 0);
            txtPrice.Margin = new Padding(4, 5, 4, 5);
            txtPrice.MinimumSize = new Size(1, 16);
            txtPrice.Name = "txtPrice";
            txtPrice.Padding = new Padding(5);
            txtPrice.ShowText = false;
            txtPrice.Size = new Size(150, 29);
            txtPrice.TabIndex = 0;
            txtPrice.TextAlignment = ContentAlignment.MiddleLeft;
            txtPrice.Watermark = "";
            // 
            // lblPublished
            // 
            lblPublished.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblPublished.ForeColor = Color.FromArgb(48, 48, 48);
            lblPublished.Location = new Point(0, 0);
            lblPublished.Name = "lblPublished";
            lblPublished.Size = new Size(100, 23);
            lblPublished.TabIndex = 0;
            // 
            // swPublished
            // 
            swPublished.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            swPublished.Location = new Point(0, 0);
            swPublished.MinimumSize = new Size(1, 1);
            swPublished.Name = "swPublished";
            swPublished.Size = new Size(75, 29);
            swPublished.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnSave.Location = new Point(0, 0);
            btnSave.MinimumSize = new Size(1, 1);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 35);
            btnSave.TabIndex = 0;
            btnSave.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnDelete.Location = new Point(0, 0);
            btnDelete.MinimumSize = new Size(1, 1);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 35);
            btnDelete.TabIndex = 0;
            btnDelete.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnRefresh.Location = new Point(0, 0);
            btnRefresh.MinimumSize = new Size(1, 1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 35);
            btnRefresh.TabIndex = 0;
            btnRefresh.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // CoursesPanel
            // 
            Controls.Add(pnlLeft);
            Controls.Add(pnlRight);
            Name = "CoursesPanel";
            Size = new Size(1700, 900);
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCourses).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            tabControlDetail.ResumeLayout(false);
            tpLessons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvLessons).EndInit();
            pnlLessonTool.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel pnlLeft, pnlSearch;
        private Sunny.UI.UILabel lblHeader;
        private Sunny.UI.UITextBox txtSearch;
        private Sunny.UI.UISymbolButton btnSearch;
        private Sunny.UI.UIDataGridView dgvCourses;

        private Sunny.UI.UITitlePanel pnlRight;
        private Sunny.UI.UITabControl tabControlDetail;
        private System.Windows.Forms.TabPage tpCourseInfo, tpLessons;

        private Sunny.UI.UILabel lblId, lblTitle, lblDesc, lblLevel, lblPrice, lblPublished;
        private Sunny.UI.UITextBox txtId, txtTitle, txtDesc, txtPrice;
        private Sunny.UI.UIComboBox cboLevel;
        private Sunny.UI.UISwitch swPublished;

        private Sunny.UI.UISymbolButton btnSave, btnDelete, btnRefresh;

        private Sunny.UI.UIPanel pnlLessonTool;
        private Sunny.UI.UISymbolButton btnAddLesson;
        private Sunny.UI.UILabel lblLessonHint;
        private Sunny.UI.UIDataGridView dgvLessons;
    }
}
