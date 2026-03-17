using System;
using System.Drawing;
using System.Windows.Forms;
using LopFund.BLL;

namespace QuyLopWinform
{
    public partial class FrmRegister : BaseForm
    {
        private readonly UserBLL _userBll = new UserBLL();

        public FrmRegister()
        {
            InitializeComponent();

            SetupRegisterUI();

            // mask password
            txtPassword.UseSystemPasswordChar = true;
            txtConfirm.UseSystemPasswordChar = true;

            // gắn event (tránh gắn nhiều lần)
            btnRegister.Click -= btnRegister_Click;
            btnRegister.Click += btnRegister_Click;

            btnCancel.Click -= btnCancel_Click;
            btnCancel.Click += btnCancel_Click;

            this.AcceptButton = btnRegister;
            this.CancelButton = btnCancel;
        }

        private void SetupRegisterUI()
        {
            this.Text = "Đăng ký";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            btnRegister.BackColor = Color.FromArgb(37, 99, 235);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Font = new Font("Segoe UI Semibold", 10F);
            btnRegister.Height = 38;
            btnRegister.Width = 120;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.UseVisualStyleBackColor = false;

            btnCancel.BackColor = Color.White;
            btnCancel.ForeColor = Color.FromArgb(37, 99, 235);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.Font = new Font("Segoe UI Semibold", 10F);
            btnCancel.Height = 38;
            btnCancel.Width = 90;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.UseVisualStyleBackColor = false;

            txtFullName.Font = new Font("Segoe UI", 10F);
            txtEmail.Font = new Font("Segoe UI", 10F);
            txtPhone.Font = new Font("Segoe UI", 10F);
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtConfirm.Font = new Font("Segoe UI", 10F);

            txtFullName.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirm.BorderStyle = BorderStyle.FixedSingle;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var fullName = (txtFullName.Text ?? "").Trim();
            var email = (txtEmail.Text ?? "").Trim();
            var phone = (txtPhone.Text ?? "").Trim();
            var pass = txtPassword.Text ?? "";
            var confirm = txtConfirm.Text ?? "";

            // validate
            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ tên.");
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Vui lòng nhập email.");
                txtEmail.Focus();
                return;
            }

            if (pass.Length < 6)
            {
                MessageBox.Show("Mật khẩu tối thiểu 6 ký tự.");
                txtPassword.Focus();
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Nhập lại mật khẩu không khớp.");
                txtConfirm.Focus();
                return;
            }

            try
            {
                _userBll.Register(fullName, email, phone, pass);

                MessageBox.Show("Đăng ký thành công! Hãy đăng nhập.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtFullName_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtConfirm_TextChanged(object sender, EventArgs e)
        {
        }
    }
}