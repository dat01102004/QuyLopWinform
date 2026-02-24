using System;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmMain : Form
    {
        private User _currentUser;
        private int _currentClassId;

        public FrmMain()
        {
            InitializeComponent();
        }

        public FrmMain(User user, int classId) : this()
        {
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
            _currentClassId = classId;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            SetupGrid();
            ReloadAll();
        }

        private void SetupGrid()
        {
            dgvMembers.AutoGenerateColumns = true;
            dgvMembers.ReadOnly = true;
            dgvMembers.AllowUserToAddRows = false;
            dgvMembers.AllowUserToDeleteRows = false;
            dgvMembers.MultiSelect = false;
            dgvMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ReloadAll();
        }

        private void ReloadAll()
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    // Title + class name
                    var cls = db.Classrooms.FirstOrDefault(x => x.ClassId == _currentClassId);
                    var className = cls?.ClassName ?? $"ClassId {_currentClassId}";
                    Text = $"Xin chào: {_currentUser.FullName} | Lớp: {className} (ID: {_currentClassId})";

                    // 1) Load members
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

                    // 2) Summary (TotalIn / TotalOut / Balance)
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

                    // 3) Load combo khoản thu
                    LoadFeeCycles(db);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Đổ danh sách FeeCycles vào cboFeeCycles (khoản thu).
        /// Có format: Title - Amount - trạng thái - hạn nộp
        /// </summary>
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
                    // lấy InvoiceId đi kèm (thường 1 feeCycle có 1 invoice "thu chung")
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

            // Enable/Disable nút Thu Tiền
            btnOpenPayments.Enabled = list.Count > 0;
        }

        private static string FormatVnd(decimal value)
        {
            return string.Format("{0:n0} đ", value);
        }

        private static string FormatVnd(int value)
        {
            return string.Format("{0:n0} đ", value);
        }

        private int? GetSelectedMemberId()
        {
            if (dgvMembers.CurrentRow == null) return null;
            var cell = dgvMembers.CurrentRow.Cells["MemberId"];
            if (cell == null || cell.Value == null) return null;
            return Convert.ToInt32(cell.Value);
        }

        // ===== Member CRUD =====

        private void btnAddMember_Click(object sender, EventArgs e)
        {
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

        // ===== Khoản thu: Tạo khoản thu =====
        private void btnNewFee_Click(object sender, EventArgs e)
        {
            using (var f = new FrmFeeCycleAdd())
            {
                if (f.ShowDialog() != DialogResult.OK) return;

                int feeCycleId;
                int invoiceId;

                using (var db = new DataClasses1DataContext())
                {
                    // 1) FeeCycle (Amount của bạn là INT)
                    var fc = new FeeCycle
                    {
                        ClassId = _currentClassId,
                        Title = f.TitleValue,
                        Amount = Convert.ToInt32(f.AmountValue), // AmountValue nên là int (nudAmount.Value)
                        DueDate = f.UseDueDate ? (DateTime?)f.DueDateValue : null,
                        AllowLate = true,
                        CreatedAt = DateTime.Now
                    };
                    db.FeeCycles.InsertOnSubmit(fc);
                    db.SubmitChanges();
                    feeCycleId = fc.FeeCycleId;

                    // 2) Invoice gắn FeeCycle
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

                // 3) Reload + chọn đúng khoản thu vừa tạo
                ReloadAll();
                cboFeeCycles.SelectedValue = feeCycleId;

                // 4) Mở form tick ai nộp
                using (var pay = new FrmInvoicePayments(_currentClassId, invoiceId))
                {
                    pay.ShowDialog();
                }

                ReloadAll();
            }
        }

        // ===== Khoản thu: Thu tiền (mở form tick ai nộp) =====
        private void btnOpenPayments_Click(object sender, EventArgs e)
        {
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

        // Có thể để trống nếu designer đang gắn event
        private void cboFeeCycles_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dgvMembers_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void grpMembers_Enter(object sender, EventArgs e) { }
        private void grpSummary_Enter(object sender, EventArgs e) { }
        private void lblBalance_Click(object sender, EventArgs e) { }
        private void lblTotalIn_Click(object sender, EventArgs e) { }
        private void lblTotalOut_Click(object sender, EventArgs e) { }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
