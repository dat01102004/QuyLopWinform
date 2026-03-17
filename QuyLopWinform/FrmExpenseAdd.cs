using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuyLopWinform
{
    public partial class FrmExpenseAdd : BaseForm
    {
        public string TitleValue { get; private set; } = "";
        public decimal AmountValue { get; private set; } = 0;
        public DateTime ExpenseDateValue { get; private set; } = DateTime.Today;

        private Label lblHeaderTitle;
        private Label lblHeaderSubTitle;
        private Label lblTitleCaption;
        private Label lblAmountCaption;
        private Label lblDateCaption;

        public FrmExpenseAdd()
        {
            InitializeComponent();
            BuildExtraUI();
            SetupControls();
            SetupExpenseUI();
            ArrangeLayout();

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
            this.Shown += FrmExpenseAdd_Shown;
            this.Resize += FrmExpenseAdd_Resize;

            Text = "Thêm khoản chi";
        }

        // Dùng cho sửa
        public FrmExpenseAdd(string title, decimal amount, DateTime expenseDate) : this()
        {
            txtTitle.Text = title;
            numAmount.Value = Math.Min(numAmount.Maximum, Math.Max(numAmount.Minimum, amount));
            dtpExpenseDate.Value = expenseDate;
            Text = "Sửa khoản chi";
            lblHeaderTitle.Text = "Sửa khoản chi";
            lblHeaderSubTitle.Text = "Cập nhật nội dung chi, số tiền và ngày chi";
        }

        private void FrmExpenseAdd_Shown(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmExpenseAdd_Resize(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void BuildExtraUI()
        {
            lblHeaderTitle = new Label();
            lblHeaderSubTitle = new Label();
            lblTitleCaption = new Label();
            lblAmountCaption = new Label();
            lblDateCaption = new Label();

            InitHeaderLabel(lblHeaderTitle, "Thêm khoản chi", 18F, true, Color.FromArgb(15, 23, 42));
            InitHeaderLabel(lblHeaderSubTitle, "Nhập nội dung chi, số tiền và ngày chi", 9.5F, false, Color.FromArgb(100, 116, 139));

            InitCaptionLabel(lblTitleCaption, "Nội dung chi");
            InitCaptionLabel(lblAmountCaption, "Số tiền");
            InitCaptionLabel(lblDateCaption, "Ngày chi");

            this.Controls.Add(lblHeaderTitle);
            this.Controls.Add(lblHeaderSubTitle);
            this.Controls.Add(lblTitleCaption);
            this.Controls.Add(lblAmountCaption);
            this.Controls.Add(lblDateCaption);

            lblHeaderTitle.BringToFront();
            lblHeaderSubTitle.BringToFront();
            lblTitleCaption.BringToFront();
            lblAmountCaption.BringToFront();
            lblDateCaption.BringToFront();
        }

        private void InitHeaderLabel(Label lbl, string text, float size, bool semibold, Color color)
        {
            lbl.AutoSize = true;
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI" + (semibold ? " Semibold" : ""), size, semibold ? FontStyle.Bold : FontStyle.Regular);
            lbl.ForeColor = color;
            lbl.BackColor = Color.Transparent;
        }

        private void InitCaptionLabel(Label lbl, string text)
        {
            lbl.AutoSize = true;
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(100, 116, 139);
            lbl.BackColor = Color.Transparent;
        }

        private void SetupExpenseUI()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(560, 360);
            this.ShowInTaskbar = false;

            StyleTextBox(txtTitle);

            numAmount.Font = new Font("Segoe UI", 10.5F);
            numAmount.BackColor = Color.White;
            numAmount.ForeColor = Color.FromArgb(31, 41, 55);
            numAmount.BorderStyle = BorderStyle.FixedSingle;
            numAmount.TextAlign = HorizontalAlignment.Left;

            dtpExpenseDate.Font = new Font("Segoe UI", 10.5F);
            dtpExpenseDate.CalendarFont = new Font("Segoe UI", 10F);

            StylePrimaryButton(btnSave, "Lưu");
            StyleSecondaryButton(btnCancel, "Hủy");
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

        private void ArrangeLayout()
        {
            int marginX = 42;
            int captionX = marginX;
            int inputX = marginX;
            int inputWidth = this.ClientSize.Width - marginX * 2;
            int top = 28;

            lblHeaderTitle.Location = new Point(marginX, top);
            lblHeaderSubTitle.Location = new Point(marginX, top + 34);

            int row1Y = 102;
            int row2Y = 164;
            int row3Y = 226;

            lblTitleCaption.Location = new Point(captionX, row1Y);
            txtTitle.Location = new Point(inputX, row1Y + 22);
            txtTitle.Size = new Size(inputWidth, 30);

            lblAmountCaption.Location = new Point(captionX, row2Y);
            numAmount.Location = new Point(inputX, row2Y + 22);
            numAmount.Size = new Size(inputWidth, 30);

            lblDateCaption.Location = new Point(captionX, row3Y);
            dtpExpenseDate.Location = new Point(inputX, row3Y + 22);
            dtpExpenseDate.Size = new Size(inputWidth, 30);

            btnSave.Size = new Size(120, 40);
            btnCancel.Size = new Size(120, 40);

            int buttonY = this.ClientSize.Height - 62;
            int gap = 14;
            int totalWidth = btnSave.Width + gap + btnCancel.Width;
            int startX = (this.ClientSize.Width - totalWidth) / 2;

            btnSave.Location = new Point(startX, buttonY);
            btnCancel.Location = new Point(btnSave.Right + gap, buttonY);
        }

        private void StyleTextBox(TextBox txt)
        {
            txt.Font = new Font("Segoe UI", 10.5F);
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = Color.White;
            txt.ForeColor = Color.FromArgb(31, 41, 55);
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