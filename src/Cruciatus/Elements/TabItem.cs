// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TabItem.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент окно.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;

    /// <summary>
    /// Представляет элемент вкладка.
    /// </summary>
    public abstract class TabItem : BaseElement<TabItem>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        private AutomationElement parent;

        protected TabItem()
        {
        }

        protected TabItem(AutomationElement parent, string automationId)
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

        public bool IsEnabled
        {
            get
            {
                return this.GetPropertyValue<TabItem, bool>(AutomationElement.IsEnabledProperty);
            }
        }

        public bool IsSelection
        {
            get
            {
                return this.GetPropertyValue<TabItem, bool>(SelectionItemPattern.IsSelectedProperty);
            }
        }

        public System.Drawing.Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<TabItem, System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new System.Drawing.Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "TabItem";
            }
        }

        internal override sealed string AutomationId { get; set; }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.TabItem;
            }
        }

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

        internal override TabItem FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            return this;
        }

        protected T GetElement<T>(string automationId) where T : ILazyInitialize, new()
        {
            if (!this.objects.ContainsKey(automationId))
            {
                var item = new T();
                item.LazyInitialize(this.Element, automationId);
                this.objects.Add(automationId, item);
            }

            if (!this.IsSelection)
            {
                this.Click();
                if (!this.Element.WaitForElementReady())
                {
                    // TODO: Стопить ли все исключением, если время ожидания готовности истекло
                    throw new Exception("Время ожидания готовности вкладки истекло!");
                }
            }

            return (T)this.objects[automationId];
        }

        private void Click(MouseButtons mouseButton = MouseButtons.Left)
        {
            if (!this.IsEnabled)
            {
                throw new ElementNotEnabledException("Вкладка отключена, нельзя выполнить переход.");
            }

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(this.ClickablePoint);
            Mouse.Click(mouseButton);
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

            // Если не нашли, то загрузить вкладку не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("вкладка не найдена");
            }
        }
    }
}
