namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using Cruciatus;
    using Cruciatus.Core;

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckThirdTab 
    {
        private WpfTestApplicationApp _application;

        private ThirdTab _thirdTab;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out _application);

            _thirdTab = _application.MainWindow.TabItem3;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(_application);
        }

        [SetUp]
        public void TestSetUp()
        {
            _thirdTab.Select();
        }

        [Test]
        public void CheckingTabItem3()
        {
            _thirdTab.Select();
            Assert.AreEqual(true, _thirdTab.IsSelection, "Третья вкладка оказалось не выбранной");
        }

        [Test]
        public void CheckingOpenFileDialog()
        {
            _thirdTab.OpenFileDialogButton.Click();

            var openFileDialog = new OpenFileDialog(_application.MainWindow);
            var fileName = openFileDialog.GetFileNameEditableComboBox().Text();
            Assert.AreEqual("Program.cs", fileName);

            openFileDialog.GetCancelButton().Click();
        }

        [Test]
        public void CheckingSaveFileDialog()
        {
            _thirdTab.SaveFileDialogButton.Click();

            var saveFileDialog = new SaveFileDialog(_application.MainWindow, By.Name("Сохранение"));
            var fileName = saveFileDialog.GetFileNameEditableComboBox().Text();
            Assert.AreEqual("Program.cs", fileName);

            var fileType = saveFileDialog.GetFileTypeComboBox().SelectedItem().Properties.Name;
            Assert.AreEqual("Visual C# Files (*.cs)", fileType);

            saveFileDialog.GetCancelButton().Click();
        }
    }
}
