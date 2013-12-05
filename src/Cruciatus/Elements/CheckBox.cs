// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления чекбокс.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент управления чекбокс.
    /// </summary>
    public class CheckBox : BaseElement<CheckBox>, ILazyInitialize
    {
        private const int MaxClickCount = 10;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CheckBox"/>.
        /// </summary>
        public CheckBox()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для чекбокса.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор чекбокса.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public CheckBox(AutomationElement parent, string automationId)
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
        /// Возвращает значение, указывающее, включен ли чекбокс.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Чекбокс не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<CheckBox, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает координаты точки, внутри чекбокса, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Чекбокс не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<CheckBox, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее, чекнут ли чекбокс.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Чекбокс не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsChecked
        {
            get
            {
                switch (this.ToggleState)
                {
                    case ToggleState.On:
                        return true;

                    case ToggleState.Off:
                        return false;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Возвращает состояние чекнутости чекбокса.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Чекбокс не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        internal ToggleState ToggleState
        {
            get
            {
                return this.GetPropertyValue<CheckBox, ToggleState>(TogglePattern.ToggleStateProperty);
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "CheckBox";
            }
        }

        /// <summary>
        /// Возвращает или задает уникальный идентификатор чекбокса.
        /// </summary>
        internal override sealed string AutomationId { get; set; }

        /// <summary>
        /// Возвращает или задает элемент, который является родителем чекбокса.
        /// </summary>
        internal AutomationElement Parent { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.CheckBox;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент чекбокса.
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

        /// <summary>
        /// Устанавливает чекбокс в состояние чекнут.
        /// </summary>
        /// <returns>
        /// Значение true если удалось установить состояние; в противном случае значение - false.
        /// </returns>
        public bool Check()
        {
            try
            {
                if (this.IsChecked)
                {
                    return true;
                }

                return this.SetState(ToggleState.On);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Устанавливает чекбокс в состояние не чекнут.
        /// </summary>
        /// <returns>
        /// Значение true если удалось установить состояние; в противном случае значение - false.
        /// </returns>
        public bool UnCheck()
        {
            try
            {
                if (!this.IsChecked)
                {
                    return true;
                }

                return this.SetState(ToggleState.Off);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.Parent = parent;
            this.AutomationId = automationId;
        }

        internal override CheckBox FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            return this;
        }

        /// <summary>
        /// Устанавливает чекбоксу заданное состояние.
        /// </summary>
        /// <param name="newState">
        /// Новое состояние.
        /// </param>
        /// <returns>
        /// Значение true если удалось установить состояние; в противном случае значение - false.
        /// </returns>
        /// <exception cref="PropertyNotSupportedException">
        /// Чекбокс не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        private bool SetState(ToggleState newState)
        {
            if (!this.IsEnabled)
            {
                this.LastErrorMessage = string.Format("{0} отключен, нельзя изменить состояние.", this.ToString());
                return false;
            }

            Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
            Mouse.Move(this.ClickablePoint);

            int maxClickCount = MaxClickCount;
            while (this.ToggleState != newState && maxClickCount != 0)
            {
                Mouse.Click(this.ClickablePoint);
                --maxClickCount;
            }

            return maxClickCount != 0;
        }

        /// <summary>
        /// Поиск чекбокса в родительском элементе.
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.element = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null,
                CruciatusFactory.Settings.SearchTimeout);

            // Если не нашли, то загрузить чекбокс не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
