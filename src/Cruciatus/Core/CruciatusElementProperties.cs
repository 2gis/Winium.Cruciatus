namespace Cruciatus.Core
{
    #region using

    using System;
    using System.Windows;
    using System.Windows.Automation;

    using Cruciatus.Elements;

    #endregion

    public class CruciatusElementProperties
    {
        private readonly AutomationElement _element;

        public CruciatusElementProperties(CruciatusElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            _element = element.Instanse;
        }

        internal CruciatusElementProperties(AutomationElement element)
        {
            _element = element;
        }

        public string Name
        {
            get
            {
                return _element.Current.Name;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return _element.Current.IsEnabled;
            }
        }

        public bool IsOffscreen
        {
            get
            {
                return _element.Current.IsOffscreen;
            }
        }

        public Point? ClickablePoint
        {
            get
            {
                Point point;
                var exists = _element.TryGetClickablePoint(out point);
                return exists ? point : new Point?();
            }
        }
    }
}
