using System.Collections.Generic;
using System.Linq;

namespace VertaIT.WPF.Models
{
    public class PersonModel : NotifyPropertyChanged
    {
        private bool _isActive = true;
        private string _name = "New";
        private int _age = 0;
        private string _selectedItem;

        public PersonModel(string name, int age) : this()
        {
            _name = name;
            _age = age;
        }

        public PersonModel()
        {
            Items = new List<string>
            {
                "Car",
                "Glass",
                "Cup"
            };

            _selectedItem = Items.FirstOrDefault();
        }

        public string Name
        {
            get => _name;
            set => RaisePropertyChanged(ref _name, value);
        }

        public int Age
        {
            get => _age;
            set => RaisePropertyChanged(ref _age, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => RaisePropertyChanged(ref _isActive, value);
        }

        public string SelectedItem
        {
            get => _selectedItem;
            set => RaisePropertyChanged(ref _selectedItem, value);
        }

        public List<string> Items { get; }
    }
}