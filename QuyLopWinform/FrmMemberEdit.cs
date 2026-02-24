using System;
using System.Windows.Forms;

namespace QuyLopWinform
{
    public partial class FrmMemberEdit : Form
    {
        public string FullNameValue => txtFullName.Text.Trim();
        public string PhoneValue => txtPhone.Text.Trim();
        public string NoteValue => txtNote.Text.Trim();
        public string RoleValue => txtRole.Text.Trim();

        public FrmMemberEdit(string fullName = "", string phone = "", string note = "", string role = "Member")
        {
            InitializeComponent();

            txtFullName.Text = fullName;
            txtPhone.Text = phone;
            txtNote.Text = note;
            txtRole.Text = role;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameValue))
            {
                MessageBox.Show("Vui lòng nhập Họ tên.");
                txtFullName.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FrmMemberEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
