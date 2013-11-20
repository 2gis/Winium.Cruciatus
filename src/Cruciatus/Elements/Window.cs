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
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;
    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    /// <summary>
    /// Представляет элемент окно.
    /// </summary>
    public abstract class Window : ILazyInitialize
    {
        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        private AutomationElement element;

        /// <summary>
        /// Возвращает инициализированный элемент окна.
        /// </summary>
        protected AutomationElement Element
        {
            get
            {
                if (this.element == null)
                {
                    this.element = WindowFactory.GetChildWindowElement(this.Parent, this.HeaderName);

                    // TODO: Нужны приложения с окнами для улучшения этих костыльных строчек
                    object objectPattern;
                    if (this.element.TryGetCurrentPattern(WindowPattern.Pattern, out objectPattern))
                    {
                        ((WindowPattern)objectPattern).WaitForInputIdle(1500);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }

                return this.element;
            }
        }

        /// <summary>
        /// Возвращает или задает имя заголовка окна.
        /// </summary>
        private string HeaderName { get; set; }

        /// <summary>
        /// Возвращает или задает элемент, который является родителем окна.
        /// </summary>
        private AutomationElement Parent { get; set; }

        public bool WaitForReady()
        {
            return this.Element.WaitForElementReady();
        }

        public void LazyInitialize(AutomationElement parent, string headerName)
        {
            if (this.element != null || this.Parent != null)
            {
                throw new LazyInitializeException("Попытка повторной инициализации дочернего окна " + headerName + ".\n");
            }

            this.Parent = parent;
            this.HeaderName = headerName;
        }

        public void LazyInitialize(AutomationElement element)
        {
            if (this.element != null || this.Parent != null)
            {
                throw new LazyInitializeException("Попытка повторной инициализации главного окна.\n");    
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
