// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonApplicationMenu.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент меню ленты.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// Представляет элемент управления меню ленты.
    /// </summary>
    public class RibbonApplicationMenu : Menu
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="RibbonApplicationMenu"/>.
        /// </summary>
        public RibbonApplicationMenu()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="RibbonApplicationMenu"/>.
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
        public RibbonApplicationMenu(CruciatusElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        internal override string ClassName
        {
            get
            {
                return "RibbonApplicationMenu";
            }
        }

        /// <summary>
        /// Выполняет проход по меню ленты.
        /// </summary>
        /// <param name="headersPath">
        /// Путь из заголовков для прохода (пример: control$view$zoom).
        /// </param>
        /// <returns>
        /// Значение true если операция завершена успешна; в противном случае значение - false.
        /// </returns>
        public override bool SelectItem(string headersPath)
        {
            var clickableElement = new ClickableElement { ElementInstance = Element };
            if (clickableElement.Click())
            {
                return base.SelectItem(headersPath);
            }

            LastErrorMessage = string.Format("Не удалось открыть меню {0}.", ToString());
            return false;
        }
    }
}
