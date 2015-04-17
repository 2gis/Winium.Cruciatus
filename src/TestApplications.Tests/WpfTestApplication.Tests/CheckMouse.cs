namespace WpfTestApplication.Tests
{
    #region using

    using NUnit.Framework;

    using Winium.Cruciatus;

    #endregion

    [TestFixture]
    public class CheckMouse
    {
        [TestCase(50, 50)]
        [TestCase(-50, -50)]
        public void MouseMoveTo(int x, int y)
        {
            var oldPoint = CruciatusFactory.Mouse.CurrentCursorPos;
            oldPoint.Offset(x, y);

            CruciatusFactory.Mouse.MoveCursorPos(x, y);
            var newPoint = CruciatusFactory.Mouse.CurrentCursorPos;

            Assert.That(oldPoint.X, Is.EqualTo(newPoint.X).Within(1));
            Assert.That(oldPoint.Y, Is.EqualTo(newPoint.Y).Within(1));
        }
    }
}
