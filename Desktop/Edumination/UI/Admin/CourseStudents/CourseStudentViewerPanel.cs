using IELTS.BLL;
using IELTS.DTO;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.Admin.CourseStudents
{
    public partial class CourseStudentViewerPanel : UserControl
    {
        private bool isViewingCourses = true;
        private CourseDTO selectedCourse;
        private int _hoverRow = -1;

        private readonly CourseBLL courseBLL = new CourseBLL();
        private readonly CourseStudentBLL courseStudentBLL = new CourseStudentBLL();

        public CourseStudentViewerPanel()
        {
            InitializeComponent();
            LoadCourses();
            ClearStudentDetail();
            StyleMainGrid();
        }

        // ================= LOAD =================

        private void LoadCourses()
        {
            isViewingCourses = true;
            btnBack.Visible = false;
            lblTitle.Text = "Danh sách khóa học";

            dgvMain.DataSource = courseBLL.GetAll(txtSearch.Text);
            ConfigureCourseGrid();
            StyleMainGrid();
        }

        private void LoadStudents()
        {
            if (selectedCourse == null) return;

            isViewingCourses = false;
            btnBack.Visible = true;
            lblTitle.Text = $"Danh sách học viên - {selectedCourse.Title}";

            dgvMain.DataSource =
                courseStudentBLL.GetEnrolledUsersByCourse(selectedCourse.Id, txtSearch.Text);

            ConfigureStudentGrid();
            StyleMainGrid();
        }

        // ================= EVENTS =================

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (isViewingCourses) LoadCourses();
            else LoadStudents();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            selectedCourse = null;
            LoadCourses();
            ClearStudentDetail();
        }

        private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (isViewingCourses)
            {
                selectedCourse = dgvMain.Rows[e.RowIndex].DataBoundItem as CourseDTO;
                LoadStudents();
                return;
            }

            long studentId =
                Convert.ToInt64(dgvMain.Rows[e.RowIndex].Cells["UserId"].Value);

            string fullName =
                dgvMain.Rows[e.RowIndex].Cells["FullName"].Value.ToString();

            string email =
                dgvMain.Rows[e.RowIndex].Cells["Email"].Value.ToString();

            lblStudentName.Text = $"👤 {fullName}";
            lblStudentEmail.Text = $"📧 {email}";

            var dt = courseStudentBLL.GetCourseStudentDetail(
                selectedCourse.Id, studentId);

            if (dt.Rows.Count == 0)
            {
                lblStudentStatus.Text = "📌 Chưa bắt đầu học";
                lblStudentStatus.ForeColor = Color.DarkOrange;
                progressStudy.Value = 0;
            }
            else
            {
                int percent = Convert.ToInt32(dt.Rows[0]["CompletionPercentage"]);
                lblStudentStatus.Text = "📘 Đang học";
                lblStudentStatus.ForeColor = Color.SeaGreen;
                progressStudy.Value = Math.Min(percent, 100);
            }
        }

        // ================= GRID STYLE =================

        private void StyleMainGrid()
        {
            dgvMain.BorderStyle = BorderStyle.None;
            dgvMain.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMain.GridColor = Color.FromArgb(235, 238, 245);

            dgvMain.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvMain.BackgroundColor = Color.White;
            dgvMain.RowHeadersVisible = false;
            dgvMain.AllowUserToResizeRows = false;
            dgvMain.MultiSelect = false;

            dgvMain.RowTemplate.Height = 46;

            // ===== HEADER (GIỐNG CoursePanel) =====
            dgvMain.ColumnHeadersHeight = 50;
            dgvMain.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(249, 250, 251); // xám rất nhạt
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.FromArgb(55, 65, 81);
            dgvMain.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10.5f, FontStyle.Bold);
            dgvMain.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;

            dgvMain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;


            // ===== CELL =====
            dgvMain.DefaultCellStyle.Font =
                new Font("Segoe UI", 10f);
            dgvMain.DefaultCellStyle.ForeColor =
                Color.FromArgb(55, 65, 81);
            dgvMain.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(235, 240, 255);
            dgvMain.DefaultCellStyle.SelectionForeColor =
                Color.FromArgb(30, 64, 175);

            dgvMain.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(249, 250, 251);

            // ===== HOVER ROW (FIX BUG DÍNH MÀU) =====
            dgvMain.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    _hoverRow = e.RowIndex;
                    dgvMain.InvalidateRow(e.RowIndex);
                }
            };

			dgvMain.CellMouseLeave += (s, e) =>
			{
				if (_hoverRow >= 0 && _hoverRow < dgvMain.Rows.Count)
				{
					dgvMain.Rows[_hoverRow].DefaultCellStyle.BackColor =
						(_hoverRow % 2 == 0)
							? Color.White
							: Color.FromArgb(249, 250, 251);
				}

				_hoverRow = -1;
			};

			dgvMain.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex == _hoverRow)
                {
                    dgvMain.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                        Color.FromArgb(243, 244, 246);
                }
            };

            dgvMain.CellFormatting += (s, e) =>
            {
                var col = dgvMain.Columns[e.ColumnIndex].Name;

                if (col == "Level" && e.Value != null)
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

                if (col == "Email" && e.Value != null)
                {
                    e.CellStyle.ForeColor = Color.FromArgb(59, 130, 246);
                }
            };

        }


        // ================= CONFIG =================

        private void ConfigureCourseGrid()
        {
            dgvMain.Columns["Id"].HeaderText = "ID";
            dgvMain.Columns["Title"].HeaderText = "Khóa học";
            dgvMain.Columns["Level"].HeaderText = "Trình độ";
            dgvMain.Columns["PriceVND"].HeaderText = "Giá";
            dgvMain.Columns["CreatedByName"].HeaderText = "Người tạo";

            dgvMain.Columns["Description"].Visible = false;
            dgvMain.Columns["IsPublished"].Visible = false;
            dgvMain.Columns["CreatedBy"].Visible = false;
            dgvMain.Columns["CreatedAt"].Visible = false;



            dgvMain.Columns["PriceVND"].DefaultCellStyle.Alignment =
    DataGridViewContentAlignment.MiddleRight;
            dgvMain.Columns["PriceVND"].DefaultCellStyle.Format = "N0";

        }

        private void ConfigureStudentGrid()
        {
            dgvMain.Columns["UserId"].HeaderText = "ID";
            dgvMain.Columns["FullName"].HeaderText = "Họ tên";
            dgvMain.Columns["Email"].HeaderText = "Email";
            dgvMain.Columns["EnrolledAt"].HeaderText = "Ngày đăng ký";



            dgvMain.Columns["EnrolledAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvMain.Columns["EnrolledAt"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

        }

        private void ClearStudentDetail()
        {
            lblStudentName.Text = "Chọn học viên";
            lblStudentEmail.Text = "";
            lblStudentStatus.Text = "";
            progressStudy.Value = 0;
        }
    }
}
