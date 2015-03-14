﻿namespace Winium.Cruciatus.Core
{
    /// <summary>
    /// Интерфейс симулятора клавиатуры.
    /// </summary>
    public interface IKeyboard
    {
        #region Public Methods and Operators

        /// <summary>
        /// Эмулирует нажатие кнопки Backspace.
        /// </summary>
        IKeyboard SendBackspace();

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + A.
        /// </summary>
        IKeyboard SendCtrlA();

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + C.
        /// </summary>
        IKeyboard SendCtrlC();

        /// <summary>
        /// Эмулирует нажатие сочетания кнопок Ctrl + V.
        /// </summary>
        IKeyboard SendCtrlV();

        /// <summary>
        /// Эмулирует нажатие кнопки Enter.
        /// </summary>
        IKeyboard SendEnter();

        /// <summary>
        /// Эмулирует нажатие кнопки Escape.
        /// </summary>
        IKeyboard SendEscape();

        /// <summary>
        /// Эмулирует ввод текста.
        /// </summary>
        /// <param name="text">
        /// Текст.
        /// </param>
        IKeyboard SendText(string text);

        #endregion
    }
}
