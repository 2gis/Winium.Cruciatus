// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableComboBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления редактируемый выпадающий список.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Drawing;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Extensions;

    using Microsoft.VisualStudio.TestTools.UITesting;

    /// <summary>
    /// Представляет элемент управления редактируемый выпадающий список.
    /// </summary>
    public class EditableComboBox : ComboBox
    {
        /// <summary>
        /// Устанавливает текст в текстовое поле выпадающего списка.
        /// </summary>
        /// <param name="text">
        /// Устанавливаемый текст.
        /// </param>
        /// <exception cref="ElementNotEnabledException">
        /// Редактируемый выпадающий список отключен.
        /// </exception>
        /// <returns>
        /// Значение true если установить текст удалось; в противном случае значение - false.
        /// </returns>
        public bool SetText(string text)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);

                if (!isEnabled)
                {
                    throw new ElementNotEnabledException(string.Format("{0} отключен, нельзя установить текст.", this.ToString()));
                }

                Mouse.MouseMoveSpeed = MouseMoveSpeed;
                Mouse.Move(this.ClickablePoint);
                Mouse.Click(MouseButtons.Left);
                Keyboard.SendKeys("^a");
                Keyboard.SendKeys(text);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }

            return true;
        }

        protected override bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);

                if (!isEnabled)
                {
                    this.LastErrorMessage = string.Format("{0} отключен, нельзя выполнить нажатие.", this.ToString());
                    return false;
                }

                var topRightPoint = this.GetPropertyValue<ComboBox, System.Windows.Rect>(AutomationElement.BoundingRectangleProperty).TopRight;
                var clickablePoint = new Point((int)topRightPoint.X - 5, (int)topRightPoint.Y + 5);

                Mouse.MouseMoveSpeed = MouseMoveSpeed;
                Mouse.Move(clickablePoint);
                Mouse.Click(mouseButton);

                if (!this.Element.WaitForElementReady())
                {
                    this.LastErrorMessage = string.Format("Время ожидания готовности для {0} истекло.", this.ToString());
                    return false;
                }

                return true;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }
        }
    }
}