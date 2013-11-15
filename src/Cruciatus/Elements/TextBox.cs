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
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент управления текстовое поле.
    /// </summary>
    public class TextBox : BaseElement<TextBox>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

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
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Возвращает значение, указывающее, включен ли данный элемент управления.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<TextBox, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<TextBox, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.GetPropertyValue<TextBox, bool>(ValuePattern.IsReadOnlyProperty);
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
                return this.GetPropertyValue<TextBox, string>(ValuePattern.ValueProperty);
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

                Mouse.MouseMoveSpeed = MouseMoveSpeed;
                Mouse.Move(this.ClickablePoint);
                Mouse.Click(MouseButtons.Left);
                Keyboard.SendKeys("^a");
                Keyboard.SendKeys(value);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "TextBox";
            }
        }

        internal override sealed string AutomationId { get; set; }

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

        internal override TextBox FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            return this;
        }

        /// <summary>
        /// Поиск элемента управления в указанном процессе.
        /// </summary>
        private void Find()
        {
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId));

            // Если не нашли, то загрузить элемент не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
