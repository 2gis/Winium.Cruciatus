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
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;
    using Point = System.Drawing.Point;
    using Size = System.Drawing.Size;

    /// <summary>
    /// Представляет элемент управления чекбокс.
    /// </summary>
    public class CheckBox : BaseElement<CheckBox>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        /// <summary>
        /// Индетификатор кнопки.
        /// </summary>
        private string automationId;

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
            this.automationId = automationId;
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
                return (bool)this.Element.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает координаты прямоугольника, который полностью охватывает элемент.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                var rect = (Rect)this.Element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                // Усечение дабла дает немного меньший прямоугольник, но он внутри изначального
                return new Rectangle(new Point((int)rect.X, (int)rect.Y), new Size((int)rect.Width, (int)rect.Height));
            }
        }

        /// <summary>
        /// Возвращает состояние чекбокса.
        /// </summary>
        public CheckState CheckState
        {
            get
            {
                return (CheckState)this.Element.GetCurrentPropertyValue(TogglePattern.ToggleStateProperty);
            }
        }

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
        protected override AutomationElement Element
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
        /// Устанавливает чекбоксу состояние сheck.
        /// </summary>
        public void Check()
        {
            var oldState = this.CheckState;
            if (oldState == CheckState.Checked)
            {
                return;
            }

            this.SetState(CheckState.Checked);
        }

        /// <summary>
        /// Устанавливает чекбоксу состояние uncheck.
        /// </summary>
        public void UnCheck()
        {
            var oldState = this.CheckState;
            if (oldState == CheckState.Unchecked)
            {
                return;
            }

            this.SetState(CheckState.Unchecked);
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.automationId = automationId;
        }

        internal override CheckBox FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            this.CheckingOfProperties();

            return this;
        }

        protected override void CheckingOfProperties()
        {
            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.IsEnabledProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство Enabled
                throw new Exception("чекбокс не поддерживает свойство Enabled");
            }

            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.BoundingRectangleProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство BoundingRectangle
                throw new Exception("чекбокс не поддерживает свойство BoundingRectangle");
            }

            if (!this.Element.GetSupportedProperties().Contains(TogglePattern.ToggleStateProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство State
                throw new Exception("чекбокс не поддерживает свойство State");
            }
        }

        /// <summary>
        /// Устанавливает чекбоксу указанное состояние.
        /// </summary>
        /// <param name="newState">
        /// Устанавливаемое состояние.
        /// </param>
        private void SetState(CheckState newState)
        {
            if (!this.IsEnabled)
            {
                throw new ElementNotEnabledException("Чекбокс отключен, нельзя изменить состояние.");
            }

            var controlBoundingRect = this.BoundingRectangle;

            // TODO Вынести это действие как расширения для типа Rectangle
            var clickablePoint = Point.Add(controlBoundingRect.Location, new Size(controlBoundingRect.Width / 2, controlBoundingRect.Height / 2));

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(clickablePoint);
            int maxClickCount = 4;
            while (this.CheckState != newState && maxClickCount != 0)
            {
                Mouse.Click(clickablePoint);
                --maxClickCount;
            }

            if (maxClickCount == 0)
            {
                // TODO: Исключение вида - не удалось установить состояние newState контролу automationId
                throw new Exception("Не получилось установить состояние чекбоксу.");
            }
        }

        /// <summary>
        /// Поиск текущего элемента в родительском
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.automationId));

            // Если не нашли, то загрузить кнопку не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("чекбокс не найден");
            }

            this.CheckingOfProperties();
        }
    }
}
