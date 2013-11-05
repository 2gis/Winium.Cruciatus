# Cruciatus

Cruciatus is a easy to use framework for UI test automation of WPF based applications. It is using Microsoft UIAutomation technology. 

## Status

Working prototype.

## Getting Started

1) Download 'Cruciatus.dll' from 'build' folder (in 'master' brunch).

2) Open or create CodedUITestProject.

3) Add 'Cruciatus.dll' to references.

4) Create a map application

5) Use created map in tests

## Example

TabItem (contains Button, TextBox).

```cs
using Cruciatus.Elements;
public class ViewRibbonTab : TabItem
{
    public Button ZoomInButton
    {
        get
        {
            return this.GetElement<Button>("ZoomInButton");
        }
    }

    public TextBox ExtentTextBox
    {
        get
        {
            return this.GetElement<TextBox>("ExtentTextBox");
        }
    }
}
```

Window (contains TabItem (...)).

```cs
using Cruciatus.Elements;
public class MainWindow : Window
{
    public ViewRibbonTab ViewRibbonTab
    {
        get
        {
            return this.GetElement<ViewRibbonTab>("ViewRibbonTab");
        }
    }
}
```

Application (contains Window (...)).

```cs
using Cruciatus;
public class App : Application<MainWindow>
{
    public App(string fullPath)
        : base(fullPath)
    {
    }
}
```

Test.

```cs
[TestMethod]
public void CodedUITestMethod1()
{
    var app = new App("D:\\App.exe");
    Assert.IsTrue(app.Start());
	
    string extent = app.MainWindow.ViewRibbonTab.ExtentTextBox.Text;
    app.MainWindow.ViewRibbonTab.ZoomInButton.Click();
    Assert.AreNotEqual(extent, app.MainWindow.ViewRibbonTab.ExtentTextBox.Text);
	
    app.Close();
}
```