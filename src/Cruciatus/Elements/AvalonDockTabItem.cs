// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvalonDockTabItem.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления вкладка (библиотеки AvalonDock).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;

    public abstract class AvalonDockTabItem : TabItem
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AvalonDockTabItem"/>.
        /// </summary>
        protected AvalonDockTabItem()
        {
        }

         /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="AvalonDockTabItem"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для вкладки.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор дочернего элемента вкладки (1 уровня).
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        protected AvalonDockTabItem(AutomationElement parent, string automationId)
            : base(parent, automationId)
        {
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "AvalonDockTabItem";
            }
        }

        /// <summary>
        /// Поиск вкладки в родительском элементе.
        /// </summary>
        internal override void Find()
        {
            // Сначала ищем дочерний элемент искомой вкладки по заданному AutomationId
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            var elementChild = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null,
                CruciatusFactory.Settings.SearchTimeout);

            // Если не нашли его, то и вкладку загрузить не удалось
            if (elementChild == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }

            var walker = new TreeWalker(Condition.TrueCondition);
            this.ElementInstance = walker.GetParent(elementChild);

            // На всякий случай проверяем, что родитель успешно получен
            if (this.ElementInstance == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
