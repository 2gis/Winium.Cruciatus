namespace Cruciatus.Core
{
    #region using

    using System.Windows;

    #endregion

    public static class ScreenCoordinatesHelper
    {
        public static readonly Point VirtualScreenLowerRightCorner = new Point(65535, 65535);

        public static Point ScreenPointToVirtualScreenPoint(Point point)
        {
            var sX = point.X;
            var sY = point.Y;

            var vsLeft = SystemParameters.VirtualScreenLeft;
            if (vsLeft < 0)
            {
                sX -= vsLeft;
            }

            var vsX = sX * (VirtualScreenLowerRightCorner.X / SystemParameters.VirtualScreenWidth);
            var vsY = sY * (VirtualScreenLowerRightCorner.Y / SystemParameters.VirtualScreenHeight);

            return new Point(vsX, vsY);
        }
    }
}
