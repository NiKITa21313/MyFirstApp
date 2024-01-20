using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using telecommunication_company.Model;
using HashPAsswords;
using System.Configuration;

namespace telecommunication_company.Pages
{
    public partial class AddSotrudnik : Page
    {
        TeleCompModel contextDB;
        Sotrudniki sotrudniki;
        User user;
        public AddSotrudnik()
        {
            InitializeComponent();
            contextDB = TeleCompModel.GetContext();//получение контекста БД
            sotrudniki = new Sotrudniki();//таблица сотрудники
            user = new User();//таблица авторизации

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txbFamiliya.Text = "";
            txbImya.Text = "";
            txbOtchestvo.Text = "";
            txbDataRojdeniya.Text = "";
            txbDoljnost.Text = "";
            txbNomerTelefona.Text = "";
            txbAdres.Text = "";
            txbLogin.Text = "";
            txbPassword.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (txbFamiliya.Text == "" || txbImya.Text == "" || txbOtchestvo.Text == "" || txbDataRojdeniya.Text == ""
                || txbDoljnost.Text == "" || txbNomerTelefona.Text == "" || txbAdres.Text == "" || txbLogin.Text == "" || txbPassword.Text == "")
            {
                MessageBox.Show("Все поля должны быть заполнены!");
            }
            else
            {
                if (IsFIO(txbFamiliya.Text) || IsFIO(txbImya.Text) || IsFIO(txbOtchestvo.Text))
                {
                    sotrudniki.Familiya = txbFamiliya.Text;
                    sotrudniki.Imya = txbImya.Text;
                    sotrudniki.Otchestvo = txbOtchestvo.Text;
                }
                else
                {
                    MessageBox.Show("Используйте только кириллицу");
                }
                if (IsDate(txbDataRojdeniya.Text))
                {
                    DateTime date;
                    CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                    DateTime.TryParseExact(txbDataRojdeniya.Text, "yyyy-mm-dd", cultureInfo, DateTimeStyles.None, out date);
                    sotrudniki.Data_rojdeniya = date;
                }
                else
                {
                    MessageBox.Show("Используйте правильный формат yyyy-mm-dd ");
                }
                int idDoljnosti = 0;
                IsDoljnost(txbDoljnost.Text, out idDoljnosti);
                if (idDoljnosti == 0)
                {
                    MessageBox.Show("Такой должности не сушествует");
                }
                else
                {
                    sotrudniki.Doljnost = idDoljnosti;
                }
                if (IsNumberPhone(txbNomerTelefona.Text))
                {
                    sotrudniki.Nomer_telefona = txbNomerTelefona.Text;
                }
                else
                {
                    MessageBox.Show("Введите номер в формате +7**********");
                }
                if (IsAddres(txbAdres.Text))
                {
                    sotrudniki.Adres = txbAdres.Text;
                }
                else
                {
                    MessageBox.Show("Сначала введите улицу, а потом номер дома");
                }
                user.Login = txbLogin.Text;
                string inputPassword = txbPassword.Text;
                string hash = HashPassvord.HashPassword(inputPassword);//хэширование пароля
                user.Password = hash;
                contextDB.User.Add(user);
                contextDB.SaveChanges();//сохранение записи в таблице User
                sotrudniki.ID_usera = user.ID_usera;//получение ID_usera из таблицы User и запись этого ID в таблицу Sotrudniki
                contextDB.Sotrudniki.Add(sotrudniki);
                contextDB.SaveChanges();
                MessageBox.Show("Новый пользователь успешно добавлен");
                NavigationService.Navigate(new Administrator());
            }
        }
        private bool IsFIO(string str)//проверка правильности ввода ФИО
        {
            Regex formatFIO = new Regex("^[а-яА-Я]+$");
            if (!formatFIO.Match(str).Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool IsDate(string strDate)//проверка ввода даты
        {
            DateTime date;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            if (DateTime.TryParseExact(strDate, "yyyy-mm-dd", cultureInfo, DateTimeStyles.None, out date))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void IsDoljnost(string doljnost, out int id)
        {
            id = 0;
            switch (doljnost)
            {
                case "Монтажник":
                    id = 1;
                    break;
                case "Менеджер":
                    id = 2;
                    break;
                case "Продавец-консультант":
                    id = 3;
                    break;
                case "Администратор":
                    id = 4;
                    break;
                default:
                    id = 0;
                    break;
            }
        }
        public bool IsAddres(string address)//проверка ввода адреса
        {
            string addresFormat = @"[а-яА-Я*\s]*\s\d+";
            if (Regex.IsMatch(address, addresFormat))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsNumberPhone(string phoneNumber)
        {
            Regex formatNomera = new Regex("^(\\+7)+([0-9]){10}$");

            if (formatNomera.Match(phoneNumber).Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}

