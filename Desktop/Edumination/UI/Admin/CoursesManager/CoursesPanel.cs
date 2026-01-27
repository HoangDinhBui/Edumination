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

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentId == 0) return;

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
