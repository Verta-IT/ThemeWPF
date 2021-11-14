# Free and easy-to-use themes for WPF application

## Light Theme

![Image of light theme](LightThemePresentation.png)

Resources that have to be assigned explicitly:

|Key|Type|Info
----|----|----
|WindowBackground|Window|
|TextBlockStyle|TextBlock|
|StrechedDataGridStyle|DataGrid| Use it when one or more columns are streched

## Dark Theme

![Image of light theme](DarkThemePresentation.png)

Resources that have to be assigned explicitly:

|Key|Type|Info
----|----|----
|WindowBackground|Window|
|TextBlockStyle|TextBlock|
|StrechedDataGridStyle|DataGrid| Use it when one or more columns are streched

## How to use

1. Copy whole 'Themes' folder and paste it to the target WPF project.
2. Add new enum. Add member (name is important only for displaying) with the attribute **ResourceUriPathAttribute(path)** where argument **path** is URI of the theme e.g. **@"pack://application:,,,/Themes/LightTheme.xaml"**. Full example:

``` cs
public enum Theme
{
    [ResourceUriPath(@"pack://application:,,,/Themes/LightTheme.xaml")]
    Light,
    [ResourceUriPath(@"pack://application:,,,/Themes/DarkTheme.xaml")]
    Dark
}
```
4. Use class 'ThemeManager' to operate on themes. Example:

    - Create theme manager. Pass merged dictionary where theme is located or do not pass anything and the 'Application.Current.Resources.MergedDictionaries' will be used.

``` cs
var themeManager = new ThemeManager<Theme>(Application.Current.Resources.MergedDictionaries);
```

- Get incorrect themes.
``` cs
var themes = themeManager.GetIncorrectThemes();

if (themes.Any())
{
    ... Do something with themes that are incorrect (notice that the 'themes' 
    is the Dictionary where value is a message with description why this theme is incorrect) ...
}
```

- Get available themes.
``` cs
var availableThemes = themeManager.GetAvailableThemes();
```

- Get actual set theme.
``` cs
var actual = themeManager.GetActualTheme();

if (actual.HasValue)
{
    ... Here do some with actual property (e.g. set to a property) ...
}
```

- Choose a theme.
``` cs
try
{
    themeManager.ChangeTheme(value.Value);
}
catch (System.Exception exc) // The exception will ocurre if the theme is incorrect (e.g. invalid URI path)
{
    MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
}
```

- Remove actual theme. If the is no theme then nothing will be happen.
``` cs
themeManager.RemoveTheme();
```

7. Add the resource dictionary of selected theme to the element resource e.g. application resources (this is the source that has to be passed into constructor argument of the class 'ThemeManager'). This step is not neccessary but do it if you want to make designer work with a theme.

``` xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/Themes/LightTheme.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

For the example of the full 'ThemeManager' implementation see: [LINK](https://github.com/Verta-IT/ThemeWPF/blob/main/Source/VertaIT.ThemeWPF/ViewModels/ThemeCollectionViewModel.cs)

Some controls (such as Window) require an explict assigment of the style and they are listed above in this document under the theme description. Use DynamicResource when you want to change theme in a run time and assigned resoruce key exists in this theme.      

## Create your own theme

To create a new theme just copy one of the existing and pase it next to the others. Change the name of the file and add a new member with **ResourceUriPathAttribute** attribute (pass correct URI to the attribue constructor) to the enum that contains your themes. In the new theme change colors placed on the top of the document or/and templates of the controls. That's it, the new theme was added and can be selected by **ThemeManager** class.
