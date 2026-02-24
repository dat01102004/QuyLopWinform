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

                // Lấy danh sách lớp theo OwnerUserId
                var myClasses = _classBll.GetAll()
                                         .Where(c => c.OwnerUserId == user.UserId)
                                         .OrderByDescending(c => c.ClassId)
                                         .ToList();

                // 1) Chưa có lớp => bắt buộc tạo lớp mới
                if (myClasses.Count == 0)
                {
                    using (var f = new FrmClassrooms(user.UserId, onboardingCreateFirst: true, selectOnly: false))
                    {
                        var r = f.ShowDialog();
                        if (r != DialogResult.OK || !f.SelectedClassId.HasValue)
                            return; // user đóng form / chưa tạo

                        SelectedClassId = f.SelectedClassId.Value;
                    }
                }
                // 2) Có đúng 1 lớp => vào thẳng
                else if (myClasses.Count == 1)
                {
                    SelectedClassId = myClasses[0].ClassId;
                }
                // 3) Có nhiều lớp => chọn lớp (double click)
                else
                {
                    using (var f = new FrmClassrooms(user.UserId, onboardingCreateFirst: false, selectOnly: true))
                    {
                        var r = f.ShowDialog();
                        if (r != DialogResult.OK || !f.SelectedClassId.HasValue)
                            return;

                        SelectedClassId = f.SelectedClassId.Value;
                    }
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

    }
}
