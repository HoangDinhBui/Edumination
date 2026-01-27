using Sunny.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Admin
{
    public partial class AdminNavBarPanel : UserControl
    {
        private UIButton activeButton;
        public event Action<string> OnMenuClicked;

        public AdminNavBarPanel()
        {
            InitializeComponent();
            BuildMenu();
        }

        private void BuildMenu()
        {
            AddMenu("🏠  Dashboard", "dashboard");
            AddMenu("📚  Khóa học", "courses");
            AddMenu("📝  Bài test", "tests");
            AddMenu("👨‍🎓  Học viên", "students");
            AddMenu("👨‍🏫  Giảng viên", "teachers");
            AddMenu("👤  Tài khoản", "accounts");
            AddMenu("📈  Báo cáo", "reports");
            AddMenu("⚙️  Cài đặt", "settings");
            AddMenu("🚪  Đăng xuất", "logout");

            // Active mặc định
            Activate(menu.Controls[0] as UIButton, "dashboard");
        }

        private void AddMenu(string text, string key)
        {
            var btn = CreateMenuButton(text);
            btn.Click += (s, e) => Activate(btn, key);
            menu.Controls.Add(btn);
        }

        private void Activate(UIButton btn, string key)
        {
            if (btn == null) return;

            // reset button cũ
            if (activeButton != null)
            {
                activeButton.FillColor = Color.Transparent;
                activeButton.RectColor = Color.Transparent;
                activeButton.ForeColor = Color.FromArgb(60, 65, 75);
            }

            // active mới
            btn.FillColor = Color.FromArgb(235, 240, 255);
            btn.RectColor = Color.FromArgb(79, 124, 255); // indicator
            btn.ForeColor = Color.FromArgb(79, 124, 255);

            activeButton = btn;
            OnMenuClicked?.Invoke(key);
        }

        private UIButton CreateMenuButton(string text)
        {
            return new UIButton
            {
                Text = text,
                Font = new Font("Segoe UI", 11F),
                TextAlign = ContentAlignment.MiddleLeft,

                Size = new Size(220, 46),
                Margin = new Padding(0, 6, 0, 6),
                Padding = new Padding(16, 0, 0, 0),

                Radius = 10,

                // ===== NORMAL =====
                FillColor = Color.Transparent,
                RectColor = Color.Transparent,
                ForeColor = Color.FromArgb(60, 65, 75),

                // ===== HOVER =====
                FillHoverColor = Color.FromArgb(238, 243, 255),
                ForeHoverColor = Color.FromArgb(79, 124, 255),

                // ===== PRESS =====
                FillPressColor = Color.FromArgb(224, 233, 255),
                ForePressColor = Color.FromArgb(79, 124, 255),

                Cursor = Cursors.Hand
            };
        }
    }
}
