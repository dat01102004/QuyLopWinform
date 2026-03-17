using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmMain : BaseForm
    {
        private User _currentUser;
        private int _currentClassId;

        private Label lblBalanceCaption;
        private Label lblTotalInCaption;
        private Label lblTotalOutCaption;
        private Label lblHeaderTitle;
        private Label lblHeaderSubTitle;

        public FrmMain()
        {
            InitializeComponent();
            BuildExtraUI();
            SetupMainUI();
        }

        public FrmMain(User user, int classId) : this()
        {
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _currentClassId = classId;

            AppSession.CurrentUserId = user.UserId;
            AppSession.CurrentClassId = classId;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            SetupGrid();
            SyncSessionToLocal();
            ArrangeMainLayout();
            ReloadAll();
        }

        private void BuildExtraUI()
        {
            lblHeaderTitle = new Label();
            lblHeaderSubTitle = new Label();

            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Text = "Quản lý quỹ lớp";
            lblHeaderTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.BackColor = Color.Transparent;

            lblHeaderSubTitle.AutoSize = true;
            lblHeaderSubTitle.Text = "Theo dõi thành viên, khoản thu, khoản chi và số dư lớp học";
            lblHeaderSubTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblHeaderSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblHeaderSubTitle.BackColor = Color.Transparent;

            this.Controls.Add(lblHeaderTitle);
            this.Controls.Add(lblHeaderSubTitle);

            lblHeaderTitle.BringToFront();
            lblHeaderSubTitle.BringToFront();

            lblBalanceCaption = new Label();
            lblTotalInCaption = new Label();
            lblTotalOutCaption = new Label();

            lblBalanceCaption.Text = "Số dư hiện tại";
            lblTotalInCaption.Text = "Tổng thu";
            lblTotalOutCaption.Text = "Tổng chi";

            InitCaptionLabel(lblBalanceCaption);
            InitCaptionLabel(lblTotalInCaption);
            InitCaptionLabel(lblTotalOutCaption);

            grpSummary.Controls.Add(lblBalanceCaption);
            grpSummary.Controls.Add(lblTotalInCaption);
            grpSummary.Controls.Add(lblTotalOutCaption);

            lblBalanceCaption.BringToFront();
            lblTotalInCaption.BringToFront();
            lblTotalOutCaption.BringToFront();
        }

        private void InitCaptionLabel(Label lbl)
        {
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(100, 116, 139);
            lbl.BackColor = Color.Transparent;
        }

        private void SetupMainUI()
        {
            this.Text = "Quản lý quỹ lớp";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(1180, 680);

            StyleGroupBox(grpSummary, "Tổng quan");
            StyleGroupBox(groupBox1, "Khoản thu");
            StyleGroupBox(groupBox2, "Khoản chi");
            StyleGroupBox(grpMembers, "Thành viên lớp");

            StylePrimaryButton(btnNewFee, "Tạo khoản thu");
            StylePrimaryButton(btnOpenPayments, "Thu tiền");
            StylePrimaryButton(btnAddExpense, "Chi tiền");
            StyleSecondaryButton(btnManageExpenses, "QL khoản chi");
            StyleSecondaryButton(btnChangeClass, "Đổi lớp");
            StyleSecondaryButton(btnLogout, "Đăng xuất");
            StylePrimaryButton(btnAddMember, "Thêm");
            StyleSecondaryButton(btnEditMember, "Sửa");
            StyleDangerButton(btnDeleteMember, "Xóa");

            StyleComboBox(cboFeeCycles);

            lblBalance.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold);
            lblBalance.ForeColor = Color.FromArgb(37, 99, 235);

            lblTotalIn.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTotalIn.ForeColor = Color.FromArgb(22, 163, 74);

            lblTotalOut.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTotalOut.ForeColor = Color.FromArgb(220, 38, 38);

            HideLabelsContaining("Tổng Thu", "Tổng Chi", "Số dư");
        }

        private void ArrangeMainLayout()
        {
            int margin = 22;
            int gap = 22;
            int leftWidth = 340;
            int top = 72;

            lblHeaderTitle.Location = new Point(margin, 18);
            lblHeaderSubTitle.Location = new Point(margin, 50);

            grpSummary.Location = new Point(margin, top);
            grpSummary.Size = new Size(leftWidth, 200);

            int middleX = grpSummary.Right + 18;
            btnChangeClass.Size = new Size(120, 38);
            btnChangeClass.Location = new Point(middleX, top + 12);

            int rightX = middleX + btnChangeClass.Width + 18;
            int rightWidth = this.ClientSize.Width - rightX - margin;

            grpMembers.Location = new Point(rightX, top);
            grpMembers.Size = new Size(rightWidth, 520);

            if (dgvMembers.Parent != grpMembers)
            {
                EnsureParent(dgvMembers, grpMembers);
            }

            dgvMembers.Location = new Point(16, 36);
            dgvMembers.Size = new Size(grpMembers.ClientSize.Width - 32, grpMembers.ClientSize.Height - 110);

            btnAddMember.Size = new Size(120, 40);
            btnEditMember.Size = new Size(120, 40);
            btnDeleteMember.Size = new Size(120, 40);

            int memberBtnY = grpMembers.Bottom + 16;
            btnAddMember.Location = new Point(rightX, memberBtnY);
            btnEditMember.Location = new Point(btnAddMember.Right + 14, memberBtnY);
            btnDeleteMember.Location = new Point(btnEditMember.Right + 14, memberBtnY);

            groupBox1.Location = new Point(margin, grpSummary.Bottom + 22);
            groupBox1.Size = new Size(leftWidth, 150);

            groupBox2.Location = new Point(margin, groupBox1.Bottom + 18);
            groupBox2.Size = new Size(leftWidth, 110);

            btnLogout.Size = new Size(120, 40);
            btnLogout.Location = new Point(margin, groupBox2.Bottom + 22);

            ArrangeSummaryContent();
            ArrangeIncomeContent();
            ArrangeExpenseContent();
        }

        private void ArrangeSummaryContent()
        {
            EnsureParent(lblBalance, grpSummary);
            EnsureParent(lblTotalIn, grpSummary);
            EnsureParent(lblTotalOut, grpSummary);

            lblBalanceCaption.Location = new Point(20, 34);
            lblBalance.Location = new Point(20, 58);

            // Tổng thu xuống 1 dòng riêng
            lblTotalInCaption.Location = new Point(20, 128);
            lblTotalIn.Location = new Point(92, 124);

            // Tổng chi xuống dòng riêng bên dưới
            lblTotalOutCaption.Location = new Point(20, 158);
            lblTotalOut.Location = new Point(92, 154);
        }

        private void ArrangeIncomeContent()
        {
            EnsureParent(cboFeeCycles, groupBox1);
            EnsureParent(btnNewFee, groupBox1);
            EnsureParent(btnOpenPayments, groupBox1);

            cboFeeCycles.Location = new Point(18, 42);
            cboFeeCycles.Size = new Size(groupBox1.ClientSize.Width - 36, 32);

            btnNewFee.Size = new Size(130, 38);
            btnOpenPayments.Size = new Size(120, 38);

            btnNewFee.Location = new Point(18, 92);
            btnOpenPayments.Location = new Point(groupBox1.ClientSize.Width - btnOpenPayments.Width - 18, 92);
        }

        private void ArrangeExpenseContent()
        {
            EnsureParent(btnAddExpense, groupBox2);
            EnsureParent(btnManageExpenses, groupBox2);

            btnAddExpense.Size = new Size(120, 38);
            btnManageExpenses.Size = new Size(145, 38);

            btnAddExpense.Location = new Point(18, 42);
            btnManageExpenses.Location = new Point(groupBox2.ClientSize.Width - btnManageExpenses.Width - 18, 42);
        }

        private void EnsureParent(Control child, Control newParent)
        {
            if (child == null || newParent == null) return;
            if (child.Parent == newParent) return;

            Control oldParent = child.Parent;
            if (oldParent != null)
            {
                oldParent.Controls.Remove(child);
            }

            newParent.Controls.Add(child);
        }

        private void HideLabelsContaining(params string[] keys)
        {
            foreach (var lbl in GetAllControls(this).OfType<Label>())
            {
                if (lbl == lblHeaderTitle || lbl == lblHeaderSubTitle ||
                    lbl == lblBalanceCaption || lbl == lblTotalInCaption || lbl == lblTotalOutCaption)
                    continue;

                string text = (lbl.Text ?? string.Empty).Trim();
                if (keys.Any(k => text.IndexOf(k, StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    lbl.Visible = false;
                }
            }
        }

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                foreach (Control child in GetAllControls(c))
                    yield return child;

                yield return c;
            }
        }

        private void StyleGroupBox(GroupBox group, string text)
        {
            group.Text = text;
            group.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            group.ForeColor = Color.FromArgb(31, 41, 55);
            group.BackColor = Color.White;
            group.Padding = new Padding(10);
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

        private void StyleComboBox(ComboBox cbo)
        {
            cbo.Font = new Font("Segoe UI", 10F);
            cbo.BackColor = Color.White;
            cbo.ForeColor = Color.FromArgb(31, 41, 55);
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SyncSessionToLocal()
        {
            _currentClassId = AppSession.CurrentClassId;

            if (_currentUser == null && AppSession.CurrentUserId != 0)
            {
                using (var db = new DataClasses1DataContext())
                {
                    _currentUser = db.Users.FirstOrDefault(u => u.UserId == AppSession.CurrentUserId);
                }
            }
        }

        private string GetMyRoleInCurrentClass()
        {
            using (var db = new DataClasses1DataContext())
            {
                var role = db.UserClassrooms
                    .Where(x => x.UserId == AppSession.CurrentUserId
                             && x.ClassId == AppSession.CurrentClassId
                             && x.IsActive)
                    .Select(x => x.Role)
                    .FirstOrDefault();

                return string.IsNullOrWhiteSpace(role) ? "Member" : role;
            }
        }

        private bool CanManage(string role)
        {
            return role == "Owner" || role == "Admin";
        }

        private bool EnsureCanManage()
        {
            var role = GetMyRoleInCurrentClass();
            if (!CanManage(role))
            {
                MessageBox.Show("Bạn không có quyền thao tác trong lớp này (chỉ Owner/Admin).");
                return false;
            }
            return true;
        }

        private void ApplyPermissions()
        {
            var role = GetMyRoleInCurrentClass();
            var canManage = CanManage(role);

            btnNewFee.Enabled = canManage;
            btnOpenPayments.Enabled = canManage;
            btnAddExpense.Enabled = canManage;
            btnManageExpenses.Enabled = canManage;

            btnAddMember.Enabled = canManage;
            btnEditMember.Enabled = canManage;
            btnDeleteMember.Enabled = canManage;

            btnChangeClass.Enabled = true;
        }

        private void SetupGrid()
        {
            dgvMembers.AutoGenerateColumns = true;
            dgvMembers.ReadOnly = true;
            dgvMembers.AllowUserToAddRows = false;
            dgvMembers.AllowUserToDeleteRows = false;
            dgvMembers.MultiSelect = false;
            dgvMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMembers.RowHeadersVisible = false;
            dgvMembers.BackgroundColor = Color.White;
            dgvMembers.BorderStyle = BorderStyle.FixedSingle;
            dgvMembers.EnableHeadersVisualStyles = false;
            dgvMembers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvMembers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvMembers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMembers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvMembers.ColumnHeadersHeight = 36;
            dgvMembers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvMembers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dgvMembers.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvMembers.RowTemplate.Height = 30;
            dgvMembers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReloadAll();
        }

        private void ReloadAll()
        {
            try
            {
                SyncSessionToLocal();

                if (_currentUser == null)
                    throw new Exception("Không tìm thấy thông tin user hiện tại.");

                ApplyPermissions();

                using (var db = new DataClasses1DataContext())
                {
                    var cls = db.Classrooms.FirstOrDefault(x => x.ClassId == _currentClassId);
                    var className = cls?.ClassName ?? $"ClassId {_currentClassId}";
                    var role = GetMyRoleInCurrentClass();
                    Text = $"Xin chào: {_currentUser.FullName} | Lớp: {className} (ID: {_currentClassId}) | Role: {role}";

                    var members = db.ClassMembers
                        .Where(m => m.ClassId == _currentClassId && m.IsActive == true)
                        .OrderBy(m => m.FullName)
                        .Select(m => new
                        {
                            m.MemberId,
                            m.FullName,
                            m.Phone,
                            Role = m.MemberRole,
                            m.JoinedAt,
                            m.Note
                        })
                        .ToList();

                    dgvMembers.DataSource = members;

                    if (dgvMembers.Columns["MemberId"] != null) dgvMembers.Columns["MemberId"].Visible = false;
                    if (dgvMembers.Columns["Note"] != null) dgvMembers.Columns["Note"].Visible = false;

                    decimal totalIn =
                        (from p in db.InvoicePayments
                         join inv in db.Invoices on p.InvoiceId equals inv.InvoiceId
                         join fc in db.FeeCycles on inv.FeeCycleId equals fc.FeeCycleId
                         where fc.ClassId == _currentClassId
                         select (decimal?)p.AmountPaid).Sum() ?? 0m;

                    decimal totalOut = db.Expenses
                        .Where(x => x.ClassId == _currentClassId)
                        .Select(x => (decimal?)x.Amount)
                        .Sum() ?? 0m;

                    decimal balance = totalIn - totalOut;

                    lblTotalIn.Text = FormatVnd(totalIn);
                    lblTotalOut.Text = FormatVnd(totalOut);
                    lblBalance.Text = FormatVnd(balance);

                    LoadFeeCycles(db);
                }

                ArrangeMainLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFeeCycles(DataClasses1DataContext db)
        {
            var list = db.FeeCycles
                .Where(x => x.ClassId == _currentClassId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    x.FeeCycleId,
                    x.Title,
                    x.Amount,
                    x.DueDate,
                    x.AllowLate,
                    InvoiceId = db.Invoices.Where(i => i.FeeCycleId == x.FeeCycleId)
                                           .Select(i => (int?)i.InvoiceId)
                                           .FirstOrDefault()
                })
                .ToList()
                .Select(x => new
                {
                    x.FeeCycleId,
                    x.InvoiceId,
                    Display = $"{x.Title} - {FormatVnd(x.Amount)}"
                              + (x.DueDate.HasValue ? $" | Hạn: {x.DueDate:dd/MM/yyyy}" : "")
                })
                .ToList();

            cboFeeCycles.DisplayMember = "Display";
            cboFeeCycles.ValueMember = "FeeCycleId";
            cboFeeCycles.DataSource = list;

            btnOpenPayments.Enabled = list.Count > 0 && btnOpenPayments.Enabled;
        }

        private static string FormatVnd(decimal value) => string.Format("{0:n0} đ", value);
        private static string FormatVnd(int value) => string.Format("{0:n0} đ", value);

        private int? GetSelectedMemberId()
        {
            if (dgvMembers.CurrentRow == null) return null;
            var cell = dgvMembers.CurrentRow.Cells["MemberId"];
            if (cell == null || cell.Value == null) return null;
            return Convert.ToInt32(cell.Value);
        }

        private void btnAddMember_Click(object sender, EventArgs e)
        {
            if (!EnsureCanManage()) return;

            using (var f = new FrmMemberEdit())
            {
                if (f.ShowDialog() != DialogResult.OK) return;

                using (var db = new DataClasses1DataContext())
                {
                    var m = new ClassMember
                    {
                        ClassId = _currentClassId,
                        FullName = f.FullNameValue,
                        Phone = f.PhoneValue,
                        Note = f.NoteValue,
                        JoinedAt = DateTime.Now,
                        MemberRole = string.IsNullOrWhiteSpace(f.RoleValue) ? "Member" : f.RoleValue,
                        IsActive = true
                    };

                    db.ClassMembers.InsertOnSubmit(m);
                    db.SubmitChanges();
                }

                ReloadAll();
            }
        }

        private void btnEditMember_Click(object sender, EventArgs e)
        {
            if (!EnsureCanManage()) return;

            var id = GetSelectedMemberId();
            if (id == null)
            {
                MessageBox.Show("Chọn 1 thành viên để sửa.");
                return;
            }

            using (var db = new DataClasses1DataContext())
            {
                var entity = db.ClassMembers.FirstOrDefault(x => x.MemberId == id.Value && x.ClassId == _currentClassId);
                if (entity == null)
                {
                    MessageBox.Show("Không tìm thấy thành viên.");
                    return;
                }

                using (var f = new FrmMemberEdit(entity.FullName, entity.Phone, entity.Note, entity.MemberRole))
                {
                    if (f.ShowDialog() != DialogResult.OK) return;

                    entity.FullName = f.FullNameValue;
                    entity.Phone = f.PhoneValue;
                    entity.Note = f.NoteValue;
                    entity.MemberRole = string.IsNullOrWhiteSpace(f.RoleValue) ? entity.MemberRole : f.RoleValue;

                    db.SubmitChanges();
                }
            }

            ReloadAll();
        }

        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            if (!EnsureCanManage()) return;

            var id = GetSelectedMemberId();
            if (id == null)
            {
                MessageBox.Show("Chọn 1 thành viên để xoá.");
                return;
            }

            var confirm = MessageBox.Show("Bạn chắc chắn muốn xoá (ẩn) thành viên này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            using (var db = new DataClasses1DataContext())
            {
                var entity = db.ClassMembers.FirstOrDefault(x => x.MemberId == id.Value && x.ClassId == _currentClassId);
                if (entity == null)
                {
                    MessageBox.Show("Không tìm thấy thành viên.");
                    return;
                }

                entity.IsActive = false;
                db.SubmitChanges();
            }

            ReloadAll();
        }

        private void btnNewFee_Click(object sender, EventArgs e)
        {
            if (!EnsureCanManage()) return;

            using (var f = new FrmFeeCycleAdd())
            {
                if (f.ShowDialog() != DialogResult.OK) return;

                int feeCycleId;
                int invoiceId;

                using (var db = new DataClasses1DataContext())
                {
                    var fc = new FeeCycle
                    {
                        ClassId = _currentClassId,
                        Title = f.TitleValue,
                        Amount = Convert.ToInt32(f.AmountValue),
                        DueDate = f.UseDueDate ? (DateTime?)f.DueDateValue : null,
                        AllowLate = true,
                        CreatedAt = DateTime.Now
                    };
                    db.FeeCycles.InsertOnSubmit(fc);
                    db.SubmitChanges();
                    feeCycleId = fc.FeeCycleId;

                    var inv = new Invoice
                    {
                        FeeCycleId = feeCycleId,
                        UserId = _currentUser.UserId,
                        Amount = Convert.ToInt32(f.AmountValue),
                        Status = "Open"
                    };
                    db.Invoices.InsertOnSubmit(inv);
                    db.SubmitChanges();
                    invoiceId = inv.InvoiceId;
                }

                ReloadAll();
                cboFeeCycles.SelectedValue = feeCycleId;

                using (var pay = new FrmInvoicePayments(_currentClassId, invoiceId))
                {
                    pay.ShowDialog();
                }

                ReloadAll();
            }
        }

        private void btnOpenPayments_Click(object sender, EventArgs e)
        {
            if (!EnsureCanManage()) return;

            if (cboFeeCycles.SelectedValue == null)
            {
                MessageBox.Show("Chọn 1 khoản thu trước.");
                return;
            }

            int feeCycleId = Convert.ToInt32(cboFeeCycles.SelectedValue);
            int invoiceId;

            using (var db = new DataClasses1DataContext())
            {
                invoiceId = db.Invoices
                    .Where(i => i.FeeCycleId == feeCycleId)
                    .Select(i => i.InvoiceId)
                    .FirstOrDefault();
            }

            if (invoiceId == 0)
            {
                MessageBox.Show("Khoản thu này chưa có Invoice.");
                return;
            }

            using (var pay = new FrmInvoicePayments(_currentClassId, invoiceId))
            {
                pay.ShowDialog();
            }

            ReloadAll();
        }

        private void btnAddExpense_Click(object sender, EventArgs e)
        {
            if (!EnsureCanManage()) return;

            using (var f = new FrmExpenseAdd())
            {
                if (f.ShowDialog() != DialogResult.OK) return;

                using (var db = new DataClasses1DataContext())
                {
                    var exp = new Expense
                    {
                        ClassId = _currentClassId,
                        Title = f.TitleValue,
                        Amount = Convert.ToInt32(f.AmountValue),
                        SpentAt = f.ExpenseDateValue,
                    };

                    db.Expenses.InsertOnSubmit(exp);
                    db.SubmitChanges();
                }

                ReloadAll();
            }
        }

        private void btnManageExpenses_Click(object sender, EventArgs e)
        {
            if (!EnsureCanManage()) return;

            using (var f = new FrmExpenses(_currentClassId))
            {
                f.ShowDialog();
            }

            ReloadAll();
        }

        private void btnChangeClass_Click(object sender, EventArgs e)
        {
            using (var f = new FrmClassPicker())
            {
                if (f.ShowDialog() != DialogResult.OK) return;
                if (!f.SelectedClassId.HasValue) return;

                AppSession.CurrentClassId = f.SelectedClassId.Value;
            }

            ReloadAll();
        }

        private void cboFeeCycles_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dgvMembers_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void grpMembers_Enter(object sender, EventArgs e) { }
        private void grpSummary_Enter(object sender, EventArgs e) { }
        private void lblBalance_Click(object sender, EventArgs e) { }
        private void lblTotalIn_Click(object sender, EventArgs e) { }
        private void lblTotalOut_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) { }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var ok = MessageBox.Show("Bạn muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ok != DialogResult.Yes) return;

            AppSession.Clear();

            Hide();

            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    _currentUser = null;
                    _currentClassId = 0;

                    SyncSessionToLocal();
                    ReloadAll();

                    Show();
                    return;
                }
            }

            Close();
        }
    }
}