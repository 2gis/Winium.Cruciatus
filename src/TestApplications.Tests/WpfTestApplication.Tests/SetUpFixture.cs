namespace WpfTestApplication.Tests
{
    #region using

    using NUnit.Framework;

    #endregion

    [SetUpFixture]
    public class SetUpFixture
    {
        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
#if REMOTE
            Winium.Cruciatus.CruciatusFactory.Settings.AutomaticScreenshotCapture = true;
#endif
        }

        #endregion
    }
}
