using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using VertaIT.WPF.Theme;

namespace VertaIT.WPF.Demo.ViewModels
{
    public enum Theme
    {
        [ResourceUriPath(@"pack://application:,,,/VertaIT.WPF.Theme;component/Themes/LightTheme.xaml")]
        Light,
        [ResourceUriPath(@"pack://application:,,,/VertaIT.WPF.Theme;component/Themes/DarkTheme.xaml")]
        Dark
    }

    public class ThemeCollectionViewModel : INotifyPropertyChanged
    {
        private readonly ThemeManager<Theme> _themeManager;
        private Theme? _selectedTheme;

        public ThemeCollectionViewModel()
        {
            _themeManager = new ThemeManager<Theme>(Application.Current.Resources.MergedDictionaries);

            DisplayMessageAboutIncorrectThemes();

            var availableThemes = _themeManager.GetAvailableThemes();

            Themes = new List<Theme>(availableThemes);

            var actual = _themeManager.GetActualTheme();

            if (actual.HasValue)
            {
                _selectedTheme = actual;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Theme> Themes { get; }

        public Theme? SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (value != _selectedTheme)
                {
                    if (value.HasValue)
                    {
                        try
                        {
                            _themeManager.ChangeTheme(value.Value);
                        }
                        catch (System.Exception exc)
                        {
                            MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                            return;
                        }

                        RaisePropertyChanged(nameof(IsThemeDisabled));
                    }
                    else
                    {
                        _themeManager.RemoveTheme();

                        RaisePropertyChanged(nameof(IsThemeDisabled));
                    }

                    RaisePropertyChanged(ref _selectedTheme, value);
                }
            }
        }

        public bool IsThemeDisabled
        {
            get => !SelectedTheme.HasValue;
            set => SelectedTheme = value ? (Theme?)null : Themes.FirstOrDefault();
        }

        public void RaisePropertyChanged<T>(ref T property, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            property = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DisplayMessageAboutIncorrectThemes()
        {
            var themes = _themeManager.GetIncorrectThemes();

            if (themes.Any())
            {
                var strBuilder = new StringBuilder();

                var indexer = 1;

                foreach (var theme in themes)
                {
                    strBuilder.AppendLine($"{indexer++}. {theme.Value}");
                }

                MessageBox.Show(strBuilder.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}