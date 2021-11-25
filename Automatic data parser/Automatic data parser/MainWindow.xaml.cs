using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Automatic_data_parser
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string url = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
            //DownloadCSVFromWebsite(url, "DownloadedData.xlsx");

            MessageBox.Show(File.Exists("DownloadedData.xlsx").ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var temp = GetCSV(url);
        }

        private static string ReadFromFile(string filePath)
        {
            return File.ReadAllText($@"{filePath}", Encoding.ASCII);
        }

        private static string CheckFileData(string filePath)
        {
            throw new NotImplementedException();
        }

        private static string UpdateFileData(string filePath)
        {
            throw new NotImplementedException();
        }

        private static void DownloadCSVFromWebsite(string url, string fileName)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, fileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}