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

        public ExpandCollapseState State
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

        public void Expand()
        {
            if (this.State != ExpandCollapseState.Expanded)
            {
                this.Click();
            }
        }

        public void Collapse()
        {
            if (this.State != ExpandCollapseState.Collapsed)
            {
                this.Click();
            }
        }

        public T Item<T>(uint number) where T : BaseElement<T>, new()
        {
            this.Expand();

            var item = new T();
            var condition = new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType);

            var items = this.Element.FindAll(TreeScope.Subtree, condition);
            if (items.Count <= number)
            {
                throw new ArgumentOutOfRangeException("number");
            }

            item.FromAutomationElement(items[(int)number]);
            return item;
        }

        public T Item<T>(string name) where T : BaseElement<T>, new()
        {
            this.Expand();

            var item = new T();
            var condition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, item.GetType),
                new PropertyCondition(AutomationElement.NameProperty, name));
            
            var elem = this.Element.FindFirst(TreeScope.Subtree, condition);
            if (elem == null)
            {
                // TODO: Исключение вида - не найдено элемента в списке, удовлетворяющему заданным условиям
                throw new Exception("не нашлось элемента в списке комбобокса");
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

        private void Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            if (!this.IsEnabled)
            {
                throw new ElementNotEnabledException("Выпадающий список отключен, нельзя выполнить нажатие.");
            }

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(this.ClickablePoint);
            Mouse.Click(mouseButton);

            if (!this.Element.WaitForElementReady())
            {
                // TODO: Стопить ли все исключением, если время ожидания готовности истекло
                throw new Exception("Время ожидания готовности выпадающего списка истекло!");
            }
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
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("выпадающий список не найден");
            }
        }
    }
}