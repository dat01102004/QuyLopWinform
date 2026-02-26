using System;
using System.Linq;
using System.Windows.Forms;
using LopFund.DAL;

namespace QuyLopWinform
{
    public partial class FrmClassPicker : Form
    {
        public int? SelectedClassId { get; private set; }

        public FrmClassPicker()
        {
            InitializeComponent();
            SetupGrid();
            WireEvents();
        }

        private void WireEvents()
        {
            this.Load -= FrmClassPicker_Load;
            this.Load += FrmClassPicker_Load;

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
        }

        private void SetupGrid()
        {
            dgvClasses.AutoGenerateColumns = true;
            dgvClasses.ReadOnly = true;
            dgvClasses.AllowUserToAddRows = false;
            dgvClasses.AllowUserToDeleteRows = false;
            dgvClasses.MultiSelect = false;
            dgvClasses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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

                if (dgvClasses.Columns["JoinedAt"] != null)
                    dgvClasses.Columns["JoinedAt"].DefaultCellStyle.Format = "dd/MM/yyyy";
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

                using (var db = new DataClasses1DataContext())
                {
                    var invite = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

                    // tạo lớp
                    var c = new Classroom
                    {
                        ClassName = name,
                        InviteCode = invite,
                        OwnerUserId = AppSession.CurrentUserId
                    };
                    db.Classrooms.InsertOnSubmit(c);
                    db.SubmitChanges();

                    // join user vào lớp (Owner)
                    var uc = new UserClassroom
                    {
                        UserId = AppSession.CurrentUserId,
                        ClassId = c.ClassId,
                        Role = "Owner",
                        JoinedAt = DateTime.Now,
                        IsActive = true
                    };
                    db.UserClassrooms.InsertOnSubmit(uc);
                    db.SubmitChanges();

                    // set luôn class hiện tại
                    AppSession.CurrentClassId = c.ClassId;
                    SelectedClassId = c.ClassId;
                }

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
            // double click = chọn nhanh
            btnSelect_Click(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            // mở quản lý lớp (tạo/sửa/xoá)
            using (var f = new FrmClassrooms(AppSession.CurrentUserId, onboardingCreateFirst: false, selectOnly: false))
            {
                f.ShowDialog();
            }

            // quay lại picker thì reload danh sách lớp
            LoadMyClasses();
        }
    }
}