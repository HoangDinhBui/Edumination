using IELTS.BLL;
using IELTS.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IELTS.UI.Admin.DashBoard
{
    public partial class AdminDashboardControl : UserControl
    {
        private DashboardBLL _dashboardBLL;
        private DashboardStatisticsDTO _statistics;

        public AdminDashboardControl()
        {
            InitializeComponent();
            _dashboardBLL = new DashboardBLL();
            LoadDashboardData();
        }

        public void LoadDashboardData()
        {
            try
            {
                // Show loading
                Cursor = Cursors.WaitCursor;

                // Get data
                _statistics = _dashboardBLL.GetDashboardData();

                // Update UI
                UpdateStatisticCards();
                UpdateRecentActivities();
                UpdateTopTests();
                UpdateTopCourses();
                UpdateCharts();

                lblLastUpdate.Text = $"Last updated: {DateTime.Now:MMM dd, yyyy HH:mm}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateStatisticCards()
        {
            // Students card
            lblTotalStudents.Text = _statistics.TotalStudents.ToString("N0");
            lblStudentGrowth.Text = $"{_statistics.StudentGrowth:+0.0;-0.0}%";
            lblStudentGrowth.ForeColor = _statistics.StudentGrowth >= 0 ? Color.Green : Color.Red;

            // Tests card
            lblTotalTests.Text = _statistics.TotalTests.ToString("N0");
            lblTestGrowth.Text = $"{_statistics.TestGrowth:+0.0;-0.0}%";
            lblTestGrowth.ForeColor = _statistics.TestGrowth >= 0 ? Color.Green : Color.Red;

            // Courses card
            lblTotalCourses.Text = _statistics.TotalCourses.ToString("N0");
            lblCourseGrowth.Text = $"{_statistics.CourseGrowth:+0.0;-0.0}%";
            lblCourseGrowth.ForeColor = _statistics.CourseGrowth >= 0 ? Color.Green : Color.Red;

            // Enrollments card
            lblActiveEnrollments.Text = _statistics.ActiveEnrollments.ToString("N0");
            lblEnrollmentGrowth.Text = $"{_statistics.EnrollmentGrowth:+0.0;-0.0}%";
            lblEnrollmentGrowth.ForeColor = _statistics.EnrollmentGrowth >= 0 ? Color.Green : Color.Red;
        }

        private void UpdateRecentActivities()
        {
            flpRecentActivities.Controls.Clear();

            foreach (var activity in _statistics.RecentActivities.Take(8))
            {
                var panel = CreateActivityPanel(activity);
                flpRecentActivities.Controls.Add(panel);
            }
        }

        private Panel CreateActivityPanel(RecentActivityDTO activity)
        {
            var panel = new Panel
            {
                Width = flpRecentActivities.Width - 30,
                Height = 70,
                Padding = new Padding(10),
                Margin = new Padding(5),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblIcon = new Label
            {
                Text = activity.Icon,
                Font = new Font("Segoe UI Emoji", 16F),
                Location = new Point(10, 15),
                Size = new Size(40, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblName = new Label
            {
                Text = activity.UserName,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(60, 10),
                AutoSize = true,
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            var lblActivity = new Label
            {
                Text = activity.Activity,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(60, 30),
                Width = panel.Width - 200,
                ForeColor = Color.Gray
            };

            var lblTime = new Label
            {
                Text = GetTimeAgo(activity.Time),
                Font = new Font("Segoe UI", 8F),
                Location = new Point(panel.Width - 120, 25),
                AutoSize = true,
                ForeColor = Color.LightGray
            };

            panel.Controls.AddRange(new Control[] { lblIcon, lblName, lblActivity, lblTime });

            return panel;
        }

        private void UpdateTopTests()
        {
            dgvTopTests.Rows.Clear();

            foreach (var test in _statistics.TopTests)
            {
                int rowIndex = dgvTopTests.Rows.Add();
                var row = dgvTopTests.Rows[rowIndex];

                row.Cells["colTestTitle"].Value = test.TestTitle;
                row.Cells["colAttempts"].Value = test.AttemptCount;
                row.Cells["colAvgBand"].Value = test.AverageBand.ToString("0.0");
            }
        }

        private void UpdateTopCourses()
        {
            dgvTopCourses.Rows.Clear();

            foreach (var course in _statistics.TopCourses)
            {
                int rowIndex = dgvTopCourses.Rows.Add();
                var row = dgvTopCourses.Rows[rowIndex];

                row.Cells["colCourseTitle"].Value = course.CourseTitle;
                row.Cells["colEnrollments"].Value = course.EnrollmentCount;
                row.Cells["colCompletion"].Value = $"{course.CompletionRate}%";
            }
        }

        private void UpdateCharts()
        {
            // Simple bar chart using panels
            DrawBarChart(pnlStudentChart, _statistics.MonthlyStudents, Color.FromArgb(80, 160, 255));
            DrawBarChart(pnlTestChart, _statistics.MonthlyTests, Color.FromArgb(255, 140, 80));
        }

        private void DrawBarChart(Panel chartPanel, System.Collections.Generic.List<MonthlyDataDTO> data, Color barColor)
        {
            chartPanel.Controls.Clear();

            if (data == null || !data.Any())
                return;

            int maxValue = data.Max(d => d.Count);
            int barWidth = (chartPanel.Width - 60) / data.Count;
            int chartHeight = chartPanel.Height - 40;

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                int barHeight = maxValue > 0 ? (int)((double)item.Count / maxValue * chartHeight) : 0;

                // Bar
                var bar = new Panel
                {
                    Width = barWidth - 10,
                    Height = barHeight,
                    BackColor = barColor,
                    Location = new Point(30 + i * barWidth, chartHeight - barHeight)
                };

                // Value label
                var lblValue = new Label
                {
                    Text = item.Count.ToString(),
                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(5, barHeight - 20),
                    AutoSize = true
                };

                // Month label
                var lblMonth = new Label
                {
                    Text = item.Month,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.Gray,
                    Location = new Point(30 + i * barWidth, chartHeight + 5),
                    Width = barWidth - 10,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                bar.Controls.Add(lblValue);
                chartPanel.Controls.Add(bar);
                chartPanel.Controls.Add(lblMonth);
            }
        }

        private string GetTimeAgo(DateTime time)
        {
            var diff = DateTime.Now - time;

            if (diff.TotalMinutes < 1)
                return "Just now";
            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes}m ago";
            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours}h ago";
            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays}d ago";

            return time.ToString("MMM dd");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboardData();
        }
    }
}
