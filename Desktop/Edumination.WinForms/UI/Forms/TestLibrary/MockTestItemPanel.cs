using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Edumination.WinForms.UI.Forms.TestLibrary
{
    public partial class MockTestItemPanel : UserControl
    {
        private Label lblTitle;
        private Label lblTaken;
        private PictureBox picIcon;

        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
        }

        public string Taken
        {
            get => lblTaken.Text;
            set => lblTaken.Text = value;
        }

        public MockTestItemPanel()
        {
            this.Size = new Size(370, 110);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Padding = new Padding(15);
            this.Margin = new Padding(10);
            this.Cursor = Cursors.Hand;

            this.Region = new Region(
                new System.Drawing.Drawing2D.GraphicsPath()
                .RoundedRect(new Rectangle(0, 0, Width, Height), 20)
            );

            lblTitle = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI Semibold", 11),
                ForeColor = Color.FromArgb(40, 50, 80),
                Dock = DockStyle.Top,
                Height = 45
            };

            lblTaken = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Dock = DockStyle.Bottom,
                Height = 25
            };

            picIcon = new PictureBox
            {
                Size = new Size(20, 20),
                Image = Image.FromFile("assets/icons/lightning.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Left = 15,
                Top = 70
            };

            this.Controls.Add(lblTaken);
            this.Controls.Add(lblTitle);
            this.Controls.Add(picIcon);
        }

        private void MockTestItemPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
