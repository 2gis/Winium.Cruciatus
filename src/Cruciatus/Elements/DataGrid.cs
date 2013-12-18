// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGrid.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления таблица.
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
    /// Представляет элемент управления таблица.
    /// </summary>
    public class DataGrid : BaseElement<DataGrid>, ILazyInitialize
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DataGrid"/>.
        /// </summary>
        public DataGrid()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="DataGrid"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для таблицы.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор таблицы.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public DataGrid(AutomationElement parent, string automationId)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            this.Parent = parent;
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Возвращает количество столбцов в таблице.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Таблица не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public int ColumnCount
        {
            get
            {
                return this.GetPropertyValue<DataGrid, int>(GridPattern.ColumnCountProperty);
            }
        }

        /// <summary>
        /// Возвращает количество строк в таблице.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Таблица не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public int RowCount
        {
            get
            {
                return this.GetPropertyValue<DataGrid, int>(GridPattern.RowCountProperty);
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "DataGrid";
            }
        }

        /// <summary>
        /// Возвращает или задает уникальный идентификатор выпадающего списка.
        /// </summary>
        internal override sealed string AutomationId { get; set; }

        /// <summary>
        /// Возвращает или задает элемент, который является родителем выпадающего списка.
        /// </summary>
        internal AutomationElement Parent { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.DataGrid;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент таблицы.
        /// </summary>
        internal override AutomationElement Element
        {
            get
            {
                if (this.element == null)
                {
                    this.Find();
                }

                return this.element;
            }
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.Parent = parent;
            this.AutomationId = automationId;
        }

        internal override DataGrid FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            return this;
        }

        /// <summary>
        /// Поиск таблицы в родительском элементе.
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.Parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId));

            // Если не нашли, то загрузить кнопку не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}