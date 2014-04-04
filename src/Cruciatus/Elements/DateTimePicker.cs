// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimePicker.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет элемент управления поле с датой.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Cruciatus.Elements
{
    #region using

    using System.Drawing;
    using System.Windows.Automation;
    using System.Windows.Forms;

    using Cruciatus.Extensions;
    using Cruciatus.Interfaces;

    using Microsoft.VisualStudio.TestTools.UITesting;

    #endregion

    public class DateTimePicker : CruciatusElement, IContainerElement
    {
        public DateTimePicker()
        {
        }

        public DateTimePicker(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        public Point ClickablePoint
        {
            get
            {
                var windowsPoint = this.GetPropertyValue<System.Windows.Point>(AutomationElement.ClickablePointProperty);

                return new Point((int)windowsPoint.X, (int)windowsPoint.Y);
            }
        }

        internal override string ClassName
        {
            get
            {
                return "DateTimePicker";
            }
        }

        internal override ControlType GetType
        {
            get
            {
                // TODO: а какой он??
                return ControlType.Custom;
            }
        }

        void IContainerElement.Initialize(AutomationElement parent, string automationId)
        {
            Initialize(parent, automationId);
        }

        public void SetDateTime(string value)
        {
            Mouse.MouseMoveSpeed = CruciatusFactory.Settings.MouseMoveSpeed;
            Mouse.Move(ClickablePoint);
            Mouse.Click(MouseButtons.Left);
            Keyboard.SendKeys("^a");
            Keyboard.SendKeys(value);
            Keyboard.SendKeys("{Enter}");
        }
    }
}
