using System;
using System.Drawing;
using System.Windows.Forms;
using LopFund.BLL;
using LopFund.DAL;

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

            btnRegister.Click -= btnRegister_Click;
            btnRegister.Click += btnRegister_Click;

            this.AcceptButton = btnLogin;
        }

        private void SetupLoginUI()
        {
            this.Text = "Đăng nhập";
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;

            btnLogin.BackColor = Color.FromArgb(37, 99, 235);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new Font("Segoe UI Semibold", 10F);
            btnLogin.Height = 38;
            btnLogin.Width = 120;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.UseVisualStyleBackColor = false;

            btnRegister.BackColor = Color.White;
            btnRegister.ForeColor = Color.FromArgb(37, 99, 235);
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
            btnRegister.FlatAppearance.BorderSize = 1;
            btnRegister.Font = new Font("Segoe UI Semibold", 10F);
            btnRegister.Height = 38;
            btnRegister.Width = 110;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.UseVisualStyleBackColor = false;

            txtEmail.Font = new Font("Segoe UI", 10F);
            txtPassword.Font = new Font("Segoe UI", 10F);

            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;

            txtEmail.Width = 220;
            txtPassword.Width = 220;

            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text.Trim();
            var pass = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Vui lòng nhập Email và Mật khẩu.");
                return;
            }

            try
            {
                var user = _userBll.Login(email, pass);
                if (user == null)
                {
                    MessageBox.Show("Sai email hoặc mật khẩu!");
                    return;
                }

                LoggedInUser = user;

                // 1) set user session
                AppSession.CurrentUserId = user.UserId;
                AppSession.CurrentClassId = 0;

                // 2) luôn mở form chọn lớp
                using (var pick = new FrmClassPicker())
                {
                    var r = pick.ShowDialog();
                    if (r != DialogResult.OK)
                        return;

                    // FrmClassPicker sẽ set SelectedClassId + AppSession.CurrentClassId
                    if (!pick.SelectedClassId.HasValue)
                    {
                        MessageBox.Show("Bạn chưa chọn lớp.");
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
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
        }

        private void btnRegister_Click(object sender, EventArgs e)
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
    }
}