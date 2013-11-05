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
    using System.Drawing;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    using ControlType = System.Windows.Automation.ControlType;
    using Point = System.Drawing.Point;
    using Size = System.Drawing.Size;

    /// <summary>
    /// Представляет элемент вкладка.
    /// </summary>
    public abstract class TabItem : BaseElement<TabItem>, ILazyInitialize
    {
        private const int MouseMoveSpeed = 2500;

        private readonly Dictionary<string, object> objects = new Dictionary<string, object>(); 

        private string automationId;

        private AutomationElement parent;

        public TabItem()
        {
        }

        public TabItem(AutomationElement parent, string automationId)
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
            this.automationId = automationId;
        }

        public bool IsEnabled
        {
            get
            {
                return (bool)this.Element.GetCurrentPropertyValue(AutomationElement.IsEnabledProperty);
            }
        }

        public bool IsSelection
        {
            get
            {
                return (bool)this.Element.GetCurrentPropertyValue(SelectionItemPattern.IsSelectedProperty);
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                var rect = (Rect)this.Element.GetCurrentPropertyValue(AutomationElement.BoundingRectangleProperty);

                // Усечение дабла дает немного меньший прямоугольник, но он внутри изначального
                return new Rectangle(new Point((int)rect.X, (int)rect.Y), new Size((int)rect.Width, (int)rect.Height));
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.TabItem;
            }
        }

        protected override AutomationElement Element
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
            this.automationId = automationId;
        }

        internal override TabItem FromAutomationElement(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            this.CheckingOfProperties();

            return this;
        }

        protected override void CheckingOfProperties()
        {
            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.IsEnabledProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство Enabled
                throw new Exception("вкладка не поддерживает свойство Enabled");
            }

            if (!this.Element.GetSupportedProperties().Contains(SelectionItemPattern.IsSelectedProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство Selection
                throw new Exception("вкладка не поддерживает свойство Selection");
            }

            if (!this.Element.GetSupportedProperties().Contains(AutomationElement.BoundingRectangleProperty))
            {
                // TODO: Исключение вида - контрол не поддерживает свойство BoundingRectangle
                throw new Exception("вкладка не поддерживает свойство BoundingRectangle");
            }
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

            var controlBoundingRect = this.BoundingRectangle;

            // TODO Вынести это действие как расширения для типа Rectangle
            var clickablePoint = Point.Add(controlBoundingRect.Location, new Size(controlBoundingRect.Width / 2, controlBoundingRect.Height / 2));

            Mouse.MouseMoveSpeed = MouseMoveSpeed;
            Mouse.Move(clickablePoint);
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
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.automationId));

            // Если не нашли, то загрузить вкладку не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("вкладка не найдена");
            }

            this.CheckingOfProperties();
        }
    }
}
