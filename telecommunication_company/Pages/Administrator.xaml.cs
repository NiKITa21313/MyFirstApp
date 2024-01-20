using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using telecommunication_company.Model;

namespace telecommunication_company.Pages
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Page
    {
        List<Sotrudniki> sotr;
        public Administrator()
        {
            InitializeComponent();
            sotr = TeleCompModel.GetContext().Sotrudniki.ToList();
            listSotrudniki.ItemsSource = sotr;
        }

        private string GetDoljnost(int idDoljnosti) 
        {
            string doljnost = "";
            switch (idDoljnosti) 
            { 
                case 1:
                    doljnost = "Монтажник";
                    break;
                case 2:
                    doljnost = "Менеджер";
                    break;
                case 3:
                    doljnost = "Продавец-консультант";
                    break;  
                case 4:
                    doljnost = "Администратор";
                    break;
            }
            return doljnost;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                int index = listSotrudniki.SelectedIndex;
                int id = sotr[index].ID_sotrudnika;
                NavigationService.Navigate(new Redactirovanie(id));
            }
            
        }

        private void LViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LViewProduct_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSotrudnik());
        }

        private void btnPoisk_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txtbPoisk.Text.ToLower();//получение строки для поиска записи в таблице
            var sotrudnik = sotr.Where(s => s.Familiya.ToLower().Contains(searchQuery) || s.Imya.ToLower().Contains(searchQuery)).ToList();
            listSotrudniki.ItemsSource = sotrudnik;//вывод найденой записи
        }
    }
}