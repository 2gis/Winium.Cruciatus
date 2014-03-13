
namespace Cruciatus.Settings.MessageBox
{
    using System;

    public class OkCancelType : ICloneable
    {
        public string Ok { get; set; }

        public string Cancel { get; set; }

        public object Clone()
        {
            return new OkCancelType { Ok = this.Ok, Cancel = this.Cancel };
        }
    }
}
