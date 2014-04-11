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
    #region using

    using System;
    using System.Drawing;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    #endregion

    /// <summary>
    /// Представляет элемент управления чекбокс.
    /// </summary>
    public class CheckBox : CruciatusElement, IContainerElement, IListElement
    {
        private const int MaxClickCount = 10;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="CheckBox"/>.
        /// </summary>
        public CheckBox()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="CheckBox"/>.
        /// </summary>
        /// <param name="parent">
        /// Родительский элемент.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор в рамках родительского элемента.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public CheckBox(CruciatusElement parent, string automationId)
        {
            Initialize(parent, automationId);
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
                return this.GetPropertyValue<bool>(AutomationElement.IsEnabledProperty);
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
        public Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new Point((int)windowsPoint.X, (int)windowsPoint.Y);
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
                switch (ToggleState)
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
                return this.GetPropertyValue<ToggleState>(TogglePattern.ToggleStateProperty);
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

        internal override ControlType GetType
        {
            get
            {
                return ControlType.CheckBox;
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
                if (IsChecked)
                {
                    return true;
                }

                return SetState(ToggleState.On);
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Устанавливает чекбокс в состояние не чекнут.
        /// </summary>
        /// <returns>
        /// Значение true если удалось установить состояние; в противном случае значение - false.
        /// </returns>
        public bool Uncheck()
        {
            try
            {
                if (!IsChecked)
                {
                    return true;
                }

                return SetState(ToggleState.Off);
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
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
            if (!IsEnabled)
            {
                LastErrorMessage = string.Format("{0} отключен, нельзя изменить состояние.", ToString());
                return false;
            }

            Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
            Mouse.Move(ClickablePoint);

            int maxClickCount = MaxClickCount;
            while (ToggleState != newState && maxClickCount != 0)
            {
                Mouse.Click(ClickablePoint);
                --maxClickCount;
            }

            return maxClickCount != 0;
        }
    }
}
