namespace WindowsFormsTestApplication
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// The form 1.
    /// </summary>
    public partial class Form1 : Form
    {
        #region Fields

        private readonly List<string> monthsList = new List<string>
                                                       {
                                                           "January", 
                                                           "February", 
                                                           "March", 
                                                           "April", 
                                                           "May", 
                                                           "June", 
                                                           "July", 
                                                           "August", 
                                                           "September", 
                                                           "October", 
                                                           "November", 
                                                           "December"
                                                       };

        private readonly List<string> timeSizeList = new List<string>
                                                         {
                                                             "Day", 
                                                             "Week", 
                                                             "Decade", 
                                                             "Month", 
                                                             "Quarter", 
                                                             "Year"
                                                         };

        #endregion

        #region Constructors and Destructors

        public Form1()
        {
            this.InitializeComponent();

            this.CheckBox1.Checked = true;

            // Fill TextComboBox
            this.TextComboBox.DataSource = this.timeSizeList;
            this.TextComboBox.SelectedIndex = -1;

            // Fill TextListBox
            this.TextListBox.DataSource = this.monthsList;
            this.TextListBox.SelectedIndex = -1;

            // Fill CheckListBox
            this.CheckListBox.DataSource = this.monthsList;

            // Fill DataGrid
            this.DataGrid.Rows.Add("1", "one");
            this.DataGrid.Rows.Add("2", "two");
            this.DataGrid.Rows.Add("3", "three");
            this.DataGrid.Rows.Add("4", "four");
            this.DataGrid.Rows.Add("5", "five");
        }

        #endregion

        #region Methods

        private void ChangeEnabledButtonClick(object sender, EventArgs e)
        {
            this.TextBox2.Enabled = !this.TextBox2.Enabled;
        }

        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            this.TextListBox.Enabled = this.CheckBox1.Checked;
        }

        private void CheckBox2CheckedChanged(object sender, EventArgs e)
        {
            var check = this.CheckBox2.Checked;
            for (int i = 0; i < this.CheckListBox.Items.Count; ++i)
            {
                this.CheckListBox.SetItemChecked(i, check);
            }
        }

        private void SetTextButtonClick(object sender, EventArgs e)
        {
            this.TextBox1.Text = @"CARAMBA";
        }

        #endregion
    }
}
