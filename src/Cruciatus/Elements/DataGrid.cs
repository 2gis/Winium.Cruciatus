// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGrid.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления таблица.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Elements
{
    using System;
    using System.Linq;
    using System.Windows.Automation;

    using Cruciatus.Interfaces;

    public class DataGrid : BaseElement<DataGrid>, ILazyInitialize
    {
        private string automationId;

        private AutomationElement parent;

        public DataGrid()
        {
        }

        public DataGrid(AutomationElement parent, string automationId)
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

        public int RowCount
        {
            get
            {
                return (int)this.Element.GetCurrentPropertyValue(GridPattern.RowCountProperty);
            }
        }

        internal override ControlType GetType
        {
            get
            {
                return ControlType.DataGrid;
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

        internal override DataGrid FromAutomationElement(AutomationElement element)
        {
            this.element = element;
            this.CheckingOfProperties();

            return this;
        }

        protected override void CheckingOfProperties()
        {
            if (!this.Element.GetSupportedProperties().Contains(GridPattern.RowCountProperty))
            {
                // TODO: Исключение вида - элемент не поддерживает свойство RowCount
                throw new Exception("таблица не поддерживает свойство RowCount");
            }
        }

        private void Find()
        {
            // Ищем в нем первый встретившийся контрол с заданным automationId
            this.element = this.parent.FindFirst(
                TreeScope.Subtree,
                new PropertyCondition(AutomationElement.AutomationIdProperty, this.automationId));

            // Если не нашли, то загрузить кнопку не удалось
            if (this.element == null)
            {
                // TODO: Исключение вида - не найдено контрола с заданным AutomationId
                throw new Exception("таблица не найдена");
            }

            this.CheckingOfProperties();
        }
    }
}