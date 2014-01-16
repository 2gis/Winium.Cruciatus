// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClickableElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет кликабельный элемент управления.
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

    public class ClickableElement : CruciatusElement, IContainerElement, IListElement, IClickable
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ClickableElement"/>.
        /// </summary>
        public ClickableElement()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ClickableElement"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для кликабельного элемента.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор кликабельного элемента.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public ClickableElement(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает координаты точки, внутри кликабельного элемента, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Элемент не поддерживает данное свойство.
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
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "ClickableElement";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Custom;
            }
        }

        /// <summary>
        /// Выполняет нажатие по кликабельному элементу кнопкой по умолчанию.
        /// </summary>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        public bool Click()
        {
            return CruciatusCommand.Click(this.ClickablePoint, out this.LastErrorMessageInstance);
        }

        /// <summary>
        /// Выполняет нажатие по кликабельному элементу.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие.
        /// </param>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        public virtual bool Click(MouseButtons mouseButton)
        {
            return CruciatusCommand.Click(this.ClickablePoint, mouseButton, out this.LastErrorMessageInstance);
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }

        void IListElement.Initialize(AutomationElement element)
        {
            this.Initialize(element);
        }
    }
}
