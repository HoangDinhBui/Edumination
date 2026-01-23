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

        private readonly CourseBLL courseBLL = new CourseBLL();
        private readonly CourseStudentBLL courseStudentBLL = new CourseStudentBLL();

        public CourseStudentViewerPanel()
        {
            InitializeComponent();
            LoadCourses();
            ClearStudentDetail();
        }

        // ================= LOAD DATA =================

        private void LoadCourses()
        {
            isViewingCourses = true;
            btnBack.Visible = false;
            lblTitle.Text = "Danh sách khóa học";

            dgvMain.DataSource = null;
            dgvMain.DataSource = courseBLL.GetAll(txtSearch.Text);

            ConfigureCourseGrid();
        }

        private void LoadStudents()
        {
            if (selectedCourse == null) return;

            isViewingCourses = false;
            btnBack.Visible = true;
            lblTitle.Text = $"Danh sách học viên - {selectedCourse.Title}";

            dgvMain.DataSource = null;
            dgvMain.DataSource =
                courseStudentBLL.GetEnrolledUsersByCourse(selectedCourse.Id, txtSearch.Text);

            ConfigureStudentGrid();
        }


        // ================= EVENTS =================

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (isViewingCourses)
                LoadCourses();
            else
                LoadStudents();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            selectedCourse = null;
            LoadCourses();
        }

        private void dgvMain_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // 1️⃣ Double click khóa học
            if (isViewingCourses)
            {
                selectedCourse =
                    dgvMain.Rows[e.RowIndex].DataBoundItem as CourseDTO;

                LoadStudents();
                return;
            }

            // 2️⃣ Double click học viên → HIỂN PANEL PHẢI
            long studentId =
                Convert.ToInt64(dgvMain.Rows[e.RowIndex].Cells["UserId"].Value);

            string fullName =
                dgvMain.Rows[e.RowIndex].Cells["FullName"].Value.ToString();

            string email =
                dgvMain.Rows[e.RowIndex].Cells["Email"].Value.ToString();

            lblStudentName.Text = $"👤 Họ tên: {fullName}";
            lblStudentEmail.Text = $"📧 Email: {email}";

            var dt = courseStudentBLL.GetCourseStudentDetail(
                selectedCourse.Id,
                studentId
            );

            if (dt.Rows.Count == 0)
            {
                lblStudentStatus.Text = "📌 Trạng thái: Chưa bắt đầu học";
                lblStudentStatus.ForeColor = Color.DarkOrange;
                progressStudy.Value = 0;
            }
            else
            {
                int percent = Convert.ToInt32(dt.Rows[0]["CompletionPercentage"]);

                lblStudentStatus.Text = "📘 Trạng thái: Đang học";
                lblStudentStatus.ForeColor = Color.Green;
                progressStudy.Value = Math.Min(percent, 100);
            }
        }

        // ================= DETAIL PANEL =================

        private void ClearStudentDetail()
        {
            lblStudentName.Text = "Chọn học viên để xem chi tiết";
            lblStudentEmail.Text = "";
            lblStudentStatus.Text = "";
            progressStudy.Value = 0;
        }

        // ================= GRID CONFIG =================

        private void ConfigureCourseGrid()
        {
            dgvMain.Columns["Id"].HeaderText = "Mã";
            dgvMain.Columns["Title"].HeaderText = "Tên khóa học";
            dgvMain.Columns["Level"].HeaderText = "Trình độ";
            dgvMain.Columns["PriceVND"].HeaderText = "Giá";
            dgvMain.Columns["CreatedByName"].HeaderText = "Người tạo";

            dgvMain.Columns["Description"].Visible = false;
            dgvMain.Columns["IsPublished"].Visible = false;
            dgvMain.Columns["CreatedBy"].Visible = false;
            dgvMain.Columns["CreatedAt"].Visible = false;
        }

        private void ConfigureStudentGrid()
        {
            dgvMain.Columns["UserId"].HeaderText = "Mã HV";
            dgvMain.Columns["FullName"].HeaderText = "Họ tên";
            dgvMain.Columns["Email"].HeaderText = "Email";
            dgvMain.Columns["EnrolledAt"].HeaderText = "Ngày đăng ký";
        }
    }
}
