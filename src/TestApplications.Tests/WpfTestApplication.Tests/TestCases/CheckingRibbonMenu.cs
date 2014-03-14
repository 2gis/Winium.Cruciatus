
namespace WpfTestApplication.Tests.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The test class 1.
    /// </summary>
    public partial class TestClass1
    {
        [TestMethod]
        public void RibbonMenuTestMethod1()
        {
            Assert.IsTrue(this.ribbonMenu.SelectItem("Print$Open"), this.simpleMenu.LastErrorMessage);
        }

        [TestMethod]
        public void RibbonMenuTestMethod2()
        {
            Assert.IsTrue(this.ribbonMenu.SelectItem("Print$New$Save"), this.simpleMenu.LastErrorMessage);
        }
    }
}