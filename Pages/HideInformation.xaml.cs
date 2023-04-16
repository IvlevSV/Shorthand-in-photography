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
using Курсовая_работа_сокрытие_информации.Pages.Hide;

namespace Курсовая_работа_сокрытие_информации.Pages
{
    /// <summary>
    /// Логика взаимодействия для HideInformation.xaml
    /// </summary>
    public partial class HideInformation : Page
    {
        public HideInformation()
        {
            InitializeComponent();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Go_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Hide_Information(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new In_Photo());
        }
    }
}
