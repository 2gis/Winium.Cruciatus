namespace Winium.Cruciatus.Settings
{
    /// <summary>
    /// Перечисление поддерживаемых симуляторов клавиатуры.
    /// </summary>
    public enum KeyboardSimulatorType
    {
        /// <summary>
        /// Список поддерживаемых ключей клавиш и конструкций https://msdn.microsoft.com/ru-ru/library/system.windows.forms.sendkeys(v=vs.110).aspx
        /// </summary>
        BasedOnWindowsFormsSendKeysClass, 

        /// <summary>
        /// Для получения допольнительных методов необходимо привести к KeyboardSimulatorExt. 
        /// Подробности о библиотеке http://inputsimulator.codeplex.com/
        /// </summary>
        BasedOnInputSimulatorLib
    }
}
