using IELTS.BLL;
using IELTS.DTO;
using Sunny.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.Admin.CoursesManager
{
    public partial class CoursesPanel : UserControl
    {
        private readonly CourseBLL _courseBll;
        private readonly LessonBLL _lessonBll;
        private long _currentCourseId = 0;

        public CoursesPanel()
        {
            InitializeComponent();
            SetupGrid(dgvCourses);
            SetupGrid(dgvLessons);
            BuildCourseInfoUI(); // ⭐ dựng layout động (designer-safe)

            _courseBll = new CourseBLL();
            _lessonBll = new LessonBLL();

            // Load ban đầu
            this.Load += (s, e) =>
            {
                LoadCourses();
                cboLevel.SelectedIndex = 0;
            };

            // Search
            btnSearch.Click += (s, e) => LoadCourses(txtSearch.Text.Trim());
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    LoadCourses(txtSearch.Text.Trim());
            };

            // Grid khóa học
            dgvCourses.CellClick += DgvCourses_CellClick;

            // CRUD khóa học
            btnSave.Click += BtnSaveCourse_Click;
            btnDelete.Click += BtnDeleteCourse_Click;
            btnRefresh.Click += (s, e) =>
            {
                ResetCourseForm();
                LoadCourses();
            };

            // Bài học
            btnAddLesson.Click += BtnAddLesson_Click;
            dgvLessons.CellDoubleClick += DgvLessons_CellDoubleClick;
        }

        private void SetupGrid(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dgv.RowTemplate.Height = 42;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;

            dgv.ColumnHeadersHeight = 42;
            dgv.EnableHeadersVisualStyles = false;

            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.FromArgb(230, 230, 230);
        }


        #region ===== UI BUILDER (TAB 1) =====

        private void BuildCourseInfoUI()
        {
            int y = 20;
            int labelW = 120;
            int inputW = 340;

            AddField(lblId, txtId, "ID");
            AddField(lblTitle, txtTitle, "Tên khóa học");
            AddField(lblDesc, txtDesc, "Mô tả");
            AddField(lblLevel, cboLevel, "Trình độ");
            AddField(lblPrice, txtPrice, "Giá (VNĐ)");

            lblPublished.Text = "Công khai";
            lblPublished.Location = new Point(20, y);
            lblPublished.Size = new Size(labelW, 24);

            swPublished.Location = new Point(200, y);
            y += 60;

            btnSave.Text = "Thêm khóa học";
            btnDelete.Text = "Xóa";
            btnRefresh.Text = "Làm mới";

            btnSave.FillColor = Color.FromArgb(110, 190, 40);
            btnDelete.FillColor = Color.IndianRed;

            btnSave.Location = new Point(20, y);
            btnDelete.Location = new Point(160, y);
            btnRefresh.Location = new Point(300, y);

            btnDelete.Enabled = false;

            tpCourseInfo.Controls.AddRange(new Control[]
            {
                lblPublished, swPublished,
                btnSave, btnDelete, btnRefresh
            });

            void AddField(UILabel lbl, Control input, string text)
            {
                lbl.Text = text;
                lbl.Location = new Point(20, y);
                lbl.Size = new Size(labelW, 24);

                input.Location = new Point(20, y + 26);
                input.Size = new Size(inputW, 34);

                tpCourseInfo.Controls.Add(lbl);
                tpCourseInfo.Controls.Add(input);

                y += 70;
            }
        }

        #endregion

        #region ===== KHÓA HỌC =====

        private void LoadCourses(string keyword = "")
        {
            try
            {
                var list = _courseBll.GetAll(keyword);
                dgvCourses.DataSource = null;
                dgvCourses.DataSource = list;

                string[] hideCols = { "Description", "Lessons", "CreatedAt", "CreatedBy" };
                foreach (var col in hideCols)
                    if (dgvCourses.Columns[col] != null)
                        dgvCourses.Columns[col].Visible = false;

                if (dgvCourses.Columns["Id"] != null)
                    dgvCourses.Columns["Id"].HeaderText = "ID";

                if (dgvCourses.Columns["Title"] != null)
                    dgvCourses.Columns["Title"].HeaderText = "Tên khóa học";

                if (dgvCourses.Columns["PriceVND"] != null)
                    dgvCourses.Columns["PriceVND"].HeaderText = "Giá (VNĐ)";

                if (dgvCourses.Columns["IsPublished"] != null)
                    dgvCourses.Columns["IsPublished"].HeaderText = "Công khai";
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError("Lỗi tải khóa học: " + ex.Message);
            }
        }

        private void DgvCourses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvCourses.Rows[e.RowIndex];
            _currentCourseId = Convert.ToInt64(row.Cells["Id"].Value);

            txtId.Text = _currentCourseId.ToString();
            txtTitle.Text = row.Cells["Title"].Value?.ToString();
            txtDesc.Text = row.Cells["Description"].Value?.ToString();
            txtPrice.Text = row.Cells["PriceVND"].Value?.ToString();
            cboLevel.Text = row.Cells["Level"].Value?.ToString();
            swPublished.Active = (bool)(row.Cells["IsPublished"].Value ?? false);

            btnSave.Text = "Cập nhật khóa học";
            btnSave.FillColor = Color.Orange;
            btnDelete.Enabled = true;

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

            string error = _currentCourseId == 0
                ? _courseBll.AddCourse(course)
                : _courseBll.UpdateCourse(course);

            if (string.IsNullOrEmpty(error))
            {
                UIMessageTip.ShowOk("Thành công!");
                ResetCourseForm();
                LoadCourses();
            }
            else
            {
                UIMessageBox.ShowError(error);
            }
        }

        private void BtnDeleteCourse_Click(object sender, EventArgs e)
        {
            if (_currentCourseId == 0) return;

            if (UIMessageBox.Show(
                "Xóa khóa học sẽ xóa toàn bộ bài học. Bạn chắc chắn?",
                "Xác nhận",
                UIStyle.Red,
                UIMessageBoxButtons.OKCancel,
                true))
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
            txtId.Text = "";
            txtTitle.Text = "";
            txtDesc.Text = "";
            txtPrice.Text = "0";
            cboLevel.SelectedIndex = 0;
            swPublished.Active = true;

            dgvLessons.DataSource = null;

            btnSave.Text = "Thêm khóa học";
            btnSave.FillColor = Color.FromArgb(110, 190, 40);
            btnDelete.Enabled = false;
        }

        #endregion

        #region ===== BÀI HỌC =====

        private void LoadLessonsByCourse(long courseId)
        {
            try
            {
                var lessons = _lessonBll.GetByCourseId(courseId);
                dgvLessons.DataSource = null;
                dgvLessons.DataSource = lessons;

                if (dgvLessons.Columns["Id"] != null)
                    dgvLessons.Columns["Id"].Visible = false;

                if (dgvLessons.Columns["CourseId"] != null)
                    dgvLessons.Columns["CourseId"].Visible = false;

                if (dgvLessons.Columns["Position"] != null)
                    dgvLessons.Columns["Position"].HeaderText = "Thứ tự";

                if (dgvLessons.Columns["Title"] != null)
                    dgvLessons.Columns["Title"].HeaderText = "Tên buổi học";
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
                UIMessageTip.ShowWarning("Vui lòng chọn khóa học!");
                return;
            }

            using (var frm = new frmLessonEditor(_currentCourseId))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadLessonsByCourse(_currentCourseId);
            }
        }

        private void DgvLessons_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            long lessonId = Convert.ToInt64(
                dgvLessons.Rows[e.RowIndex].Cells["Id"].Value);

            using (var frm = new frmLessonEditor(lessonId, true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    LoadLessonsByCourse(_currentCourseId);
            }
        }

        #endregion
    }
}
