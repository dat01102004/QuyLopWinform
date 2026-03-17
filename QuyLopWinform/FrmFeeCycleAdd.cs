using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuyLopWinform
{
    public partial class FrmFeeCycleAdd : BaseForm
    {
        public string TitleValue => txtTitle.Text.Trim();
        public decimal AmountValue => nudAmount.Value;
        public bool UseDueDate => chkDueDate.Checked;
        public DateTime DueDateValue => dtpDueDate.Value;

        private Label lblHeaderTitle;
        private Label lblHeaderSubTitle;
        private Label lblTitleCaption;
        private Label lblAmountCaption;
        private Label lblDueDateCaption;

        public FrmFeeCycleAdd()
        {
            InitializeComponent();

            BuildExtraUI();
            SetupControls();
            SetupFeeCycleUI();
            ArrangeLayout();

            dtpDueDate.Enabled = false;
            chkDueDate.CheckedChanged -= chkDueDate_CheckedChanged;
            chkDueDate.CheckedChanged += chkDueDate_CheckedChanged;

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            this.Shown += FrmFeeCycleAdd_Shown;
            this.Resize += FrmFeeCycleAdd_Resize;
        }

        private void FrmFeeCycleAdd_Shown(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmFeeCycleAdd_Resize(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void BuildExtraUI()
        {
            lblHeaderTitle = new Label();
            lblHeaderSubTitle = new Label();
            lblTitleCaption = new Label();
            lblAmountCaption = new Label();
            lblDueDateCaption = new Label();

            InitHeaderLabel(lblHeaderTitle, "Thêm khoản thu", 18F, true, Color.FromArgb(15, 23, 42));
            InitHeaderLabel(lblHeaderSubTitle, "Nhập tiêu đề, số tiền và tùy chọn hạn nộp", 9.5F, false, Color.FromArgb(100, 116, 139));

            InitCaptionLabel(lblTitleCaption, "Tiêu đề khoản thu");
            InitCaptionLabel(lblAmountCaption, "Số tiền");
            InitCaptionLabel(lblDueDateCaption, "Hạn nộp");

            this.Controls.Add(lblHeaderTitle);
            this.Controls.Add(lblHeaderSubTitle);
            this.Controls.Add(lblTitleCaption);
            this.Controls.Add(lblAmountCaption);
            this.Controls.Add(lblDueDateCaption);

            lblHeaderTitle.BringToFront();
            lblHeaderSubTitle.BringToFront();
            lblTitleCaption.BringToFront();
            lblAmountCaption.BringToFront();
            lblDueDateCaption.BringToFront();
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

        private void SetupControls()
        {
            nudAmount.DecimalPlaces = 0;
            nudAmount.Minimum = 0;
            nudAmount.Maximum = 1000000000;
            nudAmount.Increment = 1000;
            nudAmount.ThousandsSeparator = true;

            dtpDueDate.Format = DateTimePickerFormat.Custom;
            dtpDueDate.CustomFormat = "dd/MM/yyyy";
            dtpDueDate.Value = DateTime.Today;
        }

        private void SetupFeeCycleUI()
        {
            this.Text = "Thêm khoản thu";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(560, 380);
            this.ShowInTaskbar = false;

            StyleTextBox(txtTitle);

            nudAmount.Font = new Font("Segoe UI", 10.5F);
            nudAmount.BackColor = Color.White;
            nudAmount.ForeColor = Color.FromArgb(31, 41, 55);
            nudAmount.BorderStyle = BorderStyle.FixedSingle;
            nudAmount.TextAlign = HorizontalAlignment.Left;

            chkDueDate.Font = new Font("Segoe UI", 10F);
            chkDueDate.ForeColor = Color.FromArgb(31, 41, 55);
            chkDueDate.BackColor = Color.Transparent;
            chkDueDate.Text = "Có hạn nộp";

            dtpDueDate.Font = new Font("Segoe UI", 10.5F);
            dtpDueDate.CalendarFont = new Font("Segoe UI", 10F);

            StylePrimaryButton(btnOK, "Lưu");
            StyleSecondaryButton(btnCancel, "Hủy");
        }

        private void ArrangeLayout()
        {
            int marginX = 42;
            int inputX = marginX;
            int inputWidth = this.ClientSize.Width - marginX * 2;
            int top = 28;

            lblHeaderTitle.Location = new Point(marginX, top);
            lblHeaderSubTitle.Location = new Point(marginX, top + 34);

            int row1Y = 102;
            int row2Y = 164;
            int row3Y = 226;

            lblTitleCaption.Location = new Point(marginX, row1Y);
            txtTitle.Location = new Point(inputX, row1Y + 22);
            txtTitle.Size = new Size(inputWidth, 30);

            lblAmountCaption.Location = new Point(marginX, row2Y);
            nudAmount.Location = new Point(inputX, row2Y + 22);
            nudAmount.Size = new Size(inputWidth, 30);

            chkDueDate.Location = new Point(marginX, row3Y - 2);
            chkDueDate.AutoSize = true;

            lblDueDateCaption.Location = new Point(marginX, row3Y + 28);
            dtpDueDate.Location = new Point(inputX, row3Y + 50);
            dtpDueDate.Size = new Size(inputWidth, 30);

            btnOK.Size = new Size(120, 40);
            btnCancel.Size = new Size(120, 40);

            int buttonY = this.ClientSize.Height - 62;
            int gap = 14;
            int totalWidth = btnOK.Width + gap + btnCancel.Width;
            int startX = (this.ClientSize.Width - totalWidth) / 2;

            btnOK.Location = new Point(startX, buttonY);
            btnCancel.Location = new Point(btnOK.Right + gap, buttonY);
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

        private void chkDueDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpDueDate.Enabled = chkDueDate.Checked;
            lblDueDateCaption.Enabled = chkDueDate.Checked;
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