﻿namespace Winium.Cruciatus.Helpers
{
    #region using

    using System.Drawing;
    using System.Windows;

    #endregion

    internal static class ScreenCoordinatesHelper
    {
        #region Static Fields

        internal static readonly Point VirtualScreenLowerRightCorner = new Point(65535, 65535);

        #endregion

        #region Methods

        internal static Point ScreenPointToVirtualScreenPoint(Point point)
        {
            double sX = point.X;
            double sY = point.Y;

            var virtualScreenLeft = SystemParameters.VirtualScreenLeft;
            if (virtualScreenLeft < 0)
            {
                sX -= virtualScreenLeft;
            }

            var vsX = sX * (VirtualScreenLowerRightCorner.X / SystemParameters.VirtualScreenWidth);
            var vsY = sY * (VirtualScreenLowerRightCorner.Y / SystemParameters.VirtualScreenHeight);

            return new Point((int)vsX, (int)vsY);
        }

        #endregion
    }
}
