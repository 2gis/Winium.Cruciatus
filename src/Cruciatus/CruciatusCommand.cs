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

    using Cruciatus.Exceptions;

    using Microsoft.VisualStudio.TestTools.UITesting;

    internal static class CruciatusCommand
    {
        /// <summary>
        /// Нажимает кнопку мыши по умолчанию в заданной точке.
        /// </summary>
        /// <param name="clickablePoint">
        /// Точка, которую надо нажать.
        /// </param>
        internal static void Click(System.Drawing.Point clickablePoint)
        {
            Click(clickablePoint, CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Нажимает заданную кнопку мыши в заданной точке.
        /// </summary>
        /// <param name="clickablePoint">
        /// Точка, которую надо нажать.
        /// </param>
        /// <param name="mouseButton">
        /// Кнопка мыши, используемая для нажатия.
        /// </param>
        /// <exception cref="CruciatusException">
        /// Ошибка при выполнении <c>CruciatusCommand.Click</c>.
        /// </exception>
        internal static void Click(System.Drawing.Point clickablePoint, MouseButtons mouseButton)
        {
            try
            {
                Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
                Mouse.Move(clickablePoint);
                Mouse.Click(mouseButton);
            }
            catch (Exception exc)
            {
                throw new CruciatusException("Ошибка при выполнении CruciatusCommand.Click", exc);
            }
        }
    }
}
