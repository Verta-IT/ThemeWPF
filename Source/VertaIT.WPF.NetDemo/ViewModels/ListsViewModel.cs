using VertaIT.WPF.NetDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertaIT.WPF.NetDemo.ViewModels
{
    public class ListsViewModel
    {
        public ListsViewModel()
        {
            PersonCollection.Add(new PersonModel("Josh", 20));
            PersonCollection.Add(new PersonModel("Sebastian", 25));
            PersonCollection.Add(new PersonModel("Alex", 50));
        }

        public ObservableCollection<PersonModel> PersonCollection { get; } = new ObservableCollection<PersonModel>();
    }
}
