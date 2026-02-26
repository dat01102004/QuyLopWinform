using System;
using System.Windows.Forms;

namespace QuyLopWinform
{
    public partial class FrmExpenseAdd : Form
    {
        public string TitleValue { get; private set; } = "";
        public decimal AmountValue { get; private set; } = 0;
        public DateTime ExpenseDateValue { get; private set; } = DateTime.Today;

        public FrmExpenseAdd()
        {
            InitializeComponent();
            SetupControls();
            Text = "Thêm khoản chi";
        }

        // Dùng cho sửa
        public FrmExpenseAdd(string title, decimal amount, DateTime expenseDate) : this()
        {
            txtTitle.Text = title;
            numAmount.Value = Math.Min(numAmount.Maximum, Math.Max(numAmount.Minimum, amount));
            dtpExpenseDate.Value = expenseDate;
            Text = "Sửa khoản chi";
        }

        private void SetupControls()
        {
            // Chống lỗi nhập 100000 bị thành 100
            numAmount.DecimalPlaces = 0;
            numAmount.Minimum = 0;
            numAmount.Maximum = 1000000000; // 1 tỷ
            numAmount.Increment = 1000;
            numAmount.ThousandsSeparator = true;

            dtpExpenseDate.Format = DateTimePickerFormat.Custom;
            dtpExpenseDate.CustomFormat = "dd/MM/yyyy";
            dtpExpenseDate.Value = DateTime.Today;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var title = (txtTitle.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Vui lòng nhập nội dung chi.");
                txtTitle.Focus();
                return;
            }

            if (numAmount.Value <= 0)
            {
                MessageBox.Show("Số tiền chi phải lớn hơn 0.");
                numAmount.Focus();
                return;
            }

            TitleValue = title;
            AmountValue = numAmount.Value;
            ExpenseDateValue = dtpExpenseDate.Value.Date;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Nếu Designer có gắn event thì để các hàm này tồn tại
        private void txtTitle_TextChanged(object sender, EventArgs e) { }
        private void numAmount_ValueChanged(object sender, EventArgs e) { }
        private void dtpExpenseDate_ValueChanged(object sender, EventArgs e) { }
    }
}