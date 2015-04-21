namespace WpfTestApplication.Tests
{
    #region using

    using NUnit.Framework;

    using Winium.Cruciatus;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckFocusedElement
    {
        #region Fields

        private WpfTestApplicationApp application;

        #endregion

        #region Public Methods and Operators

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out this.application);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        [Test]
        public void GetFocusedElement()
        {
            var textBox = this.application.MainWindow.TabItem1.TextBox1;

            textBox.Click();
            var textBoxToo = CruciatusFactory.FocusedElement;

            Assert.AreEqual(textBox, textBoxToo);
        }

        #endregion
    }
}
