using System;
using System.Windows.Forms;
using LopFund.BLL;

namespace QuyLopWinform
{
    public partial class FrmRegister : Form
    {
        private readonly UserBLL _userBll = new UserBLL();

        public FrmRegister()
        {
            InitializeComponent();

            // mask password
            txtPassword.UseSystemPasswordChar = true;
            txtConfirm.UseSystemPasswordChar = true;

            // gắn event (tránh gắn nhiều lần)
            btnRegister.Click -= btnRegister_Click;
            btnRegister.Click += btnRegister_Click;

            btnCancel.Click -= btnCancel_Click;
            btnCancel.Click += btnCancel_Click;
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