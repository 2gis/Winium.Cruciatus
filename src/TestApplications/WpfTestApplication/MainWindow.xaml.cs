
namespace WpfTestApplication
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Ribbon;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
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

        private void SetTextButtonClick(object sender, RoutedEventArgs e)
        {
            this.TextBox1.Text = "CARAMBA";
        }

        private void ChangeEnabledButtonClick(object sender, RoutedEventArgs e)
        {
            this.TextBox2.IsEnabled = !this.TextBox2.IsEnabled;
        }
    }
}
