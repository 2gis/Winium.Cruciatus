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
        /// Возвращает значение, указывающее, включена ли таблица.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Таблица не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<DataGrid, bool>(AutomationElement.IsEnabledProperty);
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

        /// <summary>
        /// Определяет, существует ли ячейка с указанным номером строки и ячейки.
        /// </summary>
        /// <param name="row">
        /// Номер строки.
        /// </param>
        /// <param name="column">
        /// Номер колонки.
        /// </param>
        /// <returns>
        /// Значение true, если ячейка существует;
        /// false, если ячейка не существует или находится вне видимости.
        /// </returns>
        public bool CellExists(int row, int column)
        {
            return row >= 0 && row < this.RowCount && column >= 0 && column <= this.RowCount;
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            this.Parent = parent;
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Возвращает элемент заданного типа с указанным номером строки и колонки.
        /// </summary>
        /// <param name="row">
        /// Номер строки.
        /// </param>
        /// <param name="column">
        /// Номер колонки.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Искомый элемент, либо null, если найти не удалось.
        /// </returns>
        public T Item<T>(int row, int column) where T : BaseElement<T>, new()
        {
            // Проверка, что таблица включена
            var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);
            if (!isEnabled)
            {
                this.LastErrorMessage = string.Format("{0} отключена.", this.ToString());
                return null;
            }

            // Проверка валидности номера строки и столбца
            if (!this.CellExists(row, column))
            {
                this.LastErrorMessage = string.Format(
                    "В {0} ячейка [{1}, {2}] не существует или находится вне видимости.",
                    this.ToString(),
                    row,
                    column);
                return null;
            }
            
            // Получение ячейки
            var gridPattern = this.Element.GetCurrentPattern(GridPattern.Pattern) as GridPattern;
            if (gridPattern == null)
            {
                this.LastErrorMessage = string.Format("{0} не поддерживает шаблон таблицы.", this.ToString());
                return null;
            }

            var cell = gridPattern.GetItem(row, column);

            // Проверка, что ячейку видно
            if (cell == null || !this.Element.GeometricallyContains(cell))
            {
                this.LastErrorMessage = string.Format("В {0} ячейка [{1}, {2}] вне видимости.", this.ToString(), row, column);
                return null;
            }

            // Поиск подходящего элемента в ячейке
            var item = new T();
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType);
            var elem = cell.FindFirst(TreeScope.Subtree, condition);
            if (elem == null)
            {
                this.LastErrorMessage = string.Format(
                    "В {0}, ячейка [{1}, {2}], нет элемента желаемого типа.",
                    this.ToString(),
                    row,
                    column);
                return null;
            }

            item.FromAutomationElement(elem);
            return item;
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
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.element = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null,
                CruciatusFactory.Settings.SearchTimeout);

            // Если не нашли, то загрузить кнопку не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}