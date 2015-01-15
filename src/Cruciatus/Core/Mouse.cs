namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Threading;

    #endregion

    public class Mouse
    {
        public bool SetCursorPos(double x, double y)
        {
            var result = MouseNativeMethods.SetCursorPos((int)x, (int)y);
            Thread.Sleep(50);
            return result;
        }

        public void Click(MouseButtons button, double x, double y)
        {
            SetCursorPos((int)x, (int)y);
            switch (button)
            {
                case MouseButtons.Left:
                    LeftClick();
                    break;
                case MouseButtons.Right:
                    RightClick();
                    break;
            }
        }

        public void DoubleClick(MouseButtons button, double x, double y)
        {
            SetCursorPos((int)x, (int)y);
            switch (button)
            {
                case MouseButtons.Left:
                    DoubleLeftClick();
                    break;
                case MouseButtons.Right:
                    DoubleRightClick();
                    break;
            }
        }

        public void LeftClick()
        {
            MouseNativeMethods.mouse_event(WindowsConstants.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
            MouseNativeMethods.mouse_event(WindowsConstants.MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
        }

        public void RightClick()
        {
            MouseNativeMethods.mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
            MouseNativeMethods.mouse_event(WindowsConstants.MOUSEEVENTF_RIGHTUP, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
        }

        public void DoubleLeftClick()
        {
            LeftClick();
            LeftClick();
        }

        public void DoubleRightClick()
        {
            RightClick();
            RightClick();
        }
    }
}
