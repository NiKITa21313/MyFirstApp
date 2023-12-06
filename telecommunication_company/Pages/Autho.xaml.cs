using HashPAsswords;
using System;
using System.Linq;
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
        private int countUnsuccessful = 0;
        private DispatcherTimer timer;
        private int countdown = 10;


        public Autho()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            txtboxCaptcha.Visibility = Visibility.Hidden;//скрытие полей
            txtBlockCaptcha.Visibility = Visibility.Hidden;//капчи от пользователя
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Guests());
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtbLogin.Text.Trim();
            string inpitPassword = pswbPassword.Password.Trim();
            string hashPassword = HashPassvord.HashPassword(inpitPassword);
 
            var user = TeleCompModel.GetContext().User.Where(p => p.Login == login && p.Password == hashPassword).FirstOrDefault();
            //int userCount = TeleCompModel.GetContext().User.Where(p => p.Login == login && p.Password == hashPassword).Count(); по сути ненужная строка

            if (login.Length > 0 && inpitPassword.Length > 0)//проверка на заполнение логина и пароля
            {
                if (countUnsuccessful < 1)//проверка на количество попыток входа 
                {
                    if (user != null)//пользователь существует, т.е. не пустое значение
                    {
                        var s = TeleCompModel.GetContext().Sotrudniki.Where(s1 => s1.ID_usera == user.ID_usera).FirstOrDefault();//получение ID должности
                        int id = s.Doljnost;
                        LoadForm(id);
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
                    txtBlockCaptcha.Visibility = Visibility.Visible;//...

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
                        Random rnd = new Random();
                        txtBlockCaptcha.Text = GetCaptcha(rnd.Next(3, 6));
                        MessageBox.Show("Текст введен неверно");
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
                            // Блокировать поле ввода пароля
                            txtbLogin.IsEnabled = false;
                            pswbPassword.IsEnabled = false;
                            countdown = 10;
                            timerLabel.Content = countdown.ToString();
                            // Запустить таймер
                            timer.Start(); 
                            timerLabel.Foreground = Brushes.Red;
                            timerLabel.Content = "10"; // Начальное значение таймера
                            countUnsuccessful = 0;
                        }
                    }
                }
            }
            else 
            {
                MessageBox.Show("Поля логина и пароля не могут быть пустыми");
            }
        }
        private string GetCaptcha(int length) 
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";         
            Random rnd = new Random();
            char[]captchaChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                captchaChars[i] = chars[rnd.Next(chars.Length)];
            }
            return new string (captchaChars);
        }
        private void LoadForm(int role)//метод для перехода на страницы в зависимоти от роли, роль определяется по ID должности из таблицы
        {   
            switch (role) 
            {
                case 1:
                    NavigationService.Navigate(new Monatajnik());
                    break;
                case 2:
                    NavigationService.Navigate(new Manager());
                    break;
                case 3:
                    NavigationService.Navigate(new Konsultant());
                    break;
            }
                
        }
        private void Timer_Tick(object sender, EventArgs e)//разблокирование полей ввода
        {       
            timerLabel.Content = ""; // Очистить значение таймера

            countdown--;

            timerLabel.Content = countdown.ToString();
            
            if (countdown == 0)//когда счетчик станет 0, то поля для ввода откроются
            {
                timer.Stop();
                txtbLogin.IsEnabled = true;
                pswbPassword.IsEnabled = true;
                txtbLogin.Text = null;
                pswbPassword.Password = null;
                timerLabel.Content = "";
            }
        }
    } 
}
