namespace WpfTestApplication
{
    #region using

    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Win32;

    #endregion

    public partial class MainWindow
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

        public MainWindow()
        {
            this.InitializeComponent();

            this.CheckBox1.IsChecked = true;

            // Fill TextkComboBox
            foreach (var size in this.timeSizeList)
            {
                this.TextComboBox.Items.Add(new TextBlock { Text = size });
            }

            // Fill CheckComboBox
            foreach (var size in this.timeSizeList)
            {
                this.CheckComboBox.Items.Add(new CheckBox { Content = size });
            }

            // Fill TextkListBox
            foreach (var month in this.monthsList)
            {
                this.TextListBox.Items.Add(new TextBlock { Text = month });
            }

            // Fill CheckListBox
            foreach (var month in this.monthsList)
            {
                this.CheckListBox.Items.Add(new CheckBox { Content = month });
            }

            // Fill RibbonTextComboBox
            foreach (var size in this.timeSizeList)
            {
                this.TextRibbonGalleryCategory.Items.Add(new TextBlock { Text = size });
            }

            // Fill RibbonCheckComboBox
            foreach (var size in this.timeSizeList)
            {
                this.CheckRibbonGalleryCategory.Items.Add(new CheckBox { Content = size });
            }

            // Fill DataGrid
            DataGrid.Items.Add(new { Id = "1", Name = "one" });
            DataGrid.Items.Add(new { Id = "2", Name = "two" });
            DataGrid.Items.Add(new { Id = "3", Name = "three" });
            DataGrid.Items.Add(new { Id = "4", Name = "four" });
            DataGrid.Items.Add(new { Id = "5", Name = "five" });
            DataGrid.Items.Add(new { Id = "6", Name = "six" });
            DataGrid.Items.Add(new { Id = "7", Name = "seven" });
            DataGrid.Items.Add(new { Id = "8", Name = "eight" });
            DataGrid.Items.Add(new { Id = "9", Name = "nine" });
            DataGrid.Items.Add(new { Id = "10", Name = "ten" });
            DataGrid.Items.Add(new { Id = "11", Name = "eleven" });
            DataGrid.Items.Add(new { Id = "12", Name = "twelve" });
            DataGrid.Items.Add(new { Id = "13", Name = "thirteen" });
            DataGrid.Items.Add(new { Id = "14", Name = "fourteen" });
            DataGrid.Items.Add(new { Id = "15", Name = "fifteen" });
        }

        #endregion

        #region Methods

        private void ChangeEnabledButtonClick(object sender, RoutedEventArgs e)
        {
            this.TextBox2.IsEnabled = !this.TextBox2.IsEnabled;
        }

        private void CheckBox1Checked(object sender, RoutedEventArgs e)
        {
            this.TextListBox.IsEnabled = true;
        }

        private void CheckBox1Unchecked(object sender, RoutedEventArgs e)
        {
            this.TextListBox.IsEnabled = false;
        }

        private void CheckBox2Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkBox in this.CheckListBox.Items)
            {
                checkBox.IsChecked = true;
            }
        }

        private void CheckBox2Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkBox in this.CheckListBox.Items)
            {
                checkBox.IsChecked = false;
            }
        }

        private void OpenFileDialogButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { FileName = "Program.cs" };
            dialog.ShowDialog();
        }

        private void SaveFileDialogButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { FileName = "Program.cs", Filter = "Visual C# Files|*.cs" };
            dialog.ShowDialog();
        }

        private void SetTextButtonClick(object sender, RoutedEventArgs e)
        {
            this.TextBox1.Text = "CARAMBA";
        }

        #endregion
    }
}
