
namespace WpfTestApplication
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation.Peers;

    using Microsoft.Windows.Controls.Ribbon;

    public class RibbonTabExt : RibbonTab
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new RibbonTabAutomationPeer(this);
        }

        private class RibbonTabAutomationPeer : Microsoft.Windows.Automation.Peers.RibbonTabAutomationPeer
        {
            public RibbonTabAutomationPeer(RibbonTab owner)
                : base(owner)
            {
            }

            protected override Point GetClickablePointCore()
            {
                var childs = this.GetChildrenCore();
                if (childs != null)
                {
                    foreach (var peer in childs.Where(peer => peer.GetAutomationControlType() == AutomationControlType.Header))
                    {
                        return peer.GetClickablePoint();
                    }
                }
                
                return base.GetClickablePointCore();
            }
        }
    }
}
