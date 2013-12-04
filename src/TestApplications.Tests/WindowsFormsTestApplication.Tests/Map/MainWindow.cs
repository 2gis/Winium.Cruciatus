
namespace WindowsFormsTestApplication.Tests.Map
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class MainWindow : Window
    {
        public FirstTab TabItem1
        {
            get
            {
                return this.GetElement<FirstTab>("TabItem1");
            }
        }

        public SecondTab TabItem2
        {
            get
            {
                return this.GetElement<SecondTab>("TabItem2");
            }
        }

        #region Временно, пока проблемы с вкладками.
        public Button SetTextButton
        {
            get
            {
                return this.GetElement<Button>("SetTextButton");
            }
        }

        public TextBox TextBox1
        {
            get
            {
                return this.GetElement<TextBox>("TextBox1");
            }
        }

        public ComboBox TextComboBox
        {
            get
            {
                return this.GetElement<ComboBox>("TextComboBox");
            }
        }

        public CheckBox CheckBox1
        {
            get
            {
                return this.GetElement<CheckBox>("CheckBox1");
            }
        }

        public ListBox TextListBox
        {
            get
            {
                return this.GetElement<ListBox>("TextListBox");
            }
        }
        #endregion

        protected override T GetElement<T>(string automationId)
        {
            var element = base.GetElement<T>(automationId);
            Assert.IsNotNull(element, this.LastErrorMessage);
            return element;
        }
    }
}