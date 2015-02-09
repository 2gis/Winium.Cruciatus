namespace Cruciatus.Core
{
    #region using

    using System.Threading;
    using System.Windows;

    using WindowsInput;

    #endregion

    public class Mouse
    {
        private readonly IMouseSimulator _mouseSimulator;

        public Mouse(IMouseSimulator mouseSimulator)
        {
            _mouseSimulator = mouseSimulator;
        }

        public void SetCursorPos(double x, double y)
        {
            var absoluteX = x * (65535 / SystemParameters.VirtualScreenWidth);
            var absoluteY = y * (65535 / SystemParameters.VirtualScreenHeight);
            _mouseSimulator.MoveMouseToPositionOnVirtualDesktop(absoluteX, absoluteY);
            Thread.Sleep(250);
        }

        public void Click(MouseButton button, double x, double y)
        {
            SetCursorPos(x, y);
            switch (button)
            {
                case MouseButton.Left:
                    LeftButtonClick();
                    break;
                case MouseButton.Right:
                    RightButtonClick();
                    break;
            }
        }

        public void DoubleClick(MouseButton button, double x, double y)
        {
            SetCursorPos(x, y);
            switch (button)
            {
                case MouseButton.Left:
                    LeftButtonDoubleClick();
                    break;
                case MouseButton.Right:
                    RightButtonDoubleClick();
                    break;
            }
        }

        public void LeftButtonClick()
        {
            _mouseSimulator.LeftButtonClick();
            Thread.Sleep(250);
        }

        public void RightButtonClick()
        {
            _mouseSimulator.RightButtonClick();
            Thread.Sleep(250);
        }

        public void LeftButtonDoubleClick()
        {
            _mouseSimulator.LeftButtonDoubleClick();
            Thread.Sleep(250);
        }

        public void RightButtonDoubleClick()
        {
            _mouseSimulator.RightButtonDoubleClick();
            Thread.Sleep(250);
        }

        public void VerticalScroll(int amountOfClicks)
        {
            _mouseSimulator.VerticalScroll(amountOfClicks);
            Thread.Sleep(250);
        }
    }
}
