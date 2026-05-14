using LopFund.BLL;
using LopFund.DAL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuyLopWinform
{
    public partial class FrmLogin : BaseForm
    {
        private readonly UserBLL _userBll = new UserBLL();
        private readonly ClassroomBLL _classBll = new ClassroomBLL();

        public User LoggedInUser { get; private set; }
        public int? SelectedClassId { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();

            SetupLoginUI();

            btnLogin.Click -= btnLogin_Click;
            btnLogin.Click += btnLogin_Click;

            if (lnkRegister != null)
            {
                lnkRegister.LinkClicked -= lnkRegister_LinkClicked;
                lnkRegister.LinkClicked += lnkRegister_LinkClicked;
            }

            this.AcceptButton = btnLogin;
        }

        private void SetupLoginUI()
        {
            // ===== FORM =====
            this.Text = "Đăng nhập";
            this.BackColor = Color.FromArgb(245, 248, 252);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;

            // Ẩn label cũ trong Designer để tránh bị nhầm label1, label2, label3
            HideOldLabels();
            if (textBox1 != null)
            {
                textBox1.Visible = false;
                textBox1.Enabled = false;
            }
            // ===== LEFT BRANDING =====
            Label lblAppTitle = CreateOrGetLabel("uiLblAppTitle");
            lblAppTitle.Text = "Quỹ Lớp";
            lblAppTitle.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.FromArgb(13, 110, 253);
            lblAppTitle.BackColor = Color.Transparent;
            lblAppTitle.AutoSize = true;
            lblAppTitle.Location = new Point(48, 185);
            lblAppTitle.Visible = true;
            lblAppTitle.BringToFront();

            Label lblSlogan = CreateOrGetLabel("uiLblSlogan");
            lblSlogan.Text = "Quản lý thu chi lớp học đơn giản, rõ ràng, nhanh chóng";
            lblSlogan.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblSlogan.ForeColor = Color.FromArgb(55, 65, 81);
            lblSlogan.BackColor = Color.Transparent;
            lblSlogan.AutoSize = true;
            lblSlogan.Location = new Point(52, 250);   // vị trí thay cho dòng cũ
            lblSlogan.Visible = true;
            lblSlogan.BringToFront();

            // ===== RIGHT LOGIN FORM =====
            int labelX = 575;
            int inputX = 650;
            int titleY = 180;
            int emailY = 240;
            int passwordY = 275;
            int buttonY = 325;
            int registerY = 405;

            Label lblLoginTitle = CreateOrGetLabel("uiLblLoginTitle");
            lblLoginTitle.Text = "Đăng Nhập";
            lblLoginTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblLoginTitle.ForeColor = Color.FromArgb(17, 24, 39);
            lblLoginTitle.BackColor = Color.Transparent;
            lblLoginTitle.AutoSize = true;
            lblLoginTitle.Location = new Point(inputX + 5, titleY);
            lblLoginTitle.Visible = true;
            lblLoginTitle.BringToFront();

            Label lblEmailNew = CreateOrGetLabel("uiLblEmail");
            lblEmailNew.Text = "Email";
            lblEmailNew.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblEmailNew.ForeColor = Color.FromArgb(55, 65, 81);
            lblEmailNew.BackColor = Color.Transparent;
            lblEmailNew.AutoSize = true;
            lblEmailNew.Location = new Point(labelX, emailY + 4);
            lblEmailNew.Visible = true;
            lblEmailNew.BringToFront();

            Label lblPasswordNew = CreateOrGetLabel("uiLblPassword");
            lblPasswordNew.Text = "Mật Khẩu";
            lblPasswordNew.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblPasswordNew.ForeColor = Color.FromArgb(55, 65, 81);
            lblPasswordNew.BackColor = Color.Transparent;
            lblPasswordNew.AutoSize = true;
            lblPasswordNew.Location = new Point(labelX, passwordY + 4);
            lblPasswordNew.Visible = true;
            lblPasswordNew.BringToFront();

            // ===== TEXTBOX =====
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Width = 220;
            txtEmail.Height = 28;
            txtEmail.Location = new Point(inputX, emailY);
            txtEmail.BringToFront();

            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Width = 220;
            txtPassword.Height = 28;
            txtPassword.Location = new Point(inputX, passwordY);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.BringToFront();

            // ===== BUTTON LOGIN =====
            btnLogin.Text = "Đăng Nhập";
            btnLogin.BackColor = Color.FromArgb(37, 99, 235);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(29, 78, 216);
            btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 64, 175);
            btnLogin.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnLogin.Width = 125;
            btnLogin.Height = 38;
            btnLogin.Location = new Point(inputX, buttonY);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.BringToFront();

            // ===== REGISTER LINE =====
            if (lblNoAccount != null)
            {
                lblNoAccount.Text = "Chưa có tài khoản?";
                lblNoAccount.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                lblNoAccount.ForeColor = Color.FromArgb(75, 85, 99);
                lblNoAccount.BackColor = Color.Transparent;
                lblNoAccount.AutoSize = true;
                lblNoAccount.Location = new Point(inputX - 70, registerY);
                lblNoAccount.Visible = true;
                lblNoAccount.BringToFront();
            }

            if (lnkRegister != null)
            {
                lnkRegister.Text = "Đăng ký ngay";
                lnkRegister.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
                lnkRegister.BackColor = Color.Transparent;
                lnkRegister.AutoSize = true;

                lnkRegister.LinkColor = Color.FromArgb(37, 99, 235);
                lnkRegister.ActiveLinkColor = Color.FromArgb(30, 64, 175);
                lnkRegister.VisitedLinkColor = Color.FromArgb(37, 99, 235);

                // Xóa gạch chân dưới chữ đăng ký
                lnkRegister.LinkBehavior = LinkBehavior.NeverUnderline;

                lnkRegister.Cursor = Cursors.Hand;
                lnkRegister.TabStop = true;
                lnkRegister.Location = new Point(inputX + 70, registerY);
                lnkRegister.Visible = true;
                lnkRegister.BringToFront();
            }
        }

 private void HideOldLabels()
{
    HideOldLabelsRecursive(this);
}

private void HideOldLabelsRecursive(Control parent)
{
    foreach (Control c in parent.Controls)
    {
        if (c is Label && !c.Name.StartsWith("uiLbl"))
        {
            c.Visible = false;
        }

        if (c.HasChildren)
        {
            HideOldLabelsRecursive(c);
        }
    }
}

        private Label CreateOrGetLabel(string name)
        {
            Control[] found = this.Controls.Find(name, true);

            if (found.Length > 0 && found[0] is Label existingLabel)
            {
                return existingLabel;
            }

            Label label = new Label();
            label.Name = name;
            this.Controls.Add(label);
            return label;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var pass = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show(
                    "Vui lòng nhập Email và Mật khẩu.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                var user = _userBll.Login(email, pass);

                if (user == null)
                {
                    MessageBox.Show(
                        "Sai email hoặc mật khẩu!",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                LoggedInUser = user;

                AppSession.CurrentUserId = user.UserId;
                AppSession.CurrentClassId = 0;

                using (var pick = new FrmClassPicker())
                {
                    var r = pick.ShowDialog();

                    if (r != DialogResult.OK)
                        return;

                    if (!pick.SelectedClassId.HasValue)
                    {
                        MessageBox.Show(
                            "Bạn chưa chọn lớp.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        return;
                    }

                    SelectedClassId = pick.SelectedClassId.Value;
                    AppSession.CurrentClassId = SelectedClassId.Value;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void OpenRegisterForm()
        {
            this.Hide();

            try
            {
                using (var f = new FrmRegister())
                {
                    var result = f.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        txtPassword.Clear();
                        txtEmail.Focus();
                    }
                }
            }
            finally
            {
                this.Show();
                this.Activate();
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenRegisterForm();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            OpenRegisterForm();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblNoAccount_Click(object sender, EventArgs e)
        {
        }
    }
}