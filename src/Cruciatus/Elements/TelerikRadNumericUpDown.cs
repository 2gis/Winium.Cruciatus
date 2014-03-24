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
    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет элемент управления числовое поле (from Telerik).
    /// </summary>
    public class TelerikRadNumericUpDown : CruciatusElement, IContainerElement, IListElement
    {
        private TextBox textBox;

        private Button increaseButton = new Button();

        private Button decreaseButton = new Button();

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
            this.Initialize(parent, automationId);
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

        public Button IncreaseButton
        {
            get
            {
                if (this.ElementInstance == null)
                {
                    this.Find();
                }

                return this.increaseButton;
            }
        }

        public Button DecreaseButton
        {
            get
            {
                if (this.ElementInstance == null)
                {
                    this.Find();
                }

                return this.decreaseButton;
            }
        }

        public string Text
        {
            get
            {
                if (this.ElementInstance == null)
                {
                    this.Find();
                }

                return this.textBox.Text;
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

        public bool SetText(string text)
        {
            if (this.ElementInstance == null)
            {
                this.Find();
            }

            return this.textBox.SetText(text);
        }

        internal override void Find()
        {
            base.Find();

            this.textBox = new TextBox(this.ElementInstance, "textbox");
            this.increaseButton = new Button(this.ElementInstance, "increase");
            this.decreaseButton = new Button(this.ElementInstance, "decrease");
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }

        void IListElement.Initialize(AutomationElement element)
        {
            this.Initialize(element);
        }
    }
}
