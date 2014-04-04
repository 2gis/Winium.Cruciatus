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
    #region using

    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;

    using Microsoft.VisualStudio.TestTools.UITesting;

    #endregion

    internal static class CruciatusCommand
    {
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
        internal static void Click(Point clickablePoint, MouseButtons mouseButton)
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
