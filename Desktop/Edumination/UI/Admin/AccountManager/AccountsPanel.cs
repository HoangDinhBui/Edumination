using IELTS.BLL;
using IELTS.DTO;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.AccountManager
{
    public partial class AccountsPanel : UserControl
    {
        private readonly UserBLL _bll;
        private long _currentUserId = 0; // 👉 QUAN TRỌNG: Sửa thành long

        public AccountsPanel()
        {
            InitializeComponent();
            _bll = new UserBLL();

            this.Load += AccountsPanel_Load;
            this.dgvAccounts.CellClick += DgvAccounts_CellClick;
            this.btnSave.Click += BtnSave_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnRefresh.Click += (s, e) => { ResetForm(); LoadData(); };
            this.btnSearch.Click += (s, e) => LoadData(txtSearch.Text.Trim());
            this.txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(txtSearch.Text.Trim()); };
        }

        private void AccountsPanel_Load(object sender, EventArgs e)
        {
            LoadData();
            cboRole.SelectedIndex = 0;
        }

        private void LoadData(string keyword = "")
        {
            try
            {
                var userList = _bll.GetAll(keyword);
                dgvAccounts.DataSource = null;
                dgvAccounts.DataSource = userList;

                string[] hideCols = { "PasswordHash", "CreatedAt", "UpdatedAt", "DateOfBirth", "Phone" };
                foreach (var col in hideCols)
                    if (dgvAccounts.Columns[col] != null)
                        dgvAccounts.Columns[col].Visible = false;

                dgvAccounts.Columns["Id"].HeaderText = "ID";
                dgvAccounts.Columns["FullName"].HeaderText = "Họ tên";
                dgvAccounts.Columns["Email"].HeaderText = "Email";
                dgvAccounts.Columns["Role"].HeaderText = "Vai trò";

                StyleAccountsGrid(); // ✅ QUAN TRỌNG
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError(ex.Message);
            }
        }


        private void DgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvAccounts.Rows[e.RowIndex];

            // 👉 Lấy ID kiểu long
            if (row.Cells["Id"].Value != null)
                _currentUserId = Convert.ToInt64(row.Cells["Id"].Value);

            // Binding lên form
            // Lưu ý: Nếu user click vào cột thì lấy giá trị trực tiếp từ Cell hoặc lấy từ List<UserDTO>
            // Ở đây lấy từ Cell cho đơn giản
            txtId.Text = _currentUserId.ToString();
            txtFullName.Text = row.Cells["FullName"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            cboRole.Text = row.Cells["Role"].Value?.ToString();

            // Check null an toàn cho IsActive
            var activeVal = row.Cells["IsActive"].Value;
            swIsActive.Active = activeVal != null && (bool)activeVal;

            // Xử lý giao diện
            txtPassword.Text = "";
            txtPassword.Watermark = "Nhập để đổi mật khẩu";
            btnSave.Text = "Cập nhật";
            btnSave.FillColor = System.Drawing.Color.Orange;
            btnDelete.Enabled = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                UIMessageTip.ShowWarning("Thiếu thông tin!");
                return;
            }

            UserDTO user = new UserDTO
            {
                Id = _currentUserId,
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Role = cboRole.Text,
                IsActive = swIsActive.Active,
                // Các trường Phone, DOB hiện chưa có trên UI, bạn có thể để null hoặc thêm TextBox
                Phone = null,
                DateOfBirth = null
            };

            string rawPassword = txtPassword.Text.Trim();
            string error = "";

            if (_currentUserId == 0) // THÊM
            {
                if (string.IsNullOrEmpty(rawPassword))
                {
                    UIMessageTip.ShowWarning("Cần nhập mật khẩu!");
                    return;
                }
                error = _bll.AddUser(user, rawPassword);
            }
            else // SỬA
            {
                error = _bll.UpdateUser(user, rawPassword);
            }

            if (string.IsNullOrEmpty(error))
            {
                UIMessageTip.ShowOk("Thành công!");
                ResetForm();
                LoadData();
            }
            else
            {
                UIMessageBox.ShowError(error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentUserId == 0) return;

            if (UIMessageBox.Show("Xóa tài khoản này?", "Xác nhận", UIStyle.Red, UIMessageBoxButtons.OKCancel, true))
            {
                if (_bll.DeleteUser(_currentUserId))
                {
                    UIMessageTip.ShowOk("Đã xóa!");
                    ResetForm();
                    LoadData();
                }
                else
                {
                    UIMessageBox.ShowError("Lỗi khi xóa!");
                }
            }
        }

        private void ResetForm()
        {
            _currentUserId = 0;
            txtId.Text = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtPassword.Watermark = "Mật khẩu";
            cboRole.SelectedIndex = 0;
            swIsActive.Active = true;
            btnSave.Text = "Thêm mới";
            btnSave.FillColor = System.Drawing.Color.FromArgb(110, 190, 40);
            btnDelete.Enabled = false;
        }

        private int _hoverRow = -1;

        private void StyleAccountsGrid()
        {
            dgvAccounts.BorderStyle = BorderStyle.None;
            dgvAccounts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAccounts.GridColor = Color.FromArgb(235, 238, 245);

            dgvAccounts.BackgroundColor = Color.White;
            dgvAccounts.RowHeadersVisible = false;
            dgvAccounts.AllowUserToResizeRows = false;
            dgvAccounts.MultiSelect = false;

            dgvAccounts.RowTemplate.Height = 46;

            // ===== HEADER (GIỐNG COURSE PANEL) =====
            dgvAccounts.ColumnHeadersHeight = 48;
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10.5f, FontStyle.Bold);
            dgvAccounts.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            dgvAccounts.EnableHeadersVisualStyles = false;

            // ===== CELL =====
            dgvAccounts.DefaultCellStyle.Font =
                new Font("Segoe UI", 10f);
            dgvAccounts.DefaultCellStyle.ForeColor =
                Color.FromArgb(55, 65, 81);
            dgvAccounts.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(235, 240, 255);
            dgvAccounts.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(30, 64, 175);

            dgvAccounts.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(249, 250, 251);

            // ===== HOVER ROW (MƯỢT – KHÔNG DÍNH MÀU) =====
            dgvAccounts.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    _hoverRow = e.RowIndex;
                    dgvAccounts.InvalidateRow(e.RowIndex);
                }
            };

            dgvAccounts.CellMouseLeave += (s, e) =>
            {
                if (_hoverRow >= 0)
                {
                    dgvAccounts.Rows[_hoverRow].DefaultCellStyle.BackColor =
                        (_hoverRow % 2 == 0)
                            ? Color.White
                            : Color.FromArgb(249, 250, 251);

                    _hoverRow = -1;
                }
            };

            dgvAccounts.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex == _hoverRow)
                {
                    dgvAccounts.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        Color.FromArgb(243, 244, 246);
                }
            };
        }

    }
}
