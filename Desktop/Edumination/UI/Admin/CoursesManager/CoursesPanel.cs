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

namespace IELTS.UI.Admin.CoursesManager
{
    public partial class CoursesPanel : UserControl
    {
        private readonly CourseBLL _bll;
        private long _currentId = 0;
        public CoursesPanel()
        {
            InitializeComponent();
            _bll = new CourseBLL();

            this.Load += (s, e) => { LoadData(); cboLevel.SelectedIndex = 0; };
            this.dgvCourses.CellClick += DgvCourses_CellClick;
            this.btnSave.Click += BtnSave_Click;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnRefresh.Click += (s, e) => { ResetForm(); LoadData(); };
            this.btnSearch.Click += (s, e) => LoadData(txtSearch.Text.Trim());
            this.txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(txtSearch.Text.Trim()); };
        }
        private void LoadData(string keyword = "")
        {
            try
            {
                var list = _bll.GetAll(keyword);
                dgvCourses.DataSource = null;
                dgvCourses.DataSource = list;

                // Ẩn cột thừa
                string[] hideCols = { "Description", "Lessons", "CreatedAt", "CreatedBy" };
                foreach (var col in hideCols)
                    if (dgvCourses.Columns[col] != null) dgvCourses.Columns[col].Visible = false;

                // Đổi tên cột
                if (dgvCourses.Columns["Id"] != null) dgvCourses.Columns["Id"].HeaderText = "ID";
                if (dgvCourses.Columns["Title"] != null) dgvCourses.Columns["Title"].HeaderText = "Tên khóa học";
                if (dgvCourses.Columns["PriceVND"] != null) dgvCourses.Columns["PriceVND"].HeaderText = "Giá (VNĐ)";
                if (dgvCourses.Columns["IsPublished"] != null) dgvCourses.Columns["IsPublished"].HeaderText = "Công khai";
                if (dgvCourses.Columns["Level"] != null) dgvCourses.Columns["Level"].HeaderText = "Trình độ";
                if (dgvCourses.Columns["CreatedByName"] != null) dgvCourses.Columns["CreatedByName"].HeaderText = "Người tạo";
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError("Lỗi: " + ex.Message);
            }
        }

        private void DgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvCourses.Rows[e.RowIndex];

            if (row.Cells["Id"].Value != null)
                _currentId = Convert.ToInt64(row.Cells["Id"].Value);

            // Binding dữ liệu lên Form
            txtId.Text = _currentId.ToString();
            txtTitle.Text = row.Cells["Title"].Value?.ToString();
            txtDesc.Text = row.Cells["Description"].Value?.ToString();
            txtPrice.Text = row.Cells["PriceVND"].Value?.ToString();
            cboLevel.Text = row.Cells["Level"].Value?.ToString();

            var pubVal = row.Cells["IsPublished"].Value;
            swPublished.Active = pubVal != null && (bool)pubVal;

            // Cập nhật trạng thái nút
            btnSave.Text = "Cập nhật";
            btnSave.FillColor = System.Drawing.Color.Orange;
            btnDelete.Enabled = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                UIMessageTip.ShowWarning("Vui lòng nhập tên khóa học!");
                return;
            }

            int price = 0;
            int.TryParse(txtPrice.Text, out price);

            var course = new CourseDTO
            {
                Id = _currentId,
                Title = txtTitle.Text.Trim(),
                Description = txtDesc.Text.Trim(),
                Level = cboLevel.Text,
                PriceVND = price,
                IsPublished = swPublished.Active,
                // CreatedBy sẽ được xử lý ở BLL hoặc lấy từ Session nếu có
            };

            string error = "";
            if (_currentId == 0) // Thêm
                error = _bll.AddCourse(course);
            else // Sửa
                error = _bll.UpdateCourse(course);

            if (string.IsNullOrEmpty(error))
            {
                UIMessageTip.ShowOk("Thao tác thành công!");
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
            if (_currentId == 0) return;

            // Hỏi xác nhận trước khi xóa
            if (UIMessageBox.Show("Bạn chắc chắn muốn xóa khóa học này?", "Xác nhận", UIStyle.Red, UIMessageBoxButtons.OKCancel, true))
            {
                try
                {
                    // Gọi lệnh xóa (Nếu lỗi SQL xảy ra, nó sẽ nhảy xuống catch ngay lập tức)
                    if (_bll.DeleteCourse(_currentId))
                    {
                        UIMessageTip.ShowOk("Đã xóa thành công!");
                        ResetForm();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    // 👉 XUẤT RA MÀN HÌNH LỖI VỪA BẮT ĐƯỢC
                    UIMessageBox.ShowError(ex.Message);
                }
            }
        }

        private void ResetForm()
        {
            _currentId = 0;
            txtId.Text = "";
            txtTitle.Text = "";
            txtDesc.Text = "";
            txtPrice.Text = "0";
            cboLevel.SelectedIndex = 0;
            swPublished.Active = true;

            btnSave.Text = "Thêm mới";
            btnSave.FillColor = System.Drawing.Color.FromArgb(110, 190, 40);
            btnDelete.Enabled = false;
        }

    }
}
