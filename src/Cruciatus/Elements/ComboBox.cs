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

    public class ComboBox : BaseElement<ComboBox>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        private AutomationElement parent;

        public ComboBox()
        {
        }

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

            this.parent = parent;
            this.AutomationId = automationId;
        }

        /// <summary>
        /// Возвращает значение, указывающее, включен ли данный элемент управления.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<ComboBox, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<ComboBox, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        internal ExpandCollapseState ExpandCollapseState
        {
            get
            {
                return this.GetPropertyValue<ComboBox, ExpandCollapseState>(ExpandCollapsePattern.ExpandCollapseStateProperty);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "ComboBox";
            }
        }

        internal override sealed string AutomationId { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.ComboBox;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент.
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
            this.parent = parent;
            this.AutomationId = automationId;
        }

        public bool Expand()
        {
            if (this.ExpandCollapseState != ExpandCollapseState.Expanded)
            {
                return this.Click();
            }

            return true;
        }

        public bool Collapse()
        {
            if (this.ExpandCollapseState != ExpandCollapseState.Collapsed)
            {
                return this.Click();
            }

            return true;
        }

        public T Item<T>(uint number) where T : BaseElement<T>, new()
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

        public T Item<T>(string name) where T : BaseElement<T>, new()
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

        internal override ComboBox FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;

            return this;
        }

        private bool Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            if (!this.IsEnabled)
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

        /// <summary>
        /// Поиск текущего элемента в родительском
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.AutomationId));

            // Если не нашли, то загрузить выпадающий список не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}