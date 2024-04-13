using HashPAsswords;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using telecommunication_company.Model;

namespace telecommunication_company.Pages
{
    public partial class Autho : Page
    {
        private int countUnsuccessful = 0;//количество попыток авторизации
        private DispatcherTimer timer;
        private int count = 11;
        int id;
        string imya;
        string familiya;
        string otchestvo;
        string privetstvie;
        string codVerification;
        string sotrudnikEmail;
        /// <summary>
        /// 
        /// </summary>
        public Autho()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            txtboxCaptcha.Visibility = Visibility.Hidden;//скрытие полей капчи от пользователя
            txtBlockCaptcha.Visibility = Visibility.Hidden;
            btnCod.Visibility = Visibility.Collapsed;
        }
        
        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Guests());
        }
        /// <summary>
        /// Обработчик нажатия на кнопку "Войти" для авторизации
        /// После заполнения полей логина и пароля пользователь нажимает кнопку и авторизуется
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtbLogin.Text.Trim();
            string inpitPassword = pswbPassword.Password.Trim();
            string hashPassword = HashPassvord.HashPassword(inpitPassword);

            var user = TelecomModels.GetContext().User.Where(p => p.Login == login && p.Password == hashPassword).FirstOrDefault();//создание обхекта user,
                                                                                                                                  //который представляет запись в БД                                                                                                                         

            if (login.Length > 0 && inpitPassword.Length > 0)//проверка на заполнение логина и пароля
            {
                if (countUnsuccessful < 1)//проверка на количество попыток входа 
                {
                    if (user != null)//пользователь существует, т.е. не пустое значение
                    {
                        var s = TelecomModels.GetContext().Sotrudniki.Where(s1 => s1.ID_usera == user.ID_usera).FirstOrDefault();//получение ID должности пользователя   
                        id = s.Doljnost;
                        sotrudnikEmail = s.Pochta;
                        privetstvie = GetDate();
                        familiya = s.Familiya;
                        imya = s.Imya;
                        otchestvo = s.Otchestvo;

                        DateTime currentTime = DateTime.Now;
                        if (currentTime.TimeOfDay >= new TimeSpan(10, 0, 0) && currentTime.TimeOfDay <= new TimeSpan(19, 0, 0))
                        {
                            txtLogin.Text = "Введите пароль для подтверждения";
                            txtbLogin.Text = "";
                            codVerification = Verification();
                            btnCod.Visibility = Visibility.Visible;
                            txtPassword.Visibility = Visibility.Hidden;
                            pswbPassword.Visibility = Visibility.Hidden;
                            btnEnter.Visibility = Visibility.Hidden;
                            btnEnterGuests.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            txtbLogin.Visibility = Visibility.Hidden;//скрыть поля ввода пароля и логина
                            txtLogin.Visibility = Visibility.Hidden;
                            txtPassword.Visibility = Visibility.Hidden;
                            pswbPassword.Visibility = Visibility.Hidden;
                            btnEnter.IsEnabled = false;
                            btnEnterGuests.IsEnabled = false;
                            MessageBox.Show("Сейчас не рабочее время");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Такой учетной записи нет");
                        txtbLogin.Text = null;
                        pswbPassword.Password = null;
                        countUnsuccessful++;
                        Random rnd = new Random();
                        txtBlockCaptcha.Text = GetCaptcha(rnd.Next(3, 6));//капча
                    }
                }
                else
                {
                    txtbLogin.Visibility = Visibility.Hidden;//скрыть поля ввода пароля и логина
                    txtLogin.Visibility = Visibility.Hidden;
                    txtPassword.Visibility = Visibility.Hidden;
                    pswbPassword.Visibility = Visibility.Hidden;

                    txtboxCaptcha.Visibility = Visibility.Visible;//показать капчу пользователю
                    txtBlockCaptcha.Visibility = Visibility.Visible;
                    btnEnter.Content = "Отправить";
                    string capthaInpit = txtboxCaptcha.Text;
                    txtBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;//зачеркнутая капча

                    if (capthaInpit == txtBlockCaptcha.Text)//проверка правильного ввода текста капчи
                    {
                        btnEnter.Content = "Войти";
                        txtbLogin.Visibility = Visibility.Visible;//окрыть поля ввода пароля и логина для пользователя,
                        txtLogin.Visibility = Visibility.Visible;//если он ввел текст капчи правильно
                        txtPassword.Visibility = Visibility.Visible;
                        pswbPassword.Visibility = Visibility.Visible;
                        txtboxCaptcha.Visibility = Visibility.Hidden;
                        txtBlockCaptcha.Visibility = Visibility.Hidden;
                        countUnsuccessful = 0;//после ввода капчи количество попыток авторизации обнуляется
                        txtbLogin.Text = null;
                        pswbPassword.Password = null;
                    }
                    else
                    {
                        MessageBox.Show("Текст введен неверно");
                        Random rnd = new Random();
                        txtBlockCaptcha.Text = GetCaptcha(rnd.Next(3, 6));
                        txtboxCaptcha.Text = null;
                        countUnsuccessful++;
                        if (countUnsuccessful == 3)//блокировка полей ввода(осталось реализовать таймер)
                        {
                            txtbLogin.Visibility = Visibility.Visible;
                            txtLogin.Visibility = Visibility.Visible;
                            txtPassword.Visibility = Visibility.Visible;
                            pswbPassword.Visibility = Visibility.Visible;
                            txtboxCaptcha.Visibility = Visibility.Hidden;
                            txtBlockCaptcha.Visibility = Visibility.Hidden;
                            txtbLogin.IsEnabled = false;// Блокировать поле ввода пароля
                            pswbPassword.IsEnabled = false;
                            btnEnter.IsEnabled = false;
                            btnEnterGuests.IsEnabled = false;
                            count = 11;
                            timer.Start();// Запустить таймер
                            timerLabel.Foreground = Brushes.Red;
                            countUnsuccessful++;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Поля логина и пароля не могут быть пустыми");
            }
        }
        /// <summary>
        /// Метод для получения набора символов капчи
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Вернет строку символов указанной длины</returns>
        private string GetCaptcha(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            Random rnd = new Random();
            char[] captchaChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                captchaChars[i] = chars[rnd.Next(chars.Length)];
            }
            return new string(captchaChars);
        }
        /// <summary>
        /// Метод для перехода на опеределенную страницу, в зависимости от должности пользователя
        /// </summary>
        /// <param name="role"></param>
        /// <param name="imya"></param>
        /// <param name="familiya"></param>
        /// <param name="otchestvo"></param>
        /// <param name = "privetstvie" ></ param >
        private void LoadForm(int role, string imya, string familiya, string otchestvo, string privetstvie)//метод для перехода на страницы в зависимоти от роли, роль определяется по ID должности из таблицы
        {
            switch (role)
            {
                case 1:
                    NavigationService.Navigate(new Monatajnik(imya, familiya, otchestvo, privetstvie));
                    break;
                case 2:
                    NavigationService.Navigate(new Manager(imya, familiya, otchestvo, privetstvie));
                    break;
                case 3:
                    NavigationService.Navigate(new Konsultant(imya, familiya, otchestvo, privetstvie));
                    break;
                case 4:
                    NavigationService.Navigate(new Administrator()); 
                    break;
            }
        }
        /// <summary>
        /// Метод запускает таймер и по истечению времени открывает поля для ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            timerLabel.Text = "";
            count--;
            timerLabel.TextDecorations = TextDecorations.Underline;
            timerLabel.Text = $"До разблокировки осталось {count} секунд";

            if (count == 0)//когда счетчик станет 0, то поля для ввода откроются
            {
                timer.Stop();
                txtbLogin.IsEnabled = true;
                pswbPassword.IsEnabled = true;
                btnEnter.IsEnabled = true;
                btnEnterGuests.IsEnabled = true;
                txtbLogin.Text = null;
                pswbPassword.Password = null;
                timerLabel.Text = "";
            }
        }
        /// <summary>
        /// Метод для получения строки приветствия
        /// </summary>
        /// <returns>Вернет строку приветствия в зависимости от времени суток</returns>
        private string GetDate()//метод для получения строки приветствия в зависимости от времени
        {
            DateTime currentTime = DateTime.Now;
            if (currentTime.TimeOfDay >= new TimeSpan(10, 0, 0) && currentTime.TimeOfDay <= new TimeSpan(12, 0, 0))
            {
                return "Доброе утро";
            }
            else if (currentTime.TimeOfDay > new TimeSpan(12, 0, 0) && currentTime.TimeOfDay <= new TimeSpan(17, 0, 0))
            {
                return "Добрый день";
            }
            else if (currentTime.TimeOfDay > new TimeSpan(17, 0, 0) && currentTime.TimeOfDay <= new TimeSpan(19, 0, 0))
            {
                return "Добрый вечер";
            }
            else
            {
                return "Доброго времени суток";
            }
        }
        /// <summary>
        /// Метод для отправки кода на почту пользователя
        /// </summary>
        /// <returns>Вернет сгенерированный четырехзначный код</returns>
        private string Verification() 
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
                    mailMessage.To.Add(sotrudnikEmail);
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
        /// Обработчик нажатия на кнопку по переходу на страницу, если пароль совпадает 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCod_Click(object sender, RoutedEventArgs e)
        {
            if (txtbLogin.Text == codVerification)
            {
                LoadForm(id, imya, familiya, otchestvo, privetstvie);
            }
            else 
            {
                txtbLogin.Visibility = Visibility.Hidden;//скрыть поля ввода пароля и логина
                txtLogin.Visibility = Visibility.Hidden;
                txtPassword.Visibility = Visibility.Hidden;
                pswbPassword.Visibility = Visibility.Hidden;
                btnEnter.IsEnabled = false;
                btnEnterGuests.IsEnabled = false;
                btnCod.Visibility = Visibility.Collapsed;
                MessageBox.Show("Вы не прошли аутентификацию");
            }
        }

        private void noPassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PodtverjdenieParola());
        }
    }
}