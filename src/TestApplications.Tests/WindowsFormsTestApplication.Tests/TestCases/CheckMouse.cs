namespace WindowsFormsTestApplication.Tests.TestCases
{
    #region using

    using NUnit.Framework;

    using WindowsFormsTestApplication.Tests.Map;

    using Winium.Cruciatus;
    using Winium.Cruciatus.Core;

    #endregion

    [TestFixture]
    public class CheckMouse
    {
        #region Static Fields

        private static WindowsFormsTestApplicationApp application;

        #endregion

        #region Public Methods and Operators

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out application);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(application);
        }

        [Test]
        public void MouseMoveTo()
        {
            // ReSharper disable once PossibleInvalidOperationException
            var textBoxCenter = application.Window.TabItem1.Properties.ClickablePoint.Value;
            CruciatusFactory.Mouse.SetCursorPos(textBoxCenter.X, textBoxCenter.Y);
            CruciatusFactory.Mouse.MoveCursorPos(50, 20);
            CruciatusFactory.Mouse.Click(MouseButton.Left);

            Assert.AreEqual("CARAMBA", application.Window.TabItem1.TextBox1.Text());
        }

        #endregion
    }
}
