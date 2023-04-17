using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace KsiazkaWPF
{
    /// <summary>
    /// Logika interakcji dla klasy OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public Person person = new Person();

        public OptionsWindow()
        {
            InitializeComponent();
        }

        public OptionsWindow(string name, string surname, string number, string email)
        {
            InitializeComponent();

            this.tbname.Text = name;
            this.tbsurname.Text = surname;
            this.tbnumber.Text = number;
            this.tbemail.Text = email;
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbname.BorderBrush = Brushes.Gray;

            var text = (TextBox)sender;
            Regex regex = new Regex("^[A-Za-z]+$");

            if (!regex.IsMatch(text.Text))
            {
                int startIndex = text.Text.Length - 1;
                if (startIndex >= 0)
                {
                    text.Text = text.Text.Remove(startIndex);
                }
            }
        }

        private void surname_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbsurname.BorderBrush = Brushes.Gray;

            var text = (TextBox)sender;
            Regex regex = new Regex("^[A-Za-z]+$");

            if (!regex.IsMatch(text.Text))
            {
                int startIndex = text.Text.Length - 1;
                if (startIndex >= 0)
                {
                    text.Text = text.Text.Remove(startIndex);
                }
            }
        }

        private void number_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbnumber.BorderBrush = Brushes.Gray;

            var text = (TextBox)sender;
            Regex regex = new Regex("^[0-9]+$");

            if (!regex.IsMatch(text.Text))
            {
                int startIndex = text.Text.Length - 1;
                if (startIndex >= 0)
                {
                    text.Text = text.Text.Remove(startIndex);
                }
            }
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbemail.BorderBrush = Brushes.Gray;
        }

        private bool RegexVal_Email(string email)
        {
            bool valide = false;

            if (Regex.IsMatch(email, @"^[A-Za-z\-]+[A-Za-z0-9]*@[A-Za-z0-9]+\.[a-z]+$"))
            {
                valide = true;
            }
            else
            {
                valide = false;
            }

            return valide;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (tbname.Text == "" || tbname.Text == " " || tbsurname.Text == "" || tbsurname.Text == " " || tbnumber.Text == "" || tbnumber.Text == " " || tbemail.Text == "" || tbemail.Text == " " || RegexVal_Email(tbemail.Text) == false)
            {
                MessageBox.Show("Błędne dane");
                if (tbname.Text == "" || tbname.Text == " ")
                {
                    tbname.BorderBrush = Brushes.HotPink;
                }

                if (tbsurname.Text == "" || tbsurname.Text == " ")
                {
                    tbsurname.BorderBrush = Brushes.HotPink;
                }

                if (tbnumber.Text == "" || tbnumber.Text == " ")
                {
                    tbnumber.BorderBrush = Brushes.HotPink;
                }

                if (tbemail.Text == "" || tbemail.Text == " " || RegexVal_Email(tbemail.Text) == false)
                {
                    tbemail.BorderBrush = Brushes.HotPink;
                }
            }
            else
            {
                person.makePerson(tbname.Text, tbsurname.Text, tbnumber.Text, tbemail.Text);
                this.Close();
            }
        }
    }
}
