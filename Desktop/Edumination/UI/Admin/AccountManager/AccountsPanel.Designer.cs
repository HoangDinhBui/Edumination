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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            pnlLeft = new Sunny.UI.UIPanel();
            dgvAccounts = new Sunny.UI.UIDataGridView();
            pnlSearch = new Sunny.UI.UIPanel();
            btnSearch = new Sunny.UI.UISymbolButton();
            txtSearch = new Sunny.UI.UITextBox();
            pnlRight = new Sunny.UI.UITitlePanel();
            btnRefresh = new Sunny.UI.UISymbolButton();
            btnDelete = new Sunny.UI.UISymbolButton();
            btnSave = new Sunny.UI.UISymbolButton();
            swIsActive = new Sunny.UI.UISwitch();
            lblActive = new Sunny.UI.UILabel();
            cboRole = new Sunny.UI.UIComboBox();
            lblRole = new Sunny.UI.UILabel();
            txtPassword = new Sunny.UI.UITextBox();
            lblPassword = new Sunny.UI.UILabel();
            txtEmail = new Sunny.UI.UITextBox();
            lblEmail = new Sunny.UI.UILabel();
            txtFullName = new Sunny.UI.UITextBox();
            lblFullName = new Sunny.UI.UILabel();
            txtId = new Sunny.UI.UITextBox();
            lblId = new Sunny.UI.UILabel();
            pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            pnlSearch.SuspendLayout();
            pnlRight.SuspendLayout();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(dgvAccounts);
            pnlLeft.Controls.Add(pnlSearch);
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.Font = new Font("Microsoft Sans Serif", 12F);
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Margin = new Padding(4, 5, 4, 5);
            pnlLeft.MinimumSize = new Size(1, 1);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.RectColor = Color.Transparent;
            pnlLeft.Size = new Size(1230, 1020);
            pnlLeft.TabIndex = 0;
            pnlLeft.Text = null;
            pnlLeft.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // dgvAccounts
            // 
            dataGridViewCellStyle1.BackColor = Color.FromArgb(235, 243, 255);
            dgvAccounts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvAccounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 12F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvAccounts.ColumnHeadersHeight = 32;
            dgvAccounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvAccounts.DefaultCellStyle = dataGridViewCellStyle3;
            dgvAccounts.Dock = DockStyle.Fill;
            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dgvAccounts.GridColor = Color.FromArgb(80, 160, 255);
            dgvAccounts.Location = new Point(0, 77);
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.ReadOnly = true;
            dgvAccounts.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(235, 243, 255);
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(48, 48, 48);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(80, 160, 255);
            dataGridViewCellStyle4.SelectionForeColor = Color.White;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvAccounts.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.RowHeadersWidth = 51;
            dataGridViewCellStyle5.BackColor = Color.White;
            dgvAccounts.RowsDefaultCellStyle = dataGridViewCellStyle5;
            dgvAccounts.SelectedIndex = -1;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.Size = new Size(1230, 943);
            dgvAccounts.StripeOddColor = Color.FromArgb(235, 243, 255);
            dgvAccounts.TabIndex = 1;
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
            pnlSearch.Size = new Size(1230, 77);
            pnlSearch.TabIndex = 0;
            pnlSearch.Text = null;
            pnlSearch.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(0, 0, 0, 0);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Font = new Font("Microsoft Sans Serif", 12F);
            btnSearch.Location = new Point(375, 15);
            btnSearch.MinimumSize = new Size(1, 1);
            btnSearch.Name = "btnSearch";
            btnSearch.Radius = 15;
            btnSearch.Size = new Size(120, 47);
            btnSearch.Symbol = 61442;
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(0, 0, 0, 0);
            txtSearch.Cursor = Cursors.IBeam;
            txtSearch.Font = new Font("Microsoft Sans Serif", 12F);
            txtSearch.Location = new Point(13, 15);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.MinimumSize = new Size(1, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.Padding = new Padding(5);
            txtSearch.Radius = 15;
            txtSearch.ShowText = false;
            txtSearch.Size = new Size(350, 47);
            txtSearch.Symbol = 61442;
            txtSearch.TabIndex = 0;
            txtSearch.TextAlignment = ContentAlignment.MiddleLeft;
            txtSearch.Watermark = "Nhập tên hoặc email...";
            // 
            // pnlRight
            // 
            pnlRight.BackColor = Color.White;
            pnlRight.Controls.Add(btnRefresh);
            pnlRight.Controls.Add(btnDelete);
            pnlRight.Controls.Add(btnSave);
            pnlRight.Controls.Add(swIsActive);
            pnlRight.Controls.Add(lblActive);
            pnlRight.Controls.Add(cboRole);
            pnlRight.Controls.Add(lblRole);
            pnlRight.Controls.Add(txtPassword);
            pnlRight.Controls.Add(lblPassword);
            pnlRight.Controls.Add(txtEmail);
            pnlRight.Controls.Add(lblEmail);
            pnlRight.Controls.Add(txtFullName);
            pnlRight.Controls.Add(lblFullName);
            pnlRight.Controls.Add(txtId);
            pnlRight.Controls.Add(lblId);
            pnlRight.Dock = DockStyle.Right;
            pnlRight.FillColor = Color.White;
            pnlRight.FillColor2 = Color.White;
            pnlRight.FillDisableColor = Color.White;
            pnlRight.Font = new Font("Microsoft Sans Serif", 12F);
            pnlRight.Location = new Point(1230, 0);
            pnlRight.Margin = new Padding(4, 5, 4, 5);
            pnlRight.MinimumSize = new Size(1, 1);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(10, 35, 10, 10);
            pnlRight.ShowText = false;
            pnlRight.Size = new Size(500, 1020);
            pnlRight.TabIndex = 1;
            pnlRight.Text = "THÔNG TIN CHI TIẾT";
            pnlRight.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            btnRefresh.FillColor = Color.Gray;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnRefresh.Location = new Point(280, 567);
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
            btnDelete.FillColor = Color.Red;
            btnDelete.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnDelete.Location = new Point(170, 567);
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
            btnSave.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnSave.Location = new Point(30, 567);
            btnSave.MinimumSize = new Size(1, 1);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 40);
            btnSave.Symbol = 61639;
            btnSave.TabIndex = 2;
            btnSave.Text = "Lưu";
            btnSave.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // swIsActive
            // 
            swIsActive.Active = true;
            swIsActive.ActiveText = "Bật";
            swIsActive.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            swIsActive.InActiveText = "Tắt";
            swIsActive.Location = new Point(200, 505);
            swIsActive.MinimumSize = new Size(1, 1);
            swIsActive.Name = "swIsActive";
            swIsActive.Size = new Size(75, 29);
            swIsActive.TabIndex = 3;
            // 
            // lblActive
            // 
            lblActive.AutoSize = true;
            lblActive.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblActive.ForeColor = Color.FromArgb(48, 48, 48);
            lblActive.Location = new Point(30, 507);
            lblActive.Name = "lblActive";
            lblActive.Size = new Size(106, 25);
            lblActive.TabIndex = 4;
            lblActive.Text = "Trạng thái:";
            // 
            // cboRole
            // 
            cboRole.DataSource = null;
            cboRole.FillColor = Color.White;
            cboRole.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            cboRole.ItemHoverColor = Color.FromArgb(155, 200, 255);
            cboRole.Items.AddRange(new object[] { "Student", "Teacher", "Admin" });
            cboRole.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
            cboRole.Location = new Point(30, 439);
            cboRole.Margin = new Padding(4, 5, 4, 5);
            cboRole.MinimumSize = new Size(63, 0);
            cboRole.Name = "cboRole";
            cboRole.Padding = new Padding(0, 0, 30, 2);
            cboRole.Size = new Size(440, 45);
            cboRole.SymbolSize = 24;
            cboRole.TabIndex = 5;
            cboRole.TextAlignment = ContentAlignment.MiddleLeft;
            cboRole.Watermark = "";
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblRole.ForeColor = Color.FromArgb(48, 48, 48);
            lblRole.Location = new Point(30, 414);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(74, 25);
            lblRole.TabIndex = 6;
            lblRole.Text = "Vai trò:";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtPassword.Location = new Point(30, 354);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.MinimumSize = new Size(1, 16);
            txtPassword.Name = "txtPassword";
            txtPassword.Padding = new Padding(5);
            txtPassword.PasswordChar = '*';
            txtPassword.ShowText = false;
            txtPassword.Size = new Size(440, 40);
            txtPassword.TabIndex = 7;
            txtPassword.TextAlignment = ContentAlignment.MiddleLeft;
            txtPassword.Watermark = "";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblPassword.ForeColor = Color.FromArgb(48, 48, 48);
            lblPassword.Location = new Point(30, 329);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(99, 25);
            lblPassword.TabIndex = 8;
            lblPassword.Text = "Mật khẩu:";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtEmail.Location = new Point(30, 265);
            txtEmail.Margin = new Padding(4, 5, 4, 5);
            txtEmail.MinimumSize = new Size(1, 16);
            txtEmail.Name = "txtEmail";
            txtEmail.Padding = new Padding(5);
            txtEmail.ShowText = false;
            txtEmail.Size = new Size(440, 40);
            txtEmail.TabIndex = 9;
            txtEmail.TextAlignment = ContentAlignment.MiddleLeft;
            txtEmail.Watermark = "";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblEmail.ForeColor = Color.FromArgb(48, 48, 48);
            lblEmail.Location = new Point(30, 240);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(66, 25);
            lblEmail.TabIndex = 10;
            lblEmail.Text = "Email:";
            // 
            // txtFullName
            // 
            txtFullName.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtFullName.Location = new Point(30, 178);
            txtFullName.Margin = new Padding(4, 5, 4, 5);
            txtFullName.MinimumSize = new Size(1, 16);
            txtFullName.Name = "txtFullName";
            txtFullName.Padding = new Padding(5);
            txtFullName.ShowText = false;
            txtFullName.Size = new Size(440, 40);
            txtFullName.TabIndex = 11;
            txtFullName.TextAlignment = ContentAlignment.MiddleLeft;
            txtFullName.Watermark = "";
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblFullName.ForeColor = Color.FromArgb(48, 48, 48);
            lblFullName.Location = new Point(30, 153);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(101, 25);
            lblFullName.TabIndex = 12;
            lblFullName.Text = "Họ và tên:";
            // 
            // txtId
            // 
            txtId.Enabled = false;
            txtId.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            txtId.Location = new Point(30, 90);
            txtId.Margin = new Padding(4, 5, 4, 5);
            txtId.MinimumSize = new Size(1, 16);
            txtId.Name = "txtId";
            txtId.Padding = new Padding(5);
            txtId.ShowText = false;
            txtId.Size = new Size(440, 43);
            txtId.TabIndex = 13;
            txtId.TextAlignment = ContentAlignment.MiddleLeft;
            txtId.Watermark = "";
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            lblId.ForeColor = Color.FromArgb(48, 48, 48);
            lblId.Location = new Point(30, 60);
            lblId.Name = "lblId";
            lblId.Size = new Size(37, 25);
            lblId.TabIndex = 14;
            lblId.Text = "ID:";
            // 
            // AccountsPanel
            // 
            Controls.Add(pnlLeft);
            Controls.Add(pnlRight);
            Name = "AccountsPanel";
            Size = new Size(1730, 1020);
            pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            pnlRight.PerformLayout();
            ResumeLayout(false);
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