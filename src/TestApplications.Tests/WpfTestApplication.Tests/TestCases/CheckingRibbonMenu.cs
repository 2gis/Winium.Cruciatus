
namespace WpfTestApplication.Tests.TestCases
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The wpf test application test class 1.
    /// </summary>
    public partial class WpfTestApplicationTestClass1
    {
        [TestMethod]
        public void RibbonMenuTestMethod1()
        {
            Assert.IsTrue(this.ribbonMenu.Select("Print$Open"), this.simpleMenu.LastErrorMessage);
        }

        [TestMethod]
        public void RibbonMenuTestMethod2()
        {
            Assert.IsTrue(this.ribbonMenu.Select("Print$New$Save"), this.simpleMenu.LastErrorMessage);
        }
    }
}