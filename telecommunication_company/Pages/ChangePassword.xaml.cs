using HashPAsswords;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using telecommunication_company.Model;

namespace telecommunication_company.Pages
{
    public partial class ChangePassword : Page
    {
        User user;
        Sotrudniki sotrudnik;
        TelecomModels context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        public ChangePassword(string email)
        {
            InitializeComponent();
            context = TelecomModels.GetContext();//получение контекста БД
            sotrudnik = TelecomModels.GetContext().Sotrudniki.Where(p => p.Pochta == email).FirstOrDefault();//получение записи БД совпадающей с введенной почтой
            user = TelecomModels.GetContext().User.Where(i => i.ID_usera == sotrudnik.ID_usera).FirstOrDefault();
        }
        /// <summary>
        /// Обработчик нажатия на кнопку смены пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (startPassword.Password == finishPassword.Password)
            {
                string password = finishPassword.Password;
                if (password.Length > 3)
                {
                    string hashPassword = HashPassvord.HashPassword(password);
                    user.Password = hashPassword;

                    context.Entry(user).State = EntityState.Modified;//редактирование таблицы User
                    context.SaveChanges();
                    MessageBox.Show("Пароль успешно изменен");
                    NavigationService.Navigate(new Autho());
                }
                else
                {
                    MessageBox.Show("Пароль должен состоять минимум из 4 символов");
                }
            }
            else
            {
                startPassword.Password = "";
                finishPassword.Password = "";
                MessageBox.Show("Пароли не совпадают, попробуйте заново");
            }
        }
    }
}
