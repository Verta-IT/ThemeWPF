using OmegaDSD.ThemeWPF.Models;
using OmegaDSD.ThemeWPF.Themes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OmegaDSD.ThemeWPF.ViewModels
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged<T>(ref T property, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            property = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        public Theme? SelectedTheme
        {
            get => ThemeManager.CurrentTheme;
            set
            {
                if (value != null)
                {
                    ThemeManager.ChangeTheme((Theme)value);
                }
                else
                {
                    ThemeManager.RemoveTheme();
                }

                RaisePropertyChanged();
            }
        }
    }
}
