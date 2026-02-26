namespace QuyLopWinform
{
    partial class FrmMain
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
            this.grpSummary = new System.Windows.Forms.GroupBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalIn = new System.Windows.Forms.Label();
            this.lblTotalOut = new System.Windows.Forms.Label();
            this.grpMembers = new System.Windows.Forms.GroupBox();
            this.dgvMembers = new System.Windows.Forms.DataGridView();
            this.btnAddMember = new System.Windows.Forms.Button();
            this.btnEditMember = new System.Windows.Forms.Button();
            this.btnDeleteMember = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnNewFee = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboFeeCycles = new System.Windows.Forms.ComboBox();
            this.btnOpenPayments = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnManageExpenses = new System.Windows.Forms.Button();
            this.btnAddExpense = new System.Windows.Forms.Button();
            this.grpSummary.SuspendLayout();
            this.grpMembers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSummary
            // 
            this.grpSummary.Controls.Add(this.lblBalance);
            this.grpSummary.Controls.Add(this.label1);
            this.grpSummary.Font = new System.Drawing.Font("Microsoft YaHei UI", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSummary.Location = new System.Drawing.Point(12, 12);
            this.grpSummary.Name = "grpSummary";
            this.grpSummary.Size = new System.Drawing.Size(271, 79);
            this.grpSummary.TabIndex = 0;
            this.grpSummary.TabStop = false;
            this.grpSummary.Text = "Tổng Quan";
            this.grpSummary.Enter += new System.EventHandler(this.grpSummary_Enter);
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblBalance.Location = new System.Drawing.Point(106, 47);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(34, 20);
            this.lblBalance.TabIndex = 2;
            this.lblBalance.Text = "0 đ";
            this.lblBalance.Click += new System.EventHandler(this.lblBalance_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "Số dư:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Tổng Thu: ";
            // 
            // lblTotalIn
            // 
            this.lblTotalIn.AutoSize = true;
            this.lblTotalIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalIn.Location = new System.Drawing.Point(82, 107);
            this.lblTotalIn.Name = "lblTotalIn";
            this.lblTotalIn.Size = new System.Drawing.Size(29, 20);
            this.lblTotalIn.TabIndex = 1;
            this.lblTotalIn.Text = "0đ";
            this.lblTotalIn.Click += new System.EventHandler(this.lblTotalIn_Click);
            // 
            // lblTotalOut
            // 
            this.lblTotalOut.AutoSize = true;
            this.lblTotalOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTotalOut.Location = new System.Drawing.Point(82, 131);
            this.lblTotalOut.Name = "lblTotalOut";
            this.lblTotalOut.Size = new System.Drawing.Size(29, 20);
            this.lblTotalOut.TabIndex = 2;
            this.lblTotalOut.Text = "0đ";
            this.lblTotalOut.Click += new System.EventHandler(this.lblTotalOut_Click);
            // 
            // grpMembers
            // 
            this.grpMembers.Controls.Add(this.dgvMembers);
            this.grpMembers.Location = new System.Drawing.Point(396, 25);
            this.grpMembers.Name = "grpMembers";
            this.grpMembers.Size = new System.Drawing.Size(381, 373);
            this.grpMembers.TabIndex = 3;
            this.grpMembers.TabStop = false;
            this.grpMembers.Text = "Thành Viên Lớp";
            this.grpMembers.Enter += new System.EventHandler(this.grpMembers_Enter);
            // 
            // dgvMembers
            // 
            this.dgvMembers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembers.Location = new System.Drawing.Point(0, 28);
            this.dgvMembers.MultiSelect = false;
            this.dgvMembers.Name = "dgvMembers";
            this.dgvMembers.ReadOnly = true;
            this.dgvMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMembers.Size = new System.Drawing.Size(381, 339);
            this.dgvMembers.TabIndex = 4;
            this.dgvMembers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMembers_CellContentClick);
            // 
            // btnAddMember
            // 
            this.btnAddMember.Location = new System.Drawing.Point(396, 415);
            this.btnAddMember.Name = "btnAddMember";
            this.btnAddMember.Size = new System.Drawing.Size(75, 23);
            this.btnAddMember.TabIndex = 5;
            this.btnAddMember.Text = "Thêm";
            this.btnAddMember.UseVisualStyleBackColor = true;
            this.btnAddMember.Click += new System.EventHandler(this.btnAddMember_Click);
            // 
            // btnEditMember
            // 
            this.btnEditMember.Location = new System.Drawing.Point(560, 415);
            this.btnEditMember.Name = "btnEditMember";
            this.btnEditMember.Size = new System.Drawing.Size(75, 23);
            this.btnEditMember.TabIndex = 6;
            this.btnEditMember.Text = "Sửa";
            this.btnEditMember.UseVisualStyleBackColor = true;
            this.btnEditMember.Click += new System.EventHandler(this.btnEditMember_Click);
            // 
            // btnDeleteMember
            // 
            this.btnDeleteMember.Location = new System.Drawing.Point(702, 415);
            this.btnDeleteMember.Name = "btnDeleteMember";
            this.btnDeleteMember.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteMember.TabIndex = 7;
            this.btnDeleteMember.Text = "Xoá";
            this.btnDeleteMember.UseVisualStyleBackColor = true;
            this.btnDeleteMember.Click += new System.EventHandler(this.btnDeleteMember_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tổng Chi:";
            // 
            // btnNewFee
            // 
            this.btnNewFee.Location = new System.Drawing.Point(6, 59);
            this.btnNewFee.Name = "btnNewFee";
            this.btnNewFee.Size = new System.Drawing.Size(105, 23);
            this.btnNewFee.TabIndex = 8;
            this.btnNewFee.Text = "Tạo Khoản Thu";
            this.btnNewFee.UseVisualStyleBackColor = true;
            this.btnNewFee.Click += new System.EventHandler(this.btnNewFee_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboFeeCycles);
            this.groupBox1.Controls.Add(this.btnOpenPayments);
            this.groupBox1.Controls.Add(this.btnNewFee);
            this.groupBox1.Location = new System.Drawing.Point(12, 222);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 95);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Khoản Thu";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cboFeeCycles
            // 
            this.cboFeeCycles.FormattingEnabled = true;
            this.cboFeeCycles.Location = new System.Drawing.Point(6, 19);
            this.cboFeeCycles.Name = "cboFeeCycles";
            this.cboFeeCycles.Size = new System.Drawing.Size(304, 21);
            this.cboFeeCycles.TabIndex = 10;
            this.cboFeeCycles.SelectedIndexChanged += new System.EventHandler(this.cboFeeCycles_SelectedIndexChanged);
            // 
            // btnOpenPayments
            // 
            this.btnOpenPayments.Location = new System.Drawing.Point(150, 59);
            this.btnOpenPayments.Name = "btnOpenPayments";
            this.btnOpenPayments.Size = new System.Drawing.Size(75, 23);
            this.btnOpenPayments.TabIndex = 9;
            this.btnOpenPayments.Text = "Thu Tiền";
            this.btnOpenPayments.UseVisualStyleBackColor = true;
            this.btnOpenPayments.Click += new System.EventHandler(this.btnOpenPayments_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnManageExpenses);
            this.groupBox2.Controls.Add(this.btnAddExpense);
            this.groupBox2.Location = new System.Drawing.Point(12, 341);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 48);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Khoản chi";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // btnManageExpenses
            // 
            this.btnManageExpenses.Location = new System.Drawing.Point(150, 19);
            this.btnManageExpenses.Name = "btnManageExpenses";
            this.btnManageExpenses.Size = new System.Drawing.Size(121, 23);
            this.btnManageExpenses.TabIndex = 1;
            this.btnManageExpenses.Text = "QL khoản chi";
            this.btnManageExpenses.UseVisualStyleBackColor = true;
            this.btnManageExpenses.Click += new System.EventHandler(this.btnManageExpenses_Click);
            // 
            // btnAddExpense
            // 
            this.btnAddExpense.Location = new System.Drawing.Point(11, 19);
            this.btnAddExpense.Name = "btnAddExpense";
            this.btnAddExpense.Size = new System.Drawing.Size(75, 23);
            this.btnAddExpense.TabIndex = 0;
            this.btnAddExpense.Text = "Chi tiền";
            this.btnAddExpense.UseVisualStyleBackColor = true;
            this.btnAddExpense.Click += new System.EventHandler(this.btnAddExpense_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotalIn);
            this.Controls.Add(this.btnDeleteMember);
            this.Controls.Add(this.lblTotalOut);
            this.Controls.Add(this.btnEditMember);
            this.Controls.Add(this.btnAddMember);
            this.Controls.Add(this.grpMembers);
            this.Controls.Add(this.grpSummary);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.grpSummary.ResumeLayout(false);
            this.grpSummary.PerformLayout();
            this.grpMembers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembers)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSummary;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalIn;
        private System.Windows.Forms.Label lblTotalOut;
        private System.Windows.Forms.GroupBox grpMembers;
        private System.Windows.Forms.DataGridView dgvMembers;
        private System.Windows.Forms.Button btnAddMember;
        private System.Windows.Forms.Button btnEditMember;
        private System.Windows.Forms.Button btnDeleteMember;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNewFee;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboFeeCycles;
        private System.Windows.Forms.Button btnOpenPayments;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnManageExpenses;
        private System.Windows.Forms.Button btnAddExpense;
    }
}