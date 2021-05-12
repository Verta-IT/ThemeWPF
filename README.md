# Free and easy-to-use themes for WPF application

## Light Theme

![Image of light theme](LightThemePresentation.png)

## Dark Theme

![Image of light theme](DarkThemePresentation.png)

## How to use

1. Copy whole 'Themes' folder and paste it to the WPF project.
3. Choose a theme.
2. Add the resource dictionary of selected theme to the element resource e.g. application resources.

``` xml

<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Themes/LightTheme.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>

```

