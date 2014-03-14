
namespace Cruciatus.Settings.MessageBoxSettings
{
    using System;

    public class YesNoCancelType : ICloneable
    {
        public string Yes { get; set; }

        public string No { get; set; }

        public string Cancel { get; set; }

        public object Clone()
        {
            return new YesNoCancelType { Yes = this.Yes, No = this.No, Cancel = this.Cancel };
        }
    }
}
