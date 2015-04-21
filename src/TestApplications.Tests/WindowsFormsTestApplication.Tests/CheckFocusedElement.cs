namespace WindowsFormsTestApplication.Tests
{
    #region using

    using NUnit.Framework;

    using WindowsFormsTestApplication.Tests.Map;

    using Winium.Cruciatus;

    #endregion

    [TestFixture]
    public class CheckFocusedElement
    {
        #region Fields

        private WindowsFormsTestApplicationApp application;

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
            var textBox = this.application.Window.TabItem1.TextBox1;

            textBox.Click();
            var textBoxToo = CruciatusFactory.FocusedElement;

            Assert.AreEqual(textBox, textBoxToo);
        }

        #endregion
    }
}
