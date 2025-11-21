using Sunny.UI;
using System.Drawing;
using System.Windows.Forms;

namespace Edumination.WinForms.UI.Forms.TestTaking.Controls
{
    public partial class TestFooterPanel : UserControl
    {
        public event Action<string>? OnPartSelected;

        private readonly List<UIButton> _buttons = new();

        public TestFooterPanel()
        {
            InitializeComponent();
        }

        // Tạo button tự động dựa trên parts mockdata
        public void LoadParts(IEnumerable<string> partNames)
        {
            Controls.Clear();
            _buttons.Clear();

            foreach (var part in partNames)
            {
                var btn = BuildButton(part);
                _buttons.Add(btn);
                Controls.Add(btn);
            }

            CenterButtons();
        }

        private UIButton BuildButton(string text)
        {
            var btn = new UIButton
            {
                Text = text,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(260, 55),
                Radius = 25,
                MinimumSize = new Size(1, 1),
                Cursor = Cursors.Hand,

                FillColor = Color.White,
                ForeColor = Color.FromArgb(39, 56, 146),
                RectColor = Color.FromArgb(39, 56, 146),
                RectSize = 2,

                FillHoverColor = Color.FromArgb(220, 235, 255),
                FillPressColor = Color.FromArgb(39, 56, 146),
                ForePressColor = Color.White
            };

            btn.Click += (s, e) =>
            {
                SetActivePart(text);
                OnPartSelected?.Invoke(text);
            };

            return btn;
        }

        // Highlight đang active
        public void SetActivePart(string partName)
        {
            foreach (var btn in _buttons)
            {
                if (btn.Text == partName)
                {
                    btn.FillColor = Color.FromArgb(39, 56, 146);
                    btn.ForeColor = Color.White;
                }
                else
                {
                    btn.FillColor = Color.White;
                    btn.ForeColor = Color.FromArgb(39, 56, 146);
                }
            }
        }

        // Căn giữa khi resize
        private void TestFooterPanel_Resize(object sender, EventArgs e)
        {
            CenterButtons();
        }

        private void CenterButtons()
        {
            if (_buttons.Count == 0) return;

            int spacing = 40;
            int totalWidth = _buttons.Sum(b => b.Width) + spacing * (_buttons.Count - 1);
            int startX = (Width - totalWidth) / 2;

            int x = startX;
            foreach (var btn in _buttons)
            {
                btn.Location = new Point(x, 18);
                x += btn.Width + spacing;
            }
        }
    }
}
