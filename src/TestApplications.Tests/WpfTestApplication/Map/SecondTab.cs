
namespace WpfTestApplication.Map
{
    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class SecondTab : TabItem
    {
        public Button ChangeEnabledButton
        {
            get
            {
                return this.GetElement<Button>("ChangeEnabledButton");
            }
        }

        public TextBox TextBox2
        {
            get
            {
                return this.GetElement<TextBox>("TextBox2");
            }
        }

        public ComboBox CheckComboBox
        {
            get
            {
                return this.GetElement<ComboBox>("CheckComboBox");
            }
        }

        public CheckBox CheckBox2
        {
            get
            {
                return this.GetElement<CheckBox>("CheckBox2");
            }
        }

        public ListBox CheckListBox
        {
            get
            {
                return this.GetElement<ListBox>("CheckListBox");
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
