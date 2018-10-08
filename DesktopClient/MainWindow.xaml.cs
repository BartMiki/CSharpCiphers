using Ciphers.Substitational;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DesktopClient
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string ErrorMessage { set => ErrorMessageTextBlock.Text = value; }
        public string PlainText { get => PlainTextBox.Text; set => PlainTextBox.Text = value; }
        public string EncryptedText { get => EncryptedTextBox.Text; set => EncryptedTextBox.Text = value; }
        public string Key { get => KeyTextBox.Text; set => KeyTextBox.Text = value; }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(Key);
                EncryptedText = cipher.Encrypt(PlainText);
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(Key);
                PlainText = cipher.Decrypt(EncryptedText);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async void SavePlainTextButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                Title = "Zapisz tekst jawny."
            };

            var pathToSave = dialog.SafeFileName;
            using (var file = new StreamWriter(pathToSave))
            {
                await file.WriteAsync(PlainText);
            }
        }

        private void LoadPlainTextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveEncryptedTextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadEncryptedTextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage = string.Empty;
            PlainText = string.Empty;
            EncryptedText = string.Empty;
            Key = string.Empty;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
