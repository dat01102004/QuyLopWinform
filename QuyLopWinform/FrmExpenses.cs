using System;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmExpenses : Form
    {
        private readonly int _classId;

        public FrmExpenses(int classId)
        {
            InitializeComponent();
            _classId = classId;
        }

        private void FrmExpenses_Load(object sender, EventArgs e)
        {
            SetupGrid();
            LoadExpenses();
        }

        private void SetupGrid()
        {
            dgvExpenses.AutoGenerateColumns = true;
            dgvExpenses.ReadOnly = true;
            dgvExpenses.AllowUserToAddRows = false;
            dgvExpenses.AllowUserToDeleteRows = false;
            dgvExpenses.MultiSelect = false;
            dgvExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadExpenses()
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var data = db.Expenses
                        .Where(x => x.ClassId == _classId)
                        .OrderByDescending(x => x.SpentAt)
                        .ThenByDescending(x => x.ExpenseId)
                        .Select(x => new
                        {
                            x.ExpenseId,
                            x.Title,
                            x.Amount,
                            x.SpentAt,
                            x.Note
                        })
                        .ToList();

                    dgvExpenses.DataSource = data;

                    if (dgvExpenses.Columns["ExpenseId"] != null)
                        dgvExpenses.Columns["ExpenseId"].Visible = false;

                    if (dgvExpenses.Columns["Note"] != null)
                        dgvExpenses.Columns["Note"].Visible = false; // ẩn note nếu muốn

                    if (dgvExpenses.Columns["Amount"] != null)
                        dgvExpenses.Columns["Amount"].DefaultCellStyle.Format = "N0";

                    // Cột đúng là SpentAt (không phải ExpenseDate)
                    if (dgvExpenses.Columns["SpentAt"] != null)
                        dgvExpenses.Columns["SpentAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải khoản chi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? GetSelectedExpenseId()
        {
            if (dgvExpenses.CurrentRow == null) return null;
            var cell = dgvExpenses.CurrentRow.Cells["ExpenseId"];
            if (cell == null || cell.Value == null) return null;
            return Convert.ToInt32(cell.Value);
        }

        private void btnAddExpense_Click(object sender, EventArgs e)
        {
            using (var f = new FrmExpenseAdd())
            {
                if (f.ShowDialog() != DialogResult.OK) return;

                using (var db = new DataClasses1DataContext())
                {
                    var exp = new Expense
                    {
                        ClassId = _classId,
                        Title = f.TitleValue,
                        Amount = Convert.ToInt32(f.AmountValue),   // DB bạn đang là int
                        SpentAt = f.ExpenseDateValue,              // đúng field DBML
                    };

                    db.Expenses.InsertOnSubmit(exp);
                    db.SubmitChanges();
                }

                LoadExpenses();
            }
        }

        private void btnEditExpense_Click(object sender, EventArgs e)
        {
            var id = GetSelectedExpenseId();
            if (id == null)
            {
                MessageBox.Show("Chọn 1 khoản chi để sửa.");
                return;
            }

            using (var db = new DataClasses1DataContext())
            {
                var exp = db.Expenses.FirstOrDefault(x => x.ExpenseId == id.Value && x.ClassId == _classId);
                if (exp == null)
                {
                    MessageBox.Show("Không tìm thấy khoản chi.");
                    return;
                }

                // Cần FrmExpenseAdd có ctor 4 tham số (mục 2 bên dưới)
                using (var f = new FrmExpenseAdd(
                    exp.Title,
                    Convert.ToDecimal(exp.Amount),
                    exp.SpentAt))
                {
                    if (f.ShowDialog() != DialogResult.OK) return;

                    exp.Title = f.TitleValue;
                    exp.Amount = Convert.ToInt32(f.AmountValue);
                    exp.SpentAt = f.ExpenseDateValue;

                    db.SubmitChanges();
                }
            }

            LoadExpenses();
        }

        private void btnDeleteExpense_Click(object sender, EventArgs e)
        {
            var id = GetSelectedExpenseId();
            if (id == null)
            {
                MessageBox.Show("Chọn 1 khoản chi để xoá.");
                return;
            }

            var confirm = MessageBox.Show(
                "Bạn chắc chắn muốn xoá khoản chi này?",
                "Xác nhận xoá",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var exp = db.Expenses.FirstOrDefault(x => x.ExpenseId == id.Value && x.ClassId == _classId);
                    if (exp == null)
                    {
                        MessageBox.Show("Không tìm thấy khoản chi.");
                        return;
                    }

                    db.Expenses.DeleteOnSubmit(exp);
                    db.SubmitChanges();
                }

                LoadExpenses();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xoá khoản chi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadExpenses();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Nếu Designer đang gắn event này thì giữ lại
        private void dgvExpenses_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}