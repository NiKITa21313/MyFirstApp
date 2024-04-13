using HashPAsswords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using telecommunication_company.Model;
using System.Security;

namespace telecommunication_company.Pages
{
    public partial class PodtverjdenieParola : Page
    {
        TelecomModels telecomModel;
        string emailPassword;
        string sotrudnikEmail;
        /// <summary>
        /// 
        /// </summary>
        public PodtverjdenieParola()
        {
            InitializeComponent();
            telecomModel = TelecomModels.GetContext();
            enterCod.Visibility = Visibility.Collapsed;
            text.Visibility = Visibility.Collapsed;
            EnterCod.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Обработчик нажатия на кнопку отправки четырехзначного кода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            sotrudnikEmail = enterEmail.Text;
            var sotrudnik = TelecomModels.GetContext().Sotrudniki.Where(p => p.Pochta == sotrudnikEmail).FirstOrDefault();
            if (sotrudnik != null)
            {
                enterCod.Visibility = Visibility.Visible;
                text.Visibility = Visibility.Visible;
                EnterCod.Visibility = Visibility.Visible;
                Enter.Visibility = Visibility.Collapsed;
                emailPassword = VerificationEmail(sotrudnikEmail);             
            }
            else 
            {
                MessageBox.Show("Проверьте правильность ввода почты");
            }
        }
        /// <summary>
        /// Метод для отправки четырехзначного кода на почту пользователя
        /// </summary>
        /// <param name="sotrudnikEm"></param>
        /// <returns>Вернет четырехзначный код</returns>
        private string VerificationEmail(string sotrudnikEm) 
        {
            Random random = new Random();
            int codEmail = random.Next(1000, 9999);
            string smtpServer = "smtp.mail.ru";
            int smtpPort = 587;
            string smtpUsername = "verification67@mail.ru";//почта, которая будет отправлять пароль
            string smtpPassword = "nh3Drm5LhgLUJEAig0gc";//пароль от этой почты

            using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = true;

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(smtpUsername);
                    mailMessage.To.Add(sotrudnikEm);
                    mailMessage.Subject = "Пароль для подтверждения";
                    mailMessage.Body = $"Введите даннй код для входа: {codEmail}";
                    try
                    {
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            return codEmail.ToString();
        }
        /// <summary>
        /// Метод для подтверждения отправленного кода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterCod_Click(object sender, RoutedEventArgs e)
        {
            if(emailPassword == enterCod.Text.ToString())
            {
                NavigationService.Navigate(new ChangePassword(sotrudnikEmail));
            }
            else 
            {
                MessageBox.Show("Неправильный код");
            }
        }
    }
}
