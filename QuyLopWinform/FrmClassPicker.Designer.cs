namespace QuyLopWinform
{
    partial class FrmClassPicker
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
            this.dgvClasses = new System.Windows.Forms.DataGridView();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnManage = new System.Windows.Forms.Button();
            this.txtInviteCode = new System.Windows.Forms.TextBox();
            this.btnJoin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvClasses
            // 
            this.dgvClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClasses.Location = new System.Drawing.Point(358, 29);
            this.dgvClasses.Name = "dgvClasses";
            this.dgvClasses.Size = new System.Drawing.Size(406, 236);
            this.dgvClasses.TabIndex = 0;
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(52, 68);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(119, 20);
            this.txtClassName.TabIndex = 1;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(12, 94);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(98, 30);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Tạo";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(127, 94);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(82, 30);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "chọn";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnClose.Location = new System.Drawing.Point(695, 274);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(89, 28);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnManage
            // 
            this.btnManage.Location = new System.Drawing.Point(234, 12);
            this.btnManage.Name = "btnManage";
            this.btnManage.Size = new System.Drawing.Size(101, 43);
            this.btnManage.TabIndex = 5;
            this.btnManage.Text = "quản lý lớp";
            this.btnManage.UseVisualStyleBackColor = true;
            this.btnManage.Click += new System.EventHandler(this.btnManage_Click);
            // 
            // txtInviteCode
            // 
            this.txtInviteCode.Location = new System.Drawing.Point(52, 205);
            this.txtInviteCode.Name = "txtInviteCode";
            this.txtInviteCode.Size = new System.Drawing.Size(100, 20);
            this.txtInviteCode.TabIndex = 6;
            // 
            // btnJoin
            // 
            this.btnJoin.Location = new System.Drawing.Point(52, 231);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(100, 34);
            this.btnJoin.TabIndex = 7;
            this.btnJoin.Text = "Tham gia";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(63, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 26);
            this.label1.TabIndex = 8;
            this.label1.Text = "Tạo Lớp";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Tham Gia lớp";
            // 
            // FrmClassPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 305);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.txtInviteCode);
            this.Controls.Add(this.btnManage);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.dgvClasses);
            this.Name = "FrmClassPicker";
            this.Text = "FrmClassPicker";
            this.Load += new System.EventHandler(this.FrmClassPicker_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvClasses;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnManage;
        private System.Windows.Forms.TextBox txtInviteCode;
        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}