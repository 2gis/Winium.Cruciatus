namespace Winium.Cruciatus.Core
{
    #region using

    using System.Windows;
    using System.Windows.Automation;

    #endregion

    /// <summary>
    /// Класс свойств CruciatusElement.
    /// </summary>
    public class CruciatusElementProperties
    {
        #region Fields

        private readonly AutomationElement element;

        #endregion

        #region Constructors and Destructors

        internal CruciatusElementProperties(AutomationElement element)
        {
            this.element = element;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Свойство BoundingRectangle.
        /// </summary>
        public Rect BoundingRectangle
        {
            get
            {
                return this.element.Current.BoundingRectangle;
            }
        }

        /// <summary>
        /// Свойство ClickablePoint. Внимание, значение может отсутствовать.
        /// </summary>
        public Point? ClickablePoint
        {
            get
            {
                Point point;
                var exists = this.element.TryGetClickablePoint(out point);
                return exists ? point : new Point?();
            }
        }

        /// <summary>
        /// Свойство IsEnabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.element.Current.IsEnabled;
            }
        }

        /// <summary>
        /// Свойство IsOffscreen.
        /// </summary>
        public bool IsOffscreen
        {
            get
            {
                return this.element.Current.IsOffscreen;
            }
        }

        /// <summary>
        /// Свойство Name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.element.Current.Name;
            }
        }

        /// <summary>
        /// Строковое представление RuntimeId элемента.
        /// </summary>
        public string RuntimeId
        {
            get
            {
                return string.Join(" ", this.element.GetRuntimeId());
            }
        }

        #endregion
    }
}
