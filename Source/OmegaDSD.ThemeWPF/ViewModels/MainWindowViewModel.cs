using OmegaDSD.ThemeWPF.Models;
using OmegaDSD.ThemeWPF.Themes;
using System;
using System.Collections.ObjectModel;

namespace OmegaDSD.ThemeWPF.ViewModels
{
    public class ThemeCollection : ObservableCollection<ThemeModel>
    {
        public ThemeCollection()
        {
            Array themeList = Enum.GetValues(typeof(Theme));

            Add(new ThemeModel(null));

            foreach (object theme in themeList)
            {
                Add(new ThemeModel((Theme)theme));
            }
        }
    }

    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            PersonCollection.Add(new PersonModel("Josh", 20));
            PersonCollection.Add(new PersonModel("Sebastian", 25));
            PersonCollection.Add(new PersonModel("Alex", 50));
        }

        public ObservableCollection<PersonModel> PersonCollection { get; } = new ObservableCollection<PersonModel>();

        public ThemeCollection ThemeCollection { get; } = new ThemeCollection();
    }
}
