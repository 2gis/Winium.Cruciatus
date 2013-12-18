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

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент управления кнопка.
    /// </summary>
    public class Button : ClickableElement
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Button"/>.
        /// </summary>
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
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public Button(AutomationElement parent, string automationId)
            : base(parent, automationId)
        {
        }

        /// <summary>
        /// Возвращает значение, указывающее, включена ли кнопка.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Кнопка не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<ClickableElement, bool>(AutomationElement.IsEnabledProperty);
            }
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
        public new System.Drawing.Point ClickablePoint
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
        /// Выполняет нажатие по кнопке за время ожидания по умолчанию.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        /// <returns>
        /// Значение true если нажать на кнопку удалось; в противном случае значение - false.
        /// </returns>
        public override bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            return this.Click(CruciatusFactory.Settings.WaitForGetValueTimeout, mouseButton);
        }

        /// <summary>
        /// Выполняет нажатие по кнопке за заданное время ожидания.
        /// </summary>
        /// <param name="waitingTime">
        /// Задает время ожидания на выполнение действия (миллисекунды).
        /// </param>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        /// <returns>
        /// Значение true если нажать на кнопку удалось; в противном случае значение - false.
        /// </returns>
        public bool Click(int waitingTime, MouseButtons mouseButton = MouseButtons.Left)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true,
                    waitingTime);

                if (!isEnabled)
                {
                    this.LastErrorMessage = string.Format("{0} отключена, нельзя выполнить нажатие.", this.ToString());
                    return false;
                }

                return base.Click(mouseButton);
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }
        }
    }
}
