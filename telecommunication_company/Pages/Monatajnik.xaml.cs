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
    
    public partial class Monatajnik : Page
    {
        public Monatajnik(string imya, string familiya, string otchestvo, string privetstvie)//принимает 3 параметра, чтобы заполнить стороку на странице
        { 
            InitializeComponent();
            txtbPrivet.Text = $"{privetstvie} {familiya} {imya} {otchestvo}";
        }
    }
}
