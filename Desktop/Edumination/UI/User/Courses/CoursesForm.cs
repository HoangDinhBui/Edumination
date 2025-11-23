using IELTS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace IELTS.UI.User.Courses
{
    public partial class CoursesForm : Form
    {
        private readonly CourseBLL _courseBLL = new CourseBLL();

        public CoursesForm()
        {
            InitializeComponent();
        }

        private void CoursesForm_Load(object sender, EventArgs e)
        {
            LoadNavbar();
            LoadCoursesFromDatabase();
        }

        private void LoadNavbar()
        {
            var nav = new IELTS.UI.User.Home.UserNavbarPanel();
            nav.Dock = DockStyle.Fill;
            panelNavbar.Controls.Add(nav);

            nav.SetActive("courses"); // highlight Courses
        }


        private void LoadCoursesFromDatabase()
        {
            try
            {
                flowCourses.Controls.Clear();

                DataTable dt = _courseBLL.GetAllPublishedCourses();
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No published courses found.", "Info");
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        string title = row["Title"].ToString();
                        string levelCode = row["Level"].ToString();
                        string desc = row["Description"]?.ToString() ?? "";
                        int priceVnd = row["PriceVND"] != DBNull.Value ? Convert.ToInt32(row["PriceVND"]) : 0;

                        string levelText = levelCode switch
                        {
                            "BEGINNER" => "Beginner",
                            "INTERMEDIATE" => "Intermediate",
                            "ADVANCED" => "Advanced",
                            _ => levelCode
                        };

                        GetBandInfo(
                            title,
                            levelCode,
                            out string bandRange,
                            out string badgeText,
                            out Color badgeColor,
                            out Color bgColor,
                            out string hoursText,
                            out string groupText,
                            out string ratingText,
                            out string successText
                        );

                        long courseId = row["Id"] != DBNull.Value ? Convert.ToInt64(row["Id"]) : 0;
                        string priceText = priceVnd.ToString("N0") + " VND";

                        var card = new CourseCardPanel();
                        card.BindCourse(
                            courseId,
                            title,
                            levelText,
                            bandRange,
                            desc,
                            "Essential grammar foundation",
                            "Basic vocabulary building",
                            "Simple exam techniques",
                            "Weekly practice tests",
                            $"{hoursText} · {groupText} · {ratingText}",
                            successText,
                            priceText,
                            priceVnd,
                            badgeText,
                            badgeColor,
                            bgColor
                        );

                        flowCourses.Controls.Add(card);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading course row: {ex.Message}");
                        continue; // Skip faulty row
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================================
        // UI mapping – bạn có thể chỉnh sửa tùy ý
        // =============================================================
        private void GetBandInfo(
    string title,
    string levelCode,
    out string bandRange,
    out string badgeText,
    out Color badgeColor,
    out Color backgroundColor,
    out string hoursText,
    out string groupText,
    out string ratingText,
    out string successText)
        {
            string t = (title ?? "").ToLower();

            switch (levelCode)
            {
                // ==============================
                // 1. BEGINNER → Foundation
                // ==============================
                case "BEGINNER":
                    bandRange = "0.0 – 5.0";
                    badgeText = "Best for Beginners";
                    badgeColor = Color.FromArgb(37, 201, 133);
                    backgroundColor = Color.FromArgb(236, 250, 244);
                    hoursText = "80 Hours";
                    groupText = "Basic Groups";
                    ratingText = "★ 4.7";
                    successText = "Success Rate 92% ↑";
                    break;

                // ==============================
                // 2. INTERMEDIATE → Booster
                // ==============================
                case "INTERMEDIATE":
                    bandRange = "5.5 – 6.0";
                    badgeText = "Most Balanced";
                    badgeColor = Color.FromArgb(90, 109, 255);
                    backgroundColor = Color.FromArgb(233, 241, 255);
                    hoursText = "100 Hours";
                    groupText = "Standard Groups";
                    ratingText = "★ 4.8";
                    successText = "Success Rate 95% ↑";
                    break;

                // ==============================
                // 3. ADVANCED → Intensive hoặc Mastery
                // ==============================
                case "ADVANCED":

                    // Mastery
                    if (t.Contains("mastery"))
                    {
                        bandRange = "7.5 – 9.0";
                        badgeText = "Elite Track";
                        badgeColor = Color.FromArgb(204, 85, 95);
                        backgroundColor = Color.FromArgb(255, 239, 242);
                        hoursText = "140 Hours";
                        groupText = "Elite Groups";
                        ratingText = "★ 4.9";
                        successText = "Success Rate 97% ↑";
                    }
                    // Intensive
                    else
                    {
                        bandRange = "6.0 – 7.5";
                        badgeText = "For High Achievers";
                        badgeColor = Color.FromArgb(255, 162, 80);
                        backgroundColor = Color.FromArgb(255, 245, 233);
                        hoursText = "120 Hours";
                        groupText = "Advanced Groups";
                        ratingText = "★ 4.9";
                        successText = "Success Rate 96% ↑";
                    }
                    break;

                // ==============================
                // DEFAULT fallback
                // ==============================
                default:
                    bandRange = "0.0 – 5.0";
                    badgeText = "Popular";
                    badgeColor = Color.Gray;
                    backgroundColor = Color.White;
                    hoursText = "80 Hours";
                    groupText = "Basic Groups";
                    ratingText = "★ 4.7";
                    successText = "Success Rate 90% ↑";
                    break;
            }
        }

    }
}
