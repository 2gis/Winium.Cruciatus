namespace WpfTestApplication.Tests.TestCases
{
    #region using

    using Cruciatus;

    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WpfTestApplication.Tests.Map;
    
    #endregion

    [CodedUITest]
    public class CheckThirdTab
    {
        private static WpfTestApplicationApp _application;

        private readonly ThirdTab _thirdTab = _application.MainWindow.TabItem3;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassHelper.ClassInitialize(out _application);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassHelper.ClassCleanup(_application);
        }

        [TestMethod]
        public void CheckingTabItem3()
        {
            Assert.IsTrue(_thirdTab.Select(), _thirdTab.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingOpenFileDialog()
        {
            Assert.IsTrue(_thirdTab.Select(), _thirdTab.LastErrorMessage);

            Assert.IsTrue(_thirdTab.OpenFileDialogButton.Click(), _thirdTab.OpenFileDialogButton.LastErrorMessage);

            var fileName = OpenFileDialog.GetFileNameEditableComboBox(_application.MainWindow).Text;
            Assert.AreEqual("Program.cs", fileName);

            var cancelButton = OpenFileDialog.GetCancelButton(_application.MainWindow);
            Assert.IsTrue(cancelButton.Click(), cancelButton.LastErrorMessage);
        }

        [TestMethod]
        public void CheckingSaveFileDialog()
        {
            Assert.IsTrue(_thirdTab.Select(), _thirdTab.LastErrorMessage);

            Assert.IsTrue(_thirdTab.SaveFileDialogButton.Click(), _thirdTab.SaveFileDialogButton.LastErrorMessage);

            var fileName = SaveFileDialog.GetFileNameEditableComboBox(_application.MainWindow).Text;
            Assert.AreEqual("Program.cs", fileName);

            var fileType = SaveFileDialog.GetFileTypeComboBox(_application.MainWindow).Text;
            Assert.AreEqual("Visual C# Files (*.cs)", fileType);

            var cancelButton = SaveFileDialog.GetCancelButton(_application.MainWindow);
            Assert.IsTrue(cancelButton.Click(), cancelButton.LastErrorMessage);
        }
    }
}
