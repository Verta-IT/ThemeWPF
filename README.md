# Free and easy-to-use themes for WPF application

## Light Theme

![Image of light theme](LightThemePresentation.png)

Styles that has to be explicit assigment:

|Key|Type|Info
----|----|----
|WindowBackground|Window|
|TextBlockStyle|TextBlock|
|DataGridCellNoBorderStyle|DataGridCell| Use it to remove border of the selected cell
|StrechedDataGridStyle|DataGrid| Use it when one or more columns are streched
|AzureToggleSwitchStyle|CheckBox| Use it to change normal CheckBox to a switch

## Dark Theme

![Image of light theme](DarkThemePresentation.png)

Styles that has to be explicit assigment:

|Key|Type|Info
----|----|----
|WindowBackground|Window|
|TextBlockStyle|TextBlock|
|DataGridCellNoBorderStyle|DataGridCell| Use it to remove border of the selected cell
|StrechedDataGridStyle|DataGrid| Use it when one or more columns are streched
|AzureToggleSwitchStyle|CheckBox| Use it to change normal CheckBox to a switch

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

Some controls (such as Window) require an explict assigment of the style and they are listed above in this document under the theme description. Use DynamicResource when you want to change theme in a run time and assigned resoruce key exists in this theme.      

## Use the premade theme view-model to change theme from view layer

1. Add reference to the namespace that contain the view-model
``` xml
xmlns:themes="clr-namespace:OmegaDSD.ThemeWPF.Themes"
```
2. Assign view-model to the data context of a control
``` xml
<ListBox ItemsSource="{Binding Collection}" SelectedItem="{Binding SelectedTheme, Mode=TwoWay}" >
    <ListBox.DataContext>
        <themes:ThemeCollectionViewModel/>
    </ListBox.DataContext>
</ListBox>
```
if you also want to be able to disable the theme:
``` xml
<ListBox ItemsSource="{Binding Collection}" SelectedItem="{Binding SelectedTheme, Mode=TwoWay}" >
    <ListBox.DataContext>
        <ObjectDataProvider ObjectType="{x:Type themes:ThemeCollectionViewModel}">
            <ObjectDataProvider.ConstructorParameters>
                <sys:Boolean>True</sys:Boolean>
            </ObjectDataProvider.ConstructorParameters>
        </ObjectDataProvider>
    </ListBox.DataContext>
</ListBox>
```
