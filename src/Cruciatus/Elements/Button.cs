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
    /// Представляет элемент управления кнопка.
    /// </summary>
    public class Button : BaseElement<Button>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        /// <summary>
        /// Индетификатор кнопки.
        /// </summary>
        private string automationId;

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
            this.automationId = automationId;
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

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.parent = parent;
            this.automationId = automationId;
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

            var controlBoundingRect = this.BoundingRectangle;

            // TODO Вынести это действие как расширения для типа Rectangle
            var clickablePoint = Point.Add(controlBoundingRect.Location, new Size(controlBoundingRect.Width / 2, controlBoundingRect.Height / 2));

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(clickablePoint);
            Mouse.Click(mouseButton);
        }

        internal override Button FromAutomationElement(AutomationElement element)
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
                throw new Exception("кнопка не поддерживает свойство Enabled");
            }

            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.BoundingRectangleProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство BoundingRectangle
                throw new Exception("кнопка не поддерживает свойство BoundingRectangle");
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
                throw new Exception("кнопка не найдена");
            }

            this.CheckingOfProperties();
        }
    }
}
