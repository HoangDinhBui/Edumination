namespace IELTS.UI.Admin.DashBoard
{
    partial class AdminDashboardForm
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

        private void InitializeComponent()
        {
            pnlHeader = new Sunny.UI.UIPanel();
            lblTitle = new Sunny.UI.UILabel();
            lblLastUpdate = new Sunny.UI.UILabel();
            btnRefresh = new Sunny.UI.UISymbolButton();
            pnlStudentCard = new Sunny.UI.UIPanel();
            lblStudentsLabel = new Sunny.UI.UILabel();
            lblTotalStudents = new Sunny.UI.UILabel();
            lblStudentGrowth = new Sunny.UI.UILabel();
            pnlTestCard = new Sunny.UI.UIPanel();
            lblTestsLabel = new Sunny.UI.UILabel();
            lblTotalTests = new Sunny.UI.UILabel();
            lblTestGrowth = new Sunny.UI.UILabel();
            pnlCourseCard = new Sunny.UI.UIPanel();
            lblCoursesLabel = new Sunny.UI.UILabel();
            lblTotalCourses = new Sunny.UI.UILabel();
            lblCourseGrowth = new Sunny.UI.UILabel();
            pnlEnrollmentCard = new Sunny.UI.UIPanel();
            lblEnrollmentsLabel = new Sunny.UI.UILabel();
            lblActiveEnrollments = new Sunny.UI.UILabel();
            lblEnrollmentGrowth = new Sunny.UI.UILabel();
            pnlActivities = new Sunny.UI.UIPanel();
            lblActivitiesTitle = new Sunny.UI.UILabel();
            flpRecentActivities = new FlowLayoutPanel();
            pnlTopTests = new Sunny.UI.UIPanel();
            lblTopTestsTitle = new Sunny.UI.UILabel();
            dgvTopTests = new DataGridView();
            colTestTitle = new DataGridViewTextBoxColumn();
            colAttempts = new DataGridViewTextBoxColumn();
            colAvgBand = new DataGridViewTextBoxColumn();
            pnlTopCourses = new Sunny.UI.UIPanel();
            lblTopCoursesTitle = new Sunny.UI.UILabel();
            dgvTopCourses = new DataGridView();
            colCourseTitle = new DataGridViewTextBoxColumn();
            colEnrollments = new DataGridViewTextBoxColumn();
            colCompletion = new DataGridViewTextBoxColumn();
            pnlCharts = new Sunny.UI.UIPanel();
            lblStudentChartTitle = new Sunny.UI.UILabel();
            pnlStudentChart = new Panel();
            lblTestChartTitle = new Sunny.UI.UILabel();
            pnlTestChart = new Panel();
            pnlHeader.SuspendLayout();
            pnlStudentCard.SuspendLayout();
            pnlTestCard.SuspendLayout();
            pnlCourseCard.SuspendLayout();
            pnlEnrollmentCard.SuspendLayout();
            pnlActivities.SuspendLayout();
            pnlTopTests.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopTests).BeginInit();
            pnlTopCourses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTopCourses).BeginInit();
            pnlCharts.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblLastUpdate);
            pnlHeader.Controls.Add(btnRefresh);
            pnlHeader.FillColor = Color.FromArgb(80, 160, 255);
            pnlHeader.Font = new Font("Microsoft Sans Serif", 12F);
            pnlHeader.Location = new Point(27, 31);
            pnlHeader.Margin = new Padding(5, 8, 5, 8);
            pnlHeader.MinimumSize = new Size(1, 2);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Radius = 10;
            pnlHeader.Size = new Size(1653, 123);
            pnlHeader.TabIndex = 0;
            pnlHeader.Text = null;
            pnlHeader.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(400, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📊 Admin Dashboard";
            // 
            // lblLastUpdate
            // 
            lblLastUpdate.BackColor = Color.Transparent;
            lblLastUpdate.Font = new Font("Segoe UI", 9F);
            lblLastUpdate.ForeColor = Color.White;
            lblLastUpdate.Location = new Point(850, 30);
            lblLastUpdate.Name = "lblLastUpdate";
            lblLastUpdate.Size = new Size(250, 20);
            lblLastUpdate.TabIndex = 1;
            lblLastUpdate.Text = "Last updated: --";
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new Font("Microsoft Sans Serif", 12F);
            btnRefresh.Location = new Point(1120, 20);
            btnRefresh.MinimumSize = new Size(1, 1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Radius = 8;
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.Symbol = 61473;
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.TipsFont = new Font("Microsoft Sans Serif", 9F);
            btnRefresh.Click += btnRefresh_Click;
            // 
            // pnlStudentCard
            // 
            pnlStudentCard.Controls.Add(lblStudentsLabel);
            pnlStudentCard.Controls.Add(lblTotalStudents);
            pnlStudentCard.Controls.Add(lblStudentGrowth);
            pnlStudentCard.FillColor = Color.FromArgb(240, 248, 255);
            pnlStudentCard.Font = new Font("Microsoft Sans Serif", 12F);
            pnlStudentCard.Location = new Point(27, 185);
            pnlStudentCard.Margin = new Padding(5, 8, 5, 8);
            pnlStudentCard.MinimumSize = new Size(1, 2);
            pnlStudentCard.Name = "pnlStudentCard";
            pnlStudentCard.Radius = 10;
            pnlStudentCard.RectSize = 2;
            pnlStudentCard.Size = new Size(387, 185);
            pnlStudentCard.TabIndex = 1;
            pnlStudentCard.Text = null;
            pnlStudentCard.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblStudentsLabel
            // 
            lblStudentsLabel.Font = new Font("Segoe UI", 10F);
            lblStudentsLabel.ForeColor = Color.Gray;
            lblStudentsLabel.Location = new Point(20, 15);
            lblStudentsLabel.Name = "lblStudentsLabel";
            lblStudentsLabel.Size = new Size(200, 20);
            lblStudentsLabel.TabIndex = 0;
            lblStudentsLabel.Text = "👥 Total Students";
            // 
            // lblTotalStudents
            // 
            lblTotalStudents.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalStudents.ForeColor = Color.FromArgb(80, 160, 255);
            lblTotalStudents.Location = new Point(20, 45);
            lblTotalStudents.Name = "lblTotalStudents";
            lblTotalStudents.Size = new Size(150, 54);
            lblTotalStudents.TabIndex = 1;
            lblTotalStudents.Text = "0";
            // 
            // lblStudentGrowth
            // 
            lblStudentGrowth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblStudentGrowth.ForeColor = Color.Green;
            lblStudentGrowth.Location = new Point(20, 113);
            lblStudentGrowth.Name = "lblStudentGrowth";
            lblStudentGrowth.Size = new Size(100, 20);
            lblStudentGrowth.TabIndex = 2;
            lblStudentGrowth.Text = "+0.0%";
            // 
            // pnlTestCard
            // 
            pnlTestCard.Controls.Add(lblTestsLabel);
            pnlTestCard.Controls.Add(lblTotalTests);
            pnlTestCard.Controls.Add(lblTestGrowth);
            pnlTestCard.FillColor = Color.FromArgb(255, 248, 240);
            pnlTestCard.Font = new Font("Microsoft Sans Serif", 12F);
            pnlTestCard.Location = new Point(440, 185);
            pnlTestCard.Margin = new Padding(5, 8, 5, 8);
            pnlTestCard.MinimumSize = new Size(1, 2);
            pnlTestCard.Name = "pnlTestCard";
            pnlTestCard.Radius = 10;
            pnlTestCard.RectColor = Color.FromArgb(255, 140, 80);
            pnlTestCard.RectSize = 2;
            pnlTestCard.Size = new Size(387, 185);
            pnlTestCard.TabIndex = 2;
            pnlTestCard.Text = null;
            pnlTestCard.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblTestsLabel
            // 
            lblTestsLabel.Font = new Font("Segoe UI", 10F);
            lblTestsLabel.ForeColor = Color.Gray;
            lblTestsLabel.Location = new Point(20, 15);
            lblTestsLabel.Name = "lblTestsLabel";
            lblTestsLabel.Size = new Size(200, 20);
            lblTestsLabel.TabIndex = 0;
            lblTestsLabel.Text = "📝 Total Tests";
            // 
            // lblTotalTests
            // 
            lblTotalTests.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalTests.ForeColor = Color.FromArgb(255, 140, 80);
            lblTotalTests.Location = new Point(20, 45);
            lblTotalTests.Name = "lblTotalTests";
            lblTotalTests.Size = new Size(150, 54);
            lblTotalTests.TabIndex = 1;
            lblTotalTests.Text = "0";
            // 
            // lblTestGrowth
            // 
            lblTestGrowth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblTestGrowth.ForeColor = Color.Green;
            lblTestGrowth.Location = new Point(20, 113);
            lblTestGrowth.Name = "lblTestGrowth";
            lblTestGrowth.Size = new Size(100, 20);
            lblTestGrowth.TabIndex = 2;
            lblTestGrowth.Text = "+0.0%";
            // 
            // pnlCourseCard
            // 
            pnlCourseCard.Controls.Add(lblCoursesLabel);
            pnlCourseCard.Controls.Add(lblTotalCourses);
            pnlCourseCard.Controls.Add(lblCourseGrowth);
            pnlCourseCard.FillColor = Color.FromArgb(248, 240, 255);
            pnlCourseCard.Font = new Font("Microsoft Sans Serif", 12F);
            pnlCourseCard.Location = new Point(853, 185);
            pnlCourseCard.Margin = new Padding(5, 8, 5, 8);
            pnlCourseCard.MinimumSize = new Size(1, 2);
            pnlCourseCard.Name = "pnlCourseCard";
            pnlCourseCard.Radius = 10;
            pnlCourseCard.RectColor = Color.FromArgb(160, 80, 255);
            pnlCourseCard.RectSize = 2;
            pnlCourseCard.Size = new Size(387, 185);
            pnlCourseCard.TabIndex = 3;
            pnlCourseCard.Text = null;
            pnlCourseCard.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblCoursesLabel
            // 
            lblCoursesLabel.Font = new Font("Segoe UI", 10F);
            lblCoursesLabel.ForeColor = Color.Gray;
            lblCoursesLabel.Location = new Point(20, 15);
            lblCoursesLabel.Name = "lblCoursesLabel";
            lblCoursesLabel.Size = new Size(200, 20);
            lblCoursesLabel.TabIndex = 0;
            lblCoursesLabel.Text = "📚 Total Courses";
            // 
            // lblTotalCourses
            // 
            lblTotalCourses.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTotalCourses.ForeColor = Color.FromArgb(160, 80, 255);
            lblTotalCourses.Location = new Point(20, 45);
            lblTotalCourses.Name = "lblTotalCourses";
            lblTotalCourses.Size = new Size(150, 54);
            lblTotalCourses.TabIndex = 1;
            lblTotalCourses.Text = "0";
            // 
            // lblCourseGrowth
            // 
            lblCourseGrowth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCourseGrowth.ForeColor = Color.Green;
            lblCourseGrowth.Location = new Point(20, 113);
            lblCourseGrowth.Name = "lblCourseGrowth";
            lblCourseGrowth.Size = new Size(100, 20);
            lblCourseGrowth.TabIndex = 2;
            lblCourseGrowth.Text = "+0.0%";
            // 
            // pnlEnrollmentCard
            // 
            pnlEnrollmentCard.Controls.Add(lblEnrollmentsLabel);
            pnlEnrollmentCard.Controls.Add(lblActiveEnrollments);
            pnlEnrollmentCard.Controls.Add(lblEnrollmentGrowth);
            pnlEnrollmentCard.FillColor = Color.FromArgb(240, 255, 248);
            pnlEnrollmentCard.Font = new Font("Microsoft Sans Serif", 12F);
            pnlEnrollmentCard.Location = new Point(1267, 185);
            pnlEnrollmentCard.Margin = new Padding(5, 8, 5, 8);
            pnlEnrollmentCard.MinimumSize = new Size(1, 2);
            pnlEnrollmentCard.Name = "pnlEnrollmentCard";
            pnlEnrollmentCard.Radius = 10;
            pnlEnrollmentCard.RectColor = Color.FromArgb(80, 200, 120);
            pnlEnrollmentCard.RectSize = 2;
            pnlEnrollmentCard.Size = new Size(387, 185);
            pnlEnrollmentCard.TabIndex = 4;
            pnlEnrollmentCard.Text = null;
            pnlEnrollmentCard.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblEnrollmentsLabel
            // 
            lblEnrollmentsLabel.Font = new Font("Segoe UI", 10F);
            lblEnrollmentsLabel.ForeColor = Color.Gray;
            lblEnrollmentsLabel.Location = new Point(20, 15);
            lblEnrollmentsLabel.Name = "lblEnrollmentsLabel";
            lblEnrollmentsLabel.Size = new Size(200, 20);
            lblEnrollmentsLabel.TabIndex = 0;
            lblEnrollmentsLabel.Text = "✅ Enrollments";
            // 
            // lblActiveEnrollments
            // 
            lblActiveEnrollments.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblActiveEnrollments.ForeColor = Color.FromArgb(80, 200, 120);
            lblActiveEnrollments.Location = new Point(20, 45);
            lblActiveEnrollments.Name = "lblActiveEnrollments";
            lblActiveEnrollments.Size = new Size(150, 54);
            lblActiveEnrollments.TabIndex = 1;
            lblActiveEnrollments.Text = "0";
            // 
            // lblEnrollmentGrowth
            // 
            lblEnrollmentGrowth.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEnrollmentGrowth.ForeColor = Color.Green;
            lblEnrollmentGrowth.Location = new Point(20, 113);
            lblEnrollmentGrowth.Name = "lblEnrollmentGrowth";
            lblEnrollmentGrowth.Size = new Size(100, 20);
            lblEnrollmentGrowth.TabIndex = 2;
            lblEnrollmentGrowth.Text = "+0.0%";
            // 
            // pnlActivities
            // 
            pnlActivities.Controls.Add(lblActivitiesTitle);
            pnlActivities.Controls.Add(flpRecentActivities);
            pnlActivities.FillColor = Color.White;
            pnlActivities.Font = new Font("Microsoft Sans Serif", 12F);
            pnlActivities.Location = new Point(27, 400);
            pnlActivities.Margin = new Padding(5, 8, 5, 8);
            pnlActivities.MinimumSize = new Size(1, 2);
            pnlActivities.Name = "pnlActivities";
            pnlActivities.Radius = 10;
            pnlActivities.Size = new Size(533, 738);
            pnlActivities.TabIndex = 5;
            pnlActivities.Text = null;
            pnlActivities.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblActivitiesTitle
            // 
            lblActivitiesTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblActivitiesTitle.ForeColor = Color.FromArgb(48, 48, 48);
            lblActivitiesTitle.Location = new Point(20, 15);
            lblActivitiesTitle.Name = "lblActivitiesTitle";
            lblActivitiesTitle.Size = new Size(300, 25);
            lblActivitiesTitle.TabIndex = 0;
            lblActivitiesTitle.Text = "📌 Recent Activities";
            // 
            // flpRecentActivities
            // 
            flpRecentActivities.AutoScroll = true;
            flpRecentActivities.FlowDirection = FlowDirection.TopDown;
            flpRecentActivities.Location = new Point(10, 50);
            flpRecentActivities.Name = "flpRecentActivities";
            flpRecentActivities.Size = new Size(380, 420);
            flpRecentActivities.TabIndex = 1;
            flpRecentActivities.WrapContents = false;
            // 
            // pnlTopTests
            // 
            pnlTopTests.Controls.Add(lblTopTestsTitle);
            pnlTopTests.Controls.Add(dgvTopTests);
            pnlTopTests.FillColor = Color.White;
            pnlTopTests.Font = new Font("Microsoft Sans Serif", 12F);
            pnlTopTests.Location = new Point(587, 400);
            pnlTopTests.Margin = new Padding(5, 8, 5, 8);
            pnlTopTests.MinimumSize = new Size(1, 2);
            pnlTopTests.Name = "pnlTopTests";
            pnlTopTests.Radius = 10;
            pnlTopTests.Size = new Size(533, 354);
            pnlTopTests.TabIndex = 6;
            pnlTopTests.Text = null;
            pnlTopTests.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblTopTestsTitle
            // 
            lblTopTestsTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTopTestsTitle.ForeColor = Color.FromArgb(48, 48, 48);
            lblTopTestsTitle.Location = new Point(20, 15);
            lblTopTestsTitle.Name = "lblTopTestsTitle";
            lblTopTestsTitle.Size = new Size(300, 25);
            lblTopTestsTitle.TabIndex = 0;
            lblTopTestsTitle.Text = "🏆 Top Tests";
            // 
            // dgvTopTests
            // 
            dgvTopTests.AllowUserToAddRows = false;
            dgvTopTests.AllowUserToDeleteRows = false;
            dgvTopTests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopTests.ColumnHeadersHeight = 29;
            dgvTopTests.Columns.AddRange(new DataGridViewColumn[] { colTestTitle, colAttempts, colAvgBand });
            dgvTopTests.Location = new Point(10, 50);
            dgvTopTests.Name = "dgvTopTests";
            dgvTopTests.ReadOnly = true;
            dgvTopTests.RowHeadersVisible = false;
            dgvTopTests.RowHeadersWidth = 51;
            dgvTopTests.Size = new Size(380, 170);
            dgvTopTests.TabIndex = 1;
            // 
            // colTestTitle
            // 
            colTestTitle.HeaderText = "Test Title";
            colTestTitle.MinimumWidth = 6;
            colTestTitle.Name = "colTestTitle";
            colTestTitle.ReadOnly = true;
            // 
            // colAttempts
            // 
            colAttempts.HeaderText = "Attempts";
            colAttempts.MinimumWidth = 6;
            colAttempts.Name = "colAttempts";
            colAttempts.ReadOnly = true;
            // 
            // colAvgBand
            // 
            colAvgBand.HeaderText = "Avg Band";
            colAvgBand.MinimumWidth = 6;
            colAvgBand.Name = "colAvgBand";
            colAvgBand.ReadOnly = true;
            // 
            // pnlTopCourses
            // 
            pnlTopCourses.Controls.Add(lblTopCoursesTitle);
            pnlTopCourses.Controls.Add(dgvTopCourses);
            pnlTopCourses.FillColor = Color.White;
            pnlTopCourses.Font = new Font("Microsoft Sans Serif", 12F);
            pnlTopCourses.Location = new Point(587, 785);
            pnlTopCourses.Margin = new Padding(5, 8, 5, 8);
            pnlTopCourses.MinimumSize = new Size(1, 2);
            pnlTopCourses.Name = "pnlTopCourses";
            pnlTopCourses.Radius = 10;
            pnlTopCourses.Size = new Size(533, 354);
            pnlTopCourses.TabIndex = 7;
            pnlTopCourses.Text = null;
            pnlTopCourses.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblTopCoursesTitle
            // 
            lblTopCoursesTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTopCoursesTitle.ForeColor = Color.FromArgb(48, 48, 48);
            lblTopCoursesTitle.Location = new Point(20, 15);
            lblTopCoursesTitle.Name = "lblTopCoursesTitle";
            lblTopCoursesTitle.Size = new Size(300, 25);
            lblTopCoursesTitle.TabIndex = 0;
            lblTopCoursesTitle.Text = "⭐ Top Courses";
            // 
            // dgvTopCourses
            // 
            dgvTopCourses.AllowUserToAddRows = false;
            dgvTopCourses.AllowUserToDeleteRows = false;
            dgvTopCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTopCourses.ColumnHeadersHeight = 29;
            dgvTopCourses.Columns.AddRange(new DataGridViewColumn[] { colCourseTitle, colEnrollments, colCompletion });
            dgvTopCourses.Location = new Point(10, 50);
            dgvTopCourses.Name = "dgvTopCourses";
            dgvTopCourses.ReadOnly = true;
            dgvTopCourses.RowHeadersVisible = false;
            dgvTopCourses.RowHeadersWidth = 51;
            dgvTopCourses.Size = new Size(380, 170);
            dgvTopCourses.TabIndex = 1;
            // 
            // colCourseTitle
            // 
            colCourseTitle.HeaderText = "Course Title";
            colCourseTitle.MinimumWidth = 6;
            colCourseTitle.Name = "colCourseTitle";
            colCourseTitle.ReadOnly = true;
            // 
            // colEnrollments
            // 
            colEnrollments.HeaderText = "Enrollments";
            colEnrollments.MinimumWidth = 6;
            colEnrollments.Name = "colEnrollments";
            colEnrollments.ReadOnly = true;
            // 
            // colCompletion
            // 
            colCompletion.HeaderText = "Completion";
            colCompletion.MinimumWidth = 6;
            colCompletion.Name = "colCompletion";
            colCompletion.ReadOnly = true;
            // 
            // pnlCharts
            // 
            pnlCharts.Controls.Add(lblStudentChartTitle);
            pnlCharts.Controls.Add(pnlStudentChart);
            pnlCharts.Controls.Add(lblTestChartTitle);
            pnlCharts.Controls.Add(pnlTestChart);
            pnlCharts.FillColor = Color.White;
            pnlCharts.Font = new Font("Microsoft Sans Serif", 12F);
            pnlCharts.Location = new Point(1147, 400);
            pnlCharts.Margin = new Padding(5, 8, 5, 8);
            pnlCharts.MinimumSize = new Size(1, 2);
            pnlCharts.Name = "pnlCharts";
            pnlCharts.Radius = 10;
            pnlCharts.Size = new Size(507, 738);
            pnlCharts.TabIndex = 8;
            pnlCharts.Text = null;
            pnlCharts.TextAlignment = ContentAlignment.MiddleCenter;
            // 
            // lblStudentChartTitle
            // 
            lblStudentChartTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStudentChartTitle.ForeColor = Color.FromArgb(48, 48, 48);
            lblStudentChartTitle.Location = new Point(20, 15);
            lblStudentChartTitle.Name = "lblStudentChartTitle";
            lblStudentChartTitle.Size = new Size(300, 25);
            lblStudentChartTitle.TabIndex = 0;
            lblStudentChartTitle.Text = "📈 Student Growth";
            // 
            // pnlStudentChart
            // 
            pnlStudentChart.BackColor = Color.FromArgb(250, 250, 250);
            pnlStudentChart.BorderStyle = BorderStyle.FixedSingle;
            pnlStudentChart.Location = new Point(10, 50);
            pnlStudentChart.Name = "pnlStudentChart";
            pnlStudentChart.Size = new Size(360, 180);
            pnlStudentChart.TabIndex = 1;
            // 
            // lblTestChartTitle
            // 
            lblTestChartTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTestChartTitle.ForeColor = Color.FromArgb(48, 48, 48);
            lblTestChartTitle.Location = new Point(20, 250);
            lblTestChartTitle.Name = "lblTestChartTitle";
            lblTestChartTitle.Size = new Size(300, 25);
            lblTestChartTitle.TabIndex = 2;
            lblTestChartTitle.Text = "📊 Test Attempts";
            // 
            // pnlTestChart
            // 
            pnlTestChart.BackColor = Color.FromArgb(250, 250, 250);
            pnlTestChart.BorderStyle = BorderStyle.FixedSingle;
            pnlTestChart.Location = new Point(10, 285);
            pnlTestChart.Name = "pnlTestChart";
            pnlTestChart.Size = new Size(360, 180);
            pnlTestChart.TabIndex = 3;
            // 
            // AdminDashboardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1707, 1055);
            Controls.Add(pnlHeader);
            Controls.Add(pnlStudentCard);
            Controls.Add(pnlTestCard);
            Controls.Add(pnlCourseCard);
            Controls.Add(pnlEnrollmentCard);
            Controls.Add(pnlActivities);
            Controls.Add(pnlTopTests);
            Controls.Add(pnlTopCourses);
            Controls.Add(pnlCharts);
            Margin = new Padding(4, 5, 4, 5);
            Name = "AdminDashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Dashboard - IELTS Learning System";
            Load += AdminDashboardForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlStudentCard.ResumeLayout(false);
            pnlTestCard.ResumeLayout(false);
            pnlCourseCard.ResumeLayout(false);
            pnlEnrollmentCard.ResumeLayout(false);
            pnlActivities.ResumeLayout(false);
            pnlTopTests.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopTests).EndInit();
            pnlTopCourses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTopCourses).EndInit();
            pnlCharts.ResumeLayout(false);
            ResumeLayout(false);
        }

        #region Designer Variables

        private Sunny.UI.UIPanel pnlHeader;
        private Sunny.UI.UILabel lblTitle;
        private Sunny.UI.UILabel lblLastUpdate;
        private Sunny.UI.UISymbolButton btnRefresh;

        private Sunny.UI.UIPanel pnlStudentCard;
        private Sunny.UI.UILabel lblTotalStudents;
        private Sunny.UI.UILabel lblStudentsLabel;
        private Sunny.UI.UILabel lblStudentGrowth;

        private Sunny.UI.UIPanel pnlTestCard;
        private Sunny.UI.UILabel lblTotalTests;
        private Sunny.UI.UILabel lblTestsLabel;
        private Sunny.UI.UILabel lblTestGrowth;

        private Sunny.UI.UIPanel pnlCourseCard;
        private Sunny.UI.UILabel lblTotalCourses;
        private Sunny.UI.UILabel lblCoursesLabel;
        private Sunny.UI.UILabel lblCourseGrowth;

        private Sunny.UI.UIPanel pnlEnrollmentCard;
        private Sunny.UI.UILabel lblActiveEnrollments;
        private Sunny.UI.UILabel lblEnrollmentsLabel;
        private Sunny.UI.UILabel lblEnrollmentGrowth;

        private Sunny.UI.UIPanel pnlActivities;
        private Sunny.UI.UILabel lblActivitiesTitle;
        private System.Windows.Forms.FlowLayoutPanel flpRecentActivities;

        private Sunny.UI.UIPanel pnlTopTests;
        private Sunny.UI.UILabel lblTopTestsTitle;
        private System.Windows.Forms.DataGridView dgvTopTests;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTestTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttempts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvgBand;

        private Sunny.UI.UIPanel pnlTopCourses;
        private Sunny.UI.UILabel lblTopCoursesTitle;
        private System.Windows.Forms.DataGridView dgvTopCourses;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCourseTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnrollments;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCompletion;

        private Sunny.UI.UIPanel pnlCharts;
        private Sunny.UI.UILabel lblStudentChartTitle;
        private System.Windows.Forms.Panel pnlStudentChart;
        private Sunny.UI.UILabel lblTestChartTitle;
        private System.Windows.Forms.Panel pnlTestChart;

        #endregion
    }
}