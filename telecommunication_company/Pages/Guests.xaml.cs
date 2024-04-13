using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using telecommunication_company.Model;

namespace telecommunication_company.Pages
{
    /// <summary>
    /// Логика взаимодействия для Guests.xaml
    /// </summary>
    public partial class Guests : Page
    {
        List<Oborudovanie> oborudovanieList;
        /// <summary>
        /// 
        /// </summary>
        public Guests()
        {
            InitializeComponent();
            oborudovanieList = TelecomModels.GetContext().Oborudovanie.ToList();
            LViewOborudovanie.ItemsSource = oborudovanieList;
        }

        private void btnSavePDF_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idp = flowDoc;
                pd.PrintDocument(idp.DocumentPaginator, Title); 
            }
        }
    }
}
