using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Globalization;

namespace Курсовая_работа_сокрытие_информации.Pages.Hide
{
    /// <summary>
    /// Логика взаимодействия для OutPhoto.xaml
    /// </summary>
    public partial class OutPhoto : Page
    {
        BitmapImage BmpImg;
        public OutPhoto()
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
        private void Input_Photo(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                BmpImg = new BitmapImage(new Uri(op.FileName));
                InPhoto.Source = BmpImg;
            }
        }
        private void Catch_Information(object sender, RoutedEventArgs e)
        {
            if (Input_Key.Text != "")
            {
                TextBlock.Text = "";
                //Считываем байты всех пикселей фотографии
                byte[] pixels = BitmapSourceToArray(BmpImg);
                //Получаем количество символов сообщения
                int text = Convert.ToInt32(Input_Key.Text);
                for (int i = 0; i < text; i++)
                {
                    string Char = "";
                    for (int j = 0; j < 4; j++)
                        Char = Char + OutPix(pixels[j + (i * 4)]);
                    TextBlock.Text = TextBlock.Text + (char)StrBitToByte(Char);
                }
            }
        }
        private string OutPix(byte Pix)
        {
            string res = ByteToStrBit(Pix);
            return res.Substring(res.Length - 2);
        }
        //Функция перевода из байтов в Строку битов
        string ByteToStrBit(byte Byte)
        {
            string res = Convert.ToString(Byte, 2);
            while (res.Length != 8)
                res = "0" + res;
            return res;
        }
        //Функция перевода из Строки битов в байты
        byte StrBitToByte(string Bits)
        {
            return Convert.ToByte(Encrypt(Bits), 2);
        }
        string Encrypt(string Bits)
        {
            string res = "";
            for (int i = 0; i < 8; i++)
                if (Bits[i] == '1')
                    res = res + '0';
                else
                    res = res + '1';
            return res;
        }
        private byte[] BitmapSourceToArray(BitmapSource bitmapSource)
        {
            // Stride = (width) x (bytes per pixel)
            int stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];

            bitmapSource.CopyPixels(pixels, stride, 0);

            return pixels;
        }
    }
}
