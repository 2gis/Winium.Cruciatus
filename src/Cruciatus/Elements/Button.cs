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
    #region using

    using System;
    using System.Drawing;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Представляет элемент управления кнопка.
    /// </summary>
    public class Button : ClickableElement
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="Button"/>.
        /// </summary>
        public Button()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="Button"/>.
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
        public Button(CruciatusElement parent, string automationId)
            : base(parent, automationId)
        {
        }

        /// <summary>
        /// Возвращает координаты точки, внутри кнопки, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Кнопка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public new Point ClickablePoint
        {
            get
            {
                return base.ClickablePoint;
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "Button";
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
        /// Выполняет попытку нажатия по кнопке за время ожидания по умолчанию и кнопкой мыши по умолчанию.
        /// </summary>
        /// <returns>
        /// Значение true если нажать на кнопку удалось; в противном случае значение - false.
        /// </returns>
        public new bool Click()
        {
            return Click(CruciatusFactory.Settings.WaitForGetValueTimeout, CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Выполняет попытку нажатия по кнопке за заданное время ожидания и кнопкой мыши по умолчанию.
        /// </summary>
        /// <param name="waitingTime">
        /// Задает время ожидания на выполнение действия (миллисекунды).
        /// </param>
        /// <returns>
        /// Значение true если нажать на кнопку удалось; в противном случае значение - false.
        /// </returns>
        public bool Click(int waitingTime)
        {
            return Click(waitingTime, CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Выполняет попытку нажатия по кнопке за заданное время ожидания и заданной кнопкой мыши.
        /// </summary>
        /// <param name="waitingTime">
        /// Задает время ожидания на выполнение действия (миллисекунды).
        /// </param>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие.
        /// </param>
        /// <returns>
        /// Значение true если нажать на кнопку удалось; в противном случае значение - false.
        /// </returns>
        public bool Click(int waitingTime, MouseButtons mouseButton)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => IsEnabled, 
                    value => value != true, 
                    waitingTime);

                if (!isEnabled)
                {
                    LastErrorMessage = string.Format("{0} отключена, нельзя выполнить нажатие.", ToString());
                    return false;
                }

                return base.Click(mouseButton);
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }
    }
}
