// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления текстовое поле.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;
    using Point = System.Drawing.Point;
    using PropertyCondition = System.Windows.Automation.PropertyCondition;
    using Size = System.Drawing.Size;

    /// <summary>
    /// Представляет элемент управления текстовое поле.
    /// </summary>
    public class TextBox : BaseElement<TextBox>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        /// <summary>
        /// Индетификатор элемента.
        /// </summary>
        private string automationId;

        private AutomationElement parent;

        public TextBox()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TextBox"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для текстового поля.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор элемента.
        /// </param>
        public TextBox(AutomationElement parent, string automationId)
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

                return new Rectangle(new Point((int)rect.X, (int)rect.Y), new Size((int)rect.Width, (int)rect.Height));
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)this.Element.GetCurrentPropertyValue(ValuePattern.IsReadOnlyProperty);
            }
        }

        /// <summary>
        /// Возвращает или задает текст в данном элементе управления.
        /// </summary>
        public string Text
        {
            get
            {
                object pattern;
                if (this.Element.TryGetCurrentPattern(TextPattern.Pattern, out pattern))
                {
                    // Если текстовый шаблон поддерживается, то вернее получить текст так
                    return ((TextPattern)pattern).DocumentRange.GetText(-1);
                }

                // Иначе текст получается так
                return (string)this.Element.GetCurrentPropertyValue(ValuePattern.ValueProperty);
            }

            set
            {
                if (!this.IsEnabled)
                {
                    throw new ElementNotEnabledException("Текстовое поле отключено, нельзя заполнить текстом.");
                }

                if (this.IsReadOnly)
                {
                    throw new ReadOnlyException("Текстовое поле доступно только для чтения.");
                }

                var controlBoundingRect = this.BoundingRectangle;

                // TODO Вынести это действие как расширения для типа Rectangle
                var clickablePoint = Point.Add(controlBoundingRect.Location, new Size(controlBoundingRect.Width / 2, controlBoundingRect.Height / 2));

                Mouse.MouseMoveSpeed = MouseMoveSpeed;
                Mouse.Move(clickablePoint);
                Mouse.Click(MouseButtons.Left);
                Keyboard.SendKeys("^a");
                Keyboard.SendKeys(value);
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Edit;
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

        internal override TextBox FromAutomationElement(AutomationElement element)
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
                // TODO Исключение вида - контрол не поддерживает свойство Enabled
                throw new Exception("текстовое поле не поддерживает свойство Enabled");
            }

            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.BoundingRectangleProperty))
            {
                // TODO Исключение вида - контрол не поддерживает свойство BoundingRectangle
                throw new Exception("текстовое поле не поддерживает свойство BoundingRectangle");
            }

            if (!this.Element.GetSupportedProperties().Contains(ValuePattern.IsReadOnlyProperty))
            {
                // TODO Исключение вида - контрол не поддерживает свойство ReadOnly
                throw new Exception("текстовое поле не поддерживает свойство ReadOnly");
            }

            if (!this.Element.GetSupportedProperties().Contains(ValuePattern.ValueProperty))
            {
                // TODO Исключение вида - контрол не поддерживает свойство Value
                throw new Exception("текстовое поле не поддерживает свойство Value");
            }
        }

        /// <summary>
        /// Поиск элемента управления в указанном процессе.
        /// </summary>
        private void Find()
        {
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.automationId));

            // Если не нашли, то загрузить элемент не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("текстовое поле не найдено");
            }

            this.CheckingOfProperties();
        }
    }
}
