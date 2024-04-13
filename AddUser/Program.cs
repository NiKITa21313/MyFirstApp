using AddUser.Model;
using HashPAsswords;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;

namespace AddUser
{
    public class Program
    {
        /// <summary>
        /// В результате работы данного метода будет добавлена новая запиь в таблицу базы данных
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            try
            {
                telekommunikationEntities db = Helper.GetContext();
                Helper helper = new Helper();
                bool write = true;
                while (write)
                {
                    Sotrudniki sotrudnik = new Sotrudniki();
                    User user = new User();

                    Console.WriteLine("Создание новой учетной записи для пользователя\n");

                    Console.Write("Введите имя пользователя: ");
                    sotrudnik.Imya = IsFIO();

                    sotrudnik.Imya = Console.ReadLine();

                    Console.Write("Введите фамилию пользователя: ");
                    sotrudnik.Familiya = IsFIO();
                    Console.Write("Введите отчество пользователя: ");
                    sotrudnik.Otchestvo = IsFIO();

                    Console.Write("Введите дату рождения в формате YYYY-MM-DD: ");
                    sotrudnik.Data_rojdeniya = IsDate();

                    Console.Write("Введите должность пользователя: ");
                    RightDoljnost(db, sotrudnik);
                    
                    Console.Write("Введите адрес проживания: ");
                    sotrudnik.Adres = IsAddres();

                    Console.Write("Введите номер телефона: ");
                    sotrudnik.Nomer_telefona = IsNumberTelephone();

                    Console.Write("Введите логин пользователя: ");
                    string login = Console.ReadLine();//логин может быть любым, но состоять из букв и цифр
                    LengthLogin(login);
                    user.Login = login;

                    Console.Write("Введите пароль пользователя: ");//пароль может состоять из любых символов, но не должен быть пустым
                    string password = Console.ReadLine();
                    LengthPassword(password);
                    string hashPassword = HashPassvord.HashPassword(password);
                    Console.Write("Хэшированный пароль пользователя:" + hashPassword);
                    user.Password = hashPassword;//запись хэшированнного пароля в БД
                    
                    helper.CreateUsers(user);//сохранение User-а
                    sotrudnik.ID_usera = user.ID_usera;//запись ID_usera из таблицы User в таблицу Sotrudniki
                    helper.CreateSotrudnik(sotrudnik);

                    Console.Write("\nУчетная запись добалена!\n");
                    Console.WriteLine("Чтобы продолжить нажмите Enter, чтобы завершить введите слово <конец>");
                    string where = Console.ReadLine();
                    if (where.ToLower() == "конец") write = false;
                    else write = true;
                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Метод принимает строку и проверяет подходит ли она под регулярное выражение
        /// </summary>
        /// <returns>Вернет строку</returns>
        public static string IsFIO()
        {
            Regex formatFIO = new Regex("^[а-яА-Я]+$");
            string str;
            do
            {
                str = Console.ReadLine();
                if (formatFIO.Match(str).Success)
                {
                    Console.WriteLine("Шикарно, вы справились");
                    return str;
                }
                else
                {
                    MessageBox.Show("Используйте кириллицу, без дополнительных символов!!!");
                }
            }
            while (true);
            
        }
        /// <summary>
        /// Метод проверяет правильность ввода даты по формату
        /// </summary>
        /// <returns>Вернет дату</returns>
        public static DateTime IsDate()
        {
            DateTime date;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            do
            {
                string dateInput = Console.ReadLine();
                if (DateTime.TryParseExact(dateInput, "yyyy-mm-dd", cultureInfo, DateTimeStyles.None, out date))
                {
                    break;
                }
                else
                {
                    MessageBox.Show("Используйте правильный формат yyyy-mm-dd ");
                }
            }
            while (true);
            Console.WriteLine("Шикарно, вы справились и с этим)");
            return date;
        }
        /// <summary>
        /// Метод проверяет правильность ввода адреса(за названием улицы должен следовать номер дома)
        /// </summary>
        /// <returns>Вернет строку адреса</returns>
        public static string IsAddres()
        {
            string addresFormat = @"[а-яА-Я*\s]*\s\d+";
            string addres;
            do
            {
                addres = Console.ReadLine();
                if (Regex.IsMatch(addres, addresFormat))
                {
                    break;
                }
                else
                {
                    MessageBox.Show("Сначала введите улицу, а затем номер дома!!!");
                }
            }
            while (true);
            Console.WriteLine("Просто бомба, вы и это смогли:)");
            return addres;
        }
        /// <summary>
        /// Метод проверяет правильность ввода номера телефона
        /// </summary>
        /// <returns>вернет строку номер телефона в формате +71231234334</returns>
        public static string IsNumberTelephone()
        {
            Regex formatNomera = new Regex("^(\\+7)+([0-9]){10}$");
            string nomer;
            do
            {
                nomer = Console.ReadLine();
                if (formatNomera.Match(nomer).Success)
                {
                    Console.WriteLine("Да вам все под силу я смотрю;)");
                    return nomer;
                }
                else
                {
                    MessageBox.Show("Вводите номер в формате +7**********");
                }
            }
            while (true);
        }
        public static string IsLogin()
        {
            Regex formatLogina = new Regex("^[а-яА-Я0-9]$");
            string login;
            do
            {
                login = Console.ReadLine();
                if (formatLogina.Match(login).Success)
                {

                    break;
                }
                else
                {
                    MessageBox.Show("Используйте буквы и цифры");
                }
            }
            while (true);
            Console.WriteLine("Это невероятно");
            return login;
        }
        /// <summary>
        /// Метод проверяет наличие должности в базе данных
        /// </summary>
        /// <param name="db"></param>
        /// <param name="sotrudnik"></param>
        public static void RightDoljnost(telekommunikationEntities db, Sotrudniki sotrudnik) //проверка на наличие должности в БД
        {
            do
            {
                string doljnostPosition = Console.ReadLine();
                var doljnost = db.Doljnost.FirstOrDefault(p => p.Nazvanie == doljnostPosition);
                if (doljnost != null)
                {
                    MessageBox.Show("Должность найдена");
                    sotrudnik.Doljnost = doljnost.ID_doljnosti;
                    break;
                }
                else
                {
                    MessageBox.Show("Должность не найдена");
                }
            } while (true);
        }
        public static void LengthLogin(string login)
        {
            if (login == null || login.Length < 5)
            {
                MessageBox.Show("Логин должен состоять минимум из 5 символов");
                return;
            }
        }
        public static void LengthPassword(string password)
        {
            if (password == null || password.Length < 5)
            {
                MessageBox.Show("Пароль должен состоять минимум из 5 символов");
                return;
            }
        }
    }

}
