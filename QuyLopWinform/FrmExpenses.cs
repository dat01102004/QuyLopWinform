using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmExpenses : BaseForm
    {
        private readonly int _classId;

        private Label lblHeaderTitle;
        private Label lblHeaderSubTitle;
        private GroupBox grpExpenses;

        public FrmExpenses(int classId)
        {
            InitializeComponent();
            _classId = classId;

            BuildExtraUI();
            SetupExpenseListUI();
            WireExtraEvents();
        }

        private void BuildExtraUI()
        {
            lblHeaderTitle = new Label();
            lblHeaderSubTitle = new Label();
            grpExpenses = new GroupBox();

            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Text = "Quản lý khoản chi";
            lblHeaderTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.BackColor = Color.Transparent;

            lblHeaderSubTitle.AutoSize = true;
            lblHeaderSubTitle.Text = "Theo dõi, thêm, sửa và xóa các khoản chi của lớp";
            lblHeaderSubTitle.Font = new Font("Segoe UI", 9.5F);
            lblHeaderSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblHeaderSubTitle.BackColor = Color.Transparent;

            grpExpenses.Text = "Danh sách khoản chi";
            grpExpenses.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            grpExpenses.ForeColor = Color.FromArgb(31, 41, 55);
            grpExpenses.BackColor = Color.White;
            grpExpenses.Padding = new Padding(10);

            this.Controls.Add(lblHeaderTitle);
            this.Controls.Add(lblHeaderSubTitle);
            this.Controls.Add(grpExpenses);

            lblHeaderTitle.BringToFront();
            lblHeaderSubTitle.BringToFront();
        }

        private void SetupExpenseListUI()
        {
            this.Text = "Quản lý khoản chi";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(980, 620);

            StylePrimaryButton(btnAddExpense, "Thêm");
            StyleSecondaryButton(btnEditExpense, "Sửa");
            StyleDangerButton(btnDeleteExpense, "Xóa");
            StyleSecondaryButton(btnRefresh, "Tải lại");
            StyleSecondaryButton(btnClose, "Đóng");
        }

        private void WireExtraEvents()
        {
            this.Shown += FrmExpenses_Shown;
            this.Resize += FrmExpenses_Resize;
        }

        private void FrmExpenses_Shown(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmExpenses_Resize(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            int margin = 22;

            lblHeaderTitle.Location = new Point(margin, 18);
            lblHeaderSubTitle.Location = new Point(margin, 50);

            grpExpenses.Location = new Point(margin, 84);
            grpExpenses.Size = new Size(this.ClientSize.Width - margin * 2, 460);

            EnsureParent(dgvExpenses, grpExpenses);
            dgvExpenses.Location = new Point(16, 34);
            dgvExpenses.Size = new Size(grpExpenses.ClientSize.Width - 32, grpExpenses.ClientSize.Height - 50);

            btnAddExpense.Size = new Size(110, 40);
            btnEditExpense.Size = new Size(110, 40);
            btnDeleteExpense.Size = new Size(110, 40);
            btnRefresh.Size = new Size(120, 40);
            btnClose.Size = new Size(110, 40);

            int buttonY = grpExpenses.Bottom + 18;
            int gap = 14;

            btnAddExpense.Location = new Point(margin, buttonY);
            btnEditExpense.Location = new Point(btnAddExpense.Right + gap, buttonY);
            btnDeleteExpense.Location = new Point(btnEditExpense.Right + gap, buttonY);
            btnRefresh.Location = new Point(btnDeleteExpense.Right + gap, buttonY);
            btnClose.Location = new Point(this.ClientSize.Width - margin - btnClose.Width, buttonY);
        }

        private void EnsureParent(Control child, Control newParent)
        {
            if (child == null || newParent == null) return;
            if (child.Parent == newParent) return;

            Control oldParent = child.Parent;
            if (oldParent != null)
                oldParent.Controls.Remove(child);

            newParent.Controls.Add(child);
        }

        private void StylePrimaryButton(Button btn, string text)
        {
            btn.Text = text;
            btn.BackColor = Color.FromArgb(37, 99, 235);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI Semibold", 10F);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        private void StyleSecondaryButton(Button btn, string text)
        {
            btn.Text = text;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(37, 99, 235);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(37, 99, 235);
            btn.FlatAppearance.BorderSize = 1;
            btn.Font = new Font("Segoe UI Semibold", 10F);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        private void StyleDangerButton(Button btn, string text)
        {
            btn.Text = text;
            btn.BackColor = Color.FromArgb(220, 38, 38);
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI Semibold", 10F);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
        }

        private void FrmExpenses_Load(object sender, EventArgs e)
        {
            SetupGrid();
            ArrangeLayout();
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
            dgvExpenses.RowHeadersVisible = false;
            dgvExpenses.BackgroundColor = Color.White;
            dgvExpenses.BorderStyle = BorderStyle.FixedSingle;
            dgvExpenses.EnableHeadersVisualStyles = false;
            dgvExpenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvExpenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvExpenses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvExpenses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvExpenses.ColumnHeadersHeight = 36;
            dgvExpenses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvExpenses.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dgvExpenses.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvExpenses.RowTemplate.Height = 30;
            dgvExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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

                    if (dgvExpenses.Columns["Title"] != null)
                    {
                        dgvExpenses.Columns["Title"].HeaderText = "Nội dung chi";
                        dgvExpenses.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvExpenses.Columns["Title"].FillWeight = 45;
                    }

                    if (dgvExpenses.Columns["Amount"] != null)
                    {
                        dgvExpenses.Columns["Amount"].HeaderText = "Số tiền";
                        dgvExpenses.Columns["Amount"].DefaultCellStyle.Format = "N0";
                        dgvExpenses.Columns["Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvExpenses.Columns["Amount"].FillWeight = 25;
                        dgvExpenses.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvExpenses.Columns["Amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    if (dgvExpenses.Columns["SpentAt"] != null)
                    {
                        dgvExpenses.Columns["SpentAt"].HeaderText = "Ngày chi";
                        dgvExpenses.Columns["SpentAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
                        dgvExpenses.Columns["SpentAt"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvExpenses.Columns["SpentAt"].FillWeight = 30;
                        dgvExpenses.Columns["SpentAt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvExpenses.Columns["SpentAt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
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
                        Amount = Convert.ToInt32(f.AmountValue),
                        SpentAt = f.ExpenseDateValue,
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