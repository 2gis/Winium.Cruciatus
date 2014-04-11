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
    #region using

    using System;
    using System.Windows.Automation;

    using Cruciatus.Interfaces;

    #endregion

    /// <summary>
    /// Представляет элемент управления меню.
    /// </summary>
    public class Menu : CruciatusElement, IContainerElement
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="Menu"/>.
        /// </summary>
        public Menu()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="Menu"/>.
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
        public Menu(CruciatusElement parent, string automationId)
        {
            Initialize(parent, automationId);
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

            var current = Element;
            foreach (var header in headers)
            {
                var condition = new PropertyCondition(AutomationElement.NameProperty, header);
                current = current.FindFirst(TreeScope.Children, condition);
                if (current == null)
                {
                    LastErrorMessage = string.Format(
                        "В {0} нет меню с заголовком {1}.", 
                        ToString(), 
                        header);
                    return false;
                }

                var clickableElement = new ClickableElement { ElementInstance = current };
                if (!clickableElement.Click())
                {
                    LastErrorMessage = string.Format(
                        "Не удалось кликнуть по меню {0}. Подробности: {1}", 
                        header, 
                        clickableElement.LastErrorMessage);
                    return false;
                }
            }

            return true;
        }
    }
}
