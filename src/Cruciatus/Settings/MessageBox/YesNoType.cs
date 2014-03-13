
namespace Cruciatus.Settings.MessageBox
{
    using System;

    public class YesNoType : ICloneable
    {
        public string Yes { get; set; }

        public string No { get; set; }

        public object Clone()
        {
            return new YesNoType { Yes = this.Yes, No = this.No };
        }
    }
}
