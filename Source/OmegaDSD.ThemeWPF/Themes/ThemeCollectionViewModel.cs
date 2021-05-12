using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OmegaDSD.ThemeWPF.Themes
{
    public class ThemeModel
    {
        public ThemeModel(Theme? theme)
        {
            Theme = theme;
        }

        public Theme? Theme { get; private set; }

        public override string ToString()
        {
            return (Theme is null) ? "None" : Theme.ToString();
        }
    }

    public class ThemeCollectionViewModel : INotifyPropertyChanged
    {
        private ThemeModel selectedTheme;

        public ThemeCollectionViewModel(bool addNone)
        {
            if (addNone)
            {
                Collection.Add(new ThemeModel(null));
            }

            Array themeList = Enum.GetValues(typeof(Theme));

            foreach (Theme theme in themeList)
            {
                ThemeModel themeModel = new ThemeModel(theme);

                Collection.Add(themeModel);

                if (theme == ThemeManager.CurrentTheme)
                {
                    selectedTheme = themeModel;
                }
            }
        }

        public ThemeCollectionViewModel() : this(false)
        {
        }

        public ObservableCollection<ThemeModel> Collection { get; } = new ObservableCollection<ThemeModel>();

        public ThemeModel SelectedTheme
        {
            get => selectedTheme;
            set
            {
                if (value != selectedTheme)
                {
                    if (value.Theme is null)
                    {
                        ThemeManager.RemoveTheme();
                    }
                    else
                    {
                        ThemeManager.ChangeTheme((Theme)value.Theme);
                    }

                    RaisePropertyChanged(ref selectedTheme, value);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged<T>(ref T property, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            property = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}