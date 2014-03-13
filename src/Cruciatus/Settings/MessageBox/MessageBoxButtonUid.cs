
namespace Cruciatus.Settings.MessageBox
{
    using System;

    public class MessageBoxButtonUid : ICloneable
    {
        public string CloseButton { get; set; }

        public OkType OkType { get; set; }

        public OkCancelType OkCancelType { get; set; }

        public YesNoType YesNoType { get; set; }

        public YesNoCancelType YesNoCancelType { get; set; }

        public object Clone()
        {
            return new MessageBoxButtonUid
                       {
                           CloseButton = this.CloseButton,
                           OkType = (OkType)this.OkType.Clone(),
                           OkCancelType = (OkCancelType)this.OkCancelType.Clone(),
                           YesNoType = (YesNoType)this.YesNoType.Clone(),
                           YesNoCancelType = (YesNoCancelType)this.YesNoCancelType.Clone()
                       };
        }
    }
}
