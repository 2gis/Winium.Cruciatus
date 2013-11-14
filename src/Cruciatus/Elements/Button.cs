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

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент управления кнопка.
    /// </summary>
    public class Button : BaseElement<Button>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        private AutomationElement parent;

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

            this.parent = parent;
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Возвращает значение, указывающее, включен ли данный элемент управления.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<Button, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<Button, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "Button";
            }
        }

        internal override sealed string AutomationId { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Button;
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

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Эмулирует нажатие кнопки мыши на данном элементе управления.
        /// </summary>
        /// <param name="mouseButton">
        /// Используемая кнопка мыши для нажатия.
        /// </param>
        public void Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            if (!this.IsEnabled)
            {
                throw new ElementNotEnabledException("Кнопка отключена, нельзя выполнить нажатие.");
            }

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(this.ClickablePoint);
            Mouse.Click(mouseButton);
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
        /// Поиск текущего элемента в родительском
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId));

            // Если не нашли, то загрузить кнопку не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("кнопка не найдена");
            }
        }
    }
}
