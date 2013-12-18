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

    using Microsoft.VisualStudio.TestTools.UITesting;

    public class ClickableElement : BaseElement<ClickableElement>, IClickable, ILazyInitialize
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
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            this.Parent = parent;
            this.AutomationId = automationId;
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
                var windowsPoint = this.GetPropertyValue<ClickableElement, System.Windows.Point>(AutomationElement.ClickablePointProperty);

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

        /// <summary>
        /// Возвращает или задает уникальный идентификатор кликабельного элемента.
        /// </summary>
        internal override sealed string AutomationId { get; set; }

        /// <summary>
        /// Возвращает или задает элемент, который является родителем кликабельного элемента.
        /// </summary>
        internal AutomationElement Parent { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Custom;
            }
        }

        /// <summary>
        /// Возвращает инициализированный кликабельный элемент.
        /// </summary>
        internal override AutomationElement Element
        {
            get
            {
                if (this.element == null)
                {
                    this.Find();
                }

                return this.element;
            }
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.Parent = parent;
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Выполняет нажатие по кликабельному элементу.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        public virtual bool Click(MouseButtons mouseButton = MouseButtons.Left)
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

        internal override ClickableElement FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            return this;
        }

        /// <summary>
        /// Поиск кликабельного элемента в родительском элементе.
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.element = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null,
                CruciatusFactory.Settings.SearchTimeout);

            // Если не нашли, то загрузить кликабельный элемент не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
