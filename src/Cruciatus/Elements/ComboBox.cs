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
    public class ComboBox : BaseElement<ComboBox>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

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
                return this.GetPropertyValue<ComboBox, bool>(AutomationElement.IsEnabledProperty);
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
                var windowsPoint = this.GetPropertyValue<ComboBox, System.Windows.Point>(AutomationElement.ClickablePointProperty);

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
                return this.GetPropertyValue<ComboBox, ExpandCollapseState>(ExpandCollapsePattern.ExpandCollapseStateProperty);
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
                return ControlType.ComboBox;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент выпадающего списка.
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
        public T Item<T>(uint number) where T : BaseElement<T>, new()
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
                    this.LastErrorMessage =
                        string.Format(
                            "Номер запрошенного элемента ({0}) превышает количество элементов ({1}) в {2}.",
                            number,
                            items.Count,
                            this.ToString());
                    return null;
                }

                item.FromAutomationElement(items[(int)number]);
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
        public T Item<T>(string name) where T : BaseElement<T>, new()
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
                    this.LastErrorMessage = string.Format("В {0} нет элемента с полем name = {1}.", this.ToString(), name);
                    return null;
                }

                item.FromAutomationElement(elem);
                return item;
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return null;
            }
        }

        internal override ComboBox FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;

            return this;
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
        private bool Click(MouseButtons mouseButton = MouseButtons.Left)
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

                Mouse.MouseMoveSpeed = MouseMoveSpeed;
                Mouse.Move(this.ClickablePoint);
                Mouse.Click(mouseButton);

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

        /// <summary>
        /// Поиск выпадающего списка в родительском элементе.
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId);
            this.element = CruciatusFactory.WaitingValues(
                () => this.Parent.FindFirst(TreeScope.Subtree, condition),
                value => value == null);

            // Если не нашли, то загрузить выпадающий список не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}