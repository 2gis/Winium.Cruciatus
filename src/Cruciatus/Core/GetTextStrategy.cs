namespace Cruciatus.Core
{
    #region using

    using System;

    #endregion

    /// <summary>
    /// ������������ �������������� ��������� ��������� ������. ����� ������� Flags.
    /// </summary>
    [Flags]
    public enum GetTextStrategies
    {
        /// <summary>
        /// ���������� ������������ ���������.
        /// </summary>
        None = 0, 

        /// <summary>
        /// ��������� ������������� ���������� TextPattern.
        /// </summary>
        TextPattern = 1, 

        /// <summary>
        /// ��������� ������������� ���������� ValuePattern.
        /// </summary>
        ValuePattern = 2
    }
}
