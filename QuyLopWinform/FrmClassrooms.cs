using System;
using System.Linq;
using System.Windows.Forms;
using LopFund.BLL;

namespace QuyLopWinform
{
    public partial class FrmClassrooms : Form
    {
        private readonly ClassroomBLL _bll = new ClassroomBLL();

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

            // set owner lên textbox (nếu có)
            txtOwnerUserId.Text = ownerUserId.ToString();
            txtOwnerUserId.ReadOnly = true;

            // mode
            if (_onboardingCreateFirst)
            {
                Text = "Tạo lớp mới";
                // bạn vẫn có thể cho xem grid, nhưng thường onboarding chỉ cần tạo
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

        private void LoadData()
        {
            var data = _bll.GetAll()
                           .Where(c => c.OwnerUserId == _ownerUserId)
                           .OrderByDescending(c => c.ClassId)
                           .ToList();

            dgvClassrooms.DataSource = data;
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
            // txtOwnerUserId giữ nguyên vì là owner
        }

        private void dgvClassrooms_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClassrooms.CurrentRow == null) return;

            // lấy theo tên cột entity (ClassId, ClassName, InviteCode, OwnerUserId)
            txtClassId.Text = dgvClassrooms.CurrentRow.Cells["ClassId"].Value?.ToString();
            txtClassName.Text = dgvClassrooms.CurrentRow.Cells["ClassName"].Value?.ToString();
            txtInviteCode.Text = dgvClassrooms.CurrentRow.Cells["InviteCode"].Value?.ToString();
            txtOwnerUserId.Text = dgvClassrooms.CurrentRow.Cells["OwnerUserId"].Value?.ToString();

            // cập nhật SelectedClassId theo dòng chọn
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int ownerId = GetInt(txtOwnerUserId, "OwnerUserId");

                if (string.IsNullOrWhiteSpace(txtClassName.Text))
                    throw new Exception("Tên lớp không được trống.");
                if (string.IsNullOrWhiteSpace(txtInviteCode.Text))
                    throw new Exception("InviteCode không được trống.");

                // Add (BLL hiện tại của bạn có thể trả void => OK)
                _bll.Add(txtClassName.Text.Trim(), txtInviteCode.Text.Trim(), ownerId);

                // reload để lấy Id mới sinh
                LoadData();

                // lấy class mới nhất theo Owner
                var newest = _bll.GetAll()
                                 .Where(c => c.OwnerUserId == ownerId)
                                 .OrderByDescending(c => c.ClassId)
                                 .FirstOrDefault();

                if (newest != null)
                {
                    SelectedClassId = newest.ClassId;
                    txtClassId.Text = newest.ClassId.ToString();
                }

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
                int ownerId = GetInt(txtOwnerUserId, "OwnerUserId");

                _bll.Update(classId, txtClassName.Text.Trim(), txtInviteCode.Text.Trim(), ownerId);

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

                var ok = MessageBox.Show("Bạn chắc chắn muốn xoá lớp này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ok != DialogResult.Yes) return;

                _bll.Delete(classId);

                LoadData();
                MessageBox.Show("Xoá thành công!");
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvClassrooms_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtClassId_TextChanged(object sender, EventArgs e) { }
        private void txtClassName_TextChanged(object sender, EventArgs e) { }
        private void txtInviteCode_TextChanged(object sender, EventArgs e) { }
        private void OwnerUserId_Click(object sender, EventArgs e) { }

    }
}
