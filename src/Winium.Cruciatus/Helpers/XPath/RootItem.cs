namespace Winium.Cruciatus.Helpers.XPath
{
    #region using

    using System.Windows.Automation;
    using System.Xml.XPath;

    #endregion

    internal class RootItem : ElementItem
    {
        #region Constructors and Destructors

        internal RootItem()
            : base(AutomationElement.RootElement)
        {
        }

        #endregion

        #region Properties

        internal override bool IsEmptyElement
        {
            get
            {
                return false;
            }
        }

        internal override string Name
        {
            get
            {
                return "Desktop Window";
            }
        }

        internal override XPathNodeType NodeType
        {
            get
            {
                return XPathNodeType.Root;
            }
        }

        #endregion

        #region Methods

        internal override XPathItem MoveToNext()
        {
            return null;
        }

        internal override XPathItem MoveToParent()
        {
            return null;
        }

        internal override XPathItem MoveToPrevious()
        {
            return null;
        }

        #endregion
    }
}
