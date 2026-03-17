using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmInvoicePayments : BaseForm
    {
        private readonly int _classId;
        private readonly int _invoiceId;

        private BindingList<PayRow> _rows = new BindingList<PayRow>();

        // giữ lại để update summary realtime
        private decimal _expectedEach = 0m;
        private int _memberCount = 0;

        private Label lblHeaderTitle;
        private Label lblHeaderSubTitle;
        private GroupBox grpPayments;
        private GroupBox grpSummary;

        private Label lblFeeTitleCaption;
        private Label lblExpectedEachCaption;
        private Label lblExpectedTotalCaption;
        private Label lblReceivedCaption;
        private Label lblRemainingCaption;

        public FrmInvoicePayments(int classId, int invoiceId)
        {
            InitializeComponent();

            _classId = classId;
            _invoiceId = invoiceId;

            BuildExtraUI();
            SetupGrid();
            SetupPaymentUI();
            WireExtraEvents();

            // Gắn event bằng code để khỏi lệ thuộc Designer
            dgvPay.CurrentCellDirtyStateChanged += dgvPay_CurrentCellDirtyStateChanged;
            dgvPay.CellValueChanged += dgvPay_CellValueChanged;
            dgvPay.DataError += dgvPay_DataError;
        }

        private void BuildExtraUI()
        {
            lblHeaderTitle = new Label();
            lblHeaderSubTitle = new Label();
            grpPayments = new GroupBox();
            grpSummary = new GroupBox();

            lblFeeTitleCaption = new Label();
            lblExpectedEachCaption = new Label();
            lblExpectedTotalCaption = new Label();
            lblReceivedCaption = new Label();
            lblRemainingCaption = new Label();

            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Text = "Quản lý khoản thu";
            lblHeaderTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.BackColor = Color.Transparent;

            lblHeaderSubTitle.AutoSize = true;
            lblHeaderSubTitle.Text = "Đánh dấu thành viên đã nộp và theo dõi tổng số tiền đã thu";
            lblHeaderSubTitle.Font = new Font("Segoe UI", 9.5F);
            lblHeaderSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblHeaderSubTitle.BackColor = Color.Transparent;

            grpPayments.Text = "Danh sách nộp tiền";
            grpPayments.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            grpPayments.ForeColor = Color.FromArgb(31, 41, 55);
            grpPayments.BackColor = Color.White;
            grpPayments.Padding = new Padding(10);

            grpSummary.Text = "Tổng quan khoản thu";
            grpSummary.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            grpSummary.ForeColor = Color.FromArgb(31, 41, 55);
            grpSummary.BackColor = Color.White;
            grpSummary.Padding = new Padding(10);

            InitCaptionLabel(lblFeeTitleCaption, "Tên khoản thu");
            InitCaptionLabel(lblExpectedEachCaption, "Mức thu mỗi người");
            InitCaptionLabel(lblExpectedTotalCaption, "Tổng phải thu");
            InitCaptionLabel(lblReceivedCaption, "Đã thu");
            InitCaptionLabel(lblRemainingCaption, "Còn lại");

            this.Controls.Add(lblHeaderTitle);
            this.Controls.Add(lblHeaderSubTitle);
            this.Controls.Add(grpPayments);
            this.Controls.Add(grpSummary);

            grpSummary.Controls.Add(lblFeeTitleCaption);
            grpSummary.Controls.Add(lblExpectedEachCaption);
            grpSummary.Controls.Add(lblExpectedTotalCaption);
            grpSummary.Controls.Add(lblReceivedCaption);
            grpSummary.Controls.Add(lblRemainingCaption);

            lblHeaderTitle.BringToFront();
            lblHeaderSubTitle.BringToFront();
        }

        private void InitCaptionLabel(Label lbl, string text)
        {
            lbl.AutoSize = true;
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(100, 116, 139);
            lbl.BackColor = Color.Transparent;
        }

        private void SetupPaymentUI()
        {
            this.Text = "Quản lý khoản thu";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(1180, 720);

            StyleValueLabel(lblTitle, false);
            StyleValueLabel(lblExpectedEach, false);
            StyleValueLabel(lblExpectedTotal, false);
            StyleValueLabel(lblReceived, true);
            StyleValueLabel(lblRemaining, true);

            StylePrimaryButton(btnCheckAll, "Check all");
            StyleSecondaryButton(btnUncheckAll, "Bỏ chọn hết");
            StylePrimaryButton(btnSave, "Lưu");
            StyleSecondaryButton(btnClose, "Đóng");

            HideOldCaptionLabels();
        }

        private void WireExtraEvents()
        {
            this.Shown += FrmInvoicePayments_Shown;
            this.Resize += FrmInvoicePayments_Resize;
        }

        private void FrmInvoicePayments_Shown(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmInvoicePayments_Resize(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            int margin = 22;

            lblHeaderTitle.Location = new Point(margin, 18);
            lblHeaderSubTitle.Location = new Point(margin, 50);

            grpPayments.Location = new Point(margin, 84);
            grpPayments.Size = new Size(this.ClientSize.Width - margin * 2, 420);

            grpSummary.Location = new Point(margin, grpPayments.Bottom + 18);
            grpSummary.Size = new Size(this.ClientSize.Width - margin * 2, 150);

            EnsureParent(dgvPay, grpPayments);
            dgvPay.Location = new Point(16, 34);
            dgvPay.Size = new Size(grpPayments.ClientSize.Width - 32, grpPayments.ClientSize.Height - 50);

            EnsureParent(lblTitle, grpSummary);
            EnsureParent(lblExpectedEach, grpSummary);
            EnsureParent(lblExpectedTotal, grpSummary);
            EnsureParent(lblReceived, grpSummary);
            EnsureParent(lblRemaining, grpSummary);

            EnsureParent(btnCheckAll, grpSummary);
            EnsureParent(btnUncheckAll, grpSummary);
            EnsureParent(btnSave, grpSummary);
            EnsureParent(btnClose, grpSummary);

            int leftX = 18;
            int valueX = 170;

            lblFeeTitleCaption.Location = new Point(leftX, 32);
            lblTitle.Location = new Point(valueX, 28);

            lblExpectedEachCaption.Location = new Point(leftX, 60);
            lblExpectedEach.Location = new Point(valueX, 56);

            lblExpectedTotalCaption.Location = new Point(leftX, 88);
            lblExpectedTotal.Location = new Point(valueX, 84);

            lblReceivedCaption.Location = new Point(leftX, 116);
            lblReceived.Location = new Point(valueX, 112);

            lblRemainingCaption.Location = new Point(380, 32);
            lblRemaining.Location = new Point(460, 28);

            btnCheckAll.Size = new Size(130, 40);
            btnUncheckAll.Size = new Size(130, 40);
            btnSave.Size = new Size(120, 40);
            btnClose.Size = new Size(120, 40);

            int rightMargin = 18;
            btnClose.Location = new Point(grpSummary.ClientSize.Width - btnClose.Width - rightMargin, 86);
            btnSave.Location = new Point(btnClose.Left - btnSave.Width - 14, 86);

            btnUncheckAll.Location = new Point(grpSummary.ClientSize.Width - btnUncheckAll.Width - rightMargin, 28);
            btnCheckAll.Location = new Point(btnUncheckAll.Left - btnCheckAll.Width - 14, 28);
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

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                foreach (Control child in GetAllControls(c))
                    yield return child;

                yield return c;
            }
        }

        private void HideOldCaptionLabels()
        {
            foreach (var lbl in GetAllControls(this).OfType<Label>())
            {
                if (lbl == lblHeaderTitle || lbl == lblHeaderSubTitle ||
                    lbl == lblFeeTitleCaption || lbl == lblExpectedEachCaption ||
                    lbl == lblExpectedTotalCaption || lbl == lblReceivedCaption ||
                    lbl == lblRemainingCaption || lbl == lblTitle ||
                    lbl == lblExpectedEach || lbl == lblExpectedTotal ||
                    lbl == lblReceived || lbl == lblRemaining)
                    continue;

                string t = (lbl.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(t)) continue;

                if (t.IndexOf("Tên khoản Thu", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Số Tiền", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Tổng chưa đóng", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Đã đóng", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Còn Lại", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    lbl.Visible = false;
                }
            }
        }

        private void StyleValueLabel(Label lbl, bool highlight)
        {
            lbl.AutoSize = true;
            lbl.Font = highlight
                ? new Font("Segoe UI Semibold", 14F, FontStyle.Bold)
                : new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            lbl.ForeColor = highlight
                ? Color.FromArgb(37, 99, 235)
                : Color.FromArgb(31, 41, 55);
            lbl.BackColor = Color.Transparent;
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
            dgvPay.RowHeadersVisible = false;
            dgvPay.BackgroundColor = Color.White;
            dgvPay.BorderStyle = BorderStyle.FixedSingle;
            dgvPay.EnableHeadersVisualStyles = false;
            dgvPay.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvPay.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvPay.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPay.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvPay.ColumnHeadersHeight = 36;
            dgvPay.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvPay.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dgvPay.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvPay.RowTemplate.Height = 30;
            dgvPay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            HideIfExists("PaymentId");
            HideIfExists("MemberId");

            if (dgvPay.Columns["Paid"] != null)
            {
                dgvPay.Columns["Paid"].HeaderText = "Đã nộp";
                dgvPay.Columns["Paid"].Width = 80;
                dgvPay.Columns["Paid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }

            if (dgvPay.Columns["FullName"] != null)
            {
                dgvPay.Columns["FullName"].HeaderText = "Họ tên";
                dgvPay.Columns["FullName"].ReadOnly = true;
                dgvPay.Columns["FullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPay.Columns["FullName"].FillWeight = 34;
            }

            if (dgvPay.Columns["Phone"] != null)
            {
                dgvPay.Columns["Phone"].HeaderText = "SĐT";
                dgvPay.Columns["Phone"].ReadOnly = true;
                dgvPay.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPay.Columns["Phone"].FillWeight = 18;
            }

            if (dgvPay.Columns["AmountPaid"] != null)
            {
                dgvPay.Columns["AmountPaid"].HeaderText = "Số tiền";
                dgvPay.Columns["AmountPaid"].DefaultCellStyle.Format = "N0";
                dgvPay.Columns["AmountPaid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPay.Columns["AmountPaid"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPay.Columns["AmountPaid"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPay.Columns["AmountPaid"].FillWeight = 20;
            }

            if (dgvPay.Columns["PaidAt"] != null)
            {
                dgvPay.Columns["PaidAt"].HeaderText = "Ngày nộp";
                dgvPay.Columns["PaidAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvPay.Columns["PaidAt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPay.Columns["PaidAt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvPay.Columns["PaidAt"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvPay.Columns["PaidAt"].FillWeight = 28;
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

                if (!row.PaidAt.HasValue)
                    row.PaidAt = DateTime.Now;

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

                            p.AmountPaid = Convert.ToInt32(row.AmountPaid);
                            p.PaidAt = row.PaidAt ?? DateTime.Now;
                            p.Status = "Paid";
                        }
                        else
                        {
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

        private void dgvPay_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPay.IsCurrentCellDirty)
            {
                dgvPay.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPay_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

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

        // Các event rỗng để Designer không lỗi
        private void dgvPay_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void lblExpectedEach_Click(object sender, EventArgs e) { }
        private void lblExpectedTotal_Click(object sender, EventArgs e) { }
        private void lblReceived_Click(object sender, EventArgs e) { }
        private void lblRemaining_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
    }
}