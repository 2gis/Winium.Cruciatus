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
    using System.Windows.Forms;

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
        private const int MouseMoveSpeed = 2500;

        private const int MaxClickCount = 10;

        private AutomationElement parent;

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

            this.parent = parent;
            this.AutomationId = automationId;
        }

        internal CheckBox(AutomationElement element)
        {
            this.element = element;
        }

        /// <summary>
        /// Возвращает значение, указывающее, включен ли данный элемент управления.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<CheckBox, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<CheckBox, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        public bool? IsChecked
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
                        return null;
                }
            }
        }

        internal ToggleState ToggleState
        {
            get
            {
                return this.GetPropertyValue<CheckBox, ToggleState>(TogglePattern.ToggleStateProperty);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "CheckBox";
            }
        }

        internal override sealed string AutomationId { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.CheckBox;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент.
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

        public bool Check()
        {
            var oldState = this.IsChecked;
            if (oldState != null && oldState.Value)
            {
                return true;
            }

            return this.SetState(ToggleState.On);
        }

        public bool UnCheck()
        {
            var oldState = this.IsChecked;
            if (oldState != null && !oldState.Value)
            {
                return true;
            }

            return this.SetState(ToggleState.Off);
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
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
        
        private bool SetState(ToggleState newState)
        {
            if (!this.IsEnabled)
            {
                this.LastErrorMessage = string.Format("{0} отключен, нельзя изменить состояние.", this.ToString());
                return false;
            }

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
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
        /// Поиск текущего элемента в родительском
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId));

            // Если не нашли, то загрузить чекбокс не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
