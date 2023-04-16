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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace Курсовая_работа_сокрытие_информации.Pages.Hide
{
    /// <summary>
    /// Логика взаимодействия для In_Photo.xaml
    /// </summary>
    public partial class In_Photo : Page
    {
        BitmapImage BmpImg;
        BitmapSource BitMapOut;
        int Pix;
        Uri uri;
        public In_Photo()
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
                uri = new Uri(op.FileName);
                BmpImg = new BitmapImage(uri);
                InPhoto.Source = BmpImg;
                Pix = BmpImg.PixelHeight * BmpImg.PixelWidth;
                CountChar.Text = "Оставшееся количество символов: " + Convert.ToString(Pix);
                TextBlock.Visibility = Visibility.Visible;
                TextBlock.MaxLength = Pix;
            }
        }
        private void Input_Information(object sender, RoutedEventArgs e)
        {
            if (TextBlock.Text != "")
            {
                //Считываем байты всех пикселей фотографии
                byte[] pixels = BitmapSourceToArray(BmpImg);
                //Получаем строку которую необходимо поместить
                string text = TextBlock.Text;
                //Переменная символа строки
                char Ch;
                //Переменная бит представления символа строки
                string BitChar;
                //цикл идущий по строке
                for (int i = 0; i < text.Length; i++)
                {
                    //Получаем символ из строки
                    Ch = text[i];
                    //Получаем бит представление символа строки
                    BitChar = Encrypt(ByteToStrBit((byte)Ch));
                    //цикл идущий по одному пиксилю 
                    for (int j = 0; j < 4; j++)
                        //помещаем в пиксиль букву
                        pixels[j + (i * 4)] = InPix(BitChar[j*2], BitChar[(j * 2) + 1], pixels[j + (i * 4)]);
                } 
                //получаем несжатое растровое изображение из представления байтов
                BitMapOut = BitmapSourceFromArray(pixels, BmpImg.PixelWidth, BmpImg.PixelHeight);
                //помещаем полученное фото в рамку
                OutPhoto.Source = BitMapOut;
                TextBlock.Text = TextBlock.Text + " " + TextBlock.Text.Length;
            }
        }
        //Функция перевода из байтов в Строку битов
        string ByteToStrBit(byte Byte)
        {
            string res = Convert.ToString(Byte, 2);
            while (res.Length != 8)
                res = "0" + res;
            return res;
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
        //Функция перевода из Строки битов в байты
        byte StrBitToByte(string Bits)
        {
            return Convert.ToByte(Bits, 2);
        }
        private byte InPix(char FirstBit, char SecondBit, byte Pix)
        {
            //MessageBox.Show(Convert.ToString(Pix));
            //Помещаем биты в 
            char[] InsChar = { FirstBit, SecondBit };
            string Insert = new string(InsChar);
            string BitPix = ByteToStrBit(Pix).Insert(6, Insert).Remove(8);
            byte res = StrBitToByte(BitPix);
            //MessageBox.Show(Convert.ToString(res));
            return res;
        }

        private void Download(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save picture as ";
            save.Filter = "Image Files(*.png;)|*.png;";
            if (BitMapOut != null)
            {
                if (save.ShowDialog() == true)
                {
                    using (var fileStream = new FileStream(save.FileName, FileMode.Create))
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(BitMapOut));
                        encoder.Save(fileStream);
                    }
                }
            }
        }
        private byte[] BitmapSourceToArray(BitmapSource bitmapSource)
        {
            // Stride = (width) x (bytes per pixel)
            int stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel / 8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];

            bitmapSource.CopyPixels(pixels, stride, 0);

            return pixels;
        }
        private BitmapSource BitmapSourceFromArray(byte[] pixels, int width, int height)
        {
            WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * (bitmap.Format.BitsPerPixel / 8), 0);

            return bitmap;
        }

        private void TextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            CountChar.Text = "Оставшееся количество символов: " + Convert.ToString(Pix - TextBlock.Text.Length);
        }
    }
}
