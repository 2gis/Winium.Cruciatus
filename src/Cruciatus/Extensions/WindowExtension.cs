namespace Cruciatus.Extensions
{
    #region using

    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    using MessageBox = Cruciatus.MessageBox;
    using Window = Cruciatus.Elements.Window;

    #endregion

    public static class WindowExtension
    {
        /// <summary>
        /// Закрывает диалоговое окно MessageBox.
        /// </summary>
        /// <param name="parent">
        /// Элемент, являющийся родительским для диалогового окна.
        /// </param>
        /// <param name="buttonsType">
        /// Тип набора кнопок в диалоговом окне.
        /// </param>
        /// <param name="button">
        /// Кнопка, которой будет производиться закрытие диалогового окна.
        /// </param>
        /// <returns>
        /// Значение true если закрыть удалось; в противном случае значение - false.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "Это расширение, а значит для конкретного типа")]
        public static bool CloseMessageBox(this Window parent, MessageBoxButton buttonsType, MessageBoxResult button)
        {
            return MessageBox.ClickButton(parent, buttonsType, button);
        }
    }
}
