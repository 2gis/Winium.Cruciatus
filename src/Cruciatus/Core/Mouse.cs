namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Threading;

    #endregion

    public static class Mouse
    {
        private const int MouseWheelClickSize = 120;

        public const int MouseEventLeftDown = 0x0002;

        public const int MouseEventLeftUp = 0x0004;

        public const int MouseEventRightDown = 0x0008;

        public const int MouseEventRightUp = 0x0010;

        public const int MouseEventWheel = 0x0800;

        public static bool SetCursorPos(double x, double y)
        {
            var result = MouseNativeMethods.SetCursorPos((int)x, (int)y);
            Thread.Sleep(50);
            return result;
        }

        public static void Click(MouseButton button, double x, double y)
        {
            SetCursorPos((int)x, (int)y);
            switch (button)
            {
                case MouseButton.Left:
                    LeftClick();
                    break;
                case MouseButton.Right:
                    RightClick();
                    break;
            }
        }

        public static void DoubleClick(MouseButton button, double x, double y)
        {
            SetCursorPos((int)x, (int)y);
            switch (button)
            {
                case MouseButton.Left:
                    DoubleLeftClick();
                    break;
                case MouseButton.Right:
                    DoubleRightClick();
                    break;
            }
        }

        public static void LeftClick()
        {
            MouseNativeMethods.mouse_event(MouseEventLeftDown, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
            MouseNativeMethods.mouse_event(MouseEventLeftUp, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
        }

        public static void RightClick()
        {
            MouseNativeMethods.mouse_event(MouseEventRightDown, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
            MouseNativeMethods.mouse_event(MouseEventRightUp, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
        }

        public static void DoubleLeftClick()
        {
            LeftClick();
            LeftClick();
        }

        public static void DoubleRightClick()
        {
            RightClick();
            RightClick();
        }

        public static void VerticalScroll(int amountOfClicks)
        {
            int data;
            try
            {
                data = checked(MouseWheelClickSize * amountOfClicks);
            }
            catch (OverflowException oe)
            {
                throw new ArgumentOutOfRangeException("amountOfClicks", oe.ToString());
            }

            MouseNativeMethods.mouse_event(MouseEventWheel, 0, 0, data, IntPtr.Zero);
            Thread.Sleep(50);
        }
    }
}
