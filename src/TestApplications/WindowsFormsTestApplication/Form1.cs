
namespace WindowsFormsTestApplication
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// The form 1.
    /// </summary>
    public partial class Form1 : Form
    {
        #region monthsList, timeSizeList
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

        public Form1()
        {
            this.InitializeComponent();

            this.CheckBox1.Checked = true;

            // Fill TextkComboBox
            this.TextComboBox.DataSource = this.timeSizeList;
            this.TextComboBox.SelectedIndex = -1;

            // Fill TextkListBox
            this.TextListBox.DataSource = this.monthsList;
            this.TextListBox.SelectedIndex = -1;

            // Fill CheckListBox
            this.CheckListBox.DataSource = this.monthsList;
        }

        private void SetTextButtonClick(object sender, System.EventArgs e)
        {
            this.TextBox1.Text = @"CARAMBA";
        }

        private void CheckBox1CheckedChanged(object sender, System.EventArgs e)
        {
            this.TextListBox.Enabled = this.CheckBox1.Checked;
        }

        private void ChangeEnabledButtonClick(object sender, System.EventArgs e)
        {
            this.TextBox2.Enabled = !this.TextBox2.Enabled;
        }

        private void CheckBox2CheckedChanged(object sender, System.EventArgs e)
        {
            var check = this.CheckBox2.Checked;
            for (int i = 0; i < this.CheckListBox.Items.Count; ++i)
            {
                this.CheckListBox.SetItemChecked(i, check);
            }
        }
    }
}
