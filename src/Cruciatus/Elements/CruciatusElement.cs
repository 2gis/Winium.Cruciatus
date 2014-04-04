// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CruciatusElement.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет базу для элементов управления.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System;
    using System.Linq;
    using System.Windows.Automation;

    using Cruciatus.Exceptions;

    #endregion

    public abstract class CruciatusElement
    {
        public string LastErrorMessage { get; internal set; }

        internal AutomationElement ElementInstance { get; set; }

        internal AutomationElement Parent { get; set; }

        internal string AutomationId { get; set; }

        internal abstract string ClassName { get; }

        internal new abstract ControlType GetType { get; }

        internal virtual AutomationElement Element
        {
            get
            {
                if (ElementInstance == null)
                {
                    Find();
                }

                return ElementInstance;
            }
        }

        public new string ToString()
        {
            return string.Format("{0} (uid: {1})", ClassName, AutomationId ?? "nonUid");
        }

        // TODO: Разобраться с ошибкой "Access to modified closure" у переменной condition
        internal virtual void Find()
        {
            var list = AutomationId.Split('/');

            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, list[0]);
            ElementInstance = CruciatusFactory.WaitingValues(
                () => Parent.FindFirst(TreeScope.Subtree, condition), 
                value => value == null, 
                CruciatusFactory.Settings.SearchTimeout);

            if (list.Count() > 1)
            {
                for (var i = 1; i < list.Count(); ++i)
                {
                    condition = new PropertyCondition(AutomationElement.AutomationIdProperty, list[i]);
                    ElementInstance = CruciatusFactory.WaitingValues(
                        () => ElementInstance.FindFirst(TreeScope.Subtree, condition), 
                        value => value == null, 
                        CruciatusFactory.Settings.SearchTimeout);
                }
            }

            // Если не нашли, то загрузить элемент не удалось
            if (ElementInstance == null)
            {
                throw new ElementNotFoundException(ToString());
            }
        }

        protected void Initialize(AutomationElement parent, string automationId)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }

            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            Parent = parent;
            AutomationId = automationId;
        }

        protected void Initialize(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            ElementInstance = element;
        }
    }
}
