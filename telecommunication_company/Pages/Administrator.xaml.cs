using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using telecommunication_company.Model;
using Excel = Microsoft.Office.Interop.Excel;

namespace telecommunication_company.Pages
{
    /// <summary>
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Page
    {
        List<Sotrudniki> sotr;//спиок сотрудников 
        /// <summary>
        /// 
        /// </summary>
        public Administrator()
        {
            InitializeComponent();
            sotr = TelecomModels.GetContext().Sotrudniki.ToList();
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
        /// <summary>
        /// <para>Обработчик двойного клика левой кнопки мыши на элемент списка</para> 
        /// <para>После двойного клика осуществляется переход на страницу редактирования записи</para>     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Обрабочтик нажатия на кнопку добавить. Переход на страницу добавления записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSotrudnik());
        }
        /// <summary>
        /// Обработчик нажатия на кнопку поиска  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPoisk_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txtbPoisk.Text.ToLower();//получение строки для поиска записи в таблице
            var sotrudnik = sotr.Where(s => s.Familiya.ToLower().Contains(searchQuery) || s.Imya.ToLower().Contains(searchQuery)).ToList();
            listSotrudniki.ItemsSource = sotrudnik;//вывод найденой записи
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SpisokSotrudnikovPDF());
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            var spisokSotrudnikov = sotr.OrderBy(p => p.Familiya).ToList();
            var excelApp = new Excel.Application();
            excelApp.SheetsInNewWorkbook = spisokSotrudnikov.Count();
            Excel.Workbook wb = excelApp.Workbooks.Add(Type.Missing);

            Excel.Worksheet ws = excelApp.Worksheets.Add(Type.Missing);
            ws.Name = "Сотрудники";
            int startIndex = 1;
            ws.Cells[1][startIndex] = "Фамилия";
            ws.Cells[2][startIndex] = "Имя";
            ws.Cells[3][startIndex] = "Отчество";
            ws.Cells[4][startIndex] = "Дата рождения";
            ws.Cells[5][startIndex] = "Должность";
            ws.Cells[6][startIndex] = "Домашний адрес";
            ws.Cells[7][startIndex] = "Номер телефона";
            ws.Cells[8][startIndex] = "Электронная почта";
            ws.Cells[9][startIndex] = "Серия паспорта";
            ws.Cells[10][startIndex] = "Номер паспорта";

            for (int i = 0; i < spisokSotrudnikov.Count; i++)
            {
                startIndex++;
                ws.Cells[1][startIndex] = spisokSotrudnikov[i].Familiya;
                ws.Cells[2][startIndex] = spisokSotrudnikov[i].Imya;
                ws.Cells[3][startIndex] = spisokSotrudnikov[i].Otchestvo;
                ws.Cells[4][startIndex] = spisokSotrudnikov[i].Data_rojdeniya;
                ws.Cells[5][startIndex] = GetDoljnost(spisokSotrudnikov[i].Doljnost);
                ws.Cells[6][startIndex] = spisokSotrudnikov[i].Adres;
                ws.Cells[7][startIndex] = spisokSotrudnikov[i].Nomer_telefona;
                ws.Cells[8][startIndex] = spisokSotrudnikov[i].Pochta;
                ws.Cells[9][startIndex] = spisokSotrudnikov[i].Pasport.seriya;
                ws.Cells[10][startIndex] = spisokSotrudnikov[i].Pasport.nomer;                
                ws.Columns.AutoFit();
            }
            excelApp.Visible = true;
        }

    }
}