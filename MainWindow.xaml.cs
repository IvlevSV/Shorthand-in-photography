using System.Windows;
using Курсовая_работа_сокрытие_информации.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Курсовая_работа_сокрытие_информации
{
    public partial class MainWindow : Window
    {
        public static MainWindow Window;
        public MainWindow()
        {
            InitializeComponent();
            frame.Navigate(new MainPage());
            Window = this;
        }
        // перемещение приложения
        private void MoveWindow(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                Window.DragMove();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}