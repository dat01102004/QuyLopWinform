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
            this.dgvClassrooms.Location = new System.Drawing.Point(525, 58);
            this.dgvClassrooms.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvClassrooms.Name = "dgvClassrooms";
            this.dgvClassrooms.RowHeadersWidth = 51;
            this.dgvClassrooms.Size = new System.Drawing.Size(639, 341);
            this.dgvClassrooms.TabIndex = 0;
            this.dgvClassrooms.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvClassrooms_CellContentClick);
            // 
            // txtClassId
            // 
            this.txtClassId.Location = new System.Drawing.Point(124, 140);
            this.txtClassId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtClassId.Name = "txtClassId";
            this.txtClassId.ReadOnly = true;
            this.txtClassId.Size = new System.Drawing.Size(148, 30);
            this.txtClassId.TabIndex = 1;
            this.txtClassId.TextChanged += new System.EventHandler(this.txtClassId_TextChanged);
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(124, 207);
            this.txtClassName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(148, 30);
            this.txtClassName.TabIndex = 2;
            this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
            // 
            // txtInviteCode
            // 
            this.txtInviteCode.Location = new System.Drawing.Point(124, 285);
            this.txtInviteCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInviteCode.Name = "txtInviteCode";
            this.txtInviteCode.Size = new System.Drawing.Size(148, 30);
            this.txtInviteCode.TabIndex = 3;
            this.txtInviteCode.TextChanged += new System.EventHandler(this.txtInviteCode_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(46, 359);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(112, 41);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Thêm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(183, 359);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(112, 41);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "Sửa";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(328, 359);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(112, 41);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "xoá";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(18, 37);
            this.btnReload.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(112, 41);
            this.btnReload.TabIndex = 7;
            this.btnReload.Text = "Tải lại";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // OwnerUserId
            // 
            this.OwnerUserId.AutoSize = true;
            this.OwnerUserId.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.OwnerUserId.Location = new System.Drawing.Point(376, 34);
            this.OwnerUserId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OwnerUserId.Name = "OwnerUserId";
            this.OwnerUserId.Size = new System.Drawing.Size(0, 38);
            this.OwnerUserId.TabIndex = 8;
            this.OwnerUserId.Click += new System.EventHandler(this.OwnerUserId_Click);
            // 
            // txtOwnerUserId
            // 
            this.txtOwnerUserId.Location = new System.Drawing.Point(276, 94);
            this.txtOwnerUserId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOwnerUserId.Name = "txtOwnerUserId";
            this.txtOwnerUserId.ReadOnly = true;
            this.txtOwnerUserId.Size = new System.Drawing.Size(148, 30);
            this.txtOwnerUserId.TabIndex = 9;
            this.txtOwnerUserId.Visible = false;
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(66, 456);
            this.dgvUsers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(639, 287);
            this.dgvUsers.TabIndex = 10;
            this.dgvUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
            // 
            // btnDemote
            // 
            this.btnDemote.Location = new System.Drawing.Point(730, 625);
            this.btnDemote.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDemote.Name = "btnDemote";
            this.btnDemote.Size = new System.Drawing.Size(112, 41);
            this.btnDemote.TabIndex = 11;
            this.btnDemote.Text = "Hạ Quyền";
            this.btnDemote.UseVisualStyleBackColor = true;
            this.btnDemote.Click += new System.EventHandler(this.btnDemote_Click);
            // 
            // btnPromote
            // 
            this.btnPromote.Location = new System.Drawing.Point(730, 545);
            this.btnPromote.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPromote.Name = "btnPromote";
            this.btnPromote.Size = new System.Drawing.Size(112, 41);
            this.btnPromote.TabIndex = 12;
            this.btnPromote.Text = "Nâng Admin";
            this.btnPromote.UseVisualStyleBackColor = true;
            this.btnPromote.Click += new System.EventHandler(this.btnPromote_Click);
            // 
            // FrmClassrooms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 796);
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
            this.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
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