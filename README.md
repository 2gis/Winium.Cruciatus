<p align="right">
English description | <a href="README_RU.md">Описание на русском</a>
</p>

# Winium for Desktop
[![Build Status](https://img.shields.io/jenkins/s/http/opensource-ci.2gis.ru/Winium.Cruciatus.svg?style=flat-square)](http://opensource-ci.2gis.ru/job/Winium.Cruciatus/)
[![Inner Server NuGet downloads](https://img.shields.io/nuget/dt/Winium.Cruciatus.svg?style=flat-square)](https://www.nuget.org/packages/Winium.Cruciatus/)
[![Inner Server NuGet version](https://img.shields.io/nuget/v/Winium.Cruciatus.svg?style=flat-square)](https://www.nuget.org/packages/Winium.Cruciatus/)

<p align="center">
<img src="https://raw.githubusercontent.com/2gis/Winium.StoreApps/assets/winium.png" alt="Winium.Cruciatus is C# Framework for automated testing of Windows application based on WinFroms and WPF platforms">
</p>

Winium.Cruciatus is an open source C# Framework for automated testing of Windows application based on WinFroms and WPF platforms.

Winium.Cruciatus is a wrapper over Microsoft UI Automation library in the [System.Windows.Automation](https://msdn.microsoft.com/en-us/library/system.windows.automation(v=vs.110).aspx) namespace.

## Why Winium.Cruciatus?

- Enough Visual Studio Professional offering
- You can use any testing framework to write tests (example [NUnit](https://www.nuget.org/packages/NUnit/))

## Quick Start

1. Add reference to `Winium.Cruciatus` in UI test project ([install NuGet package](https://www.nuget.org/packages/Winium.Cruciatus/))

2. Create a map application

3. Use created map in tests

4. Run your tests and watch the magic happening

## Example
- [Example test applications](src/TestApplications)
- [Example test projects](src/TestApplications.Tests)

## Very Quick Start

1. Add reference to `Winium.Cruciatus` in UI test project ([install NuGet package](https://www.nuget.org/packages/Winium.Cruciatus/))

2. Create C# Console Application project and use this code:

    ```c#
    namespace ConsoleApplication
    {
        using System.Windows.Automation;
        using Winium.Cruciatus.Core;
        using Winium.Cruciatus.Extensions;

        public class Program
        {
            private static void Main(string[] args)
            {
                var calc = new Winium.Cruciatus.Application("C:/windows/system32/calc.exe");
                calc.Start();

                var winFinder = By.Name("Calculator").AndType(ControlType.Window);
                var win = Winium.Cruciatus.CruciatusFactory.Root.FindElement(winFinder);
                var menu = win.FindElementByUid("MenuBar").ToMenu();

                menu.SelectItem("View$Scientific");
                menu.SelectItem("View$History");

                win.FindElementByUid("132").Click(); // 2
                win.FindElementByUid("93").Click(); // +
                win.FindElementByUid("134").Click(); // 4
                win.FindElementByUid("97").Click(); // ^
                win.FindElementByUid("138").Click(); // 8
                win.FindElementByUid("121").Click(); // =

                calc.Close();
            }
        }
    }
    ```

3. Run ConsoleApplication and watch the magic happening

## Contributing

Contributions are welcome!

1. Check for open issues or open a fresh issue to start a discussion around a feature idea or a bug.
2. Fork the repository to start making your changes to the master branch (or branch off of it).
3. We recommend to write a test which shows that the bug was fixed or that the feature works as expected.
4. Send a pull request and bug the maintainer until it gets merged and published. :smiley:

## Fix issue there're some applications that launched process exists immediately(Ex: microsoft calculator, firefox, chrome, iexplorer), so in close/quit function it throws exception Process Not Found.
Add UpdateRunApplicationProcessBy(string name) in Winium.Cruciatus/Application.cs file to attach Application's process property to running process.
Add HasExited() method to get running state of launched application.

## Contact

Have some questions? Found a bug? Create [new issue](https://github.com/2gis/Winium.Cruciatus/issues/new) or contact us at g.golovin@2gis.ru

## License

Winium is released under the MPL 2.0 license. See [LICENSE](LICENSE) for details.
