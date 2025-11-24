using IELTS.BLL;
using Sunny.UI;
using System;
using System.IO;
using System.Windows.Forms;
// using IELTS.BLL; // Xóa dòng thừa này

namespace IELTS.UI.Admin.ReportManager // Đảm bảo namespace khớp với Designer
{
    public partial class ReportsPanel : UserControl
    {
        private readonly ReportBLL _bll;

        public ReportsPanel()
        {
            InitializeComponent();
            _bll = new ReportBLL();

            // Gán sự kiện
            this.Load += (s, e) => LoadData();
            this.btnRefresh.Click += (s, e) => LoadData();
            this.btnExportPdf.Click += BtnExportPdf_Click;
        }

        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog sdf = new SaveFileDialog();
            sdf.Filter = "PDF Files (*.pdf)|*.pdf";
            sdf.FileName = $"BaoCao_IELTS_{DateTime.Now:yyyyMMdd}.pdf";

            if (sdf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 1. Lấy dữ liệu hiện tại
                    var summary = _bll.GetDashboardSummary();
                    var revenueList = _bll.GetRevenueChart();

                    // 2. Chụp hình biểu đồ
                    byte[] chartBytes;
                    chartRevenue.Update(); // Đảm bảo biểu đồ đã vẽ xong
                    using (MemoryStream ms = new MemoryStream())
                    {
                        chartRevenue.SaveImage(ms, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                        chartBytes = ms.ToArray();
                    }

                    // 3. Xuất PDF
                    var pdfService = new PdfReportService();
                    pdfService.ExportDashboardToPdf(sdf.FileName, summary, revenueList, chartBytes);

                    UIMessageTip.ShowOk("Xuất báo cáo thành công!");

                    // 4. Mở file (An toàn hơn)
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = sdf.FileName,
                            UseShellExecute = true
                        });
                    }
                    catch { /* Không mở được thì thôi, không cần báo lỗi */ }
                }
                catch (Exception ex)
                {
                    UIMessageBox.ShowError("Lỗi xuất PDF: " + ex.Message);
                }
            }
        }

        private void LoadData()
        {
            try
            {
                // Load Thẻ bài
                var summary = _bll.GetDashboardSummary();
                cardStudents.Text = $"Học viên\n{summary.TotalStudents}";
                cardCourses.Text = $"Khóa học\n{summary.TotalCourses}";
                cardTests.Text = $"Lượt thi\n{summary.TotalTestsTaken}";
                cardRevenue.Text = $"Doanh thu\n{string.Format("{0:N0} VNĐ", summary.TotalRevenue)}";

                // Load Biểu đồ
                var chartData = _bll.GetRevenueChart();
                chartRevenue.Series["Doanh thu (VNĐ)"].Points.Clear();

                foreach (var item in chartData)
                {
                    chartRevenue.Series["Doanh thu (VNĐ)"].Points.AddXY(item.Month, item.Revenue);
                }

                chartRevenue.Series["Doanh thu (VNĐ)"].IsValueShownAsLabel = true;
                chartRevenue.Series["Doanh thu (VNĐ)"].LabelFormat = "{0:N0}";
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError("Lỗi tải báo cáo: " + ex.Message);
            }
        }
    }
}