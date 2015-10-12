namespace Winium.Cruciatus.Helpers.XPath
{
    #region using

    using System.Xml.XPath;

    #endregion

    internal abstract class XPathItem
    {
        #region Properties

        internal abstract bool IsEmptyElement { get; }

        internal abstract string Name { get; }

        internal abstract XPathNodeType NodeType { get; }

        internal virtual string Value
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion

        #region Public Methods and Operators

        public abstract object TypedValue();

        #endregion

        #region Methods

        internal abstract bool IsSamePosition(XPathItem item);

        internal abstract XPathItem MoveToFirstChild();

        internal abstract XPathItem MoveToFirstProperty();

        internal abstract XPathItem MoveToNext();

        internal abstract XPathItem MoveToNextProperty();

        internal abstract XPathItem MoveToParent();

        internal abstract XPathItem MoveToPrevious();

        #endregion
    }
}
