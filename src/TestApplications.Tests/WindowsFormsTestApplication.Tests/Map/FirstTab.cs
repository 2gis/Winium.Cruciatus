namespace WindowsFormsTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    public class FirstTab : TabItem
    {
        public Button SetTextButton
        {
            get
            {
                return GetElement<Button>("SetTextButton");
            }
        }

        public TextBox TextBox1
        {
            get
            {
                return GetElement<TextBox>("TextBox1");
            }
        }

        public ComboBox TextComboBox
        {
            get
            {
                return GetElement<ComboBox>("TextComboBox");
            }
        }

        public CheckBox CheckBox1
        {
            get
            {
                return GetElement<CheckBox>("CheckBox1");
            }
        }

        public ListBox TextListBox
        {
            get
            {
                return GetElement<ListBox>("TextListBox");
            }
        }

        protected override T GetElement<T>(string automationId)
        {
            var element = base.GetElement<T>(automationId);
            Assert.IsNotNull(element, LastErrorMessage);
            return element;
        }
    }
}
