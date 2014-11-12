// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextMenu.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент контекстного меню.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Windows.Automation;

    #endregion

    /// <summary>
    /// Представляет элемент управления контекстным меню.
    /// </summary>
    public class ContextMenu : Menu
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="ContextMenu"/>.
        /// </summary>
        public ContextMenu()
        {
        }

        /// <summary>
        /// Создает и инициализирует новый экземпляр класса <see cref="ContextMenu"/>.
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
        public ContextMenu(CruciatusElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        internal override string ClassName
        {
            get
            {
                return "ContextMenu";
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
        /// Выбирает элемент контекстного меню, указанного последним в пути.
        /// </summary>
        /// <param name="headersPath">
        /// Путь из заголовков для прохода (пример: control$view$zoom).
        /// </param>
        /// <returns>
        /// Значение true если операция завершена успешна; в противном случае значение - false.
        /// </returns>
        public new bool SelectItem(string headersPath)
        {
            try
            {
                return base.SelectItem(headersPath);
            }
            finally
            {
                ElementInstance = null;
            }
        }
    }
}
