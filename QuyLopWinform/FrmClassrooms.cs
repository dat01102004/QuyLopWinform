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

            // set user lên textbox (bạn đang đặt tên txtOwnerUserId)
            txtOwnerUserId.Text = ownerUserId.ToString();
            txtOwnerUserId.ReadOnly = true;

            // mode
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
            }
        }

        private void SetupGrid()
        {
            dgvClassrooms.AutoGenerateColumns = true;
            dgvClassrooms.ReadOnly = true;
            dgvClassrooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClassrooms.MultiSelect = false;
            dgvClassrooms.AllowUserToAddRows = false;
            dgvClassrooms.AllowUserToDeleteRows = false;
        }

        private void WireEvents()
        {
            // tránh bị gắn event nhiều lần
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
            // txtOwnerUserId giữ nguyên vì là user đang login
        }

        private void dgvClassrooms_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClassrooms.CurrentRow == null) return;

            // lấy theo tên cột đang bind (ClassId, ClassName, InviteCode, OwnerUserId)
            txtClassId.Text = dgvClassrooms.CurrentRow.Cells["ClassId"].Value?.ToString();
            txtClassName.Text = dgvClassrooms.CurrentRow.Cells["ClassName"].Value?.ToString();
            txtInviteCode.Text = dgvClassrooms.CurrentRow.Cells["InviteCode"].Value?.ToString();
            txtOwnerUserId.Text = dgvClassrooms.CurrentRow.Cells["OwnerUserId"].Value?.ToString();

            if (int.TryParse(txtClassId.Text, out int id))
                SelectedClassId = id;
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

                // Nếu bạn muốn bắt nhập invite code thì giữ check cũ.
                // Ở đây: cho phép bỏ trống -> tự sinh code
                var invite = (txtInviteCode.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(invite))
                    invite = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

                using (var db = new DataClasses1DataContext())
                {
                    // tạo lớp
                    var c = new Classroom
                    {
                        ClassName = txtClassName.Text.Trim(),
                        InviteCode = invite,
                        OwnerUserId = ownerId
                    };

                    db.Classrooms.InsertOnSubmit(c);
                    db.SubmitChanges(); // có c.ClassId

                    // tạo liên kết user - lớp (quan trọng!)
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

                // nếu là onboarding: tạo xong -> đóng form trả về classId
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
                    // xoá liên kết user-class trước
                    var links = db.UserClassrooms.Where(x => x.ClassId == classId).ToList();
                    db.UserClassrooms.DeleteAllOnSubmit(links);

                    // rồi xoá lớp
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

        // ====== giữ các event stub để Designer không lỗi ======
        private void dgvClassrooms_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtClassId_TextChanged(object sender, EventArgs e) { }
        private void txtClassName_TextChanged(object sender, EventArgs e) { }
        private void txtInviteCode_TextChanged(object sender, EventArgs e) { }
        private void OwnerUserId_Click(object sender, EventArgs e) { }
    }
}