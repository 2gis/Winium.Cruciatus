// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RadioButton.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления переключатель.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Drawing;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    #endregion

    /// <summary>
    /// Представляет элемент управления переключатель.
    /// </summary>
    public class RadioButton : CruciatusElement, IContainerElement, IListElement, IClickable
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="RadioButton"/>.
        /// </summary>
        public RadioButton()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="RadioButton"/>.
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
        public RadioButton(CruciatusElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "RadioButton";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.RadioButton;
            }
        }

        /// <summary>
        /// Возвращает координаты точки, внутри переключателя, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Переключатель не поддерживает данное свойство.
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
        /// Выполняет нажатие по переключателю кнопкой по умолчанию.
        /// </summary>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        public bool Click()
        {
            return Click(CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Выполняет нажатие по переключателю.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие.
        /// </param>
        /// <returns>
        /// Значение true если нажать на текстовый блок удалось; в противном случае значение - false.
        /// </returns>
        public bool Click(MouseButtons mouseButton)
        {
            try
            {
                CruciatusCommand.Click(ClickablePoint, mouseButton);
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
