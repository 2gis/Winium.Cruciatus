// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Menu.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент меню.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Windows.Automation;

    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет элемент управления меню.
    /// </summary>
    public class Menu : CruciatusElement, IContainerElement
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Menu"/>.
        /// </summary>
        public Menu()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Menu"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для меню.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор меню.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public Menu(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }

        internal override string ClassName
        {
            get
            {
                return "Menu";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Menu;
            }
        }

        /// <summary>
        /// Выбирает элемент меню, указанный последним в пути.
        /// </summary>
        /// <param name="headersPath">
        /// Путь из заголовков для прохода (пример: control$view$zoom).
        /// </param>
        /// <returns>
        /// Значение true если операция завершена успешна; в противном случае значение - false.
        /// </returns>
        public virtual bool SelectItem(string headersPath)
        {
            if (headersPath == null)
            {
                throw new ArgumentNullException("headersPath");
            }

            var headers = headersPath.Split('$');

            var current = this.Element;
            foreach (var header in headers)
            {
                var condition = new PropertyCondition(AutomationElement.NameProperty, header);
                current = current.FindFirst(TreeScope.Children, condition);
                if (current == null)
                {
                    this.LastErrorMessage = string.Format(
                        "В {0} нет меню с заголовком {1}.",
                        this.ToString(),
                        header);
                    return false;
                }

                var clickableElement = new ClickableElement();
                ((IListElement)clickableElement).Initialize(current);
                if (!clickableElement.Click())
                {
                    this.LastErrorMessage = string.Format(
                        "Не удалось кликнуть по меню {0}. Подробности: {1}",
                        header,
                        clickableElement.LastErrorMessage);
                    return false;
                }
            }

            return true;
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            this.Initialize(parent, automationId);
        }
    }
}