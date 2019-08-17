namespace 第一版
{
    partial class Form4
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.smsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bayes分类DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.knn分类DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.notyetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.短信DataSet = new 第一版.短信DataSet();
            this.notyetTableAdapter = new 第一版.短信DataSetTableAdapters.notyetTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notyetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.短信DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.smsDataGridViewTextBoxColumn,
            this.bayes分类DataGridViewTextBoxColumn,
            this.knn分类DataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.notyetBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(594, 264);
            this.dataGridView1.TabIndex = 0;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.Width = 50;
            // 
            // smsDataGridViewTextBoxColumn
            // 
            this.smsDataGridViewTextBoxColumn.DataPropertyName = "sms";
            this.smsDataGridViewTextBoxColumn.HeaderText = "sms";
            this.smsDataGridViewTextBoxColumn.Name = "smsDataGridViewTextBoxColumn";
            this.smsDataGridViewTextBoxColumn.Width = 300;
            // 
            // bayes分类DataGridViewTextBoxColumn
            // 
            this.bayes分类DataGridViewTextBoxColumn.DataPropertyName = "bayes分类";
            this.bayes分类DataGridViewTextBoxColumn.HeaderText = "bayes分类";
            this.bayes分类DataGridViewTextBoxColumn.Name = "bayes分类DataGridViewTextBoxColumn";
            // 
            // knn分类DataGridViewTextBoxColumn
            // 
            this.knn分类DataGridViewTextBoxColumn.DataPropertyName = "knn分类";
            this.knn分类DataGridViewTextBoxColumn.HeaderText = "knn分类";
            this.knn分类DataGridViewTextBoxColumn.Name = "knn分类DataGridViewTextBoxColumn";
            // 
            // notyetBindingSource
            // 
            this.notyetBindingSource.DataMember = "notyet";
            this.notyetBindingSource.DataSource = this.短信DataSet;
            // 
            // 短信DataSet
            // 
            this.短信DataSet.DataSetName = "短信DataSet";
            this.短信DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // notyetTableAdapter
            // 
            this.notyetTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(612, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "beyes分类";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(612, 103);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "knn分类";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(612, 146);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "保存更改";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(612, 187);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "取消更改";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 288);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form4";
            this.Text = "批量分类";
            this.Load += new System.EventHandler(this.Form4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notyetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.短信DataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private 短信DataSet 短信DataSet;
        private System.Windows.Forms.BindingSource notyetBindingSource;
        private 短信DataSetTableAdapters.notyetTableAdapter notyetTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn smsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bayes分类DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn knn分类DataGridViewTextBoxColumn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}