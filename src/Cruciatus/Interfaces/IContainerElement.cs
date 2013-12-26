
namespace Cruciatus.Interfaces
{
    using System.Windows.Automation;

    public interface IContainerElement
    {
        void Initialize(AutomationElement parent, string automationId);
    }
}
