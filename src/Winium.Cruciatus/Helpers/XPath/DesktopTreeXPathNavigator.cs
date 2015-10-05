namespace Winium.Cruciatus.Helpers.XPath
{
    #region using

    using System;
    using System.Windows.Automation;
    using System.Xml;
    using System.Xml.XPath;

    #endregion

    internal class DesktopTreeXPathNavigator : XPathNavigator
    {
        #region Fields

        private XPathItem item;

        #endregion

        #region Constructors and Destructors

        public DesktopTreeXPathNavigator()
            : this(AutomationElement.RootElement)
        {
        }

        internal DesktopTreeXPathNavigator(AutomationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.item = ElementItem.Create(element);
        }

        internal DesktopTreeXPathNavigator(XPathItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.item = item;
        }

        #endregion

        #region Public Properties

        public override string BaseURI
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool IsEmptyElement
        {
            get
            {
                return this.item.IsEmptyElement;
            }
        }

        public override string LocalName
        {
            get
            {
                return this.Name;
            }
        }

        public override string Name
        {
            get
            {
                return this.item.Name;
            }
        }

        public override XmlNameTable NameTable
        {
            get
            {
                return null;
            }
        }

        public override string NamespaceURI
        {
            get
            {
                return string.Empty;
            }
        }

        public override XPathNodeType NodeType
        {
            get
            {
                return this.item.NodeType;
            }
        }

        public override string Prefix
        {
            get
            {
                return string.Empty;
            }
        }

        public override object TypedValue
        {
            get
            {
                return this.item.TypedValue();
            }
        }

        public override string Value
        {
            get
            {
                return this.item.Value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override XPathNavigator Clone()
        {
            return new DesktopTreeXPathNavigator(this.item);
        }

        public override bool IsSamePosition(XPathNavigator other)
        {
            var obj = other as DesktopTreeXPathNavigator;
            return obj != null && obj.item.IsSamePosition(this.item);
        }

        public override bool MoveTo(XPathNavigator other)
        {
            var obj = other as DesktopTreeXPathNavigator;
            if (obj == null)
            {
                return false;
            }

            this.item = obj.item;
            return true;
        }

        public override bool MoveToFirstAttribute()
        {
            return this.MoveToItem(this.item.MoveToFirstProperty());
        }

        public override bool MoveToFirstChild()
        {
            return this.MoveToItem(this.item.MoveToFirstChild());
        }

        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
        {
            return false;
        }

        public override bool MoveToId(string id)
        {
            return false;
        }

        public override bool MoveToNext()
        {
            return this.MoveToItem(this.item.MoveToNext());
        }

        public override bool MoveToNextAttribute()
        {
            return this.MoveToItem(this.item.MoveToNextProperty());
        }

        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
        {
            return false;
        }

        public override bool MoveToParent()
        {
            return this.MoveToItem(this.item.MoveToParent());
        }

        public override bool MoveToPrevious()
        {
            return this.MoveToItem(this.item.MoveToPrevious());
        }

        #endregion

        #region Methods

        private bool MoveToItem(XPathItem newItem)
        {
            if (newItem == null)
            {
                return false;
            }

            this.item = newItem;
            return true;
        }

        #endregion
    }
}
