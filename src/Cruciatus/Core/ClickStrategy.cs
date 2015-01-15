namespace Cruciatus.Core
{
    #region using

    using System;

    #endregion

    [Flags]
    public enum ClickStrategies
    {
        None = 0, 

        ClickablePoint = 1, 

        BoundingRectangleCenter = 2, 

        InvokePattern = 4
    }
}
