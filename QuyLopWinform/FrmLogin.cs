using System;
using System.Linq;
using System.Windows.Forms;
using LopFund.BLL;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmLogin : Form
    {
        private readonly UserBLL _userBll = new UserBLL();
        private readonly ClassroomBLL _classBll = new ClassroomBLL();

        public User LoggedInUser { get; private set; }
        public int? SelectedClassId { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();

            // tránh bị gắn event nhiều lần
            btnLogin.Click -= btnLogin_Click;
            btnLogin.Click += btnLogin_Click;

            btnRegister.Click -= btnRegister_Click;
            btnRegister.Click += btnRegister_Click;
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
        private void FrmLogin_Load(object sender, EventArgs e) { }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            using (var f = new FrmRegister())
            {
                f.ShowDialog();
            }
        }
    }
}
