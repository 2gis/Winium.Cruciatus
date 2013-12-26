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
    public abstract class Window : CruciatusElement, IContainerElement
    {
        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal override string ClassName
        {
            get
            {
                return "Window";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.Window;
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент окна.
        /// </summary>
        internal override AutomationElement Element
        {
            get
            {
                if (this.ElementInstance == null)
                {
                    this.Find();

                    //// TODO: Нужны приложения с окнами для улучшения этих костыльных строчек
                    //object objectPattern;
                    //if (this.element.TryGetCurrentPattern(WindowPattern.Pattern, out objectPattern))
                    //{
                    //    ((WindowPattern)objectPattern).WaitForInputIdle(1500);
                    //}
                    //else
                    //{
                    //    Thread.Sleep(500);
                    //}
                }

                return this.ElementInstance;
            }
        }

        /// <summary>
        /// Делает пометку, что окно закрыто (удаляет ссылки на дочерние элементы окна).
        /// </summary>
        public void Closed()
        {
            this.ElementInstance = null;
            this.objects.Clear();
        }

        public bool WaitForReady()
        {
            return this.Element.WaitForElementReady();
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        protected virtual T GetElement<T>(string automationId) where T : CruciatusElement, IContainerElement, new()
        {
            try
            {
                if (!this.objects.ContainsKey(automationId))
                {
                    var item = new T();
                    item.Initialize(this.Element, automationId);
                    this.objects.Add(automationId, item);
                }

                return (T)this.objects[automationId];
            }
            catch (Exception exc)
            {
                this.LastErrorMessage = exc.Message;
                return null;
            }
        }
    }
}
