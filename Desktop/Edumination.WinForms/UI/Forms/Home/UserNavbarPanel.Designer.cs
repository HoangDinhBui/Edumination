namespace Edumination.WinForms.UI.Forms.Home
{
    partial class UserNavbarPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picLogo = new PictureBox();
            btnHome = new Sunny.UI.UIButton();
            btnLibrary = new Sunny.UI.UIButton();
            btnAvt = new Sunny.UI.UIImageButton();
            btnLogout = new Sunny.UI.UIButton();
            btnCourse = new Sunny.UI.UIButton();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnAvt).BeginInit();
            SuspendLayout();
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.edm_logo;
            picLogo.Location = new Point(59, 16);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(99, 40);
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // btnHome
            // 
            btnHome.FillColor = Color.FromArgb(0, 0, 0, 0);
            btnHome.Font = new Font("Noto Serif SC", 13.8F, FontStyle.Bold);
            btnHome.ForeColor = Color.FromArgb(39, 56, 146);
            btnHome.Location = new Point(208, 16);
            btnHome.MinimumSize = new Size(1, 1);
            btnHome.Name = "btnHome";
            btnHome.RectColor = Color.FromArgb(0, 0, 0, 0);
            btnHome.Size = new Size(125, 44);
            btnHome.TabIndex = 1;
            btnHome.Text = "Home";
            btnHome.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // btnLibrary
            // 
            btnLibrary.FillColor = Color.FromArgb(0, 0, 0, 0);
            btnLibrary.Font = new Font("Noto Serif SC", 13.8F, FontStyle.Bold);
            btnLibrary.ForeColor = Color.FromArgb(39, 56, 146);
            btnLibrary.Location = new Point(371, 16);
            btnLibrary.MinimumSize = new Size(1, 1);
            btnLibrary.Name = "btnLibrary";
            btnLibrary.RectColor = Color.FromArgb(0, 0, 0, 0);
            btnLibrary.Size = new Size(249, 44);
            btnLibrary.TabIndex = 2;
            btnLibrary.Text = "IELTS Exam Library";
            btnLibrary.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // btnAvt
            // 
            btnAvt.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnAvt.Image = Properties.Resources.TTMD;
            btnAvt.Location = new Point(1702, 7);
            btnAvt.Name = "btnAvt";
            btnAvt.Size = new Size(54, 53);
            btnAvt.SizeMode = PictureBoxSizeMode.StretchImage;
            btnAvt.TabIndex = 3;
            btnAvt.TabStop = false;
            btnAvt.Text = null;
            // 
            // btnLogout
            // 
            btnLogout.FillColor = Color.Empty;
            btnLogout.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnLogout.ForeColor = Color.DarkGray;
            btnLogout.ForeHoverColor = Color.FromArgb(64, 64, 64);
            btnLogout.Location = new Point(1771, 16);
            btnLogout.MinimumSize = new Size(1, 1);
            btnLogout.Name = "btnLogout";
            btnLogout.Radius = 30;
            btnLogout.RectColor = Color.FromArgb(255, 128, 128);
            btnLogout.RectHoverColor = Color.Red;
            btnLogout.Size = new Size(112, 40);
            btnLogout.TabIndex = 4;
            btnLogout.Text = "Logout";
            btnLogout.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            btnLogout.Click += btnLogout_Click;
            // 
            // btnCourse
            // 
            btnCourse.FillColor = Color.FromArgb(0, 0, 0, 0);
            btnCourse.Font = new Font("Noto Serif SC", 13.8F, FontStyle.Bold);
            btnCourse.ForeColor = Color.FromArgb(39, 56, 146);
            btnCourse.Location = new Point(690, 16);
            btnCourse.MinimumSize = new Size(1, 1);
            btnCourse.Name = "btnCourse";
            btnCourse.RectColor = Color.FromArgb(0, 0, 0, 0);
            btnCourse.Size = new Size(196, 44);
            btnCourse.TabIndex = 5;
            btnCourse.Text = "IELTS COURSES";
            btnCourse.TipsFont = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point, 163);
            // 
            // UserNavbarPanel
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnCourse);
            Controls.Add(btnLogout);
            Controls.Add(btnAvt);
            Controls.Add(btnLibrary);
            Controls.Add(btnHome);
            Controls.Add(picLogo);
            Name = "UserNavbarPanel";
            Size = new Size(1920, 70);
            Load += UserNavbarPanel_Load;
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnAvt).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox picLogo;
        private Sunny.UI.UIButton btnHome;
        private Sunny.UI.UIButton btnLibrary;
        private Sunny.UI.UIImageButton btnAvt;
        private Sunny.UI.UIButton btnLogout;
        private Sunny.UI.UIButton btnCourse;
    }
}
