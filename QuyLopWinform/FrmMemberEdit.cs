using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuyLopWinform
{
    public partial class FrmMemberEdit : BaseForm
    {
        public string FullNameValue => txtFullName.Text.Trim();
        public string PhoneValue => txtPhone.Text.Trim();
        public string NoteValue => txtNote.Text.Trim();
        public string RoleValue => txtRole.Text.Trim();

        private Label lblHeaderTitle;
        private Label lblHeaderSubTitle;
        private Label lblFullNameCaption;
        private Label lblPhoneCaption;
        private Label lblNoteCaption;
        private Label lblRoleCaption;

        private readonly bool _isEditMode;

        public FrmMemberEdit(string fullName = "", string phone = "", string note = "", string role = "Member")
        {
            InitializeComponent();

            _isEditMode =
                !string.IsNullOrWhiteSpace(fullName) ||
                !string.IsNullOrWhiteSpace(phone) ||
                !string.IsNullOrWhiteSpace(note) ||
                !string.Equals(role ?? "Member", "Member", StringComparison.CurrentCultureIgnoreCase);

            BuildExtraUI();
            SetupMemberEditUI();
            ArrangeLayout();

            txtFullName.Text = fullName;
            txtPhone.Text = phone;
            txtNote.Text = note;
            txtRole.Text = role;

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            this.Shown += FrmMemberEdit_Shown;
            this.Resize += FrmMemberEdit_Resize;
        }

        private void FrmMemberEdit_Shown(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmMemberEdit_Resize(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void BuildExtraUI()
        {
            lblHeaderTitle = new Label();
            lblHeaderSubTitle = new Label();
            lblFullNameCaption = new Label();
            lblPhoneCaption = new Label();
            lblNoteCaption = new Label();
            lblRoleCaption = new Label();

            InitHeaderLabel(
                lblHeaderTitle,
                _isEditMode ? "Sửa thành viên" : "Thêm thành viên",
                18F,
                true,
                Color.FromArgb(15, 23, 42));

            InitHeaderLabel(
                lblHeaderSubTitle,
                _isEditMode
                    ? "Cập nhật thông tin thành viên trong lớp"
                    : "Nhập thông tin thành viên mới cho lớp",
                9.5F,
                false,
                Color.FromArgb(100, 116, 139));

            InitCaptionLabel(lblFullNameCaption, "Họ và tên");
            InitCaptionLabel(lblPhoneCaption, "Số điện thoại");
            InitCaptionLabel(lblNoteCaption, "Ghi chú");
            InitCaptionLabel(lblRoleCaption, "Vai trò");

            this.Controls.Add(lblHeaderTitle);
            this.Controls.Add(lblHeaderSubTitle);
            this.Controls.Add(lblFullNameCaption);
            this.Controls.Add(lblPhoneCaption);
            this.Controls.Add(lblNoteCaption);
            this.Controls.Add(lblRoleCaption);

            lblHeaderTitle.BringToFront();
            lblHeaderSubTitle.BringToFront();
            lblFullNameCaption.BringToFront();
            lblPhoneCaption.BringToFront();
            lblNoteCaption.BringToFront();
            lblRoleCaption.BringToFront();
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

        private void SetupMemberEditUI()
        {
            this.Text = _isEditMode ? "Sửa thành viên" : "Thêm thành viên";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(560, 430);
            this.ShowInTaskbar = false;

            StyleTextBox(txtFullName);
            StyleTextBox(txtPhone);
            StyleTextBox(txtNote);
            StyleTextBox(txtRole);

            txtRole.Text = string.IsNullOrWhiteSpace(txtRole.Text) ? "Member" : txtRole.Text;

            StylePrimaryButton(btnOK, _isEditMode ? "Lưu" : "Thêm");
            StyleSecondaryButton(btnCancel, "Hủy");

            HideOldLabels();
        }

        private void HideOldLabels()
        {
            foreach (var lbl in this.Controls.OfType<Label>())
            {
                if (lbl == lblHeaderTitle || lbl == lblHeaderSubTitle ||
                    lbl == lblFullNameCaption || lbl == lblPhoneCaption ||
                    lbl == lblNoteCaption || lbl == lblRoleCaption)
                    continue;

                string t = (lbl.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(t)) continue;

                if (t.IndexOf("Họ", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Số điện thoại", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Ghi Chú", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Vai Trò", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    lbl.Visible = false;
                }
            }
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
            int row2Y = 162;
            int row3Y = 222;
            int row4Y = 282;

            lblFullNameCaption.Location = new Point(marginX, row1Y);
            txtFullName.Location = new Point(inputX, row1Y + 22);
            txtFullName.Size = new Size(inputWidth, 30);

            lblPhoneCaption.Location = new Point(marginX, row2Y);
            txtPhone.Location = new Point(inputX, row2Y + 22);
            txtPhone.Size = new Size(inputWidth, 30);

            lblNoteCaption.Location = new Point(marginX, row3Y);
            txtNote.Location = new Point(inputX, row3Y + 22);
            txtNote.Size = new Size(inputWidth, 30);

            lblRoleCaption.Location = new Point(marginX, row4Y);
            txtRole.Location = new Point(inputX, row4Y + 22);
            txtRole.Size = new Size(inputWidth, 30);

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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameValue))
            {
                MessageBox.Show("Vui lòng nhập Họ tên.");
                txtFullName.Focus();
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

        private void FrmMemberEdit_Load(object sender, EventArgs e)
        {
        }
    }
}