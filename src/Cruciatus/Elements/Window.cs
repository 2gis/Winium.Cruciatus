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

        public string LastErrorMessage { get; internal set; }

        /// <summary>
        /// Возвращает текстовое представление имени класса.
        /// </summary>
        internal string ClassName
        {
            get
            {
                return "Window";
            }
        }

        /// <summary>
        /// Возвращает инициализированный элемент окна.
        /// </summary>
        protected AutomationElement Element
        {
            get
            {
                if (this.element == null)
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

                return this.element;
            }
        }

        /// <summary>
        /// Возвращает или задает уникальный идентификатор окна.
        /// </summary>
        private string AutomationId { get; set; }

        /// <summary>
        /// Возвращает или задает элемент, который является родителем окна.
        /// </summary>
        private AutomationElement Parent { get; set; }

        /// <summary>
        /// Делает пометку, что окно закрыто (удаляет ссылки на дочерние элементы окна).
        /// </summary>
        public void Closed()
        {
            this.element = null;
            this.objects.Clear();
        }

        public bool WaitForReady()
        {
            return this.Element.WaitForElementReady();
        }

        public void LazyInitialize(AutomationElement parent, string automationId)
        {
            if (this.element != null || this.Parent != null)
            {
                throw new LazyInitializeException("Попытка повторной инициализации дочернего окна " + automationId + ".\n");
            }

            this.Parent = parent;
            this.AutomationId = automationId;
        }

        public void LazyInitialize(AutomationElement element)
        {
            if (this.element != null || this.Parent != null)
            {
                throw new LazyInitializeException("Попытка повторной инициализации главного окна.\n");    
            }

            this.element = element;
        }

        public new string ToString()
        {
            return string.Format("{0} (uid: {1})", this.ClassName, this.AutomationId ?? "nonUid");
        }

        protected virtual T GetElement<T>(string automationId) where T : class, ILazyInitialize, new()
        {
            try
            {
                if (!this.objects.ContainsKey(automationId))
                {
                    var item = new T();
                    item.LazyInitialize(this.Element, automationId);
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

        /// <summary>
        /// Поиск окна внутри родительского элемента.
        /// </summary>
        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = CruciatusFactory.WaitingValues(
                () => WindowFactory.GetChildWindowElement(this.Parent, this.AutomationId),
                value => value == null,
                CruciatusFactory.Settings.SearchTimeout);

            // Если не нашли, то загрузить окно не удалось
            if (this.element == null)
            {
                throw new ElementNotFoundException(this.ToString());
            }
        }
    }
}
