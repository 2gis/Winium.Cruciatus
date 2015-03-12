namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using NUnit.Framework;

    using WpfTestApplication.Tests.Map;

    #endregion

    [TestFixture]
    public class CheckThirdTab
    {
        #region Fields

        private WpfTestApplicationApp application;

        private ThirdTab thirdTab;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void CheckingOpenFileDialog()
        {
            this.thirdTab.OpenFileDialogButton.Click();

            var openFileDialog = this.application.MainWindow.OpenFileDialog;
            var fileName = openFileDialog.FileNameComboBox.Text();
            Assert.AreEqual("Program.cs", fileName);

            openFileDialog.CancelButton.Click();
        }

        [Test]
        public void CheckingSaveFileDialog()
        {
            this.thirdTab.SaveFileDialogButton.Click();

            var saveFileDialog = this.application.MainWindow.SaveFileDialog;
            var fileName = saveFileDialog.FileNameComboBox.Text();
            Assert.AreEqual("Program.cs", fileName);

            var fileType = saveFileDialog.FileTypeComboBox.SelectedItem().Properties.Name;
            Assert.AreEqual("Visual C# Files (*.cs)", fileType);

            saveFileDialog.CancelButton.Click();
        }

        [Test]
        public void CheckingTabItem3()
        {
            this.thirdTab.Select();
            Assert.AreEqual(true, this.thirdTab.IsSelection, "Третья вкладка оказалось не выбранной");
        }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            TestClassHelper.Initialize(out this.application);

            this.thirdTab = this.application.MainWindow.TabItem3;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            TestClassHelper.Cleanup(this.application);
        }

        [SetUp]
        public void TestSetUp()
        {
            this.thirdTab.Select();
        }

        #endregion
    }
}
