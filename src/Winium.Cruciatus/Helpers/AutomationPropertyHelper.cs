namespace Winium.Cruciatus.Helpers
{
    #region using

    using System.Text.RegularExpressions;
    using System.Windows.Automation;

    #endregion

    internal static class AutomationPropertyHelper
    {
        #region Methods

        internal static string GetPropertyName(AutomationIdentifier property)
        {
            var pattern = new Regex(@".*\.(?<name>.*)Property");
            return pattern.Match(property.ProgrammaticName).Groups["name"].Value;
        }

        #endregion
    }
}
