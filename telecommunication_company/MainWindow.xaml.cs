using System;
using System.Windows;
using telecommunication_company.Pages;

namespace telecommunication_company
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            FrmMain.Navigate(new Autho());
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrmMain.GoBack();
        }
        private void FrmMain_ContentRendered(object sender, EventArgs e) 
        {
            if (FrmMain.CanGoBack)
            {
                btnBack.Visibility = Visibility.Visible;
            }
            else
            {
                btnBack.Visibility = Visibility.Hidden;
            }
        }
        private void FrmMain_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
