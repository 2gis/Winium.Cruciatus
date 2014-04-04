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
    #region using

    using System;
    using System.Data;
    using System.Drawing;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    #endregion

    /// <summary>
    /// Представляет элемент управления текстовое поле.
    /// </summary>
    public class TextBox : CruciatusElement, IContainerElement, IListElement
    {
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
            Initialize(parent, automationId);
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
                return this.GetPropertyValue<bool>(AutomationElement.IsEnabledProperty);
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
        public Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new Point((int)windowsPoint.X, (int)windowsPoint.Y);
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
                return this.GetPropertyValue<bool>(ValuePattern.IsReadOnlyProperty);
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
                    if (Element.TryGetCurrentPattern(TextPattern.Pattern, out pattern))
                    {
                        // Если текстовый шаблон поддерживается, то вернее получить текст так
                        return ((TextPattern)pattern).DocumentRange.GetText(-1);
                    }

                    // Иначе текст получается так
                    return this.GetPropertyValue<string>(ValuePattern.ValueProperty);
                }
                catch (CruciatusException exc)
                {
                    LastErrorMessage = exc.Message;
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

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Edit;
            }
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        void IListElement.Initialize(AutomationElement element)
        {
            Initialize(element);
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
                if (!IsEnabled)
                {
                    LastErrorMessage = string.Format("{0} отключен, нельзя заполнить текстом.", ToString());
                    return false;
                }

                if (IsReadOnly)
                {
                    LastErrorMessage = string.Format("{0} доступен только для чтения.", ToString());
                    return false;
                }

                CruciatusCommand.Click(ClickablePoint, MouseButtons.Left);

                Keyboard.SendKeys("^a");
                Keyboard.SendKeys(string.IsNullOrEmpty(text)
                                      ? "{Back}"
                                      : text);
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }

            return true;
        }
    }
}
