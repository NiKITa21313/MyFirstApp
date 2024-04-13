using HashPAsswords;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using telecommunication_company.Model;

namespace telecommunication_company.Pages
{
    public partial class AddSotrudnik : System.Windows.Controls.Page
    {
        TelecomModels contextDB;
        Sotrudniki sotrudnik;
        User user;
        Pasport pasport;
        /// <summary>
        /// 
        /// </summary>
        public AddSotrudnik()
        {
            InitializeComponent();
            contextDB = TelecomModels.GetContext();//получение контекста БД
            sotrudnik = new Sotrudniki();//таблица сотрудники
            user = new User();//таблица авторизации
            pasport = new Pasport();//таблица паспорт
        }
        /// <summary>
        /// Метод для очистки полей записи
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
            txbAdres.Text = "";
            txbLogin.Text = "";
            txbPassword.Text = "";
            txbPochta.Text = "";
            txbSeriya.Text = "";
            txbNomer.Text = "";
            txbVydan.Text = "";
        }
        /// <summary>
        /// Обработчик нажатия на кнопку добавить. Осуществляется добавление записи в БД 
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
            if (inputPassword.Length >= 4)
            {
                string hash = HashPassvord.HashPassword(inputPassword);
                user.Password = hash;
            }
            else
            {
                errorPassword = "Пароль обязателен и должен состоять минимум из 4 символов";
            }
            pasport.seriya = Convert.ToInt32(txbSeriya.Text);
            pasport.nomer = Convert.ToInt32(txbNomer.Text);
            pasport.Vydan = txbVydan.Text;
            var contextSotrudniki = new ValidationContext(sotrudnik);//ошибки по сотрудникам
            var contextUser = new ValidationContext(user);//ошибки по юзеру
            var contextPasport = new ValidationContext(pasport);//ошибки по паспорту
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();//явно указали ссылку на пространстов имен ибо возникает неоднозначность между пространтсвами имен БЛИНСКИЙ           
            bool res1, res2, res3;//переменные с содержанием валидации данных
            res1 = Validator.TryValidateObject(sotrudnik, contextSotrudniki, results, true);//валидация таблицы сотрудники
            res2 = Validator.TryValidateObject(user, contextUser, results, true);//валидация таблицы юзер (авторизация)
            res3 = Validator.TryValidateObject(pasport, contextPasport, results, true);//валидация таблицы паспорт
            if (res1 && res2 && res3)
            {

                //здесь надо прописать логику для формирования договора****************************************
                string zarplata = "";
                string zarplataSlovami = "";
                if (sotrudnik.Doljnost == 1) 
                {
                    zarplata = "20000";
                    zarplataSlovami = "Двадцать тысяч";
                }                    
                if (sotrudnik.Doljnost == 2)
                {
                    zarplata = "30000";
                    zarplataSlovami = "Тридцать тысяч";
                }
                if (sotrudnik.Doljnost == 3)
                {
                    zarplata = "40000";
                    zarplataSlovami = "Сорок тысяч";
                }
                if (sotrudnik.Doljnost == 4)
                {
                    zarplata = "50000";
                    zarplataSlovami = "Пятьдесят тысяч";
                }

                string imyaRabotnika = sotrudnik.Familiya + " " + sotrudnik.Imya + " "+ sotrudnik.Otchestvo;
                     
                var items = new Dictionary<string, string>()
                {
                    {"<nomerDogovora>", sotrudnik.ID_sotrudnika.ToString()},
                    {"<den>", DateTime.Today.DayOfWeek.ToString()},
                    {"<mesaz>", DateTime.Today.Month.ToString()},
                    {"<god>", DateTime.Today.Year.ToString()},
                    {"<nazvanieKompanii>", "NewCom"},
                    {"<nazvanieDoljnosti>", txbDoljnost.Text },
                    {"<imyaDirectora>", "Овчинников Никита Васильевич"},
                    {"<ispitatelnySrok>", "две недели"},
                    {"<zarplata>", zarplata},
                    {"<zarplataSlovami>", zarplataSlovami},
                    {"<inyeViplaty>", "Не предусмотрены"},
                    {"<inn>", "1456235478"},
                    {"<adres>", "Новосибирск ул. Фадеева д. 66/5 кв. 1"},
                    {"<imyaRabotnika>", imyaRabotnika},
                    {"<seriya>", pasport.seriya.ToString()},
                    {"<nomer>", pasport.nomer.ToString()},
                    {"<kemVydan>", pasport.Vydan}
                };

                Microsoft.Office.Interop.Word.Application wordApp = null;
                Document wordDoc;

                try
                {
                    wordApp = new Microsoft.Office.Interop.Word.Application();

                    object missing = Type.Missing;
                    object fileName = "D:\\Проекты C#\\telecommunication_company\\telecommunication_company\\Files\\Dogovor.docx"; //Путь к шаблону документа

                    wordDoc = wordApp.Documents.Open(ref fileName, ref missing, ref missing, ref missing); //открываем шаблон документа

                    foreach (var item in items) // Перебор всех тегов и значений словаря, с последующей
                                                // заменой каждого тега на соответствующее для него значение
                    {
                        Find find = wordApp.Selection.Find;
                        find.Text = item.Key;
                        find.Replacement.Text = item.Value;

                        object wrap = WdFindWrap.wdFindContinue;
                        object replace = WdReplace.wdReplaceAll;

                        find.Execute(FindText: Type.Missing,
                            MatchCase: false, MatchWholeWord: false, MatchWildcards: false,
                            MatchSoundsLike: missing, MatchAllWordForms: false, Forward: true,
                            Wrap: wrap, Format: false, ReplaceWith: missing, Replace: replace);
                    }
                    // путь по которому будет сохранен файла и имя файла - сохранять на рабочий стол текущего пользователя (или выбор места сохранения через диалоговое окно)                  
                    object newFile = "C:\\Users\\Никита\\Desktop\\Договоры\\NewDogovor.docx";
                    wordDoc.SaveAs2(newFile); //сохранить заполненный данными шаблон как новый документ
                    wordApp.ActiveDocument.Close(); //закрытие активного документа
                    wordApp?.Quit(); //отключение от приложения для работы с документами типа Word
                }
                catch (Exception ex)
                {
                    wordApp.ActiveDocument.Close(); //закрытие активного документа
                    wordApp?.Quit();
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }

                contextDB.User.Add(user);//добавление юзера 
                contextDB.SaveChanges();//сохранение записи в таблице User, чтобы получить ID_usera и поставить его в таблицу Sotrudniki
                sotrudnik.ID_usera = user.ID_usera;//получение ID_usera из таблицы User и запись этого ID в таблицу Sotrudniki
                contextDB.Pasport.Add(pasport);
                contextDB.SaveChanges();
                sotrudnik.id_pasporta = pasport.id_pasporta;//добавление паспорта в таблицу Sotrudniki
                contextDB.Sotrudniki.Add(sotrudnik);//добавление сотрудника
                contextDB.SaveChanges();//сохранение записи в таблице Sotrudniki

                System.Windows.Forms.MessageBox.Show("Новый пользователь успешно добавлен");
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
                System.Windows.Forms.MessageBox.Show(errorMessage);
                errorPassword = "";
            }
        }

        /// <summary>
        /// Метод для получения кода должности из БД
        /// </summary>
        /// <param name="doljnost"></param>
        /// <returns>Вернет код должности, если должности нет вернет 0</returns>
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

