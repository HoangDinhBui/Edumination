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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle style1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle style2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle style3 = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlLeft = new Sunny.UI.UIPanel();
            this.dgvCourses = new Sunny.UI.UIDataGridView();
            this.pnlSearch = new Sunny.UI.UIPanel();
            this.btnSearch = new Sunny.UI.UISymbolButton();
            this.txtSearch = new Sunny.UI.UITextBox();
            this.pnlRight = new Sunny.UI.UITitlePanel();
            this.btnRefresh = new Sunny.UI.UISymbolButton();
            this.btnDelete = new Sunny.UI.UISymbolButton();
            this.btnSave = new Sunny.UI.UISymbolButton();

            // Các control nhập liệu
            this.swPublished = new Sunny.UI.UISwitch();
            this.lblPublished = new Sunny.UI.UILabel();
            this.txtPrice = new Sunny.UI.UITextBox(); // Hoặc UIIntegerUpDown
            this.lblPrice = new Sunny.UI.UILabel();
            this.cboLevel = new Sunny.UI.UIComboBox();
            this.lblLevel = new Sunny.UI.UILabel();
            this.txtDesc = new Sunny.UI.UITextBox();
            this.lblDesc = new Sunny.UI.UILabel();
            this.txtTitle = new Sunny.UI.UITextBox();
            this.lblTitle = new Sunny.UI.UILabel();
            this.txtId = new Sunny.UI.UITextBox();
            this.lblId = new Sunny.UI.UILabel();

            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();

            // --- Left Panel ---
            this.pnlLeft.Controls.Add(this.dgvCourses);
            this.pnlLeft.Controls.Add(this.pnlSearch);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(1230, 1020);
            this.pnlLeft.TabIndex = 0;
            this.pnlLeft.RectColor = System.Drawing.Color.Transparent;

            // --- DataGridView ---
            style1.BackColor = System.Drawing.Color.FromArgb(235, 243, 255);
            this.dgvCourses.AlternatingRowsDefaultCellStyle = style1;
            this.dgvCourses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCourses.BackgroundColor = System.Drawing.Color.White;
            this.dgvCourses.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            style2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            style2.BackColor = System.Drawing.Color.FromArgb(80, 160, 255);
            style2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            style2.ForeColor = System.Drawing.Color.White;
            this.dgvCourses.ColumnHeadersDefaultCellStyle = style2;
            this.dgvCourses.ColumnHeadersHeight = 32;
            this.dgvCourses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCourses.EnableHeadersVisualStyles = false;
            this.dgvCourses.GridColor = System.Drawing.Color.FromArgb(80, 160, 255);
            this.dgvCourses.Location = new System.Drawing.Point(0, 60);
            this.dgvCourses.Name = "dgvCourses";
            this.dgvCourses.ReadOnly = true;
            this.dgvCourses.RowHeadersVisible = false;
            style3.BackColor = System.Drawing.Color.White;
            style3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dgvCourses.RowsDefaultCellStyle = style3;
            this.dgvCourses.RowTemplate.Height = 29;
            this.dgvCourses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCourses.Size = new System.Drawing.Size(1230, 960);
            this.dgvCourses.TabIndex = 1;

            // --- Search Panel ---
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1230, 60);
            this.pnlSearch.TabIndex = 0;

            // Search Controls
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtSearch.Location = new System.Drawing.Point(13, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Padding = new System.Windows.Forms.Padding(5);
            this.txtSearch.Size = new System.Drawing.Size(350, 29);
            this.txtSearch.Symbol = 61442;
            this.txtSearch.Watermark = "Tìm khóa học...";

            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnSearch.Location = new System.Drawing.Point(375, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 35);
            this.btnSearch.Symbol = 61442;
            this.btnSearch.Text = "Tìm kiếm";

            // --- Right Panel (Input Form) ---
            this.pnlRight.Controls.Add(this.btnRefresh);
            this.pnlRight.Controls.Add(this.btnDelete);
            this.pnlRight.Controls.Add(this.btnSave);
            this.pnlRight.Controls.Add(this.swPublished);
            this.pnlRight.Controls.Add(this.lblPublished);
            this.pnlRight.Controls.Add(this.txtPrice);
            this.pnlRight.Controls.Add(this.lblPrice);
            this.pnlRight.Controls.Add(this.cboLevel);
            this.pnlRight.Controls.Add(this.lblLevel);
            this.pnlRight.Controls.Add(this.txtDesc);
            this.pnlRight.Controls.Add(this.lblDesc);
            this.pnlRight.Controls.Add(this.txtTitle);
            this.pnlRight.Controls.Add(this.lblTitle);
            this.pnlRight.Controls.Add(this.txtId);
            this.pnlRight.Controls.Add(this.lblId);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlRight.Location = new System.Drawing.Point(1230, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10, 35, 10, 10);
            this.pnlRight.Size = new System.Drawing.Size(500, 1020);
            this.pnlRight.TabIndex = 1;
            this.pnlRight.Text = "CHI TIẾT KHÓA HỌC";
            this.pnlRight.TitleColor = System.Drawing.Color.FromArgb(80, 160, 255);

            // --- Input Controls ---
            // ID
            this.lblId.AutoSize = true; this.lblId.Location = new System.Drawing.Point(30, 60); this.lblId.Text = "Mã KH:";
            this.txtId.Enabled = false; this.txtId.Location = new System.Drawing.Point(30, 85); this.txtId.Size = new System.Drawing.Size(440, 29);

            // Title
            this.lblTitle.AutoSize = true; this.lblTitle.Location = new System.Drawing.Point(30, 130); this.lblTitle.Text = "Tên khóa học:";
            this.txtTitle.Location = new System.Drawing.Point(30, 155); this.txtTitle.Size = new System.Drawing.Size(440, 29);

            // Description (Multiline)
            this.lblDesc.AutoSize = true; this.lblDesc.Location = new System.Drawing.Point(30, 200); this.lblDesc.Text = "Mô tả:";
            this.txtDesc.Location = new System.Drawing.Point(30, 225); this.txtDesc.Size = new System.Drawing.Size(440, 100);
            this.txtDesc.Multiline = true; // Cho phép nhập nhiều dòng

            // Level
            this.lblLevel.AutoSize = true; this.lblLevel.Location = new System.Drawing.Point(30, 340); this.lblLevel.Text = "Trình độ:";
            this.cboLevel.Items.AddRange(new object[] { "BEGINNER", "INTERMEDIATE", "ADVANCED" });
            this.cboLevel.Location = new System.Drawing.Point(30, 365); this.cboLevel.Size = new System.Drawing.Size(440, 29);

            // Price
            this.lblPrice.AutoSize = true; this.lblPrice.Location = new System.Drawing.Point(30, 410); this.lblPrice.Text = "Giá (VNĐ):";
            this.txtPrice.Location = new System.Drawing.Point(30, 435); this.txtPrice.Size = new System.Drawing.Size(440, 29);
            this.txtPrice.Type = Sunny.UI.UITextBox.UIEditType.Integer; // Chỉ cho nhập số

            // Published
            this.lblPublished.AutoSize = true; this.lblPublished.Location = new System.Drawing.Point(30, 480); this.lblPublished.Text = "Xuất bản:";
            this.swPublished.Active = true; this.swPublished.Location = new System.Drawing.Point(200, 478);
            this.swPublished.ActiveText = "Có"; this.swPublished.InActiveText = "Không";

            // Buttons
            this.btnSave.Location = new System.Drawing.Point(30, 540); this.btnSave.Size = new System.Drawing.Size(130, 40);
            this.btnSave.Symbol = 61639; this.btnSave.Text = "Lưu";
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(110, 190, 40);

            this.btnDelete.Location = new System.Drawing.Point(170, 540); this.btnDelete.Size = new System.Drawing.Size(100, 40);
            this.btnDelete.Symbol = 61453; this.btnDelete.Text = "Xóa";
            this.btnDelete.FillColor = System.Drawing.Color.Red;
            this.btnDelete.Enabled = false;

            this.btnRefresh.Location = new System.Drawing.Point(280, 540); this.btnRefresh.Size = new System.Drawing.Size(120, 40);
            this.btnRefresh.Symbol = 61473; this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.FillColor = System.Drawing.Color.Gray;

            // Main Panel
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Name = "CoursesPanel";
            this.Size = new System.Drawing.Size(1730, 1020);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);
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
