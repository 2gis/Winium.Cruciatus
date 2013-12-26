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
        /// Инициализирует новый экземпляр класса <see cref="EditableComboBox"/>.
        /// </summary>
        public EditableComboBox()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EditableComboBox"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для редактируемого выпадающего списка.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор редактируемого выпадающего списка.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public EditableComboBox(AutomationElement parent, string automationId)
            : base(parent, automationId)
        {
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "EditableComboBox";
            }
        }

        /// <summary>
        /// Устанавливает текст в текстовое поле редактируемого выпадающего списка.
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
                    this.LastErrorMessage = string.Format("{0} отключен, нельзя установить текст.", this.ToString());
                    return false;
                }

                Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
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

                var topRightPoint = this.GetPropertyValue<System.Windows.Rect>(AutomationElement.BoundingRectangleProperty).TopRight;
                var clickablePoint = new Point((int)topRightPoint.X - 5, (int)topRightPoint.Y + 5);

                if (!CruciatusCommand.Click(clickablePoint, mouseButton, out this.LastErrorMessageInstance))
                {
                    return false;
                }

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