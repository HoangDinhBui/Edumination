using IELTS.BLL;
using IELTS.DTO;
using Sunny.UI;
using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> feat/Course
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.Admin.CoursesManager
{
<<<<<<< HEAD
    public partial class CoursesPanel : UserControl
    {
        private readonly CourseBLL _bll;
        private long _currentId = 0;
        private int _hoverRow = -1;

        public CoursesPanel()
        {
            InitializeComponent();
            _bll = new CourseBLL();

            Load += (s, e) =>
            {
                LoadData();
                cboLevel.SelectedIndex = 0;
            };

            dgvCourses.CellClick += DgvCourses_CellClick;
            btnSave.Click += BtnSave_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRefresh.Click += (s, e) => { ResetForm(); LoadData(); };
            btnSearch.Click += (s, e) => LoadData(txtSearch.Text.Trim());
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    LoadData(txtSearch.Text.Trim());
            };

            InitGridEffects();
        }

        private void LoadData(string keyword = "")
        {
            try
            {
                var list = _bll.GetAll(keyword);
                dgvCourses.DataSource = null;
                dgvCourses.DataSource = list;

                string[] hideCols = { "Description", "Lessons", "CreatedAt", "CreatedBy" };
                foreach (var col in hideCols)
                    if (dgvCourses.Columns[col] != null)
                        dgvCourses.Columns[col].Visible = false;

                dgvCourses.Columns["Id"].HeaderText = "ID";
                dgvCourses.Columns["Title"].HeaderText = "Course";
                dgvCourses.Columns["PriceVND"].HeaderText = "Price (VND)";
                dgvCourses.Columns["IsPublished"].HeaderText = "Published";
                dgvCourses.Columns["Level"].HeaderText = "Level";
                dgvCourses.Columns["CreatedByName"].HeaderText = "Created by";

                StyleGrid();
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError(ex.Message);
            }
        }

        private void StyleGrid()
        {
            dgvCourses.BorderStyle = BorderStyle.None;
            dgvCourses.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCourses.GridColor = Color.FromArgb(235, 238, 245);

            dgvCourses.RowTemplate.Height = 46;
            dgvCourses.AllowUserToResizeRows = false;

            dgvCourses.ColumnHeadersHeight = 48;
            dgvCourses.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvCourses.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 81);
            dgvCourses.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10.5f, FontStyle.Bold);
            dgvCourses.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            dgvCourses.EnableHeadersVisualStyles = false;

            dgvCourses.DefaultCellStyle.Font =
                new Font("Segoe UI", 10f);
            dgvCourses.DefaultCellStyle.ForeColor =
                Color.FromArgb(55, 65, 81);
            dgvCourses.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(235, 240, 255);
            dgvCourses.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(30, 64, 175);

            dgvCourses.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(249, 250, 251);

            dgvCourses.RowHeadersVisible = false;

            dgvCourses.Columns["PriceVND"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;
            dgvCourses.Columns["PriceVND"].DefaultCellStyle.Format = "N0";

            dgvCourses.Columns["IsPublished"].Width = 90;
            dgvCourses.Columns["IsPublished"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }

        private void InitGridEffects()
        {
            dgvCourses.CellFormatting += (s, e) =>
            {
                if (dgvCourses.Columns[e.ColumnIndex].Name == "Level" && e.Value != null)
                {
                    e.CellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                    e.CellStyle.ForeColor = e.Value.ToString() switch
                    {
                        "BEGINNER" => Color.FromArgb(16, 185, 129),
                        "INTERMEDIATE" => Color.FromArgb(234, 179, 8),
                        "ADVANCED" => Color.FromArgb(239, 68, 68),
                        _ => e.CellStyle.ForeColor
                    };
                }
            };

            dgvCourses.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    _hoverRow = e.RowIndex;
                    dgvCourses.InvalidateRow(e.RowIndex);
                }
            };

            dgvCourses.CellMouseLeave += (s, e) =>
            {
                if (_hoverRow >= 0)
                {
                    dgvCourses.InvalidateRow(_hoverRow);
                    _hoverRow = -1;
                }
            };

            dgvCourses.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex == _hoverRow)
                {
                    dgvCourses.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        Color.FromArgb(243, 244, 246);
                }
            };
        }

        private void DgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvCourses.Rows[e.RowIndex];

            _currentId = Convert.ToInt64(row.Cells["Id"].Value);
            txtId.Text = _currentId.ToString();
            txtTitle.Text = row.Cells["Title"].Value?.ToString();
            txtDesc.Text = row.Cells["Description"].Value?.ToString();
            txtPrice.Text = row.Cells["PriceVND"].Value?.ToString();
            cboLevel.Text = row.Cells["Level"].Value?.ToString();
            swPublished.Active = (bool)row.Cells["IsPublished"].Value;

            btnSave.Text = "Cập nhật";
            btnSave.FillColor = Color.Orange;
            btnDelete.Enabled = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                UIMessageTip.ShowWarning("Nhập tên khóa học!");
                return;
            }

            var course = new CourseDTO
            {
                Id = _currentId,
                Title = txtTitle.Text.Trim(),
                Description = txtDesc.Text.Trim(),
                Level = cboLevel.Text,
                PriceVND = int.Parse(txtPrice.Text),
                IsPublished = swPublished.Active
            };

            string err = _currentId == 0
                ? _bll.AddCourse(course)
                : _bll.UpdateCourse(course);

            if (string.IsNullOrEmpty(err))
            {
                UIMessageTip.ShowOk("Thành công!");
                ResetForm();
                LoadData();
            }
            else UIMessageBox.ShowError(err);
        }
=======
	public partial class CoursesPanel : UserControl
	{
		private readonly CourseBLL _courseBll;
		private readonly LessonBLL _lessonBll; // BLL xử lý bảng Lessons
		private long _currentCourseId = 0;

		public CoursesPanel()
		{
			InitializeComponent();
			_courseBll = new CourseBLL();
			_lessonBll = new LessonBLL(); // Khởi tạo BLL bài học

			// Đăng ký các sự kiện hệ thống
			this.Load += (s, e) => { LoadCourses(); cboLevel.SelectedIndex = 0; };

			// Sự kiện Grid Khóa học (Bên trái)
			this.dgvCourses.CellClick += DgvCourses_CellClick;
			this.btnSearch.Click += (s, e) => LoadCourses(txtSearch.Text.Trim());
			this.txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadCourses(txtSearch.Text.Trim()); };

			// Sự kiện Form Khóa học (Tab 1)
			this.btnSave.Click += BtnSaveCourse_Click;
			this.btnDelete.Click += BtnDeleteCourse_Click;
			this.btnRefresh.Click += (s, e) => { ResetCourseForm(); LoadCourses(); };

			// Sự kiện Quản lý bài học (Tab 2)
			this.btnAddLesson.Click += BtnAddLesson_Click;
			this.dgvLessons.CellDoubleClick += DgvLessons_CellDoubleClick;
		}

		#region XỬ LÝ KHÓA HỌC (COURSE)

		private void LoadCourses(string keyword = "")
		{
			try
			{
				var list = _courseBll.GetAll(keyword);
				dgvCourses.DataSource = null;
				dgvCourses.DataSource = list;

				// Định dạng hiển thị Grid Khóa học
				string[] hideCols = { "Description", "Lessons", "CreatedAt", "CreatedBy" };
				foreach (var col in hideCols)
					if (dgvCourses.Columns[col] != null) dgvCourses.Columns[col].Visible = false;

				if (dgvCourses.Columns["Id"] != null) dgvCourses.Columns["Id"].HeaderText = "ID";
				if (dgvCourses.Columns["Title"] != null) dgvCourses.Columns["Title"].HeaderText = "Tên khóa học";
				if (dgvCourses.Columns["PriceVND"] != null) dgvCourses.Columns["PriceVND"].HeaderText = "Giá (VNĐ)";
				if (dgvCourses.Columns["IsPublished"] != null) dgvCourses.Columns["IsPublished"].HeaderText = "Công khai";
			}
			catch (Exception ex)
			{
				UIMessageBox.ShowError("Lỗi tải danh sách khóa học: " + ex.Message);
			}
		}

		private void DgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;
			var row = dgvCourses.Rows[e.RowIndex];

			if (row.Cells["Id"].Value != null)
				_currentCourseId = Convert.ToInt64(row.Cells["Id"].Value);

			// Binding dữ liệu lên Tab 1
			txtId.Text = _currentCourseId.ToString();
			txtTitle.Text = row.Cells["Title"].Value?.ToString();
			txtDesc.Text = row.Cells["Description"].Value?.ToString();
			txtPrice.Text = row.Cells["PriceVND"].Value?.ToString();
			cboLevel.Text = row.Cells["Level"].Value?.ToString();
			swPublished.Active = (bool)(row.Cells["IsPublished"].Value ?? false);

			// Cập nhật trạng thái UI
			btnSave.Text = "Cập nhật khóa học";
			btnSave.FillColor = Color.Orange;
			btnDelete.Enabled = true;
>>>>>>> feat/Course

			// NGHIỆP VỤ: Tự động load danh sách bài học thuộc khóa học này sang Tab 2
			LoadLessonsByCourse(_currentCourseId);
		}

<<<<<<< HEAD
            if (UIMessageBox.Show("Xóa khóa học?", "Confirm",
                UIStyle.Red, UIMessageBoxButtons.OKCancel))
            {
                _bll.DeleteCourse(_currentId);
                ResetForm();
                LoadData();
            }
        }

        private void ResetForm()
        {
            _currentId = 0;
            txtId.Clear();
            txtTitle.Clear();
            txtDesc.Clear();
            txtPrice.Text = "0";
            cboLevel.SelectedIndex = 0;
            swPublished.Active = true;

            btnSave.Text = "Thêm mới";
            btnSave.FillColor = Color.FromArgb(110, 190, 40);
            btnDelete.Enabled = false;
        }
    }
}
=======
		private void BtnSaveCourse_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTitle.Text))
			{
				UIMessageTip.ShowWarning("Vui lòng nhập tên khóa học!");
				return;
			}

			int.TryParse(txtPrice.Text, out int price);
			var course = new CourseDTO
			{
				Id = _currentCourseId,
				Title = txtTitle.Text.Trim(),
				Description = txtDesc.Text.Trim(),
				Level = cboLevel.Text,
				PriceVND = price,
				IsPublished = swPublished.Active
			};

			string error = _currentCourseId == 0 ? _courseBll.AddCourse(course) : _courseBll.UpdateCourse(course);

			if (string.IsNullOrEmpty(error))
			{
				UIMessageTip.ShowOk("Thành công!");
				ResetCourseForm();
				LoadCourses();
			}
			else UIMessageBox.ShowError(error);
		}

		private void BtnDeleteCourse_Click(object sender, EventArgs e)
		{
			if (_currentCourseId == 0) return;
			if (UIMessageBox.Show("Xóa khóa học sẽ xóa toàn bộ bài học bên trong. Bạn chắc chắn chứ?", "Xác nhận", UIStyle.Red, UIMessageBoxButtons.OKCancel, true))
			{
				if (_courseBll.DeleteCourse(_currentCourseId))
				{
					UIMessageTip.ShowOk("Đã xóa!");
					ResetCourseForm();
					LoadCourses();
				}
			}
		}

		private void ResetCourseForm()
		{
			_currentCourseId = 0;
			txtId.Text = ""; txtTitle.Text = ""; txtDesc.Text = ""; txtPrice.Text = "0";
			cboLevel.SelectedIndex = 0; swPublished.Active = true;
			dgvLessons.DataSource = null; // Xóa danh sách bài học khi reset
			btnSave.Text = "Thêm khóa học";
			btnSave.FillColor = Color.FromArgb(110, 190, 40);
			btnDelete.Enabled = false;
		}

		#endregion

		#region XỬ LÝ BÀI HỌC (LESSON)

		private void LoadLessonsByCourse(long courseId)
		{
			try
			{
				// Giả định BLL của bạn có hàm GetByCourseId
				var lessons = _lessonBll.GetByCourseId(courseId);
				dgvLessons.DataSource = null;
				dgvLessons.DataSource = lessons;

				// Định dạng hiển thị Grid Bài học
				if (dgvLessons.Columns["Id"] != null) dgvLessons.Columns["Id"].Visible = false;
				if (dgvLessons.Columns["CourseId"] != null) dgvLessons.Columns["CourseId"].Visible = false;
				if (dgvLessons.Columns["Position"] != null) dgvLessons.Columns["Position"].HeaderText = "Thứ tự";
				if (dgvLessons.Columns["Title"] != null) dgvLessons.Columns["Title"].HeaderText = "Tên buổi học";
				if (dgvLessons.Columns["VideoFilePath"] != null) dgvLessons.Columns["VideoFilePath"].HeaderText = "Video";
			}
			catch (Exception ex)
			{
				UIMessageTip.ShowError("Lỗi tải bài học: " + ex.Message);
			}
		}

		private void BtnAddLesson_Click(object sender, EventArgs e)
		{
			if (_currentCourseId == 0)
			{
				UIMessageTip.ShowWarning("Vui lòng chọn 1 khóa học trước khi thêm buổi học!");
				return;
			}

			// Mở form nhỏ để thêm buổi học mới
			using (var frm = new frmLessonEditor(_currentCourseId))
			{
				if (frm.ShowDialog() == DialogResult.OK)
				{
					LoadLessonsByCourse(_currentCourseId);
				}
			}
		}

		private void DgvLessons_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;
			long lessonId = Convert.ToInt64(dgvLessons.Rows[e.RowIndex].Cells["Id"].Value);

			// Mở form chỉnh sửa chi tiết (CRUD Video/Quiz)
			using (var frm = new frmLessonEditor(lessonId, isEdit: true))
			{
				if (frm.ShowDialog() == DialogResult.OK)
				{
					LoadLessonsByCourse(_currentCourseId);
				}
			}
		}

		#endregion
	}
}
>>>>>>> feat/Course
