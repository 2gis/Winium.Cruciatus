// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClickableElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет кликабельный элемент управления.
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

    public class ClickableElement : CruciatusElement, IContainerElement, IListElement, IClickable
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ClickableElement"/>.
        /// </summary>
        public ClickableElement()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ClickableElement"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для кликабельного элемента.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор кликабельного элемента.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public ClickableElement(AutomationElement parent, string automationId)
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
                return "ClickableElement";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Custom;
            }
        }

        /// <summary>
        /// Возвращает координаты точки, внутри кликабельного элемента, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Элемент не поддерживает данное свойство.
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
        /// Выполняет нажатие по кликабельному элементу кнопкой по умолчанию.
        /// </summary>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        public bool Click()
        {
            return Click(CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Выполняет нажатие по кликабельному элементу.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие.
        /// </param>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        public virtual bool Click(MouseButtons mouseButton)
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

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        void IListElement.Initialize(AutomationElement element)
        {
            Initialize(element);
        }
    }
}
