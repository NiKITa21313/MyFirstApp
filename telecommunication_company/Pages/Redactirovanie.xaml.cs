using HashPAsswords;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using telecommunication_company.Model;

namespace telecommunication_company.Pages
{

    public partial class Redactirovanie : Page
    {
        TelecomModels context;
        Sotrudniki sotrudnik;
        User user;
        Pasport pasport;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Redactirovanie(int id)
        {
            InitializeComponent();

            context = TelecomModels.GetContext();//получение контекста БД
            sotrudnik = TelecomModels.GetContext().Sotrudniki.Where(i => i.ID_sotrudnika == id).FirstOrDefault();
            user = TelecomModels.GetContext().User.Where(i => i.ID_usera == sotrudnik.ID_usera).FirstOrDefault();
            pasport = context.Pasport.Where(p => p.id_pasporta == sotrudnik.id_pasporta).FirstOrDefault();
            //это просто вывод информации из таблицы БД в поля на странице редакирования
            txbFamiliya.Text = sotrudnik.Familiya;
            txbImya.Text = sotrudnik.Imya;
            txbOtchestvo.Text = sotrudnik.Otchestvo;
            DateTime date = (DateTime)sotrudnik.Data_rojdeniya;
            txbDataRojdeniya.Text = date.ToString("yyyy-MM-dd");            
            txbDoljnost.Text = GetDoljnost(sotrudnik);//вывод должности
            string nomer = sotrudnik.Nomer_telefona;
            txbNomerTelefona.Text = nomer.Substring(0, 12);
            txbAdres.Text = sotrudnik.Adres;
            txbLogin.Text = user.Login;
            txbPochta.Text = sotrudnik.Pochta;
            txbSeriya.Text = pasport.seriya.ToString();
            txbNomer.Text = pasport.nomer.ToString();
            txbVydan.Text = pasport.Vydan;

            
        }
        /// <summary>
        /// Метод для полчения строки должности в зависимости от кода
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Вернет строку название должности</returns>
        private string GetDoljnost(Sotrudniki s)
        {
            string doljnost = "";
            if (s.Doljnost == 1) doljnost = "Монтажник";
            if (s.Doljnost == 2) doljnost = "Менеджер";
            if (s.Doljnost == 3) doljnost = "Продавец-консультант";
            if (s.Doljnost == 4) doljnost = "Администратор";
            return doljnost;
        }
        /// <summary>
        /// Обработчик нажатия на кнопку очистить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txbFamiliya.Text = "";
            txbImya.Text = "";
            txbOtchestvo.Text = "";
            txbDataRojdeniya.Text = "";
            txbDoljnost.Text = "";
            txbNomerTelefona.Text = "";
            txbPochta.Text = "";
            txbAdres.Text = "";
        }
        /// <summary>
        /// Обработчик нажатия на кнопку редактировать. При нажатии запись в БД будет изменена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string errorPassword = "";
            sotrudnik.Familiya = txbFamiliya.Text;
            sotrudnik.Imya = txbImya.Text;
            sotrudnik.Otchestvo = txbOtchestvo.Text;
            DateTime date;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            if (DateTime.TryParseExact(txbDataRojdeniya.Text, "yyyy-mm-dd", cultureInfo, DateTimeStyles.None, out date))
            {
                sotrudnik.Data_rojdeniya = date;
            }
            if (Doljnost(txbDoljnost.Text) != 0) sotrudnik.Doljnost = Doljnost(txbDoljnost.Text);
            sotrudnik.Nomer_telefona = txbNomerTelefona.Text;
            sotrudnik.Pochta = txbPochta.Text;
            sotrudnik.Adres = txbAdres.Text;
            user.Login = txbLogin.Text;
            string inputPassword = txbPassword.Text;
            if (inputPassword.Length != 0)
            {
                string hash = HashPassvord.HashPassword(inputPassword);//хэширование пароля
                user.Password = hash;
            }
            else
            {
                //пароль не записан
            }
            pasport.seriya = Convert.ToInt32(txbSeriya.Text);
            pasport.nomer = Convert.ToInt32(txbNomer.Text);
            pasport.Vydan = txbVydan.Text;
            var contextSotrudniki = new ValidationContext(sotrudnik);//ошибки по сотрудникам
            var contextUser = new ValidationContext(user);//ошибки по юзеру
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();//явно указали ссылку на пространстов имен ибо возникает неоднозначность между пространтсвами имен БЛИН           
            bool res1, res2;//переменные с содержанием валидации данных
            res1 = Validator.TryValidateObject(sotrudnik, contextSotrudniki, results, true);//валидация таблицы сотрудники
            res2 = Validator.TryValidateObject(user, contextUser, results, true);//валидация таблицы юзер(авторизация)
            if (res1 && res2)
            {
                context.Entry(user).State = EntityState.Modified;//редактирование таблицы User
                context.Entry(sotrudnik).State = EntityState.Modified;//редактирование таблицы Sotrudniki
                context.Entry(pasport).State = EntityState.Modified;                                                      
                context.SaveChanges();
                MessageBox.Show("Данные пользователя успешно измненены");
                NavigationService.Navigate(new Administrator());
            }
            else
            {
                string errorMessage = "Ошибки валидации:\n";
                foreach (var error in results)
                {
                    errorMessage += error.ErrorMessage + "\n";
                }
                errorMessage += errorPassword;
                MessageBox.Show(errorMessage);
                errorPassword = "";
            }
        }
        /// <summary>
        /// Метод для получения кода должности
        /// </summary>
        /// <param name="doljnost"></param>
        /// <returns>Вернет код должности</returns>
        private int Doljnost(string doljnost)
        {
            if (doljnost == "Монтажник") return 1;
            else if (doljnost == "Менеджер") return 2;
            else if (doljnost == "Продавец-консультант") return 3;
            else if (doljnost == "Администратор") return 4;
            else return 0;
        }
    }
}
