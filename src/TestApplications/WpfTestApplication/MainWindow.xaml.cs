namespace WpfTestApplication
{
    #region using

    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Win32;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
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

        public MainWindow()
        {
            InitializeComponent();

            CheckBox1.IsChecked = true;

            // Fill TextkComboBox
            foreach (var size in _timeSizeList)
            {
                TextComboBox.Items.Add(new TextBlock { Text = size });
            }

            // Fill CheckComboBox
            foreach (var size in _timeSizeList)
            {
                CheckComboBox.Items.Add(new CheckBox { Content = size });
            }

            // Fill TextkListBox
            foreach (var month in _monthsList)
            {
                TextListBox.Items.Add(new TextBlock { Text = month });
            }

            // Fill CheckListBox
            foreach (var month in _monthsList)
            {
                CheckListBox.Items.Add(new CheckBox { Content = month });
            }

            // Fill RibbonTextComboBox
            foreach (var size in _timeSizeList)
            {
                TextRibbonGalleryCategory.Items.Add(new TextBlock { Text = size });
            }

            // Fill RibbonCheckComboBox
            foreach (var size in _timeSizeList)
            {
                CheckRibbonGalleryCategory.Items.Add(new CheckBox { Content = size });
            }
        }

        private void CheckBox1Checked(object sender, RoutedEventArgs e)
        {
            TextListBox.IsEnabled = true;
        }

        private void CheckBox1Unchecked(object sender, RoutedEventArgs e)
        {
            TextListBox.IsEnabled = false;
        }

        private void CheckBox2Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkBox in CheckListBox.Items)
            {
                checkBox.IsChecked = true;
            }
        }

        private void CheckBox2Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox checkBox in CheckListBox.Items)
            {
                checkBox.IsChecked = false;
            }
        }

        private void SetTextButtonClick(object sender, RoutedEventArgs e)
        {
            TextBox1.Text = "CARAMBA";
        }

        private void ChangeEnabledButtonClick(object sender, RoutedEventArgs e)
        {
            TextBox2.IsEnabled = !TextBox2.IsEnabled;
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
    }
}
