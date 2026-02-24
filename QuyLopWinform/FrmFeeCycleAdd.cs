using System;
using System.Windows.Forms;

namespace QuyLopWinform
{
    public partial class FrmFeeCycleAdd : Form
    {
        public string TitleValue => txtTitle.Text.Trim();
        public decimal AmountValue => nudAmount.Value;
        public bool UseDueDate => chkDueDate.Checked;
        public DateTime DueDateValue => dtpDueDate.Value;

        public FrmFeeCycleAdd()
        {
            InitializeComponent();
            dtpDueDate.Enabled = false;
            chkDueDate.CheckedChanged += chkDueDate_CheckedChanged;
        }

        private void chkDueDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpDueDate.Enabled = chkDueDate.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleValue))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề khoản thu.");
                txtTitle.Focus();
                return;
            }
            if (AmountValue <= 0)
            {
                MessageBox.Show("Số tiền phải > 0.");
                nudAmount.Focus();
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
    }
}
