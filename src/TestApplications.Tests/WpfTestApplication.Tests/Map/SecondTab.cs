namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    public class SecondTab : TabItem
    {
        public Button ChangeEnabledButton
        {
            get
            {
                return GetElement<Button>("ChangeEnabledButton");
            }
        }

        public TextBox TextBox2
        {
            get
            {
                return GetElement<TextBox>("TextBox2");
            }
        }

        public ComboBox CheckComboBox
        {
            get
            {
                return GetElement<ComboBox>("CheckComboBox");
            }
        }

        public CheckBox CheckBox2
        {
            get
            {
                return GetElement<CheckBox>("CheckBox2");
            }
        }

        public ListBox CheckListBox
        {
            get
            {
                return GetElement<ListBox>("CheckListBox");
            }
        }

        public override T GetElement<T>(string automationId)
        {
            var element = base.GetElement<T>(automationId);
            Assert.IsNotNull(element, LastErrorMessage);
            return element;
        }
    }
}
