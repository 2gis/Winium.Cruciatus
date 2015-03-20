# Winium для Desktop
[![Inner Server NuGet downloads](https://img.shields.io/nuget/dt/Winium.Cruciatus.svg?style=flat-square)](https://www.nuget.org/packages/Winium.Cruciatus/)
[![Inner Server NuGet version](https://img.shields.io/nuget/v/Winium.Cruciatus.svg?style=flat-square)](https://www.nuget.org/packages/Winium.Cruciatus/)

<p align="center">
<img src="https://raw.githubusercontent.com/2gis/Winium/master/assets/winium.png" alt="Winium.Cruciatus это C# фреймворк для автоматизации тестирования Windows приложений построенных на WinFroms или WPF платформах">
</p>

Winium.Cruciatus это open-source C# фреймворк для автоматизации тестирования Windows приложений построенных на WinFroms или WPF платформах.

Winium.Cruciatus это обёртка над библиотекой Microsoft UI Automation из пространства имён [System.Windows.Automation](https://msdn.microsoft.com/en-us/library/system.windows.automation(v=vs.110).aspx).

## Почему Winium.Cruciatus?

- Для работы достаточно редакции Visual Studio Professional
- Вы можете использовать любой тестовый фреймворк для написания тестов (например [NUnit](https://www.nuget.org/packages/NUnit/))

## Быстрый старт

1. Добавить ссылку на `Winium.Cruciatus` в тестовом проекте ([через NuGet пакет](https://www.nuget.org/packages/Winium.Cruciatus/))

2. Описать карту приложения

3. Используя карту написать тесты

4. Запустить тесты и балдеть от происходящей магии

## Примеры
- [Примеры тестовый приложений](src/TestApplications)
- [Примеры тестовых проектов](src/TestApplications.Tests)

## Очень быстрый старт

1. Создать C# Console Application проект

2. Добавить ссылку на `Winium.Cruciatus` ([через NuGet пакет](https://www.nuget.org/packages/Winium.Cruciatus/))

3. Использовать следующий код:

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

                var winFinder = By.Name("Калькулятор").AndType(ControlType.Window);
                var win = Winium.Cruciatus.CruciatusFactory.Root.FindElement(winFinder);
                var menu = win.FindElementByUid("MenuBar").ToMenu();

                menu.SelectItem("Вид$Инженерный");
                menu.SelectItem("Вид$Журнал");

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

3. Запустить ConsoleApplication и балдеть от происходящей магии

## Вклад в развитие

Мы открыты для сотрудничества!

1. Проверьте нет ли уже открытого issue или заведите новый issue для обсуждения новой фичи или бага.
2. Форкните репозиторий и начните делать свои изменения в ветке мастер или новой ветке
3. Мы советуем написать тест, который покажет, что баг был починен или что новая фича работает как ожидается.
4. Создайте pull-request и тыкайте в мэнтейнера до тех пор, пока он не примет и не сольет ваши изменения. :smiley:

## Контакты

Есть вопросы? Нашли ошибку? Создавайте [новое issue](https://github.com/2gis/Winium.Cruciatus/issues/new) или пишите g.golovin@2gis.ru

## Лицензия

Winium выпущен под MPL 2.0 лицензией. [Подробности](LICENSE).
