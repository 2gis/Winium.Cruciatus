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
        #region monthsList, timeSizeList

        private readonly List<string> _monthsList = new List<string>
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

        private readonly List<string> _timeSizeList = new List<string>
        {
            "Day", 
            "Week", 
            "Decade", 
            "Month", 
            "Quarter", 
            "Year"
        };

        #endregion

        public Form1()
        {
            InitializeComponent();

            CheckBox1.Checked = true;

            // Fill TextkComboBox
            TextComboBox.DataSource = _timeSizeList;
            TextComboBox.SelectedIndex = -1;

            // Fill TextkListBox
            TextListBox.DataSource = _monthsList;
            TextListBox.SelectedIndex = -1;

            // Fill CheckListBox
            CheckListBox.DataSource = _monthsList;
        }

        private void SetTextButtonClick(object sender, EventArgs e)
        {
            TextBox1.Text = @"CARAMBA";
        }

        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            TextListBox.Enabled = CheckBox1.Checked;
        }

        private void ChangeEnabledButtonClick(object sender, EventArgs e)
        {
            TextBox2.Enabled = !TextBox2.Enabled;
        }

        private void CheckBox2CheckedChanged(object sender, EventArgs e)
        {
            var check = CheckBox2.Checked;
            for (int i = 0; i < CheckListBox.Items.Count; ++i)
            {
                CheckListBox.SetItemChecked(i, check);
            }
        }
    }
}
