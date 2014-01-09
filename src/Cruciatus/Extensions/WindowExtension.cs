// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtension.cs" company="2GIS">
//   Cruciatus
// </copyright>
// <summary>
//   Представляет расширения для элемента Window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Cruciatus.Extensions
{
    using System.Windows;

    using MessageBox = Cruciatus.MessageBox;
    using Window = Cruciatus.Elements.Window;

    public static class WindowExtension
    {
        /// <summary>
        /// Закрывает диалог MessageBox.
        /// </summary>
        /// <param name="window">
        /// Окно, в котором открыт диалог MessageBox.
        /// </param>
        /// <param name="buttonsType">
        /// Тип набора кнопок в диалоге MessageBox.
        /// </param>
        /// <param name="button">
        /// Кнопка, которой будет производиться закрытие диалога.
        /// </param>
        /// <returns>
        /// Значение true если закрыть удалось; в противном случае значение - false.
        /// </returns>
        public static bool CloseMessageBox(this Window window, MessageBoxButton buttonsType, MessageBoxResult button)
        {
            return MessageBox.ClickButton(window, buttonsType, button);
        }
    }
}
