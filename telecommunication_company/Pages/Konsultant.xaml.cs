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

namespace telecommunication_company.Pages
{
    /// <summary>
    /// Логика взаимодействия для Konsultant.xaml
    /// </summary>
    public partial class Konsultant : Page
    {
        public Konsultant(string imya, string familiya, string otchestvo, string privetstvie)
        {
            InitializeComponent();

            txtbPrivet.Text = $"{privetstvie} {familiya} {imya} {otchestvo}";
        }
    }
}
