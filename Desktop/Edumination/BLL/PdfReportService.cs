using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IELTS.DTO;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Windows.Media;
using QuestPDF.Fluent;        // 👉 Chứa class Document
using QuestPDF.Helpers;       // 👉 Chứa Colors, PageSizes
using QuestPDF.Infrastructure; // 👉 Chứa Unit, LicenseType
using Colors = QuestPDF.Helpers.Colors;

namespace IELTS.BLL
{
    // 👇 BẠN ĐANG THIẾU DÒNG CLASS NÀY
    public class PdfReportService
    {
        public void ExportDashboardToPdf(string filePath, DashboardSummaryDTO summary, List<RevenueChartDTO> revenueList, byte[] chartImageBytes)
        {
            // Cấu hình License
            QuestPDF.Settings.License = LicenseType.Community;

            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                    // HEADER
                    page.Header()
                        .Text("BÁO CÁO THỐNG KÊ HỆ THỐNG IELTS LEARNING")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    // CONTENT
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            // A. Tổng quan
                            x.Item().Text("1. Tổng quan hệ thống").Bold().FontSize(16);
                            x.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Text("Tổng Học viên").Bold();
                                    header.Cell().Text("Tổng Khóa học").Bold();
                                    header.Cell().Text("Lượt thi").Bold();
                                    header.Cell().Text("Tổng Doanh thu").Bold();
                                });

                                table.Cell().Text(summary.TotalStudents.ToString());
                                table.Cell().Text(summary.TotalCourses.ToString());
                                table.Cell().Text(summary.TotalTestsTaken.ToString());
                                table.Cell().Text(string.Format("{0:N0} VNĐ", summary.TotalRevenue));
                            });

                            x.Item().PaddingVertical(20);

                            // B. Biểu đồ
                            x.Item().Text("2. Biểu đồ tăng trưởng doanh thu").Bold().FontSize(16);
                            x.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            if (chartImageBytes != null)
                            {
                                x.Item().Image(chartImageBytes).FitArea();
                            }

                            x.Item().PaddingVertical(20);

                            // C. Chi tiết
                            x.Item().Text("3. Chi tiết doanh thu theo tháng").Bold().FontSize(16);
                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(50);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("STT");
                                    header.Cell().Element(CellStyle).Text("Tháng/Năm");
                                    header.Cell().Element(CellStyle).Text("Doanh thu (VNĐ)");
                                });

                                int stt = 1;
                                foreach (var item in revenueList)
                                {
                                    table.Cell().Element(CellStyle).Text(stt++);
                                    table.Cell().Element(CellStyle).Text(item.Month);
                                    table.Cell().Element(CellStyle).Text(string.Format("{0:N0}", item.Revenue));
                                }
                            });
                        });

                    // FOOTER
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Trang ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                            x.Span($" - Xuất ngày: {DateTime.Now:dd/MM/yyyy HH:mm}");
                        });
                });
            })
            .GeneratePdf(filePath);
        }

        // Helper function
        // ✅ Chỉ định rõ dùng IContainer của QuestPDF
        static QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
        {
            return container.BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).PaddingVertical(5);
        }
    } // 👈 ĐÓNG CLASS TẠI ĐÂY
}