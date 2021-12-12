using Microsoft.Win32;
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

        #region Encoding buttons
        private void Enc_Open_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Word files (*.docx)|*.docx";
                fileDialog.ShowDialog();
                string filePath = fileDialog.FileName.ToString();

                Utils.WordInteractImpl wordInteract = new Utils.WordInteractImpl();
                EncryptedText.Text = wordInteract.LoadFromFile(filePath);
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
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Word files (*.docx)|*.docx";
                fileDialog.ShowDialog();
                string filePath = fileDialog.FileName.ToString();

                Utils.WordInteractImpl wordInteract = new Utils.WordInteractImpl();
                wordInteract.SaveToFile(filePath, EncryptedText.Text);
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
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Word files (*.docx)|*.docx";
                fileDialog.ShowDialog();
                string filePath = fileDialog.FileName.ToString();

                Utils.WordInteractImpl wordInteract = new Utils.WordInteractImpl();
                DecryptedText.Text = wordInteract.LoadFromFile(filePath);
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
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Word files (*.docx)|*.docx";
                fileDialog.ShowDialog();
                string filePath = fileDialog.FileName.ToString();

                Utils.WordInteractImpl wordInteract = new Utils.WordInteractImpl();
                wordInteract.SaveToFile(filePath, DecryptedText.Text);
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
