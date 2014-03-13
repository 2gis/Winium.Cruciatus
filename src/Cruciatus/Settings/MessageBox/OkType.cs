
namespace Cruciatus.Settings.MessageBox
{
    using System;

    public class OkType : ICloneable
    {
        public string Ok { get; set; }

        public object Clone()
        {
            return new OkType { Ok = this.Ok };
        }
    }
}
