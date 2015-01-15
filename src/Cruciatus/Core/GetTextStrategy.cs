namespace Cruciatus.Core
{
    #region using

    using System;

    #endregion

    [Flags]
    public enum GetTextStrategy
    {
        None = 0, 

        TextPattern = 1, 

        ValuePattern = 2
    }
}
