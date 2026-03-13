namespace QuyLopWinform
{
    partial class FrmClassrooms
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvClassrooms = new System.Windows.Forms.DataGridView();
            this.txtClassId = new System.Windows.Forms.TextBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.txtInviteCode = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.OwnerUserId = new System.Windows.Forms.Label();
            this.txtOwnerUserId = new System.Windows.Forms.TextBox();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnDemote = new System.Windows.Forms.Button();
            this.btnPromote = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClassrooms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvClassrooms
            // 
            this.dgvClassrooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClassrooms.Location = new System.Drawing.Point(350, 33);
            this.dgvClassrooms.Name = "dgvClassrooms";
            this.dgvClassrooms.Size = new System.Drawing.Size(426, 193);
            this.dgvClassrooms.TabIndex = 0;
            this.dgvClassrooms.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClassrooms_CellContentClick);
            // 
            // txtClassId
            // 
            this.txtClassId.Location = new System.Drawing.Point(83, 79);
            this.txtClassId.Name = "txtClassId";
            this.txtClassId.ReadOnly = true;
            this.txtClassId.Size = new System.Drawing.Size(100, 20);
            this.txtClassId.TabIndex = 1;
            this.txtClassId.TextChanged += new System.EventHandler(this.txtClassId_TextChanged);
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(83, 117);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(100, 20);
            this.txtClassName.TabIndex = 2;
            this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
            // 
            // txtInviteCode
            // 
            this.txtInviteCode.Location = new System.Drawing.Point(83, 161);
            this.txtInviteCode.Name = "txtInviteCode";
            this.txtInviteCode.Size = new System.Drawing.Size(100, 20);
            this.txtInviteCode.TabIndex = 3;
            this.txtInviteCode.TextChanged += new System.EventHandler(this.txtInviteCode_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(31, 203);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(122, 203);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Sửa";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(219, 203);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "xoá";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(12, 21);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 7;
            this.btnReload.Text = "Tải lại";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // OwnerUserId
            // 
            this.OwnerUserId.AutoSize = true;
            this.OwnerUserId.Location = new System.Drawing.Point(216, 26);
            this.OwnerUserId.Name = "OwnerUserId";
            this.OwnerUserId.Size = new System.Drawing.Size(48, 13);
            this.OwnerUserId.TabIndex = 8;
            this.OwnerUserId.Text = "Thủ Quỹ";
            this.OwnerUserId.Click += new System.EventHandler(this.OwnerUserId_Click);
            // 
            // txtOwnerUserId
            // 
            this.txtOwnerUserId.Location = new System.Drawing.Point(184, 53);
            this.txtOwnerUserId.Name = "txtOwnerUserId";
            this.txtOwnerUserId.ReadOnly = true;
            this.txtOwnerUserId.Size = new System.Drawing.Size(100, 20);
            this.txtOwnerUserId.TabIndex = 9;
            this.txtOwnerUserId.Visible = false;
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(44, 258);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(426, 162);
            this.dgvUsers.TabIndex = 10;
            this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            // 
            // btnDemote
            // 
            this.btnDemote.Location = new System.Drawing.Point(487, 353);
            this.btnDemote.Name = "btnDemote";
            this.btnDemote.Size = new System.Drawing.Size(75, 23);
            this.btnDemote.TabIndex = 11;
            this.btnDemote.Text = "Hạ Quyền";
            this.btnDemote.UseVisualStyleBackColor = true;
            this.btnDemote.Click += new System.EventHandler(this.btnDemote_Click);
            // 
            // btnPromote
            // 
            this.btnPromote.Location = new System.Drawing.Point(487, 308);
            this.btnPromote.Name = "btnPromote";
            this.btnPromote.Size = new System.Drawing.Size(75, 23);
            this.btnPromote.TabIndex = 12;
            this.btnPromote.Text = "Nâng Admin";
            this.btnPromote.UseVisualStyleBackColor = true;
            this.btnPromote.Click += new System.EventHandler(this.btnPromote_Click);
            // 
            // FrmClassrooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnPromote);
            this.Controls.Add(this.btnDemote);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.txtOwnerUserId);
            this.Controls.Add(this.OwnerUserId);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtInviteCode);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.txtClassId);
            this.Controls.Add(this.dgvClassrooms);
            this.Name = "FrmClassrooms";
            this.Text = "FrmClassrooms";
            this.Load += new System.EventHandler(this.FrmClassrooms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClassrooms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvClassrooms;
        private System.Windows.Forms.TextBox txtClassId;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.TextBox txtInviteCode;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label OwnerUserId;
        private System.Windows.Forms.TextBox txtOwnerUserId;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnDemote;
        private System.Windows.Forms.Button btnPromote;
    }
}