# Cruciatus

Cruciatus is a easy to use framework for UI test automation of WPF based applications. It is using Microsoft UIAutomation technology. 

## Status

Working prototype.

## Getting Started

1) Download `Cruciatus.dll` from build folder (in master brunch).

2) Open or create CodedUITestProject.

3) Add `Cruciatus.dll` to references.

4) Create a map application

5) Use created map in tests

## Example
### Example test applications
[https://github.com/2gis/cruciatus/tree/master/src/TestApplications](https://github.com/2gis/cruciatus/tree/master/src/TestApplications)

### Example test projects
[https://github.com/2gis/cruciatus/tree/master/src/TestApplications.Tests](https://github.com/2gis/cruciatus/tree/master/src/TestApplications.Tests)

### Example maps application

TabItem (contains Button, TextBox).

```cs
using Cruciatus.Elements;
public class ViewRibbonTab : TabItem
{
    public Button ZoomInButton
    {
        get
        {
            return this.GetElement<Button>("ZoomInButtonUid");
        }
    }

    public TextBox ExtentTextBox
    {
        get
        {
            return this.GetElement<TextBox>("ExtentTextBoxUid");
        }
    }
}
```

Window (contains ViewRibbonTab (...)).

```cs
using Cruciatus.Elements;
public class MainWindow : Window
{
    public ViewRibbonTab ViewRibbonTab
    {
        get
        {
            return this.GetElement<ViewRibbonTab>("ViewRibbonTabUid");
        }
    }
}
```

Application (contains MainWindow (...)).

```cs
using Cruciatus;
public class App : Application<MainWindow>
{
    public App(string fullPath)
        : base(fullPath, "MainWindowUid")
    {
    }
}
```

### Example test

```cs
[TestMethod]
public void CruciatusUITestMethod()
{
    var app = new App("D:\\App.exe");
    Assert.IsTrue(app.Start());
	
    string extent = app.MainWindow.ViewRibbonTab.ExtentTextBox.Text;
    app.MainWindow.ViewRibbonTab.ZoomInButton.Click();
    Assert.AreNotEqual(extent, app.MainWindow.ViewRibbonTab.ExtentTextBox.Text);
	
    Assert.IsTrue(app.Close());
}
```
