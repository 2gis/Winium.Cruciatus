// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBlock.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления текстовый блок.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент управления текстовый блок.
    /// </summary>
    public class TextBlock : CruciatusElement, IContainerElement, IListElement
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TextBlock"/>.
        /// </summary>
        public TextBlock()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TextBlock"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для текстового блока.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор текстового блока.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public TextBlock(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает координаты точки, внутри текстового блока, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Текстовый блок не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        /// <summary>
        /// Возвращает текст из текстового блока.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Текстовый блок не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public string Text
        {
            get
            {
                try
                {
                    return this.GetPropertyValue<string>(AutomationElement.NameProperty);
                }
                catch (Exception exc)
                {
                    this.LastErrorMessage = exc.Message;
                    return null;
                }
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "TextBlock";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Text;
            }
        }

        /// <summary>
        /// Выполняет нажатие по текстовому блоку.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        /// <returns>
        /// Значение true если нажать на выпадающий список удалось; в противном случае значение - false.
        /// </returns>
        public bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            try
            {
                Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
                Mouse.Move(this.ClickablePoint);
                Mouse.Click(mouseButton);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }

            return true;
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        void IListElement.Initialize(AutomationElement element)
        {
            Initialize(element);
        }
    }
}
