namespace IELTS.UI.User.TestTaking.Controls
{
    partial class TestNavBarPanel
    {
        private System.ComponentModel.IContainer components = null;

        private Button btnExit;
        private Button btnSubmit;
        private Label lblTimer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnExit = new Button();
            btnSubmit = new Button();
            lblTimer = new Label();
            SuspendLayout();
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.White;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnExit.Location = new Point(1543, 20);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(120, 40);
            btnExit.TabIndex = 0;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.DodgerBlue;
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Location = new Point(1700, 20);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(150, 40);
            btnSubmit.TabIndex = 1;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTimer.ForeColor = Color.ForestGreen;
            lblTimer.Location = new Point(800, 25);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(249, 28);
            lblTimer.TabIndex = 2;
            lblTimer.Text = "00:00 minutes remaining";
            // 
            // TestNavBarPanel
            // 
            Controls.Add(btnExit);
            Controls.Add(btnSubmit);
            Controls.Add(lblTimer);
            Name = "TestNavBarPanel";
            Size = new Size(1920, 80);
            Load += TestNavBarPanel_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
