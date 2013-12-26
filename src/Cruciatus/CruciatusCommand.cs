// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusCommand.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет внутренние команды фреймворка Cruciatus.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus
{
    using System;
    using System.Windows.Forms;

    using Microsoft.VisualStudio.TestTools.UITesting;

    internal static class CruciatusCommand
    {
        internal static bool Click(System.Drawing.Point clickablePoint, out string message)
        {
            return Click(clickablePoint, CruciatusFactory.Settings.ClickButton, out message);
        }

        internal static bool Click(System.Drawing.Point clickablePoint, MouseButtons mouseButton, out string message)
        {
            try
            {
                Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
                Mouse.Move(clickablePoint);
                Mouse.Click(mouseButton);
            }
            catch (Exception exc)
            {
                message = exc.Message;
                return false;
            }

            message = string.Empty;
            return true;
        }
    }
}
