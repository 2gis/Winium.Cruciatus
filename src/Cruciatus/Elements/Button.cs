// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Button.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления кнопка.
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
    /// Представляет элемент управления кнопка.
    /// </summary>
    public class Button : BaseElement<Button>, ILazyInitialize
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Button"/>.
        /// </summary>
        public Button()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Button"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для кнопки.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор кнопки.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public Button(AutomationElement parent, string automationId)
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
        /// Возвращает значение, указывающее, включена ли кнопка.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Кнопка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<Button, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает координаты точки, внутри кнопки, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Кнопка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<Button, System.Windows.Point>(AutomationElement.ClickablePointProperty);

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
                return "Button";
            }
        }

        /// <summary>
        /// Возвращает или задает уникальный идентификатор кнопки.
        /// </summary>
        internal override sealed string AutomationId { get; set; }

        /// <summary>
        /// Возвращает или задает элемент, который является родителем кнопки.
        /// </summary>
        internal AutomationElement Parent { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Button;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент кнопки.
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
        /// Выполняет нажатие по кнопке.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        /// <param name="waitingTime">
        /// Задает время ожидания на выполнение действия (миллисекунды).
        /// </param>
        /// <returns>
        /// Значение true если нажать на кнопку удалось; в противном случае значение - false.
        /// </returns>
        public bool Click(MouseButtons mouseButton = MouseButtons.Left, int waitingTime = 5000)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true,
                    waitingTime);

                if (!isEnabled)
                {
                    this.LastErrorMessage = string.Format("{0} отключена, нельзя выполнить нажатие.", this.ToString());
                    return false;
                }

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

        internal override Button FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            return this;
        }

        /// <summary>
        /// Поиск кнопки в родительском элементе.
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.element = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null);

            // Если не нашли, то загрузить кнопку не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
