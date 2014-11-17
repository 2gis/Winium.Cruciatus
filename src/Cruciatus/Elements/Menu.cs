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
    using System.Linq;
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
            string name;
            var element = GetItem(headersPath, out name);
            if (element == null)
            {
                return false;
            }

            var clickableElement = new ClickableElement { ElementInstance = element };
            if (!clickableElement.Click())
            {
                LastErrorMessage = string.Format(
                    "Не удалось кликнуть по элементу меню {0}. Подробности: {1}",
                    name,
                    clickableElement.LastErrorMessage);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Возвращает значение, указывающее, включен ли элемент меню.
        /// </summary>
        /// <param name="headersPath">
        /// Путь из заголовков для прохода (пример: control$view$zoom).
        /// </param>
        public bool ItemIsEnabled(string headersPath)
        {
            string name;
            var element = GetItem(headersPath, out name);
            if (element == null)
            {
                return false;
            }

            var clickableElement = new ClickableElement { ElementInstance = element };
            return clickableElement.IsEnabled;
        }

        private AutomationElement GetItem(string headersPath, out string name)
        {
            if (headersPath == null)
            {
                throw new ArgumentNullException("headersPath");
            }

            var headers = headersPath.Split('$');

            var item = Element;
            for (var i = 0; i < headers.Length; ++i)
            {
                var condition = new PropertyCondition(AutomationElement.NameProperty, headers[i]);
                item = item.FindFirst(TreeScope.Children, condition);
                if (item == null)
                {
                    LastErrorMessage = string.Format(
                        "В {0} нет меню с заголовком {1}.",
                        ToString(),
                        headers[i]);
                    name = string.Empty;
                    return null;
                }

                if (i == headers.Length - 1)
                {
                    break;
                }

                var clickableElement = new ClickableElement { ElementInstance = item };
                if (!clickableElement.Click())
                {
                    LastErrorMessage = string.Format(
                        "Не удалось кликнуть по элементу меню {0}. Подробности: {1}",
                        headers[i],
                        clickableElement.LastErrorMessage);
                    name = string.Empty;
                    return null;
                }
            }

            name = headers.Last();
            return item;
        }
    }
}
