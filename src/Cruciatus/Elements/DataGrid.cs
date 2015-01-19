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
    #region using

    using System;
    using System.Windows.Automation;

    using Cruciatus.Core;
    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;

    #endregion

    /// <summary>
    /// Представляет элемент управления таблица.
    /// </summary>
    public class DataGrid : CruciatusElement
    {
        public DataGrid(CruciatusElement parent, By getStrategy)
            : base(parent, getStrategy)
        {
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
                return this.GetPropertyValue<int>(GridPattern.RowCountProperty);
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
                return this.GetPropertyValue<int>(GridPattern.ColumnCountProperty);
            }
        }

        /// <summary>
        /// Выполняет прокрутку до ячейки с указанным номером строки и колонки.
        /// </summary>
        /// <param name="row">
        /// Номер строки.
        /// </param>
        /// <param name="column">
        /// Номер колонки.
        /// </param>
        /// <returns>
        /// Значение true если прокрутить удалось либо в этом нет необходимости;
        /// в противном случае значение - false.
        /// </returns>
        public virtual void ScrollTo(int row, int column)
        {
            if (!Instanse.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", ToString());
                throw new ElementNotEnabledException("NOT SCROLL");
            }

            // Проверка на дурака
            if (row < 0 || column < 0)
            {
                var msg = string.Format(
                    "В {0} ячейка [{1}, {2}] не существует, т.к. задан отрицательный номер.", 
                    ToString(), 
                    row, 
                    column);
                Logger.Error(msg);
                throw new CruciatusException("NOT SCROLL");
            }

            // Получение шаблона прокрутки у таблицы
            var scrollPattern = Instanse.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                Logger.Error("{0} не поддерживает шаблон прокрутки.", ToString());
                throw new CruciatusException("NOT SCROLL");
            }

            // Условие для вертикального поиска ячейки [row, 0] (через строку)
            var cellCondition = new AndCondition(
                new PropertyCondition(AutomationElement.IsGridItemPatternAvailableProperty, true), 
                new PropertyCondition(GridItemPattern.RowProperty, row));

            // Стартовый поиск ячейки
            var cell = AutomationElementHelper.FindFirst(Instanse, TreeScope.Subtree, cellCondition);

            // Вертикальная прокрутка (при необходимости и возможности)
            if (cell == null && scrollPattern.Current.VerticallyScrollable)
            {
                // Установка самого верхнего положения прокрутки
                while (scrollPattern.Current.VerticalScrollPercent > 0.1)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);
                }

                // Установка самого левого положения прокрутки (при возможности)
                if (scrollPattern.Current.HorizontallyScrollable)
                {
                    while (scrollPattern.Current.HorizontalScrollPercent > 0.1)
                    {
                        scrollPattern.ScrollHorizontal(ScrollAmount.LargeIncrement);
                    }
                }

                // Основная вертикальная прокрутка
                while (cell == null && scrollPattern.Current.VerticalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);
                    cell = AutomationElementHelper.FindFirst(Instanse, TreeScope.Subtree, cellCondition);
                }
            }

            // Если прокрутив до конца ячейка не найдена, то номер строки не действительный
            if (cell == null)
            {
                Logger.Error("В {0} нет строки с номером {1}.", ToString(), row);
                throw new CruciatusException("NOT SCROLL");
            }

            // Если точка клика ячейки [row, 0] под границей таблицы - докручиваем по вертикали вниз
            while (cell.ClickablePointUnder(Instanse, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }

            // Если точка клика ячейки [row, 0] над границей таблицы - докручиваем по вертикали вверх
            while (cell.ClickablePointOver(Instanse))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
            }

            // Условие для горизонтального поиска ячейки [row, column]
            cellCondition = new AndCondition(
                new PropertyCondition(AutomationElement.IsGridItemPatternAvailableProperty, true), 
                new PropertyCondition(GridItemPattern.RowProperty, row), 
                new PropertyCondition(GridItemPattern.ColumnProperty, column));

            // Стартовый поиск ячейки
            cell = AutomationElementHelper.FindFirst(Instanse, TreeScope.Subtree, cellCondition);

            // Основная горизонтальная прокрутка (при необходимости и возможности)
            if (cell == null && scrollPattern.Current.HorizontallyScrollable)
            {
                while (cell == null && scrollPattern.Current.HorizontalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.LargeIncrement);
                    cell = AutomationElementHelper.FindFirst(Instanse, TreeScope.Subtree, cellCondition);
                }
            }

            // Если прокрутив до конца ячейка не найдена, то номер колонки не действительный
            if (cell == null)
            {
                Logger.Error("В {0} нет колонки с номером {1}.", ToString(), column);
                throw new CruciatusException("NOT SCROLL");
            }

            // Если точка клика ячейки [row, column] справа от границы таблицы - докручиваем по горизонтали вправо
            while (cell.ClickablePointRight(Instanse, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallIncrement);
            }

            // Если точка клика ячейки [row, column] слева от границы таблицы - докручиваем по горизонтали влево
            while (cell.ClickablePointLeft(Instanse))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
            }
        }

        /// <summary>
        /// Выбирает ячейку с указанным номером строки и колонки.
        /// </summary>
        /// <param name="row">
        /// Номер строки.
        /// </param>
        /// <param name="column">
        /// Номер колонки.
        /// </param>
        /// <returns>
        /// Значение true если выбрать удалось; в противном случае значение - false.
        /// </returns>
        public virtual void SelectCell(int row, int column)
        {
            var cell = Item(row, column);
            if (cell == null)
            {
                Logger.Error("В {0} нет ячейки с номером {1}, {2}.", ToString(), row, column);
                throw new CruciatusException("NOT SELECT CELL");
            }

            cell.Click();
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
        public virtual CruciatusElement Item(int row, int column)
        {
            if (!Instanse.Current.IsEnabled)
            {
                Logger.Error("Element '{0}' not enabled. Scroll failed.", ToString());
                throw new ElementNotEnabledException("NOT GET ITEM");
            }

            // Проверка на дурака
            if (row < 0 || column < 0)
            {
                Logger.Error("В {0} ячейка [{1}, {2}] не существует, т.к. задан отрицательный номер.", ToString(), row, column);
                throw new CruciatusException("NOT GET ITEM");
            }

            // Условие для поиска ячейки [row, column]
            var cellCondition = new AndCondition(
                new PropertyCondition(AutomationElement.IsGridItemPatternAvailableProperty, true), 
                new PropertyCondition(GridItemPattern.RowProperty, row), 
                new PropertyCondition(GridItemPattern.ColumnProperty, column));
            var cell = AutomationElementHelper.FindFirst(Instanse, TreeScope.Subtree, cellCondition);

            // Проверка, что ячейку видно
            if (cell == null || !Instanse.ContainsClickablePoint(cell))
            {
                Logger.Error(
                    "В {0} ячейка [{1}, {2}] вне видимости или не существует.", 
                    ToString(), 
                    row, 
                    column);
                throw new CruciatusException("NOT GET ITEM");
            }

            // Поиск подходящего элемента в ячейке
            /*var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType);*/
            var elem = cell.FindFirst(TreeScope.Subtree, Condition.TrueCondition);
            if (elem == null)
            {
                Logger.Error(
                    "В {0}, ячейка [{1}, {2}], нет элемента желаемого типа.", 
                    ToString(), 
                    row,
                    column);
                throw new CruciatusException("NOT GET ITEM");
            }

            return new CruciatusElement(this, elem, null);
        }
    }
}
