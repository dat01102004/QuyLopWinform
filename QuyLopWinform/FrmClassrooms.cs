using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmClassrooms : BaseForm
    {
        // user đang login (giữ tên biến _ownerUserId để khỏi sửa nhiều)
        private int _ownerUserId;
        private bool _onboardingCreateFirst;
        private bool _selectOnly;

        // classId được chọn / vừa tạo xong
        public int? SelectedClassId { get; private set; }

        private Label lblHeaderTitle;
        private Label lblHeaderSubTitle;

        private GroupBox grpClassInfo;
        private GroupBox grpClassList;
        private GroupBox grpUsers;

        private Label lblClassIdCaption;
        private Label lblClassNameCaption;
        private Label lblInviteCodeCaption;
        private Label lblOwnerCaption;

        // ctor rỗng để Designer mở
        public FrmClassrooms()
        {
            InitializeComponent();
            BuildExtraUI();
            SetupGrid();
            SetupUI();
            WireEvents();
        }

        // ctor dùng thật khi chạy
        public FrmClassrooms(int ownerUserId, bool onboardingCreateFirst, bool selectOnly) : this()
        {
            _ownerUserId = ownerUserId;
            _onboardingCreateFirst = onboardingCreateFirst;
            _selectOnly = selectOnly;

            txtOwnerUserId.Text = ownerUserId.ToString();
            txtOwnerUserId.ReadOnly = true;

            if (_onboardingCreateFirst)
            {
                this.Text = "Tạo lớp mới";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }

            if (_selectOnly)
            {
                this.Text = "Chọn lớp (double click để vào)";
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                if (btnPromote != null) btnPromote.Enabled = false;
                if (btnDemote != null) btnDemote.Enabled = false;
            }
        }

        private void BuildExtraUI()
        {
            lblHeaderTitle = new Label();
            lblHeaderSubTitle = new Label();

            grpClassInfo = new GroupBox();
            grpClassList = new GroupBox();
            grpUsers = new GroupBox();

            lblClassIdCaption = new Label();
            lblClassNameCaption = new Label();
            lblInviteCodeCaption = new Label();
            lblOwnerCaption = new Label();

            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Text = "Quản lý lớp học";
            lblHeaderTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.BackColor = Color.Transparent;

            lblHeaderSubTitle.AutoSize = true;
            lblHeaderSubTitle.Text = "Tạo lớp, sửa lớp, chọn lớp và quản lý thành viên trong lớp";
            lblHeaderSubTitle.Font = new Font("Segoe UI", 9.5F);
            lblHeaderSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblHeaderSubTitle.BackColor = Color.Transparent;

            InitGroupBox(grpClassInfo, "Thông tin lớp");
            InitGroupBox(grpClassList, _selectOnly ? "Danh sách lớp (double click để chọn)" : "Danh sách lớp");
            InitGroupBox(grpUsers, "Thành viên trong lớp");

            InitCaptionLabel(lblClassIdCaption, "Mã lớp");
            InitCaptionLabel(lblClassNameCaption, "Tên lớp");
            InitCaptionLabel(lblInviteCodeCaption, "Mã mời");
            InitCaptionLabel(lblOwnerCaption, "Owner UserId");

            this.Controls.Add(lblHeaderTitle);
            this.Controls.Add(lblHeaderSubTitle);
            this.Controls.Add(grpClassInfo);
            this.Controls.Add(grpClassList);
            this.Controls.Add(grpUsers);

            grpClassInfo.Controls.Add(lblClassIdCaption);
            grpClassInfo.Controls.Add(lblClassNameCaption);
            grpClassInfo.Controls.Add(lblInviteCodeCaption);
            grpClassInfo.Controls.Add(lblOwnerCaption);

            lblHeaderTitle.BringToFront();
            lblHeaderSubTitle.BringToFront();
        }

        private void InitGroupBox(GroupBox grp, string text)
        {
            grp.Text = text;
            grp.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            grp.ForeColor = Color.FromArgb(31, 41, 55);
            grp.BackColor = Color.White;
            grp.Padding = new Padding(10);
        }

        private void InitCaptionLabel(Label lbl, string text)
        {
            lbl.AutoSize = true;
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(100, 116, 139);
            lbl.BackColor = Color.Transparent;
        }

        private void SetupUI()
        {
            this.Text = "Quản lý lớp";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.FromArgb(244, 247, 251);
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(1180, 720);

            StyleTextBox(txtClassId, true);
            StyleTextBox(txtClassName, false);
            StyleTextBox(txtInviteCode, false);
            StyleTextBox(txtOwnerUserId, true);

            StylePrimaryButton(btnAdd, "Thêm");
            StyleSecondaryButton(btnUpdate, "Sửa");
            StyleDangerButton(btnDelete, "Xóa");
            StyleSecondaryButton(btnReload, "Tải lại");
            StylePrimaryButton(btnPromote, "Nâng Admin");
            StyleSecondaryButton(btnDemote, "Hạ quyền");

            HideOldLabels();

            ArrangeLayout();
        }

        private void HideOldLabels()
        {
            foreach (var lbl in GetAllControls(this).OfType<Label>())
            {
                if (lbl == lblHeaderTitle || lbl == lblHeaderSubTitle ||
                    lbl == lblClassIdCaption || lbl == lblClassNameCaption ||
                    lbl == lblInviteCodeCaption || lbl == lblOwnerCaption)
                    continue;

                string t = (lbl.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(t)) continue;

                if (t.IndexOf("ClassId", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("ClassName", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Invite", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Owner", StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                    t.IndexOf("Thu Quỹ", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    lbl.Visible = false;
                }
            }
        }

        private void ArrangeLayout()
        {
            int margin = 22;
            int gap = 22;
            int leftWidth = 340;
            int rightWidth = this.ClientSize.Width - leftWidth - margin * 2 - gap;

            lblHeaderTitle.Location = new Point(margin, 18);
            lblHeaderSubTitle.Location = new Point(margin, 50);

            grpClassInfo.Location = new Point(margin, 84);
            grpClassInfo.Size = new Size(leftWidth, 290);

            grpClassList.Location = new Point(grpClassInfo.Right + gap, 84);
            grpClassList.Size = new Size(rightWidth, 290);

            grpUsers.Location = new Point(margin, grpClassInfo.Bottom + gap);
            grpUsers.Size = new Size(this.ClientSize.Width - margin * 2, 300);

            ArrangeClassInfoContent();
            ArrangeClassListContent();
            ArrangeUsersContent();
        }

        private void ArrangeClassInfoContent()
        {
            EnsureParent(txtClassId, grpClassInfo);
            EnsureParent(txtClassName, grpClassInfo);
            EnsureParent(txtInviteCode, grpClassInfo);
            EnsureParent(txtOwnerUserId, grpClassInfo);

            EnsureParent(btnReload, grpClassInfo);
            EnsureParent(btnAdd, grpClassInfo);
            EnsureParent(btnUpdate, grpClassInfo);
            EnsureParent(btnDelete, grpClassInfo);

            int labelX = 18;
            int inputX = 18;
            int inputWidth = grpClassInfo.ClientSize.Width - 36;
            int rowGap = 58;

            lblClassIdCaption.Location = new Point(labelX, 34);
            txtClassId.Location = new Point(inputX, 56);
            txtClassId.Size = new Size(inputWidth, 30);

            lblClassNameCaption.Location = new Point(labelX, 92);
            txtClassName.Location = new Point(inputX, 114);
            txtClassName.Size = new Size(inputWidth, 30);

            lblInviteCodeCaption.Location = new Point(labelX, 150);
            txtInviteCode.Location = new Point(inputX, 172);
            txtInviteCode.Size = new Size(inputWidth, 30);

            lblOwnerCaption.Location = new Point(labelX, 208);
            txtOwnerUserId.Location = new Point(inputX, 230);
            txtOwnerUserId.Size = new Size(inputWidth, 30);

            btnReload.Size = new Size(110, 38);
            btnAdd.Size = new Size(90, 38);
            btnUpdate.Size = new Size(90, 38);
            btnDelete.Size = new Size(90, 38);

            btnReload.Location = new Point(grpClassInfo.ClientSize.Width - btnReload.Width - 18, 18);

            int actionY = grpClassInfo.ClientSize.Height - 56;
            int spacing = 12;
            int totalWidth = btnAdd.Width + spacing + btnUpdate.Width + spacing + btnDelete.Width;
            int startX = (grpClassInfo.ClientSize.Width - totalWidth) / 2;

            btnAdd.Location = new Point(startX, actionY);
            btnUpdate.Location = new Point(btnAdd.Right + spacing, actionY);
            btnDelete.Location = new Point(btnUpdate.Right + spacing, actionY);
        }

        private void ArrangeClassListContent()
        {
            EnsureParent(dgvClassrooms, grpClassList);

            dgvClassrooms.Location = new Point(16, 34);
            dgvClassrooms.Size = new Size(grpClassList.ClientSize.Width - 32, grpClassList.ClientSize.Height - 50);
        }

        private void ArrangeUsersContent()
        {
            EnsureParent(dgvUsers, grpUsers);
            EnsureParent(btnPromote, grpUsers);
            EnsureParent(btnDemote, grpUsers);

            dgvUsers.Location = new Point(16, 34);
            dgvUsers.Size = new Size(grpUsers.ClientSize.Width - 190, grpUsers.ClientSize.Height - 50);

            btnPromote.Size = new Size(130, 40);
            btnDemote.Size = new Size(130, 40);

            int sideX = dgvUsers.Right + 16;
            btnPromote.Location = new Point(sideX, 70);
            btnDemote.Location = new Point(sideX, 122);
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

        private void StyleTextBox(TextBox txt, bool readOnly)
        {
            txt.Font = new Font("Segoe UI", 10F);
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = readOnly ? Color.FromArgb(248, 250, 252) : Color.White;
            txt.ForeColor = Color.FromArgb(31, 41, 55);
            txt.ReadOnly = readOnly;
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

        private void SetupGrid()
        {
            // grid lớp
            dgvClassrooms.AutoGenerateColumns = true;
            dgvClassrooms.ReadOnly = true;
            dgvClassrooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClassrooms.MultiSelect = false;
            dgvClassrooms.AllowUserToAddRows = false;
            dgvClassrooms.AllowUserToDeleteRows = false;
            dgvClassrooms.RowHeadersVisible = false;
            dgvClassrooms.BackgroundColor = Color.White;
            dgvClassrooms.BorderStyle = BorderStyle.FixedSingle;
            dgvClassrooms.EnableHeadersVisualStyles = false;
            dgvClassrooms.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvClassrooms.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvClassrooms.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClassrooms.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvClassrooms.ColumnHeadersHeight = 36;
            dgvClassrooms.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvClassrooms.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dgvClassrooms.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvClassrooms.RowTemplate.Height = 30;
            dgvClassrooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // grid user trong lớp
            dgvUsers.AutoGenerateColumns = true;
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.FixedSingle;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 99, 235);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvUsers.ColumnHeadersHeight = 36;
            dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvUsers.RowTemplate.Height = 30;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void WireEvents()
        {
            this.Load -= FrmClassrooms_Load;
            this.Load += FrmClassrooms_Load;

            this.Shown -= FrmClassrooms_Shown;
            this.Shown += FrmClassrooms_Shown;

            this.Resize -= FrmClassrooms_Resize;
            this.Resize += FrmClassrooms_Resize;

            btnReload.Click -= btnReload_Click;
            btnReload.Click += btnReload_Click;

            btnAdd.Click -= btnAdd_Click;
            btnAdd.Click += btnAdd_Click;

            btnUpdate.Click -= btnUpdate_Click;
            btnUpdate.Click += btnUpdate_Click;

            btnDelete.Click -= btnDelete_Click;
            btnDelete.Click += btnDelete_Click;

            dgvClassrooms.SelectionChanged -= dgvClassrooms_SelectionChanged;
            dgvClassrooms.SelectionChanged += dgvClassrooms_SelectionChanged;

            dgvClassrooms.CellDoubleClick -= dgvClassrooms_CellDoubleClick;
            dgvClassrooms.CellDoubleClick += dgvClassrooms_CellDoubleClick;

            btnPromote.Click -= btnPromote_Click;
            btnPromote.Click += btnPromote_Click;

            btnDemote.Click -= btnDemote_Click;
            btnDemote.Click += btnDemote_Click;
        }

        private void FrmClassrooms_Shown(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmClassrooms_Resize(object sender, EventArgs e)
        {
            ArrangeLayout();
        }

        private void FrmClassrooms_Load(object sender, EventArgs e)
        {
            LoadData();
            ArrangeLayout();
        }

        /// <summary>
        /// Load lớp theo bảng UserClassrooms (user có thể là Owner hoặc Member)
        /// </summary>
        private void LoadData()
        {
            using (var db = new DataClasses1DataContext())
            {
                var data = (from uc in db.UserClassrooms
                            join c in db.Classrooms on uc.ClassId equals c.ClassId
                            where uc.UserId == _ownerUserId && uc.IsActive
                            orderby c.ClassId descending
                            select new
                            {
                                c.ClassId,
                                c.ClassName,
                                c.InviteCode,
                                c.OwnerUserId,
                                uc.Role,
                                uc.JoinedAt
                            }).ToList();

                dgvClassrooms.DataSource = data;

                if (dgvClassrooms.Columns["ClassId"] != null)
                    dgvClassrooms.Columns["ClassId"].Visible = false;

                if (dgvClassrooms.Columns["ClassName"] != null)
                    dgvClassrooms.Columns["ClassName"].HeaderText = "Tên lớp";

                if (dgvClassrooms.Columns["InviteCode"] != null)
                    dgvClassrooms.Columns["InviteCode"].HeaderText = "Mã mời";

                if (dgvClassrooms.Columns["OwnerUserId"] != null)
                    dgvClassrooms.Columns["OwnerUserId"].HeaderText = "Owner";

                if (dgvClassrooms.Columns["Role"] != null)
                    dgvClassrooms.Columns["Role"].HeaderText = "Vai trò";

                if (dgvClassrooms.Columns["JoinedAt"] != null)
                {
                    dgvClassrooms.Columns["JoinedAt"].HeaderText = "Ngày tham gia";
                    dgvClassrooms.Columns["JoinedAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }

            dgvUsers.DataSource = null;
        }

        private void LoadUsersInClass(int classId)
        {
            using (var db = new DataClasses1DataContext())
            {
                var data = (from uc in db.UserClassrooms
                            join u in db.Users on uc.UserId equals u.UserId
                            where uc.ClassId == classId && uc.IsActive
                            orderby uc.Role descending, u.FullName
                            select new
                            {
                                uc.UserId,
                                u.FullName,
                                u.Email,
                                u.Phone,
                                uc.Role,
                                uc.JoinedAt
                            }).ToList();

                dgvUsers.DataSource = data;

                if (dgvUsers.Columns["UserId"] != null)
                    dgvUsers.Columns["UserId"].Visible = false;

                if (dgvUsers.Columns["FullName"] != null)
                    dgvUsers.Columns["FullName"].HeaderText = "Họ và tên";

                if (dgvUsers.Columns["Email"] != null)
                    dgvUsers.Columns["Email"].HeaderText = "Email";

                if (dgvUsers.Columns["Phone"] != null)
                    dgvUsers.Columns["Phone"].HeaderText = "SĐT";

                if (dgvUsers.Columns["Role"] != null)
                    dgvUsers.Columns["Role"].HeaderText = "Vai trò";

                if (dgvUsers.Columns["JoinedAt"] != null)
                {
                    dgvUsers.Columns["JoinedAt"].HeaderText = "Ngày tham gia";
                    dgvUsers.Columns["JoinedAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }

            bool canChangeRole = IsOwnerOfClass(classId);
            btnPromote.Enabled = canChangeRole;
            btnDemote.Enabled = canChangeRole;
        }

        private int? GetSelectedUserId()
        {
            if (dgvUsers.CurrentRow == null) return null;
            var cell = dgvUsers.CurrentRow.Cells["UserId"];
            if (cell?.Value == null) return null;
            return Convert.ToInt32(cell.Value);
        }

        private int GetInt(TextBox txt, string fieldName)
        {
            if (!int.TryParse(txt.Text.Trim(), out int v))
                throw new Exception($"{fieldName} phải là số.");
            return v;
        }

        private void ClearInputs()
        {
            txtClassId.Text = "";
            txtClassName.Text = "";
            txtInviteCode.Text = "";
        }

        private void dgvClassrooms_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClassrooms.CurrentRow == null) return;

            txtClassId.Text = dgvClassrooms.CurrentRow.Cells["ClassId"].Value?.ToString();
            txtClassName.Text = dgvClassrooms.CurrentRow.Cells["ClassName"].Value?.ToString();
            txtInviteCode.Text = dgvClassrooms.CurrentRow.Cells["InviteCode"].Value?.ToString();
            txtOwnerUserId.Text = dgvClassrooms.CurrentRow.Cells["OwnerUserId"].Value?.ToString();

            if (int.TryParse(txtClassId.Text, out int id))
            {
                SelectedClassId = id;
                LoadUsersInClass(id);
            }
        }

        private void dgvClassrooms_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_selectOnly) return;
            if (dgvClassrooms.CurrentRow == null) return;

            var idObj = dgvClassrooms.CurrentRow.Cells["ClassId"].Value;
            if (idObj == null) return;

            SelectedClassId = Convert.ToInt32(idObj);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private bool IsOwnerOfClass(int classId)
        {
            using (var db = new DataClasses1DataContext())
            {
                return db.Classrooms.Any(c => c.ClassId == classId && c.OwnerUserId == _ownerUserId);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int ownerId = GetInt(txtOwnerUserId, "OwnerUserId");

                if (string.IsNullOrWhiteSpace(txtClassName.Text))
                    throw new Exception("Tên lớp không được trống.");

                var invite = (txtInviteCode.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(invite))
                    invite = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

                using (var db = new DataClasses1DataContext())
                {
                    var c = new Classroom
                    {
                        ClassName = txtClassName.Text.Trim(),
                        InviteCode = invite,
                        OwnerUserId = ownerId
                    };

                    db.Classrooms.InsertOnSubmit(c);
                    db.SubmitChanges();

                    var uc = new UserClassroom
                    {
                        UserId = ownerId,
                        ClassId = c.ClassId,
                        Role = "Owner",
                        JoinedAt = DateTime.Now,
                        IsActive = true
                    };

                    db.UserClassrooms.InsertOnSubmit(uc);
                    db.SubmitChanges();

                    SelectedClassId = c.ClassId;
                    txtClassId.Text = c.ClassId.ToString();
                    txtInviteCode.Text = invite;
                }

                LoadData();
                MessageBox.Show("Thêm lớp thành công!");

                if (_onboardingCreateFirst && SelectedClassId.HasValue)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int classId = GetInt(txtClassId, "ClassId");

                if (!IsOwnerOfClass(classId))
                    throw new Exception("Chỉ Owner mới được sửa lớp.");

                using (var db = new DataClasses1DataContext())
                {
                    var c = db.Classrooms.FirstOrDefault(x => x.ClassId == classId);
                    if (c == null) throw new Exception("Không tìm thấy lớp.");

                    c.ClassName = txtClassName.Text.Trim();
                    c.InviteCode = txtInviteCode.Text.Trim();

                    db.SubmitChanges();
                }

                LoadData();
                MessageBox.Show("Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int classId = GetInt(txtClassId, "ClassId");

                if (!IsOwnerOfClass(classId))
                    throw new Exception("Chỉ Owner mới được xoá lớp.");

                var ok = MessageBox.Show("Bạn chắc chắn muốn xoá lớp này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ok != DialogResult.Yes) return;

                using (var db = new DataClasses1DataContext())
                {
                    var links = db.UserClassrooms.Where(x => x.ClassId == classId).ToList();
                    db.UserClassrooms.DeleteAllOnSubmit(links);

                    var c = db.Classrooms.FirstOrDefault(x => x.ClassId == classId);
                    if (c != null) db.Classrooms.DeleteOnSubmit(c);

                    db.SubmitChanges();
                }

                LoadData();
                MessageBox.Show("Xoá thành công!");
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== nâng quyền Member -> Admin =====
        private void btnPromote_Click(object sender, EventArgs e)
        {
            if (!SelectedClassId.HasValue)
            {
                MessageBox.Show("Chọn lớp trước.");
                return;
            }

            int classId = SelectedClassId.Value;

            if (!IsOwnerOfClass(classId))
            {
                MessageBox.Show("Chỉ Owner mới được nâng quyền Admin.");
                return;
            }

            var userId = GetSelectedUserId();
            if (userId == null)
            {
                MessageBox.Show("Chọn 1 người để nâng quyền.");
                return;
            }

            if (userId.Value == _ownerUserId)
            {
                MessageBox.Show("Bạn là Owner, không cần nâng.");
                return;
            }

            using (var db = new DataClasses1DataContext())
            {
                var uc = db.UserClassrooms.FirstOrDefault(x =>
                    x.UserId == userId.Value && x.ClassId == classId && x.IsActive);

                if (uc == null)
                {
                    MessageBox.Show("Không tìm thấy user trong lớp.");
                    return;
                }

                if (uc.Role == "Owner")
                {
                    MessageBox.Show("Không thể đổi quyền Owner.");
                    return;
                }

                uc.Role = "Admin";
                db.SubmitChanges();
            }

            LoadUsersInClass(classId);
            MessageBox.Show("Đã nâng quyền Admin.");
        }

        // ===== hạ quyền Admin -> Member =====
        private void btnDemote_Click(object sender, EventArgs e)
        {
            if (!SelectedClassId.HasValue)
            {
                MessageBox.Show("Chọn lớp trước.");
                return;
            }

            int classId = SelectedClassId.Value;

            if (!IsOwnerOfClass(classId))
            {
                MessageBox.Show("Chỉ Owner mới được hạ quyền.");
                return;
            }

            var userId = GetSelectedUserId();
            if (userId == null)
            {
                MessageBox.Show("Chọn 1 người để hạ quyền.");
                return;
            }

            if (userId.Value == _ownerUserId)
            {
                MessageBox.Show("Không thể hạ quyền Owner.");
                return;
            }

            using (var db = new DataClasses1DataContext())
            {
                var uc = db.UserClassrooms.FirstOrDefault(x =>
                    x.UserId == userId.Value && x.ClassId == classId && x.IsActive);

                if (uc == null)
                {
                    MessageBox.Show("Không tìm thấy user trong lớp.");
                    return;
                }

                if (uc.Role == "Owner")
                {
                    MessageBox.Show("Không thể đổi quyền Owner.");
                    return;
                }

                uc.Role = "Member";
                db.SubmitChanges();
            }

            LoadUsersInClass(classId);
            MessageBox.Show("Đã hạ về Member.");
        }

        // ====== giữ các event stub để Designer không lỗi ======
        private void dgvClassrooms_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtClassId_TextChanged(object sender, EventArgs e) { }
        private void txtClassName_TextChanged(object sender, EventArgs e) { }
        private void txtInviteCode_TextChanged(object sender, EventArgs e) { }
        private void OwnerUserId_Click(object sender, EventArgs e) { }
        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}