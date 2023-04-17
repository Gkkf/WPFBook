using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KsiazkaWPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int page = 1;
        public int pageSize = 15;
        public string url = "starting.txt";
        public ObservableCollection<Person> persons = new ObservableCollection<Person>();
        public ObservableCollection<Person> accPersons = new ObservableCollection<Person>();
        int counter = 0;

        public MainWindow()
        {
            InitializeComponent();
            LoadData(url);
            Load();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveData(url);
        }

        private void Load()
        {
            counter = persons.Count;

            accPersons.Clear();

            for(int i=(page-1)*pageSize; i<page*pageSize; i++) 
            {
                if(i<counter)
                {
                    accPersons.Add(persons[i]);
                }
            }

            dataGrid.ItemsSource = accPersons;
            pageCount.Content = page.ToString();
        }

        private void LoadData(string path) 
        {
            if(File.Exists(path)) 
            {
                foreach(var line in File.ReadAllLines(path)) 
                {
                    var person = Person.Deserialize(line);
                    persons.Add(new Person { Name = person.Name, Surname = person.Surname, Number = person.Number, Email = person.Email });
                }

                page = 1;
                Load();
            }
            else
            {
                MessageBox.Show("Plik nie został odczytany");
            }
        }

        private void SaveData(string path) 
        {
            if(persons!=null || persons.Count!=0) 
            {
                string data = "";

                foreach(var person in persons) 
                {
                    string line = person.Serialize();
                    data += line;
                }

                File.WriteAllText(path, data);
            }
        }

        //zapis do pliku
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveData(url);
        }

        //odczyt z pliku
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".txt";

            //nullable bool (true, false, null)
            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                persons.Clear();
                url = dialog.FileName;
                LoadData(url);
            }
        }

        //dodawanie
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();

            var person = optionsWindow.person;

            optionsWindow.ShowDialog();

            if(!string.IsNullOrEmpty(person.Name) || !string.IsNullOrEmpty(person.Surname) || !string.IsNullOrEmpty(person.Number) || !string.IsNullOrEmpty(person.Email))
            {
                persons.Add(new Person { Name = person.Name, Surname = person.Surname, Number = person.Number, Email = person.Email });
            }

            Load();
        }

        //edycja
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            int i = dataGrid.SelectedIndex;
            object selectedObj = dataGrid.SelectedItem;

            if (selectedObj != null)
            {
                Person selectedPerson = (Person)dataGrid.SelectedItem;

                OptionsWindow optionsWindow = new OptionsWindow(selectedPerson.Name, selectedPerson.Surname, selectedPerson.Number, selectedPerson.Email);

                var person = optionsWindow.person;

                optionsWindow.ShowDialog();

                if (!string.IsNullOrEmpty(person.Name) || !string.IsNullOrEmpty(person.Surname) || !string.IsNullOrEmpty(person.Number) || !string.IsNullOrEmpty(person.Email))
                {
                    //wyszukiwanie elementu (funkcja FirstOrDefault zwraca pierwszy element z listy który spełnia warunek określony w wyrażeniu lambda)
                    //wyrażenie lambda to definicja funkcji (strzałkowej), nie posiada nazwy i nie jest zdefiniowana jako osobny element programu, ale może zostać użyta jako argument funkcji
                    var edited = persons.FirstOrDefault(p => p.Name == selectedPerson.Name && p.Surname == selectedPerson.Surname && p.Number == selectedPerson.Number && p.Email == selectedPerson.Email);

                    if (edited != null)
                    {
                        edited.Name = person.Name;
                        edited.Surname = person.Surname;
                        edited.Number = person.Number;
                        edited.Email = person.Email;
                    }
                }

                Load();
            }
            else
            {
                MessageBox.Show("Nie wybrano kontaktu");
            }
        }

        //usuwanie
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            int i = dataGrid.SelectedIndex;
            object selectedObj = dataGrid.SelectedItem;

            if(selectedObj != null)
            {
                Person selectedPerson = (Person)dataGrid.SelectedCells[0].Item;

                var sureTest = MessageBox.Show("Usunąć kontakt: " + selectedPerson.Name + " " + selectedPerson.Surname + "?", "", MessageBoxButton.YesNo);

                if(sureTest == MessageBoxResult.Yes)
                {
                    var toDelete = persons.FirstOrDefault(p => p.Name == selectedPerson.Name && p.Surname == selectedPerson.Surname && p.Number == selectedPerson.Number && p.Email == selectedPerson.Email);
                    
                    if(toDelete!=null)
                    {
                        persons.Remove(toDelete);

                        if(dataGrid.Items.Count <= 1)
                        {
                            back_Click(sender, e);
                        }
                    }
                }

                Load();
            }
            else
            {
                MessageBox.Show("Nie wybrano kontaktu");
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if(page - 1 >= 1)
            {
                page--;
            }

            Load();
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if(persons.Count > page * pageSize)
            {
                page++;
            }

            Load();
        }
    }
}
