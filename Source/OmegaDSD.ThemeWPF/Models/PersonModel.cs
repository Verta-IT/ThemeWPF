namespace OmegaDSD.ThemeWPF.Models
{
    public class PersonModel : NotifyPropertyChanged
    {
        public PersonModel(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public PersonModel()
        {
        }

        private bool isActive = true;
        private string name = "New";
        private int age = 0;

        public string Name
        {
            get => name;
            set => RaisePropertyChanged(ref name, value);
        }

        public int Age
        {
            get => age;
            set => RaisePropertyChanged(ref age, value);
        }

        public bool IsActive
        {
            get => isActive;
            set => RaisePropertyChanged(ref isActive, value);
        }
    }
}