namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Runtime.InteropServices;

    #endregion

    internal static class MouseNativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        internal static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, IntPtr dwExtraInfo);
    }
}
