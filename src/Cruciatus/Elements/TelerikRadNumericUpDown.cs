// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelerikRadNumericUpDown.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления числовое поле (from Telerik).
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    #endregion

    /// <summary>
    /// Представляет элемент управления числовое поле (from Telerik).
    /// </summary>
    public class TelerikRadNumericUpDown : CruciatusElement, IContainerElement, IListElement
    {
        private Button _decreaseButton = new Button();

        private Button _increaseButton = new Button();

        private TextBox _textBox;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TelerikRadNumericUpDown"/>.
        /// </summary>
        public TelerikRadNumericUpDown()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TelerikRadNumericUpDown"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для числового поля.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор числового поля.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public TelerikRadNumericUpDown(AutomationElement parent, string automationId)
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

        public string Text
        {
            get
            {
                if (ElementInstance == null)
                {
                    Find();
                }

                return _textBox.Text;
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "TelerikRadNumericUpDown";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Spinner;
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

        public bool SetText(string text)
        {
            if (ElementInstance == null)
            {
                Find();
            }

            if (_textBox.SetText(text))
            {
                return true;
            }

            // TODO: информация об ошибке должна быть более точной
            LastErrorMessage = string.Format("Не удалось установить текст элементу {0}.", ToString());
            return false;
        }

        public bool ClickIncreaseButton()
        {
            if (ElementInstance == null)
            {
                Find();
            }

            if (_increaseButton.Click())
            {
                return true;
            }

            // TODO: информация об ошибке должна быть более точной
            LastErrorMessage = string.Format("Не удалось нажать по увеличивающей значение кнопке {0}.", ToString());
            return false;
        }

        public bool ClickDecreaseButton()
        {
            if (ElementInstance == null)
            {
                Find();
            }

            if (_decreaseButton.Click())
            {
                return true;
            }

            // TODO: информация об ошибке должна быть более точной
            LastErrorMessage = string.Format("Не удалось нажать по уменьшающей значение кнопке {0}.", ToString());
            return false;
        }

        internal override void Find()
        {
            base.Find();

            _textBox = new TextBox(ElementInstance, "textbox");
            _increaseButton = new Button(ElementInstance, "increase");
            _decreaseButton = new Button(ElementInstance, "decrease");
        }
    }
}
