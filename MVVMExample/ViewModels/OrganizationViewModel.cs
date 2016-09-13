using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMExample.Models;

namespace MVVMExample.ViewModels
{
    public class OrganizationViewModel : NotificationBase
    {
        private readonly Organization _organization;

        public OrganizationViewModel(string name)
        {
            _organization = new Organization(name);
            _selectedIndex = -1;
            //Load the database
            foreach (var person in _organization.People)
            {
                var np = new PersonViewModel(person);
                np.PropertyChanged += Person_OnNotifyPropertyChanged;
                _people.Add(np);
            }
        }

        ObservableCollection<PersonViewModel> _people
            = new ObservableCollection<PersonViewModel>();

        public ObservableCollection<PersonViewModel> People
        {
            get { return _people; }
            set { SetProperty(ref _people, value); }
        }

        public string Name
        {
            get { return _organization.Name; }
        }

        int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (SetProperty(ref _selectedIndex, value))
                { RaisePropertyChanged(nameof(SelectedPerson)); }
            }
        }

        public PersonViewModel SelectedPerson
        {
            get { return (_selectedIndex >= 0) ? _people[_selectedIndex] : null; }
        }

        public void Add()
        {
            var person = new PersonViewModel();
            person.PropertyChanged += Person_OnNotifyPropertyChanged;
            People.Add(person);
            _organization.Add(person);
            SelectedIndex = People.IndexOf(person);
        }

        public void Delete()
        {
            if (SelectedIndex != -1)
            {
                var person = People[SelectedIndex];
                People.RemoveAt(SelectedIndex);
                _organization.Delete(person);
            }
        }

        void Person_OnNotifyPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            _organization.Update((PersonViewModel)sender);
        }
    }
}
