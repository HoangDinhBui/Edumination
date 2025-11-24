namespace IELTS.UI.Admin.ReportManager
{
    partial class ReportsPanel
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            // 1. Khai báo các thành phần của Chart (QUAN TRỌNG - BẠN ĐANG THIẾU ĐOẠN NÀY)
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();

            pnlTop = new Sunny.UI.UIPanel();
            btnExportPdf = new Sunny.UI.UISymbolButton();
            btnRefresh = new Sunny.UI.UISymbolButton();
            cardRevenue = new Sunny.UI.UISymbolLabel();
            cardTests = new Sunny.UI.UISymbolLabel();
            cardCourses = new Sunny.UI.UISymbolLabel();
            cardStudents = new Sunny.UI.UISymbolLabel();
            pnlChart = new Sunny.UI.UITitlePanel();

            // 2. Khởi tạo biến Chart (QUAN TRỌNG)
            chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();

            pnlTop.SuspendLayout();
            pnlChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(chartRevenue)).BeginInit();
            SuspendLayout();

            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(btnExportPdf);
            pnlTop.Controls.Add(btnRefresh);
            pnlTop.Controls.Add(cardRevenue);
            pnlTop.Controls.Add(cardTests);
            pnlTop.Controls.Add(cardCourses);
            pnlTop.Controls.Add(cardStudents);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.FillColor = Color.White;
            pnlTop.Font = new Font("Microsoft Sans Serif", 12F);
            pnlTop.Location = new Point(0, 0);
            pnlTop.Margin = new Padding(4, 5, 4, 5);
            pnlTop.MinimumSize = new Size(1, 1);
            pnlTop.Name = "pnlTop";
            pnlTop.RectColor = Color.White;
            pnlTop.Size = new Size(1230, 160);
            pnlTop.TabIndex = 0;
            pnlTop.Text = null;
            pnlTop.TextAlignment = ContentAlignment.MiddleCenter;

            // 
            // btnExportPdf
            // 
            btnExportPdf.Cursor = Cursors.Hand;
            btnExportPdf.FillColor = Color.Purple;
            btnExportPdf.FillHoverColor = Color.MediumPurple;
            btnExportPdf.FillPressColor = Color.Indigo;
            btnExportPdf.Font = new Font("Microsoft Sans Serif", 12F);
            btnExportPdf.Location = new Point(970, 20);
            btnExportPdf.MinimumSize = new Size(1, 1);
            btnExportPdf.Name = "btnExportPdf";
            btnExportPdf.RectColor = Color.Purple;
            btnExportPdf.Size = new Size(120, 40);
            btnExportPdf.Symbol = 61888;
            btnExportPdf.TabIndex = 0;
            btnExportPdf.Text = "Xuất PDF";

            // 
            // btnRefresh
            // 
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 12F);
            btnRefresh.Location = new Point(1100, 20);
            btnRefresh.MinimumSize = new Size(1, 1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.Symbol = 61473;
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Tải lại";

            // 
            // cardRevenue
            // 
            cardRevenue.Font = new Font("Microsoft Sans Serif", 12F);
            cardRevenue.Location = new Point(20 + 280 * 3 + 60, 20); // Chỉnh lại vị trí cho đẹp
            cardRevenue.MinimumSize = new Size(1, 1);
            cardRevenue.Name = "cardRevenue";
            cardRevenue.Size = new Size(280, 120);
            cardRevenue.Style = Sunny.UI.UIStyle.Red; // Đổi màu
            cardRevenue.Symbol = 61785;
            cardRevenue.SymbolSize = 64;
            cardRevenue.TabIndex = 2;
            cardRevenue.Text = "Doanh thu\n0 VNĐ";

            // 
            // cardTests
            // 
            cardTests.Font = new Font("Microsoft Sans Serif", 12F);
            cardTests.Location = new Point(20 + 280 * 2 + 40, 20);
            cardTests.MinimumSize = new Size(1, 1);
            cardTests.Name = "cardTests";
            cardTests.Size = new Size(280, 120);
            cardTests.Style = Sunny.UI.UIStyle.Orange;
            cardTests.Symbol = 61568;
            cardTests.SymbolSize = 64;
            cardTests.TabIndex = 3;
            cardTests.Text = "Lượt thi\n0";

            // 
            // cardCourses
            // 
            cardCourses.Font = new Font("Microsoft Sans Serif", 12F);
            cardCourses.Location = new Point(20 + 280 + 20, 20);
            cardCourses.MinimumSize = new Size(1, 1);
            cardCourses.Name = "cardCourses";
            cardCourses.Size = new Size(280, 120);
            cardCourses.Style = Sunny.UI.UIStyle.Green;
            cardCourses.Symbol = 61869;
            cardCourses.SymbolSize = 64;
            cardCourses.TabIndex = 4;
            cardCourses.Text = "Khóa học\n0";

            // 
            // cardStudents
            // 
            cardStudents.Font = new Font("Microsoft Sans Serif", 12F);
            cardStudents.Location = new Point(20, 20);
            cardStudents.MinimumSize = new Size(1, 1);
            cardStudents.Name = "cardStudents";
            cardStudents.Size = new Size(280, 120);
            cardStudents.Style = Sunny.UI.UIStyle.Blue;
            cardStudents.Symbol = 61447;
            cardStudents.SymbolSize = 64;
            cardStudents.TabIndex = 5;
            cardStudents.Text = "Học viên\n0";
            cardStudents.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // pnlChart
            // 
            pnlChart.Controls.Add(chartRevenue); // 3. THÊM CHART VÀO PANEL (QUAN TRỌNG)
            pnlChart.Dock = DockStyle.Fill;
            pnlChart.Font = new Font("Microsoft Sans Serif", 12F);
            pnlChart.Location = new Point(0, 160);
            pnlChart.Margin = new Padding(4, 5, 4, 5);
            pnlChart.MinimumSize = new Size(1, 1);
            pnlChart.Name = "pnlChart";
            pnlChart.Padding = new Padding(10, 35, 10, 10);
            pnlChart.ShowText = false;
            pnlChart.Size = new Size(1230, 860);
            pnlChart.TabIndex = 1;
            pnlChart.Text = "BIỂU ĐỒ DOANH THU 6 THÁNG GẦN NHẤT";
            pnlChart.TextAlignment = ContentAlignment.MiddleCenter;
            pnlChart.TitleColor = Color.FromArgb(80, 160, 255);

            // 
            // chartRevenue (Cấu hình chi tiết Chart)
            // 
            chartArea1.Name = "ChartArea1";
            chartRevenue.ChartAreas.Add(chartArea1);
            chartRevenue.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chartRevenue.Legends.Add(legend1);
            chartRevenue.Location = new Point(10, 35);
            chartRevenue.Name = "chartRevenue";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Doanh thu (VNĐ)";
            chartRevenue.Series.Add(series1);
            chartRevenue.Size = new Size(1210, 815);
            chartRevenue.TabIndex = 0;
            chartRevenue.Text = "chartRevenue";
            title1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            title1.Name = "Title1";
            title1.Text = "Tăng trưởng doanh thu";
            chartRevenue.Titles.Add(title1);

            // 
            // ReportsPanel
            // 
            Controls.Add(pnlChart);
            Controls.Add(pnlTop);
            Name = "ReportsPanel";
            Size = new Size(1230, 1020);
            pnlTop.ResumeLayout(false);
            pnlChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(chartRevenue)).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private Sunny.UI.UIPanel pnlTop;
        private Sunny.UI.UISymbolLabel cardStudents;
        private Sunny.UI.UISymbolLabel cardCourses;
        private Sunny.UI.UISymbolLabel cardTests;
        private Sunny.UI.UISymbolLabel cardRevenue;
        private Sunny.UI.UITitlePanel pnlChart;
        private Sunny.UI.UISymbolButton btnRefresh;
        private Sunny.UI.UISymbolButton btnExportPdf; // 4. Khai báo control
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
    }
}