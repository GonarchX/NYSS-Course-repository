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

namespace Course_Work
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Work with files in windows
        private string ReadDataFromAvailableFiles()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "All available files|*.docx;*.txt|Word files (*.docx)|*.docx|Text files (*.txt)|*.txt"
            };
            fileDialog.ShowDialog();
            string filePath = fileDialog.FileName.ToString();

            if (!string.IsNullOrEmpty(filePath))
            {
                switch (filePath.Substring(filePath.LastIndexOf('.')))
                {
                    case ".txt":
                        return File.ReadAllText(filePath);
                    case ".docx":
                        Utils.WordInteractImpl wordInteract = new Utils.WordInteractImpl();
                        return wordInteract.LoadFromFile(filePath);
                    default:
                        MessageBox.Show("Incorrect file extension type specified!");
                        break;
                }
            }
            return "";
        }

        private void SaveDataToAvailableFiles(string inputData)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Filter = "Word files (*.docx)|*.docx|Text files (*.txt)|*.txt"
            };
            fileDialog.ShowDialog();
            string filePath = fileDialog.FileName.ToString();

            if (!string.IsNullOrEmpty(filePath))
            {
                Utils.WordInteractImpl wordInteract = new Utils.WordInteractImpl();
                wordInteract.SaveToFile(filePath, inputData);
            }
        }
        #endregion
       
        #region Encoding buttons
        private void Enc_Open_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EncryptedText.Text = ReadDataFromAvailableFiles();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Enc_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveDataToAvailableFiles(EncryptedText.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Enc_Decrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            DecryptedText.Text = Utils.VigenereCipher.DecryptText(EncryptedText.Text, Keyword.Text);

        }
        #endregion

        #region Decoding buttons
        private void Dec_Open_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DecryptedText.Text = ReadDataFromAvailableFiles();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Dec_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveDataToAvailableFiles(DecryptedText.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Dec_Encrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            EncryptedText.Text = Utils.VigenereCipher.EncryptText(DecryptedText.Text, Keyword.Text);
        }
        #endregion

        #region Common buttons
        private void ClearText_Button_Click(object sender, RoutedEventArgs e)
        {
            EncryptedText.Text = "";
            DecryptedText.Text = "";
        }

        private void ReplaceText_Button_Click(object sender, RoutedEventArgs e)
        {
            var temp = EncryptedText.Text;
            EncryptedText.Text = DecryptedText.Text;
            DecryptedText.Text = temp;
        }
        #endregion
    }
}
