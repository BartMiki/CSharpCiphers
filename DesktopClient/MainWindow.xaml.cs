using Ciphers.Substitational;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;

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

        public string ErrorMessage
        {
            set
            {
                ErrorMessageTextBlock.Text = value;
                ClearErrorButton.Visibility = string.IsNullOrWhiteSpace(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public string PlainText { get => PlainTextBox.Text; set => PlainTextBox.Text = value; }
        public string EncryptedText { get => EncryptedTextBox.Text; set => EncryptedTextBox.Text = value; }
        public string Key { get => KeyTextBox.Text; set => KeyTextBox.Text = value; }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(Key);
                EncryptedText = ProcessInput(PlainText, cipher.Encrypt);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cipher = new VigenereCipher(Key);
                PlainText = ProcessInput(EncryptedText, cipher.Decrypt);
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
                Title = "Zapisz tekst jawny",
                Filter = "Plik tekstowy (*.txt)|*.txt"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                using (var file = new StreamWriter(dialog.OpenFile()))
                {
                    await file.WriteAsync(PlainText);
                }
            }
        }

        private async void LoadPlainTextButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                Title = "Wczytaj tekst jawny",
                Filter = "Plik tekstowy (*.txt)|*.txt"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                using (var file = new StreamReader(dialog.OpenFile()))
                {
                    PlainText = await file.ReadToEndAsync();
                }
            }
        }

        private async void SaveEncryptedTextButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                Title = "Zapisz tekst zaszyfrowany",
                Filter = "Plik tekstowy (*.txt)|*.txt"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                using (var file = new StreamWriter(dialog.OpenFile()))
                {
                    await file.WriteAsync(EncryptedText);
                }
            }
        }

        private async void LoadEncryptedTextButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                Title = "Wczytaj tekst zaszyfrowany",
                Filter = "Plik tekstowy (*.txt)|*.txt"
            };

            var result = dialog.ShowDialog();



            if (result.HasValue && result.Value)
            {
                using (var file = new StreamReader(dialog.OpenFile()))
                {
                    EncryptedText = await file.ReadToEndAsync();
                }
            }
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
            MessageBox.Show(@"Pole po lewej stronie służy do wpisywania tekstu jawnego. Jest także wyjściem dla tekstu odszyfrowanego.

Pole po prawej stronie służy do wpisywania tekstu zaszyfrowanego. Jest także wyjściem dla tekstu zaszyfrowanego.

Pod każdym polem znajdują się przyciski Zapisz i Wczytaj które odpowiadają zapisowi do pliku zawartości pola powyżej lub wczytania zawartości pliku do pola powyżej.", "Pomoc");
        }

        private string ProcessInput(string input, Func<string, string> cipher)
        {
            var lines = input
                .Replace("\t", " ")
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Where(IsNotEmpty)
                .Select(cipher);

            return string.Join(Environment.NewLine, lines);
        }

        private bool IsNotEmpty(string str) => !string.IsNullOrWhiteSpace(str);

        private async void SaveVigenereSquareButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                Title = "Zapisz tablice Vigenere'a",
                Filter = "Plik tekstowy (*.txt)|*.txt"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                using (var file = new StreamWriter(dialog.OpenFile()))
                {
                    var square = VigenereCipher.VigenereSquare;
                    var alphabet = square[0];

                    var header = string.Join("  ", alphabet.ToArray());
                    await file.WriteLineAsync($"#  {header}");

                    for (int i = 0; i < square.Count; i++)
                    {
                        var contnent = string.Join("  ", square[i].ToArray());
                        await file.WriteLineAsync($"{alphabet[i]}  {contnent}");
                    }
                }
            }
        }

        private void ClearErrorButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage = string.Empty;
        }
    }
}
