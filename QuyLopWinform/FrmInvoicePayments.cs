using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmInvoicePayments : Form
    {
        private readonly int _classId;
        private readonly int _invoiceId;

        private BindingList<PayRow> _rows = new BindingList<PayRow>();

        // giữ lại để update summary realtime
        private decimal _expectedEach = 0m;
        private int _memberCount = 0;

        public FrmInvoicePayments(int classId, int invoiceId)
        {
            InitializeComponent();

            _classId = classId;
            _invoiceId = invoiceId;

            SetupGrid();

            // Gắn event bằng code để khỏi lệ thuộc Designer
            dgvPay.CurrentCellDirtyStateChanged += dgvPay_CurrentCellDirtyStateChanged;
            dgvPay.CellValueChanged += dgvPay_CellValueChanged;
            dgvPay.DataError += dgvPay_DataError;
        }

        private void FrmInvoicePayments_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SetupGrid()
        {
            dgvPay.AutoGenerateColumns = true;
            dgvPay.ReadOnly = false;
            dgvPay.AllowUserToAddRows = false;
            dgvPay.AllowUserToDeleteRows = false;
            dgvPay.MultiSelect = false;
            dgvPay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadData()
        {
            using (var db = new DataClasses1DataContext())
            {
                var inv = db.Invoices.First(x => x.InvoiceId == _invoiceId);
                var fc = db.FeeCycles.First(x => x.FeeCycleId == inv.FeeCycleId);

                _expectedEach = Convert.ToDecimal(fc.Amount);

                lblTitle.Text = fc.Title;
                lblExpectedEach.Text = string.Format("{0:N0} đ", _expectedEach);

                var members = db.ClassMembers
                    .Where(m => m.ClassId == _classId && m.IsActive == true)
                    .OrderBy(m => m.FullName)
                    .ToList();

                _memberCount = members.Count;

                var payments = db.InvoicePayments
                    .Where(p => p.InvoiceId == _invoiceId)
                    .ToList();

                var payByMember = payments.ToDictionary(p => p.MemberId, p => p);

                var list = new List<PayRow>();

                foreach (var member in members)
                {
                    InvoicePayment p = null;
                    if (payByMember.ContainsKey(member.MemberId))
                        p = payByMember[member.MemberId];

                    list.Add(new PayRow
                    {
                        Paid = (p != null),
                        PaymentId = (p != null ? (int?)p.PaymentId : null),
                        MemberId = member.MemberId,
                        FullName = member.FullName,
                        Phone = member.Phone,
                        AmountPaid = (p != null ? Convert.ToDecimal(p.AmountPaid) : _expectedEach),
                        PaidAt = (p != null ? p.PaidAt : (DateTime?)null)
                    });
                }

                _rows = new BindingList<PayRow>(list);
                dgvPay.DataSource = _rows;

                ConfigureGridAfterBind();
                UpdateSummary();
            }
        }

        private void ConfigureGridAfterBind()
        {
            // Ẩn cột kỹ thuật
            HideIfExists("PaymentId");
            HideIfExists("MemberId");

            // Header tiếng Việt cho dễ nhìn
            if (dgvPay.Columns["Paid"] != null)
                dgvPay.Columns["Paid"].HeaderText = "Đã nộp";

            if (dgvPay.Columns["FullName"] != null)
            {
                dgvPay.Columns["FullName"].HeaderText = "Họ tên";
                dgvPay.Columns["FullName"].ReadOnly = true;
                dgvPay.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvPay.Columns["Phone"] != null)
            {
                dgvPay.Columns["Phone"].HeaderText = "SĐT";
                dgvPay.Columns["Phone"].ReadOnly = true;
                dgvPay.Columns["Phone"].Width = 120;
            }

            if (dgvPay.Columns["AmountPaid"] != null)
            {
                dgvPay.Columns["AmountPaid"].HeaderText = "Số tiền";
                dgvPay.Columns["AmountPaid"].DefaultCellStyle.Format = "N0";
                dgvPay.Columns["AmountPaid"].Width = 120;
            }

            if (dgvPay.Columns["PaidAt"] != null)
            {
                dgvPay.Columns["PaidAt"].HeaderText = "Ngày nộp";
                dgvPay.Columns["PaidAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvPay.Columns["PaidAt"].Width = 150;
            }
        }

        private void HideIfExists(string colName)
        {
            if (dgvPay.Columns[colName] != null)
                dgvPay.Columns[colName].Visible = false;
        }

        private void UpdateSummary()
        {
            decimal expectedTotal = _expectedEach * _memberCount;
            decimal received = _rows.Where(r => r.Paid).Sum(r => r.AmountPaid);
            decimal remaining = expectedTotal - received;

            lblExpectedTotal.Text = string.Format("{0:N0} đ", expectedTotal);
            lblReceived.Text = string.Format("{0:N0} đ", received);
            lblRemaining.Text = string.Format("{0:N0} đ", remaining);
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (var row in _rows)
            {
                row.Paid = true;

                // Nếu chưa có ngày nộp thì set hiện tại
                if (!row.PaidAt.HasValue)
                    row.PaidAt = DateTime.Now;

                // Nếu số tiền <= 0 thì set theo mức phải nộp
                if (row.AmountPaid <= 0)
                    row.AmountPaid = _expectedEach;
            }

            dgvPay.Refresh();
            UpdateSummary();
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (var row in _rows)
            {
                row.Paid = false;
                // Có thể giữ PaidAt/AmountPaid để tiện check lại, hoặc xoá đi
                // row.PaidAt = null;
            }

            dgvPay.Refresh();
            UpdateSummary();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new DataClasses1DataContext())
                {
                    var existing = db.InvoicePayments
                        .Where(p => p.InvoiceId == _invoiceId)
                        .ToList();

                    var existingByMember = existing.ToDictionary(p => p.MemberId, p => p);

                    foreach (var row in _rows)
                    {
                        InvoicePayment p = null;
                        if (existingByMember.ContainsKey(row.MemberId))
                            p = existingByMember[row.MemberId];

                        if (row.Paid)
                        {
                            if (p == null)
                            {
                                p = new InvoicePayment
                                {
                                    InvoiceId = _invoiceId,
                                    MemberId = row.MemberId
                                };
                                db.InvoicePayments.InsertOnSubmit(p);
                            }

                            // DB của bạn khả năng cao là INT -> phải convert
                            p.AmountPaid = Convert.ToInt32(row.AmountPaid);

                            // Nếu cột PaidAt trong DB không cho null, set DateTime.Now
                            p.PaidAt = row.PaidAt ?? DateTime.Now;
                            p.Status = "Paid";
                        }
                        else
                        {
                            // Bỏ tick => xoá payment record
                            if (p != null)
                                db.InvoicePayments.DeleteOnSubmit(p);
                        }
                    }

                    db.SubmitChanges();
                }

                MessageBox.Show("Đã lưu danh sách nộp.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi lưu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Khi click checkbox trong DataGridView, cần commit ngay để CellValueChanged chạy
        private void dgvPay_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPay.IsCurrentCellDirty)
            {
                dgvPay.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        // Realtime update summary khi tick/uncheck hoặc sửa số tiền
        private void dgvPay_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Nếu vừa tick Paid = true mà chưa có PaidAt => set hiện tại
            if (dgvPay.Columns[e.ColumnIndex].Name == "Paid")
            {
                var row = dgvPay.Rows[e.RowIndex].DataBoundItem as PayRow;
                if (row != null && row.Paid)
                {
                    if (!row.PaidAt.HasValue) row.PaidAt = DateTime.Now;
                    if (row.AmountPaid <= 0) row.AmountPaid = _expectedEach;
                    dgvPay.Refresh();
                }
            }

            UpdateSummary();
        }

        private void dgvPay_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Tránh văng lỗi khi nhập sai format ô số tiền / ngày
            e.ThrowException = false;
        }

        // ---- Model bind lên DataGridView ----
        public class PayRow
        {
            public bool Paid { get; set; }
            public int? PaymentId { get; set; }
            public int MemberId { get; set; }
            public string FullName { get; set; }
            public string Phone { get; set; }
            public decimal AmountPaid { get; set; }
            public DateTime? PaidAt { get; set; }
        }

        // Các event rỗng để Designer không lỗi (nếu đã gắn)
        private void dgvPay_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void lblExpectedEach_Click(object sender, EventArgs e) { }
        private void lblExpectedTotal_Click(object sender, EventArgs e) { }
        private void lblReceived_Click(object sender, EventArgs e) { }
        private void lblRemaining_Click(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}