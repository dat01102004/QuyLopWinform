using System;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmClassrooms : Form
    {
        // user đang login (giữ tên biến _ownerUserId để khỏi sửa nhiều)
        private int _ownerUserId;
        private bool _onboardingCreateFirst;
        private bool _selectOnly;

        // classId được chọn / vừa tạo xong
        public int? SelectedClassId { get; private set; }

        // ctor rỗng để Designer mở
        public FrmClassrooms()
        {
            InitializeComponent();
            SetupGrid();
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
                Text = "Tạo lớp mới";
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }

            if (_selectOnly)
            {
                Text = "Chọn lớp (double click để vào)";
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                // chọn lớp thì không cần nâng/hạ role
                if (btnPromote != null) btnPromote.Enabled = false;
                if (btnDemote != null) btnDemote.Enabled = false;
            }
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

            // grid user trong lớp
            dgvUsers.AutoGenerateColumns = true;
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
        }

        private void WireEvents()
        {
            this.Load -= FrmClassrooms_Load;
            this.Load += FrmClassrooms_Load;

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

            // nâng/hạ quyền
            btnPromote.Click -= btnPromote_Click;
            btnPromote.Click += btnPromote_Click;

            btnDemote.Click -= btnDemote_Click;
            btnDemote.Click += btnDemote_Click;
        }

        private void FrmClassrooms_Load(object sender, EventArgs e)
        {
            LoadData();
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
            }

            // clear user grid khi reload list lớp
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

                if (dgvUsers.Columns["JoinedAt"] != null)
                    dgvUsers.Columns["JoinedAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            // chỉ Owner mới được bấm nâng/hạ
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

                // load danh sách user của lớp đang chọn
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