using System;
using System.Collections.Generic;
using System.Linq;
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

namespace telecommunication_company.Pages
{
    /// <summary>
    /// Логика взаимодействия для SpisokSotrudnikovPDF.xaml
    /// </summary>
    public partial class SpisokSotrudnikovPDF : Page
    {
        List<Sotrudniki> sotr;
        /// <summary>
        /// 
        /// </summary>
        public SpisokSotrudnikovPDF()
        {
            InitializeComponent();
            sotr = TelecomModels.GetContext().Sotrudniki.ToList();
            LViewSotrudniki.ItemsSource = sotr;        
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
