namespace IELTS.UI.Admin.AccountManager
{
    // Đảm bảo tên class là AccountsPanel (KHÔNG có dấu gạch dưới)
    partial class AccountsPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

            this.pnlLeft = new Sunny.UI.UIPanel();
            this.dgvAccounts = new Sunny.UI.UIDataGridView();
            this.pnlSearch = new Sunny.UI.UIPanel();
            this.btnSearch = new Sunny.UI.UISymbolButton();
            this.txtSearch = new Sunny.UI.UITextBox();
            this.pnlRight = new Sunny.UI.UITitlePanel();
            this.btnRefresh = new Sunny.UI.UISymbolButton();
            this.btnDelete = new Sunny.UI.UISymbolButton();
            this.btnSave = new Sunny.UI.UISymbolButton();
            this.swIsActive = new Sunny.UI.UISwitch();
            this.lblActive = new Sunny.UI.UILabel();
            this.cboRole = new Sunny.UI.UIComboBox();
            this.lblRole = new Sunny.UI.UILabel();
            this.txtPassword = new Sunny.UI.UITextBox();
            this.lblPassword = new Sunny.UI.UILabel();
            this.txtEmail = new Sunny.UI.UITextBox();
            this.lblEmail = new Sunny.UI.UILabel();
            this.txtFullName = new Sunny.UI.UITextBox();
            this.lblFullName = new Sunny.UI.UILabel();
            this.txtId = new Sunny.UI.UITextBox();
            this.lblId = new Sunny.UI.UILabel();

            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();

            // pnlLeft
            this.pnlLeft.Controls.Add(this.dgvAccounts);
            this.pnlLeft.Controls.Add(this.pnlSearch);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.RectColor = System.Drawing.Color.Transparent;
            this.pnlLeft.Size = new System.Drawing.Size(1230, 1020);
            this.pnlLeft.TabIndex = 0;
            this.pnlLeft.Text = null;

            // dgvAccounts
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.dgvAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAccounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAccounts.BackgroundColor = System.Drawing.Color.White;
            this.dgvAccounts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAccounts.ColumnHeadersHeight = 32;
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccounts.EnableHeadersVisualStyles = false;
            this.dgvAccounts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(160)))), ((int)(((byte)(255)))));
            this.dgvAccounts.Location = new System.Drawing.Point(0, 60);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.ReadOnly = true;
            this.dgvAccounts.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            this.dgvAccounts.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAccounts.RowTemplate.Height = 29;
            this.dgvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccounts.Size = new System.Drawing.Size(1230, 960);
            this.dgvAccounts.TabIndex = 1;

            // pnlSearch
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1230, 60);
            this.pnlSearch.TabIndex = 0;

            // txtSearch
            this.txtSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtSearch.Location = new System.Drawing.Point(13, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Padding = new System.Windows.Forms.Padding(5);
            this.txtSearch.Size = new System.Drawing.Size(350, 29);
            this.txtSearch.Symbol = 61442;
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Watermark = "Nhập tên hoặc email...";

            // btnSearch
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnSearch.Location = new System.Drawing.Point(375, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 35);
            this.btnSearch.Symbol = 61442;
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Tìm kiếm";

            // pnlRight
            this.pnlRight.Controls.Add(this.btnRefresh);
            this.pnlRight.Controls.Add(this.btnDelete);
            this.pnlRight.Controls.Add(this.btnSave);
            this.pnlRight.Controls.Add(this.swIsActive);
            this.pnlRight.Controls.Add(this.lblActive);
            this.pnlRight.Controls.Add(this.cboRole);
            this.pnlRight.Controls.Add(this.lblRole);
            this.pnlRight.Controls.Add(this.txtPassword);
            this.pnlRight.Controls.Add(this.lblPassword);
            this.pnlRight.Controls.Add(this.txtEmail);
            this.pnlRight.Controls.Add(this.lblEmail);
            this.pnlRight.Controls.Add(this.txtFullName);
            this.pnlRight.Controls.Add(this.lblFullName);
            this.pnlRight.Controls.Add(this.txtId);
            this.pnlRight.Controls.Add(this.lblId);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlRight.Location = new System.Drawing.Point(1230, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10, 35, 10, 10);
            this.pnlRight.Size = new System.Drawing.Size(500, 1020);
            this.pnlRight.TabIndex = 1;
            this.pnlRight.Text = "THÔNG TIN CHI TIẾT";
            this.pnlRight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // Labels & Textboxes
            this.lblId.AutoSize = true; this.lblId.Location = new System.Drawing.Point(30, 60); this.lblId.Text = "ID:";
            this.txtId.Enabled = false; this.txtId.Location = new System.Drawing.Point(30, 85); this.txtId.Size = new System.Drawing.Size(440, 29);

            this.lblFullName.AutoSize = true; this.lblFullName.Location = new System.Drawing.Point(30, 130); this.lblFullName.Text = "Họ và tên:";
            this.txtFullName.Location = new System.Drawing.Point(30, 155); this.txtFullName.Size = new System.Drawing.Size(440, 29);

            this.lblEmail.AutoSize = true; this.lblEmail.Location = new System.Drawing.Point(30, 200); this.lblEmail.Text = "Email:";
            this.txtEmail.Location = new System.Drawing.Point(30, 225); this.txtEmail.Size = new System.Drawing.Size(440, 29);

            this.lblPassword.AutoSize = true; this.lblPassword.Location = new System.Drawing.Point(30, 270); this.lblPassword.Text = "Mật khẩu:";
            this.txtPassword.Location = new System.Drawing.Point(30, 295); this.txtPassword.PasswordChar = '*'; this.txtPassword.Size = new System.Drawing.Size(440, 29);

            this.lblRole.AutoSize = true; this.lblRole.Location = new System.Drawing.Point(30, 340); this.lblRole.Text = "Vai trò:";
            this.cboRole.Items.AddRange(new object[] { "Student", "Teacher", "Admin" });
            this.cboRole.Location = new System.Drawing.Point(30, 365); this.cboRole.Size = new System.Drawing.Size(440, 29);

            this.lblActive.AutoSize = true; this.lblActive.Location = new System.Drawing.Point(30, 420); this.lblActive.Text = "Trạng thái:";
            this.swIsActive.Active = true; this.swIsActive.Location = new System.Drawing.Point(200, 418); this.swIsActive.ActiveText = "Bật"; this.swIsActive.InActiveText = "Tắt";

            // Buttons
            this.btnSave.Location = new System.Drawing.Point(30, 480); this.btnSave.Size = new System.Drawing.Size(130, 40); this.btnSave.Symbol = 61639; this.btnSave.Text = "Lưu";
            this.btnDelete.Location = new System.Drawing.Point(170, 480); this.btnDelete.Size = new System.Drawing.Size(100, 40); this.btnDelete.Symbol = 61453; this.btnDelete.Text = "Xóa"; this.btnDelete.FillColor = System.Drawing.Color.Red;
            this.btnRefresh.Location = new System.Drawing.Point(280, 480); this.btnRefresh.Size = new System.Drawing.Size(120, 40); this.btnRefresh.Symbol = 61473; this.btnRefresh.Text = "Làm mới"; this.btnRefresh.FillColor = System.Drawing.Color.Gray;

            // AccountsPanel
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Name = "AccountsPanel"; // 👉 Đã sửa lại tên cho khớp
            this.Size = new System.Drawing.Size(1730, 1020);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlRight.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Sunny.UI.UIPanel pnlLeft;
        private Sunny.UI.UITitlePanel pnlRight;
        private Sunny.UI.UIPanel pnlSearch;
        private Sunny.UI.UIDataGridView dgvAccounts;
        private Sunny.UI.UITextBox txtSearch;
        private Sunny.UI.UISymbolButton btnSearch;
        private Sunny.UI.UILabel lblId, lblFullName, lblEmail, lblPassword, lblRole, lblActive;
        private Sunny.UI.UITextBox txtId, txtFullName, txtEmail, txtPassword;
        private Sunny.UI.UIComboBox cboRole;
        private Sunny.UI.UISwitch swIsActive;
        private Sunny.UI.UISymbolButton btnSave, btnDelete, btnRefresh;
    }
}