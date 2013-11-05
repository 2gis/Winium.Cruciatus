// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window.cs" company="2GIS">
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

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет элемент окно.
    /// </summary>
    public abstract class Window : ILazyInitialize
    {
        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        private string headerName;

        private AutomationElement parent;

        private AutomationElement element;

        protected AutomationElement Element
        {
            get
            {
                if (this.element == null)
                {
                    this.element = WindowFactory.GetChildWindowElement(this.parent, this.headerName);
                }

                return this.element;
            }
        }

        public bool WaitForReady()
        {
            return this.Element.WaitForElementReady();
        }

        public void LazyInitialize(AutomationElement parent, string headerName)
        {
            if (this.element != null || this.parent != null)
            {
                // TODO: Надо адекватное, понятное исключение
                throw new Exception("Отложенная инициализация доступна только для пустого элемента");
            }

            this.parent = parent;
            this.headerName = headerName;
        }

        public void LazyInitialize(AutomationElement element)
        {
            if (this.element != null || this.parent != null)
            {
                // TODO: Надо адекватное, понятное исключение
                throw new Exception("Отложенная инициализация доступна только для пустого элемента");    
            }

            this.element = element;
        }

        protected T GetElement<T>(string automationId) where T : ILazyInitialize, new()
        {
            if (!this.objects.ContainsKey(automationId))
            {
                var item = new T();
                item.LazyInitialize(this.Element, automationId);
                this.objects.Add(automationId, item);
            }

            return (T)this.objects[automationId];
        }
    }
}
