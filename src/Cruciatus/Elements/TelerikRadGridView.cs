// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelerikRadGridView.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления таблица (from Telerik).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Windows.Automation;

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет элемент управления таблица (from Telerik).
    /// </summary>
    public class TelerikRadGridView : DataGrid
    {
         /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TelerikRadGridView"/>.
        /// </summary>
        public TelerikRadGridView()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="TelerikRadGridView"/>.
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
        public TelerikRadGridView(AutomationElement parent, string automationId)
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
                return "TelerikRadGridView";
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
        public override bool ScrollTo(int row, int column)
        {
            // Проверка, что таблица включена
            var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);
            if (!isEnabled)
            {
                this.LastErrorMessage = string.Format("{0} отключена.", this.ToString());
                return false;
            }

            // Проверка на дурака
            if (row < 0 || column < 0)
            {
                this.LastErrorMessage = string.Format(
                    "В {0} ячейка [{1}, {2}] не существует, т.к. задан отрицательный номер.",
                    this.ToString(),
                    row,
                    column);
                return false;
            }

            // Получение шаблона прокрутки у таблицы
            var scrollPattern = this.Element.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
            if (scrollPattern == null)
            {
                this.LastErrorMessage = string.Format("{0} не поддерживает шаблон прокрутки.", this.ToString());
                return false;
            }

            // Условие для вертикального поиска ячейки [row, 0] (через строку)
            var cellCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, string.Format("Cell_{0}_0", row));

            // Стартовый поиск ячейки
            var cell = this.Element.FindFirst(TreeScope.Subtree, cellCondition);

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
                    cell = this.Element.FindFirst(TreeScope.Subtree, cellCondition);
                }
            }

            // Если прокрутив до конца ячейка не найдена, то номер строки не действительный
            if (cell == null)
            {
                this.LastErrorMessage = string.Format("В {0} нет строки с номером {1}.", this.ToString(), row);
                return false;
            }

            // Если точка клика ячейки [row, 0] под границей таблицы - докручиваем по вертикали вниз
            while (cell.ClickablePointUnder(this.Element, scrollPattern))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
            }

            // Если точка клика ячейки [row, 0] над границей таблицы - докручиваем по вертикали вверх
            while (cell.ClickablePointOver(this.Element))
            {
                scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
            }

            // Условие для горизонтального поиска ячейки [row, column]
            cellCondition = new PropertyCondition(AutomationElement.AutomationIdProperty, string.Format("Cell_{0}_{1}", row, column));

            // Стартовый поиск ячейки
            cell = this.Element.FindFirst(TreeScope.Subtree, cellCondition);

            // Основная горизонтальная прокрутка (при необходимости и возможности)
            if (cell == null && scrollPattern.Current.HorizontallyScrollable)
            {
                while (cell == null && scrollPattern.Current.HorizontalScrollPercent < 99.9)
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.LargeIncrement);
                    cell = this.Element.FindFirst(TreeScope.Subtree, cellCondition);
                }
            }

            // Если прокрутив до конца ячейка не найдена, то номер колонки не действительный
            if (cell == null)
            {
                this.LastErrorMessage = string.Format("В {0} нет колонки с номером {1}.", this.ToString(), column);
                return false;
            }

            // Если точка клика ячейки [row, column] справа от границы таблицы - докручиваем по горизонтали вправо
            while (cell.ClickablePointRight(this.Element, scrollPattern))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallIncrement);
            }

            // Если точка клика ячейки [row, column] слева от границы таблицы - докручиваем по горизонтали влево
            while (cell.ClickablePointLeft(this.Element))
            {
                scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
            }

            return true;
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
        public override bool SelectCell(int row, int column)
        {
            // Проверка, что таблица включена
            var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);
            if (!isEnabled)
            {
                this.LastErrorMessage = string.Format("{0} отключена.", this.ToString());
                return false;
            }

            // Проверка на дурака
            if (row < 0 || column < 0)
            {
                this.LastErrorMessage = string.Format(
                    "В {0} ячейка [{1}, {2}] не существует, т.к. задан отрицательный номер.",
                    this.ToString(),
                    row,
                    column);
                return false;
            }

            // Поиск подходящей ячейки [row, column]
            var cell = new ClickableElement();
            var cellCondition = new AndCondition(
                new PropertyCondition(AutomationElement.AutomationIdProperty, string.Format("Cell_{0}_{1}", row, column)),
                new PropertyCondition(AutomationElement.ControlTypeProperty, cell.GetType));
            var elem = this.Element.FindFirst(TreeScope.Subtree, cellCondition);

            // Проверка, что ячейку видно
            if (elem == null || !this.Element.ContainsClickablePoint(elem))
            {
                this.LastErrorMessage = string.Format(
                    "В {0} елемент ячейки [{1}, {2}] вне видимости или не существует.",
                    this.ToString(),
                    row,
                    column);
                return false;
            }

            ((IListElement)cell).Initialize(elem);
            var result = cell.Click();
            if (result)
            {
                return true;
            }

            this.LastErrorMessage = cell.LastErrorMessage;
            return false;
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
        public override T Item<T>(int row, int column)
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

            // Проверка на дурака
            if (row < 0 || column < 0)
            {
                this.LastErrorMessage = string.Format(
                    "В {0} ячейка [{1}, {2}] не существует, т.к. задан отрицательный номер.",
                    this.ToString(),
                    row,
                    column);
                return null;
            }

            // Поиск подходящего элемента [row, column]
            var item = new T();
            var cellCondition = new AndCondition(
                new PropertyCondition(AutomationElement.AutomationIdProperty, string.Format("CellElement_{0}_{1}", row, column)),
                new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType));
            var elem = this.Element.FindFirst(TreeScope.Subtree, cellCondition);

            // Проверка, что элемент видно
            if (elem == null || !this.Element.ContainsClickablePoint(elem))
            {
                this.LastErrorMessage = string.Format(
                    "В {0} елемент ячейки [{1}, {2}] вне видимости или не существует.",
                    this.ToString(),
                    row,
                    column);
                return null;
            }

            item.Initialize(elem);
            return item;
        }
    }
}
