using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using telecommunication_company.Model;
using static System.Windows.Forms.AxHost;
using System.Data.Entity;

namespace telecommunication_company.Pages
{

    public partial class Redactirovanie : Page
    {
        public Sotrudniki sotrudniki;
        public TeleCompModel context;
        public Redactirovanie(int id)
        {
            InitializeComponent();
            context = TeleCompModel.GetContext();//получение контекста БД
            sotrudniki = TeleCompModel.GetContext().Sotrudniki.Where(i => i.ID_sotrudnika == id).FirstOrDefault();
            //это просто вывод информации из таблицы БД в поля на странице редакирования
            txbFamiliya.Text = sotrudniki.Familiya;
            txbImya.Text = sotrudniki.Imya;
            txbOtchestvo.Text = sotrudniki.Otchestvo;
            DateTime date = (DateTime)sotrudniki.Data_rojdeniya;
            txbDataRojdeniya.Text = date.ToString("yyyy-MM-dd");            
            txbDoljnost.Text = GetDoljnost(sotrudniki);//вывод должности
            string nomer = sotrudniki.Nomer_telefona;
            txbNomerTelefona.Text = nomer.Substring(0, 12);
            txbAdres.Text = sotrudniki.Adres;

        }
        private string GetDoljnost(Sotrudniki s)
        {
            string doljnost = "";
            if (s.Doljnost == 1) doljnost = "Монтажник";
            if (s.Doljnost == 2) doljnost = "Менеджер";
            if (s.Doljnost == 3) doljnost = "Продавец-консультант";
            if (s.Doljnost == 4) doljnost = "Администратор";
            return doljnost;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txbFamiliya.Text = String.Empty;
            txbImya.Text = String.Empty;
            txbOtchestvo.Text = String.Empty;
            txbDataRojdeniya.Text = String.Empty;
            txbDoljnost.Text = String.Empty;
            txbNomerTelefona.Text = String.Empty;
            txbAdres.Text = String.Empty;
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            if (txbFamiliya.Text == "" || txbImya.Text == "" || txbOtchestvo.Text == "" || txbDataRojdeniya.Text == "" 
                || txbDoljnost.Text == "" || txbNomerTelefona.Text == "" || txbAdres.Text == "") 
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
                IsDoljnost(txbDoljnost.Text,  out idDoljnosti);
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
                context.Entry(sotrudniki).State = EntityState.Modified;
                context.SaveChanges();
                MessageBox.Show("Изменение произошло успешно");
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
