// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComboBox.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления выпадающий список.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    #endregion

    /// <summary>
    /// Представляет элемент управления выпадающий список.
    /// </summary>
    public class ComboBox : CruciatusElement, IContainerElement, IClickable
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ComboBox"/>.
        /// </summary>
        public ComboBox()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ComboBox"/>.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родителем для выпадающего списка.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор выпадающего списка.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        public ComboBox(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        /// <summary>
        /// Возвращает значение, указывающее, включен ли выпадающий список.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Выпадающий список не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<bool>(AutomationElement.IsEnabledProperty);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее, раскрыт ли выпадающий список.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Выпадающий список не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public bool IsExpanded
        {
            get
            {
                return ExpandCollapseState == ExpandCollapseState.Expanded;
            }
        }

        public string Text
        {
            get
            {
                object obj;
                if (Element.TryGetCurrentPattern(ValuePattern.Pattern, out obj))
                {
                    // Если шаблон значения поддерживается, то текст вернее получить так
                    return this.GetPropertyValue<string>(ValuePattern.ValueProperty);
                }

                // В противном случае работаем с шаблоном выбора
                obj = this.GetPropertyValue<object>(SelectionPattern.SelectionProperty);
                var tempList = (AutomationElement[])obj;
                if (!tempList.Any())
                {
                    // Если получили 0 элементов, то возвращаем пустую строку,
                    // так как возврат null только при ошибке
                    return string.Empty;
                }

                if (tempList.Count() == 1)
                {
                    // При одном элементе все здорово, возвращаем его свойство Name
                    return tempList[0].GetPropertyValue<string>(AutomationElement.NameProperty);
                }

                // В противном случае кол-во полученных элементов странное - ошибка
                LastErrorMessage = string.Format("Не удалось получить текст из {0}", ToString());
                return null;
            }
        }

        /// <summary>
        /// Возвращает состояние раскрытости выпадающего списка.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Выпадающий список не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        internal ExpandCollapseState ExpandCollapseState
        {
            get
            {
                return this.GetPropertyValue<ExpandCollapseState>(ExpandCollapsePattern.ExpandCollapseStateProperty);
            }
        }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "ComboBox";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.ComboBox;
            }
        }

        /// <summary>
        /// Возвращает координаты точки, внутри выпадающего списка, которые можно использовать для нажатия.
        /// </summary>
        /// <exception cref="PropertyNotSupportedException">
        /// Выпадающий список не поддерживает данное свойство.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// При получении значения свойства не удалось привести его к ожидаемому типу.
        /// </exception>
        public Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        /// <summary>
        /// Выполняет нажатие по выпадающему списку.
        /// </summary>
        /// <returns>
        /// Значение true если нажать на элемент удалось; в противном случае значение - false.
        /// </returns>
        public bool Click()
        {
            return Click(CruciatusFactory.Settings.ClickButton);
        }

        /// <summary>
        /// Выполняет нажатие по выпадающему списку.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие.
        /// </param>
        /// <returns>
        /// Значение true если нажать на выпадающий список удалось; в противном случае значение - false.
        /// </returns>
        public virtual bool Click(MouseButtons mouseButton)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => IsEnabled, 
                    value => value != true);

                if (isEnabled)
                {
                    CruciatusCommand.Click(ClickablePoint, mouseButton);
                    return true;
                }

                LastErrorMessage = string.Format("{0} отключен, нельзя выполнить нажатие.", ToString());
                return false;
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        /// <summary>
        /// Раскрывает выпадающий список.
        /// </summary>
        /// <returns>
        /// Значение true если удалось раскрыть либо если уже раскрыт; в противном случае значение - false.
        /// </returns>
        public bool Expand()
        {
            try
            {
                if (ExpandCollapseState == ExpandCollapseState.Expanded)
                {
                    return true;
                }

                var res = Click();
                if (!res)
                {
                    return false;
                }

                Thread.Sleep(250);
                return true;
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Сворачивает выпадающий список.
        /// </summary>
        /// <returns>
        /// Значение true если удалось свернуть либо если уже свернут; в противном случае значение - false.
        /// </returns>
        public bool Collapse()
        {
            try
            {
                if (ExpandCollapseState != ExpandCollapseState.Collapsed)
                {
                    return Click();
                }

                return true;
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Прокручивает выпадающий список до элемента с указанным именем.
        /// </summary>
        /// <param name="name">
        /// Имя элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Значение true если прокрутить удалось либо в этом нет необходимости; в противном случае значение - false.
        /// </returns>
        public T ScrollTo<T>(string name) where T : CruciatusElement, IListElement, new()
        {
            try
            {
                // Проверка на дурака
                if (string.IsNullOrEmpty(name))
                {
                    LastErrorMessage = "Параметр name не должен быть пустым.";
                    return null;
                }

                // Проверка, что выпадающий список включен
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => IsEnabled, 
                    value => value != true);
                if (!isEnabled)
                {
                    LastErrorMessage = string.Format("{0} отключен.", ToString());
                    return null;
                }

                // Проверка, что выпадающий список раскрыт
                if (ExpandCollapseState != ExpandCollapseState.Expanded)
                {
                    LastErrorMessage = string.Format("Элемент {0} не развернут.", ToString());
                    return null;
                }

                // Получение шаблона прокрутки у выпадающего списка
                var scrollPattern = Element.GetCurrentPattern(ScrollPattern.Pattern) as ScrollPattern;
                if (scrollPattern == null)
                {
                    LastErrorMessage = string.Format("{0} не поддерживает шаблон прокрутки.", ToString());
                    return null;
                }

                var item = new T();
                var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType), 
                    new PropertyCondition(AutomationElement.NameProperty, name));

                // Стартовый поиск элемента
                var element = Element.FindFirst(TreeScope.Subtree, condition);

                // Вертикальная прокрутка (при необходимости и возможности)
                if (element == null && scrollPattern.Current.VerticallyScrollable)
                {
                    // Установка самого верхнего положения прокрутки
                    while (scrollPattern.Current.VerticalScrollPercent > 0.1)
                    {
                        scrollPattern.ScrollVertical(ScrollAmount.LargeDecrement);
                    }

                    // Установка самого левого положения прокрутки (при возможности)
                    if (scrollPattern.Current.HorizontallyScrollable)
                    {
                        while (scrollPattern.Current.HorizontalScrollPercent > 0.1)
                        {
                            scrollPattern.ScrollHorizontal(ScrollAmount.LargeDecrement);
                        }
                    }

                    // Основная вертикальная прокрутка
                    while (element == null && scrollPattern.Current.VerticalScrollPercent < 99.9)
                    {
                        scrollPattern.ScrollVertical(ScrollAmount.LargeIncrement);
                        element = Element.FindFirst(TreeScope.Subtree, condition);
                    }
                }

                // Если прокрутив до конца элемент не найден, то его нет (кэп)
                if (element == null)
                {
                    LastErrorMessage = string.Format("В {0} нет элемента с name = {1}.", ToString(), name);
                    return null;
                }

                // Поиск окна, которое является выпавшим списком
                condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Popup"), 
                    new PropertyCondition(AutomationElement.ProcessIdProperty, Element.Current.ProcessId));
                var popupWindow = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, condition);
                if (popupWindow == null)
                {
                    LastErrorMessage = string.Format("Попытка прокрутки у элемента {0}, но нет popup окна.", ToString());
                    return null;
                }

                // Если точка клика элемента под границей списка - докручиваем по вертикали вниз
                while (element.ClickablePointUnder(popupWindow, scrollPattern))
                {
                    scrollPattern.ScrollVertical(ScrollAmount.SmallIncrement);
                }

                // Если точка клика элемента над границей списка - докручиваем по вертикали вверх
                while (element.ClickablePointOver(popupWindow))
                {
                    scrollPattern.ScrollVertical(ScrollAmount.SmallDecrement);
                }

                // Если точка клика элемента справа от границы списка - докручиваем по горизонтали вправо
                while (element.ClickablePointRight(popupWindow, scrollPattern))
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.SmallIncrement);
                }

                // Если точка клика элемента слева от границы списка - докручиваем по горизонтали влево
                while (element.ClickablePointLeft(popupWindow))
                {
                    scrollPattern.ScrollHorizontal(ScrollAmount.SmallDecrement);
                }

                item.Initialize(element);
                return item;
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return null;
            }
        }

        /// <summary>
        /// Возвращает элемент заданного типа с указанным именем.
        /// </summary>
        /// <param name="name">
        /// Имя элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Искомый элемент, либо null, если найти не удалось.
        /// </returns>
        public T Item<T>(string name) where T : CruciatusElement, IListElement, new()
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => IsEnabled, 
                    value => value != true);
                if (!isEnabled)
                {
                    LastErrorMessage = string.Format("{0} отключен.", ToString());
                    return null;
                }

                if (ExpandCollapseState != ExpandCollapseState.Expanded)
                {
                    LastErrorMessage = string.Format("Элемент {0} не развернут.", ToString());
                    return null;
                }

                var item = new T();
                var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType), 
                    new PropertyCondition(AutomationElement.NameProperty, name));

                var searchElement = Element.FindFirst(TreeScope.Subtree, condition);

                if (searchElement == null)
                {
                    LastErrorMessage =
                        string.Format(
                            "В {0} элемент с полем name = {1} не существует или вне видимости.", 
                            ToString(), 
                            name);
                    return null;
                }

                // Поиск окна, которое является выпавшим списком
                condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ClassNameProperty, "Popup"), 
                    new PropertyCondition(AutomationElement.ProcessIdProperty, Element.Current.ProcessId));
                var popupWindow = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, condition);
                if (popupWindow == null)
                {
                    LastErrorMessage = string.Format("У элемента {0} не обнаружено popup окно.", ToString());
                    return null;
                }

                if (!popupWindow.ContainsClickablePoint(searchElement))
                {
                    LastErrorMessage = string.Format("В {0} элемент с полем name = {1} вне видимости.", ToString(), name);
                    return null;
                }

                item.Initialize(searchElement);
                return item;
            }
            catch (CruciatusException exc)
            {
                LastErrorMessage = exc.Message;
                return null;
            }
        }

        // private AutomationElement SearchElement(string name, ControlType type)
        // {
        // // TODO: Это для WinForms надо, но стоит действовать иначе глобально (определяя что это WinForms)
        // var condition = new AndCondition(
        // new PropertyCondition(AutomationElement.ControlTypeProperty, type),
        // new PropertyCondition(AutomationElement.NameProperty, name));

        // var searchElement = this.Element.FindFirst(TreeScope.Subtree, condition);

        // if (searchElement == null && !type.Equals(ControlType.ListItem))
        // {
        // searchElement = this.SearchElement(name, ControlType.ListItem);
        // }

        // return searchElement;
        // }
    }
}
