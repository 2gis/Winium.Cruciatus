// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowFactory.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет WindowFactory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus
{
    using System;
    using System.Windows.Automation;

    internal static class WindowFactory
    {
        /// <summary>
        /// Возвращает главное окно по соответствию processId и automationId.
        /// </summary>
        /// <param name="processId">
        /// Уникальный идентификатор процесса.
        /// </param>
        /// <param name="automationId">
        /// Уникальный идентификатор окна.
        /// </param>
        /// <returns>
        /// Найденное окно либо null.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Входные параметры не должны быть нулевыми.
        /// </exception>
        internal static AutomationElement GetMainWindowElement(int processId, string automationId)
        {
            if (processId <= 0)
            {
                throw new ArgumentException("processId");
            }

            if (automationId == null)
            {
                throw new ArgumentNullException("automationId");
            }

            var condition = new AndCondition(
                new PropertyCondition(AutomationElement.ProcessIdProperty, processId),
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
            var mainWindow = AutomationElement.RootElement.FindFirst(TreeScope.Children, condition);
            return mainWindow;
        }
    }
}
