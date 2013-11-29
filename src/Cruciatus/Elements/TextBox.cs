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

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TextBox"/>.
        /// </summary>
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
        /// Уникальный идентификатор текстового поля.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
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

            this.Parent = parent;
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Возвращает значение, указывающее, включено ли текстовое поле.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Текстовое поле не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<TextBox, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает координаты точки, внутри текстового поля, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Текстовое поле не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<TextBox, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее, доступно ли текстовое поле только для чтения.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Текстовое поле не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsReadOnly
        {
            get
            {
                return this.GetPropertyValue<TextBox, bool>(ValuePattern.IsReadOnlyProperty);
            }
        }

        /// <summary>
        /// Возвращает текст из текстового поля.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Текстовое поле не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public string Text
        {
            get
            {
                try
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
                catch (Exception exc)
                {
                    this.LastErrorMessage = exc.Message;
                    return null;
                }
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "TextBox";
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
                return ControlType.Edit;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент текстового поля.
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
        /// Устанавливает текст в текстовое поле.
        /// </summary>
        /// <param name="text">
        /// Устанавливаемый текст.
        /// </param>
        /// <exception cref="ElementNotEnabledException">
        /// Текстовое поле не включено.
        /// </exception>
        /// <exception cref="ReadOnlyException">
        /// Текстовое поле доступно только для чтения.
        /// </exception>
        /// <returns>
        /// Значение true если установить текст удалось; в противном случае значение - false.
        /// </returns>
        public bool SetText(string text)
        {
            try
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
                Keyboard.SendKeys(text);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }

            return true;
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.Parent = parent;
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
        /// Поиск текстового поля в родительском элементе.
        /// </summary>
        private void Find()
        {
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.element = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null);

            // Если не нашли, то загрузить элемент не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
