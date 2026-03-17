using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmClassPicker : BaseForm
    {
        public int? SelectedClassId { get; private set; }

        private Label lblTitle;
        private Label lblSubTitle;
        private GroupBox grpCreate;
        private GroupBox grpJoin;
        private Label lblMyClasses;

        public FrmClassPicker()
        {
            InitializeComponent();
            BuildExtraUI();
            SetupGrid();
            SetupUI();
            WireEvents();
        }

        private void BuildExtraUI()
        {
            lblTitle = new Label();
            lblSubTitle = new Label();
            grpCreate = new GroupBox();
            grpJoin = new GroupBox();
            lblMyClasses = new Label();

            lblTitle.Text = "Chọn lớp học";
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(15, 23, 42);

            lblSubTitle.Text = "Tạo lớp mới, tham gia bằng mã mời hoặc chọn lớp đang tham gia";
            lblSubTitle.AutoSize = true;
            lblSubTitle.Font = new Font("Segoe UI", 9.5F);
            lblSubTitle.ForeColor = Color.FromArgb(100, 116, 139);

            grpCreate.Text = "Tạo lớp mới";
            grpCreate.Font = new Font("Segoe UI Semibold", 10F);
            grpCreate.ForeColor = Color.FromArgb(31, 41, 55);
            grpCreate.BackColor = Color.White;

            grpJoin.Text = "Tham gia bằng mã mời";
            grpJoin.Font = new Font("Segoe UI Semibold", 10F);
            grpJoin.ForeColor = Color.FromArgb(31, 41, 55);
            grpJoin.BackColor = Color.White;

            lblMyClasses.Text = "Lớp của tôi";
            lblMyClasses.AutoSize = true;
            lblMyClasses.Font = new Font("Segoe UI Semibold", 11F);
            lblMyClasses.ForeColor = Color.FromArgb(31, 41, 55);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubTitle);
            this.Controls.Add(grpCreate);
            this.Controls.Add(grpJoin);
            this.Controls.Add(lblMyClasses);

            grpCreate.SendToBack();
            grpJoin.SendToBack();
            lblTitle.BringToFront();
            lblSubTitle.BringToFront();
            lblMyClasses.BringToFront();
        }

        private void WireEvents()
        {
            this.Load -= FrmClassPicker_Load;
            this.Load += FrmClassPicker_Load;

            this.Shown -= FrmClassPicker_Shown;
            this.Shown += FrmClassPicker_Shown;

            this.Resize -= FrmClassPicker_Resize;
            this.Resize += FrmClassPicker_Resize;

            btnCreate.Click -= btnCreate_Click;
            btnCreate.Click += btnCreate_Click;

            btnSelect.Click -= btnSelect_Click;
            btnSelect.Click += btnSelect_Click;

            btnClose.Click -= btnClose_Click;
            btnClose.Click += btnClose_Click;

            dgvClasses.CellDoubleClick -= dgvClasses_CellDoubleClick;
            dgvClasses.CellDoubleClick += dgvClasses_CellDoubleClick;

            btnManage.Click -= btnManage_Click;
            btnManage.Click += btnManage_Click;

            btnJoin.Click -= btnJoin_Click;
            btnJoin.Click += btnJoin_Click;
        }

        private void SetupUI()
        {
            this.Text = "Chọn lớp";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.ClientSize = new Size(980, 560);
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);

            StyleTextBox(txtClassName);
            StyleTextBox(txtInviteCode);

            StylePrimaryButton(btnCreate, "Tạo lớp");
            StylePrimaryButton(btnJoin, "Tham gia");
            StylePrimaryButton(btnSelect, "Chọn lớp");
            StyleSecondaryButton(btnManage, "Quản lý lớp");
            StyleSecondaryButton(btnClose, "Đóng");

            if (label1 != null) label1.Visible = false;
            if (label2 != null) label2.Visible = false;

            ArrangeLayout();
        }

        private void SetupGrid()
        {
            dgvClasses.AutoGenerateColumns = true;
            dgvClasses.ReadOnly = true;
            dgvClasses.AllowUserToAddRows = false;
            dgvClasses.AllowUserToDeleteRows = false;
            dgvClasses.MultiSelect = false;
            dgvClasses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClasses.RowHeadersVisible = false;
            dgvClasses.BackgroundColor = Color.White;
            dgvClasses.BorderStyle = BorderStyle.FixedSingle;
            dgvClasses.EnableHeadersVisualStyles = false;
            dgvClasses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvClasses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvClasses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClasses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvClasses.ColumnHeadersHeight = 36;
            dgvClasses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvClasses.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dgvClasses.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvClasses.RowTemplate.Height = 30;
            dgvClasses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void FrmClassPicker_Shown(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmClassPicker_Resize(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void ArrangeLayout()
        {
            int margin = 24;
            int leftWidth = 270;
            int gap = 24;
            int rightX = margin + leftWidth + gap;
            int rightWidth = this.ClientSize.Width - rightX - margin;

            lblTitle.Location = new Point(margin, 20);
            lblSubTitle.Location = new Point(margin, 56);

            grpCreate.Location = new Point(margin, 100);
            grpCreate.Size = new Size(leftWidth, 150);

            grpJoin.Location = new Point(margin, 270);
            grpJoin.Size = new Size(leftWidth, 150);

            lblMyClasses.Location = new Point(rightX, 100);

            dgvClasses.Location = new Point(rightX, 132);
            dgvClasses.Size = new Size(rightWidth, 288);

            btnManage.Size = new Size(120, 38);
            btnSelect.Size = new Size(120, 38);
            btnClose.Size = new Size(90, 38);

            int bottomY = 450;
            btnManage.Location = new Point(rightX, bottomY);
            btnSelect.Location = new Point(btnManage.Right + 12, bottomY);
            btnClose.Location = new Point(rightX + rightWidth - btnClose.Width, bottomY);

            MoveCreateControlsIntoGroup();
            MoveJoinControlsIntoGroup();
        }

        private void MoveCreateControlsIntoGroup()
        {
            EnsureParent(txtClassName, grpCreate);
            EnsureParent(btnCreate, grpCreate);

            txtClassName.Location = new Point(18, 40);
            txtClassName.Size = new Size(230, 30);

            btnCreate.Location = new Point(18, 88);
            btnCreate.Size = new Size(110, 38);
        }

        private void MoveJoinControlsIntoGroup()
        {
            EnsureParent(txtInviteCode, grpJoin);
            EnsureParent(btnJoin, grpJoin);

            txtInviteCode.Location = new Point(18, 40);
            txtInviteCode.Size = new Size(230, 30);

            btnJoin.Location = new Point(18, 88);
            btnJoin.Size = new Size(110, 38);
        }

        private void EnsureParent(Control control, Control newParent)
        {
            if (control == null || newParent == null) return;
            if (control.Parent == newParent) return;

            Control oldParent = control.Parent;
            if (oldParent != null)
                oldParent.Controls.Remove(control);

            newParent.Controls.Add(control);
        }

        private void StyleTextBox(TextBox txt)
        {
            txt.Font = new Font("Segoe UI", 10F);
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

        private void FrmClassPicker_Load(object sender, EventArgs e)
        {
            LoadMyClasses();
        }

        private void LoadMyClasses()
        {
            using (var db = new DataClasses1DataContext())
            {
                var data = (from uc in db.UserClassrooms
                            join c in db.Classrooms on uc.ClassId equals c.ClassId
                            where uc.UserId == AppSession.CurrentUserId && uc.IsActive
                            orderby c.ClassId descending
                            select new
                            {
                                c.ClassId,
                                c.ClassName,
                                uc.Role,
                                uc.JoinedAt
                            }).ToList();

                dgvClasses.DataSource = data;

                if (dgvClasses.Columns["ClassId"] != null)
                    dgvClasses.Columns["ClassId"].Visible = false;

                if (dgvClasses.Columns["ClassName"] != null)
                    dgvClasses.Columns["ClassName"].HeaderText = "Tên lớp";

                if (dgvClasses.Columns["Role"] != null)
                    dgvClasses.Columns["Role"].HeaderText = "Vai trò";

                if (dgvClasses.Columns["JoinedAt"] != null)
                {
                    dgvClasses.Columns["JoinedAt"].HeaderText = "Ngày tham gia";
                    dgvClasses.Columns["JoinedAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
        }

        private int? GetSelectedClassIdFromGrid()
        {
            if (dgvClasses.CurrentRow == null) return null;
            var cell = dgvClasses.CurrentRow.Cells["ClassId"];
            if (cell?.Value == null) return null;
            return Convert.ToInt32(cell.Value);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var name = (txtClassName.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Vui lòng nhập tên lớp.");
                    txtClassName.Focus();
                    return;
                }

                int newClassId;

                using (var db = new DataClasses1DataContext())
                {
                    string invite;
                    do
                    {
                        invite = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                    } while (db.Classrooms.Any(x => x.InviteCode == invite));

                    var c = new Classroom
                    {
                        ClassName = name,
                        InviteCode = invite,
                        OwnerUserId = AppSession.CurrentUserId
                    };
                    db.Classrooms.InsertOnSubmit(c);
                    db.SubmitChanges();
                    newClassId = c.ClassId;

                    var uc = new UserClassroom
                    {
                        UserId = AppSession.CurrentUserId,
                        ClassId = newClassId,
                        Role = "Owner",
                        JoinedAt = DateTime.Now,
                        IsActive = true
                    };
                    db.UserClassrooms.InsertOnSubmit(uc);
                    db.SubmitChanges();
                }

                AppSession.CurrentClassId = newClassId;
                SelectedClassId = newClassId;

                txtClassName.Text = "";
                LoadMyClasses();

                MessageBox.Show("Tạo lớp thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            var id = GetSelectedClassIdFromGrid();
            if (id == null)
            {
                MessageBox.Show("Vui lòng chọn 1 lớp.");
                return;
            }

            SelectedClassId = id.Value;
            AppSession.CurrentClassId = id.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void dgvClasses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnSelect_Click(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            using (var f = new FrmClassrooms(AppSession.CurrentUserId, onboardingCreateFirst: false, selectOnly: false))
            {
                f.ShowDialog();
            }

            LoadMyClasses();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            var code = (txtInviteCode.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Vui lòng nhập mã mời (Invite Code).");
                txtInviteCode.Focus();
                return;
            }

            try
            {
                int joinedClassId;

                using (var db = new DataClasses1DataContext())
                {
                    var cls = db.Classrooms.FirstOrDefault(c => c.InviteCode == code);
                    if (cls == null)
                    {
                        MessageBox.Show("Mã mời không đúng hoặc lớp không tồn tại.");
                        return;
                    }

                    joinedClassId = cls.ClassId;

                    var existed = db.UserClassrooms.FirstOrDefault(x =>
                        x.UserId == AppSession.CurrentUserId && x.ClassId == joinedClassId);

                    if (existed != null)
                    {
                        if (!existed.IsActive)
                        {
                            existed.IsActive = true;
                            existed.JoinedAt = DateTime.Now;
                            if (string.IsNullOrWhiteSpace(existed.Role))
                                existed.Role = "Member";
                            db.SubmitChanges();
                        }

                        MessageBox.Show("Bạn đã tham gia lớp này rồi.");
                    }
                    else
                    {
                        var uc = new UserClassroom
                        {
                            UserId = AppSession.CurrentUserId,
                            ClassId = joinedClassId,
                            Role = "Member",
                            JoinedAt = DateTime.Now,
                            IsActive = true
                        };
                        db.UserClassrooms.InsertOnSubmit(uc);
                        db.SubmitChanges();

                        MessageBox.Show("Tham gia lớp thành công!");
                    }
                }

                AppSession.CurrentClassId = joinedClassId;
                SelectedClassId = joinedClassId;

                txtInviteCode.Text = "";
                LoadMyClasses();

                // Nếu muốn join xong vào luôn lớp thì bỏ comment 2 dòng dưới:
                // DialogResult = DialogResult.OK;
                // Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tham gia lớp", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmClassPicker_Load_1(object sender, EventArgs e)
        {
        }
    }
}