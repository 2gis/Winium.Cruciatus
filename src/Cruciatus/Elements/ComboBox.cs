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
    using System;
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент управления выпадающий список.
    /// </summary>
    public class ComboBox : CruciatusElement, IContainerElement
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
                return this.ExpandCollapseState == ExpandCollapseState.Expanded;
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
        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
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

        public string Text
        {
            get
            {
                object obj;
                if (this.Element.TryGetCurrentPattern(ValuePattern.Pattern, out obj))
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
                this.LastErrorMessage = string.Format("Не удалось получить текст из {0}", this.ToString());
                return null;
            }
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
                if (this.ExpandCollapseState != ExpandCollapseState.Expanded)
                {
                    return this.Click();
                }

                return true;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
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
                if (this.ExpandCollapseState != ExpandCollapseState.Collapsed)
                {
                    return this.Click();
                }

                return true;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }
        }

        /// <summary>
        /// Возвращает элемент заданного типа с указанным номером.
        /// </summary>
        /// <param name="number">
        /// Номер элемента.
        /// </param>
        /// <typeparam name="T">
        /// Тип элемента.
        /// </typeparam>
        /// <returns>
        /// Искомый элемент, либо null, если найти не удалось.
        /// </returns>
        public T Item<T>(uint number) where T : CruciatusElement, IListElement, new()
        {
            try
            {
                if (this.ExpandCollapseState != ExpandCollapseState.Expanded)
                {
                    this.LastErrorMessage = string.Format("Элемент {0} не развернут.", this.ToString());
                    return null;
                }

                var item = new T();
                var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType);

                var items = this.Element.FindAll(TreeScope.Subtree, condition);

                if (items.Count <= number)
                {
                    condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem);
                    items = this.Element.FindAll(TreeScope.Subtree, condition);
                }

                if (items.Count <= number)
                {
                    this.LastErrorMessage =
                        string.Format(
                            "Номер запрошенного элемента ({0}) превышает количество элементов ({1}) в {2}.",
                            number,
                            items.Count,
                            this.ToString());
                    return null;
                }

                item.Initialize(items[(int)number]);
                return item;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
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
                if (this.ExpandCollapseState != ExpandCollapseState.Expanded)
                {
                    this.LastErrorMessage = string.Format("Элемент {0} не развернут.", this.ToString());
                    return null;
                }

                var item = new T();
                var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                    new PropertyCondition(AutomationElement.NameProperty, name));
            
                var elem = this.Element.FindFirst(TreeScope.Subtree, condition);

                if (elem == null)
                {
                    condition = new AndCondition(
                        new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
                        new PropertyCondition(AutomationElement.NameProperty, name));
                    elem = this.Element.FindFirst(TreeScope.Subtree, condition);
                }

                if (elem == null)
                {
                    this.LastErrorMessage = string.Format("В {0} нет элемента с полем name = {1}.", this.ToString(), name);
                    return null;
                }

                item.Initialize(elem);
                return item;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return null;
            }
        }

        /// <summary>
        /// Прокручивает список до элемента заданного типа с указанным именем.
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
        public bool ScrollTo<T>(string name) where T : CruciatusElement, IListElement, new()
        {
            var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);

            if (!isEnabled)
            {
                this.LastErrorMessage = string.Format("{0} отключен, нельзя выполнить прокрутку.", this.ToString());
                return false;
            }

            if (this.ExpandCollapseState != ExpandCollapseState.Expanded)
            {
                this.LastErrorMessage = string.Format("Элемент {0} не развернут.", this.ToString());
                return false;
            }

            var searchElement = this.SearchElement(name, new T().GetType);
            if (searchElement == null)
            {
                this.LastErrorMessage = string.Format("В {0} нет элемента с полем name = {1}.", this.ToString(), name);
                return false;
            }

            return this.Scrolling(searchElement);
        }

        /// <summary>
        /// Выполняет нажатие по выпадающему списку.
        /// </summary>
        /// <param name="mouseButton">
        /// Задает кнопку мыши, которой будет произведено нажатие; либо кнопка по умолчанию.
        /// </param>
        /// <returns>
        /// Значение true если нажать на выпадающий список удалось; в противном случае значение - false.
        /// </returns>
        protected virtual bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            try
            {
                var isEnabled = CruciatusFactory.WaitingValues(
                    () => this.IsEnabled,
                    value => value != true);

                if (!isEnabled)
                {
                    this.LastErrorMessage = string.Format("{0} отключен, нельзя выполнить нажатие.", this.ToString());
                    return false;
                }

                if (!CruciatusCommand.Click(this.ClickablePoint, mouseButton, out this.LastErrorMessageInstance))
                {
                    return false;
                }

                if (!this.Element.WaitForElementReady())
                {
                    this.LastErrorMessage = string.Format("Время ожидания готовности для {0} истекло.", this.ToString());
                    return false;
                }

                return true;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return false;
            }
        }

        private AutomationElement SearchElement(string name, ControlType type)
        {
            var condition = new AndCondition(
                    new PropertyCondition(AutomationElement.ControlTypeProperty, type),
                    new PropertyCondition(AutomationElement.NameProperty, name));

            var searchElement = this.Element.SearchSpecificElementConsideringScroll(
                elem => elem.FindFirst(TreeScope.Subtree, condition),
                elem => elem == null,
                elem => elem);

            if (searchElement == null && !type.Equals(ControlType.ListItem))
            {
                searchElement = this.SearchElement(name, ControlType.ListItem);
            }

            return searchElement;
        }

        private bool Scrolling(AutomationElement innerElement)
        {
            var condition = new AndCondition(
                new PropertyCondition(AutomationElement.ClassNameProperty, "Popup"),
                new PropertyCondition(AutomationElement.ProcessIdProperty, this.Element.Current.ProcessId));
            var popupWindow = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, condition);

            if (popupWindow == null)
            {
                this.LastErrorMessage = string.Format("Попытка прокрутки у элемента {0}, но нет popup окна.", this.ToString());
                return false;
            }

            var scrollingResult = this.Element.ScrollingForComboBox(innerElement, popupWindow);
            if (!scrollingResult)
            {
                this.LastErrorMessage = string.Format("Элемент {0} не поддерживает прокрутку содержимого.", this.ToString());
                return false;
            }

            return true;
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }
    }
}