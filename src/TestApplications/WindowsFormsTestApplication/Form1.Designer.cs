namespace WindowsFormsTestApplication
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabItem1 = new System.Windows.Forms.TabPage();
            this.TextListBox = new System.Windows.Forms.ListBox();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.TextComboBox = new System.Windows.Forms.ComboBox();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.SetTextButton = new System.Windows.Forms.Button();
            this.TabItem2 = new System.Windows.Forms.TabPage();
            this.CheckListBox = new System.Windows.Forms.CheckedListBox();
            this.CheckBox2 = new System.Windows.Forms.CheckBox();
            this.CheckComboBox = new System.Windows.Forms.ComboBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.ChangeEnabledButton = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DataGrid = new System.Windows.Forms.DataGridView();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.TabItem1.SuspendLayout();
            this.TabItem2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TabItem1);
            this.tabControl1.Controls.Add(this.TabItem2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(223, 242);
            this.tabControl1.TabIndex = 0;
            // 
            // TabItem1
            // 
            this.TabItem1.Controls.Add(this.TextListBox);
            this.TabItem1.Controls.Add(this.CheckBox1);
            this.TabItem1.Controls.Add(this.TextComboBox);
            this.TabItem1.Controls.Add(this.TextBox1);
            this.TabItem1.Controls.Add(this.SetTextButton);
            this.TabItem1.Location = new System.Drawing.Point(4, 22);
            this.TabItem1.Name = "TabItem1";
            this.TabItem1.Padding = new System.Windows.Forms.Padding(3);
            this.TabItem1.Size = new System.Drawing.Size(215, 216);
            this.TabItem1.TabIndex = 0;
            this.TabItem1.Text = "TabItem1";
            this.TabItem1.UseVisualStyleBackColor = true;
            // 
            // TextListBox
            // 
            this.TextListBox.FormattingEnabled = true;
            this.TextListBox.Location = new System.Drawing.Point(8, 111);
            this.TextListBox.Name = "TextListBox";
            this.TextListBox.Size = new System.Drawing.Size(200, 95);
            this.TextListBox.TabIndex = 4;
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.Location = new System.Drawing.Point(8, 88);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(128, 17);
            this.CheckBox1.TabIndex = 3;
            this.CheckBox1.Text = "IsEnabledTextListBox";
            this.CheckBox1.UseVisualStyleBackColor = true;
            this.CheckBox1.CheckedChanged += new System.EventHandler(this.CheckBox1CheckedChanged);
            // 
            // TextComboBox
            // 
            this.TextComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TextComboBox.FormattingEnabled = true;
            this.TextComboBox.Location = new System.Drawing.Point(8, 61);
            this.TextComboBox.Name = "TextComboBox";
            this.TextComboBox.Size = new System.Drawing.Size(200, 21);
            this.TextComboBox.TabIndex = 2;
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(8, 35);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(200, 20);
            this.TextBox1.TabIndex = 1;
            this.TextBox1.Text = "TextBox1";
            // 
            // SetTextButton
            // 
            this.SetTextButton.Location = new System.Drawing.Point(8, 6);
            this.SetTextButton.Name = "SetTextButton";
            this.SetTextButton.Size = new System.Drawing.Size(200, 23);
            this.SetTextButton.TabIndex = 0;
            this.SetTextButton.Text = "Set \'CARAMBA\' to TextBox1";
            this.SetTextButton.UseVisualStyleBackColor = true;
            this.SetTextButton.Click += new System.EventHandler(this.SetTextButtonClick);
            // 
            // TabItem2
            // 
            this.TabItem2.Controls.Add(this.CheckListBox);
            this.TabItem2.Controls.Add(this.CheckBox2);
            this.TabItem2.Controls.Add(this.CheckComboBox);
            this.TabItem2.Controls.Add(this.TextBox2);
            this.TabItem2.Controls.Add(this.ChangeEnabledButton);
            this.TabItem2.Location = new System.Drawing.Point(4, 22);
            this.TabItem2.Name = "TabItem2";
            this.TabItem2.Padding = new System.Windows.Forms.Padding(3);
            this.TabItem2.Size = new System.Drawing.Size(215, 216);
            this.TabItem2.TabIndex = 1;
            this.TabItem2.Text = "TabItem2";
            this.TabItem2.UseVisualStyleBackColor = true;
            // 
            // CheckListBox
            // 
            this.CheckListBox.FormattingEnabled = true;
            this.CheckListBox.Location = new System.Drawing.Point(9, 112);
            this.CheckListBox.Name = "CheckListBox";
            this.CheckListBox.Size = new System.Drawing.Size(199, 94);
            this.CheckListBox.TabIndex = 9;
            // 
            // CheckBox2
            // 
            this.CheckBox2.AutoSize = true;
            this.CheckBox2.Location = new System.Drawing.Point(8, 88);
            this.CheckBox2.Name = "CheckBox2";
            this.CheckBox2.Size = new System.Drawing.Size(74, 17);
            this.CheckBox2.TabIndex = 8;
            this.CheckBox2.Text = "All months";
            this.CheckBox2.UseVisualStyleBackColor = true;
            this.CheckBox2.CheckedChanged += new System.EventHandler(this.CheckBox2CheckedChanged);
            // 
            // CheckComboBox
            // 
            this.CheckComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CheckComboBox.Enabled = false;
            this.CheckComboBox.FormattingEnabled = true;
            this.CheckComboBox.Location = new System.Drawing.Point(8, 61);
            this.CheckComboBox.Name = "CheckComboBox";
            this.CheckComboBox.Size = new System.Drawing.Size(200, 21);
            this.CheckComboBox.TabIndex = 7;
            // 
            // TextBox2
            // 
            this.TextBox2.Location = new System.Drawing.Point(8, 35);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(200, 20);
            this.TextBox2.TabIndex = 6;
            this.TextBox2.Text = "TextBox2";
            // 
            // ChangeEnabledButton
            // 
            this.ChangeEnabledButton.Location = new System.Drawing.Point(8, 6);
            this.ChangeEnabledButton.Name = "ChangeEnabledButton";
            this.ChangeEnabledButton.Size = new System.Drawing.Size(200, 23);
            this.ChangeEnabledButton.TabIndex = 5;
            this.ChangeEnabledButton.Text = "Change enabled to TextBox2";
            this.ChangeEnabledButton.UseVisualStyleBackColor = true;
            this.ChangeEnabledButton.Click += new System.EventHandler(this.ChangeEnabledButtonClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DataGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(215, 216);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "TabItem3";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // DataGrid
            // 
            this.DataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdColumn,
            this.NameColumn});
            this.DataGrid.Location = new System.Drawing.Point(6, 6);
            this.DataGrid.Name = "DataGrid";
            this.DataGrid.Size = new System.Drawing.Size(203, 204);
            this.DataGrid.TabIndex = 0;
            // 
            // IdColumn
            // 
            this.IdColumn.HeaderText = "Id";
            this.IdColumn.Name = "IdColumn";
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 242);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.TabItem1.ResumeLayout(false);
            this.TabItem1.PerformLayout();
            this.TabItem2.ResumeLayout(false);
            this.TabItem2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabItem1;
        private System.Windows.Forms.ListBox TextListBox;
        private System.Windows.Forms.CheckBox CheckBox1;
        private System.Windows.Forms.ComboBox TextComboBox;
        private System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.Button SetTextButton;
        private System.Windows.Forms.TabPage TabItem2;
        private System.Windows.Forms.CheckBox CheckBox2;
        private System.Windows.Forms.ComboBox CheckComboBox;
        private System.Windows.Forms.TextBox TextBox2;
        private System.Windows.Forms.Button ChangeEnabledButton;
        private System.Windows.Forms.CheckedListBox CheckListBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView DataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
    }
}

