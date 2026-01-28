﻿using IELTS.BLL;
using IELTS.DTO;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.Admin.CoursesManager
{
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

			// NGHIỆP VỤ: Tự động load danh sách bài học thuộc khóa học này sang Tab 2
			LoadLessonsByCourse(_currentCourseId);
		}

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