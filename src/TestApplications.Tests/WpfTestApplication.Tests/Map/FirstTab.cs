
namespace WpfTestApplication.Tests.Map
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class FirstTab : TabItem
    {
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

        protected override T GetElement<T>(string automationId)
        {
            var element = base.GetElement<T>(automationId);
            Assert.IsNotNull(element, this.LastErrorMessage);
            return element;
        }
    }
}
