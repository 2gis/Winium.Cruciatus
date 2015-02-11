namespace Cruciatus.Core
{
    public interface IKeyboard
    {
        IKeyboard SendText(string text);

        IKeyboard SendEnter();

        IKeyboard SendBackspace();

        IKeyboard SendEscape();

        IKeyboard SendCtrlA();

        IKeyboard SendCtrlC();

        IKeyboard SendCtrlV();
    }
}
