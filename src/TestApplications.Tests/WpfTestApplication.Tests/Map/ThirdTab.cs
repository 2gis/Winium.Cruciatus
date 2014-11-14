namespace WpfTestApplication.Tests.Map
{
    #region using

    using Cruciatus.Elements;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    public class ThirdTab : TabItem
    {
        public Button OpenFileDialogButton
        {
            get
            {
                return GetElement<Button>("OpenFileDialogButton");
            }
        }

        public Button SaveFileDialogButton
        {
            get
            {
                return GetElement<Button>("SaveFileDialogButton");
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
