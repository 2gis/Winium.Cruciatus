
namespace WpfTestApplication.Tests.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test class 1.
    /// </summary>
    public partial class TestClass1
    {
        [TestMethod]
        public void SimpleMenuTestMethod1()
        {
            const string HeadersPath = "Level1$MultiLevel2$Level3";
            Assert.IsTrue(this.simpleMenu.SelectItem(HeadersPath), this.simpleMenu.LastErrorMessage);
        }

        [TestMethod]
        public void SimpleMenuTestMethod2()
        {
            const string HeadersPath = "Level1$MultiLevel2$MultiLevel3$MultiLevel4$Level5";
            Assert.IsTrue(this.simpleMenu.SelectItem(HeadersPath), this.simpleMenu.LastErrorMessage);
        }
    }
}