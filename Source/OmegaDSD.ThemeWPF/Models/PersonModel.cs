using System;

namespace OmegaDSD.ThemeWPF.Models
{
    public class PersonModel
    {
        public PersonModel(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}