using Automatic_data_parser.Model;
using ExcelDataReader;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        private readonly string URL = "https://bdu.fstec.ru/files/documents/thrlist.xlsx";
        private readonly string LOADED_DATA_FILE_NAME = "DownloadedData.xlsx";
        private readonly string LOCAL_DATA_FILE_NAME = "LocalData.xlsx";
        private readonly DataGridComponent dataGridComponent;

        public MainWindow()
        {
            ObservableCollection<ThreatInfoModel> observableThreatData =
                new ObservableCollection<ThreatInfoModel>(Utils.ParseExcelToThreatInfo(Utils.GetExcelFromFile(LOCAL_DATA_FILE_NAME)));

            dataGridComponent = DataGridComponent.GetInstance(observableThreatData);
            InitializeComponent();
            DataContext = dataGridComponent;

            MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
        }

        private void UpdateData_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(LOCAL_DATA_FILE_NAME))
                {
                    Utils.DownloadExcelFromWebsiteToDirectory(URL, LOADED_DATA_FILE_NAME);
                }
                else
                {
                    Utils.DownloadExcelFromWebsiteToDirectory(URL, LOCAL_DATA_FILE_NAME);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void FirstPage_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridComponent.CurrentPage == 1) return;
            dataGridComponent.CurrentPage = 1;

            if (!dataGridComponent.IsAbbreviated) 
                MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
            else 
                MainDataGrid.ItemsSource = dataGridComponent.AbbreviatedPositionsOnCurrentPage();
        }

        private void LastPage_Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridComponent.CurrentPage == dataGridComponent.PagesCount) return;
            dataGridComponent.CurrentPage = dataGridComponent.PagesCount;

            if (!dataGridComponent.IsAbbreviated) 
                MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
            else 
                MainDataGrid.ItemsSource = dataGridComponent.AbbreviatedPositionsOnCurrentPage();
        }

        private void PrevPage_Button_Click(object sender, RoutedEventArgs e)
        {
            int numberOfPageBeforeClick = dataGridComponent.CurrentPage;
            dataGridComponent.CurrentPage--;

            if (numberOfPageBeforeClick != dataGridComponent.CurrentPage)
            {
                if (!dataGridComponent.IsAbbreviated) 
                    MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
                else
                    MainDataGrid.ItemsSource = dataGridComponent.AbbreviatedPositionsOnCurrentPage();
            }
        }

        private void NextPage_Button_Click(object sender, RoutedEventArgs e)
        {
            int numberOfPageBeforeClick = dataGridComponent.CurrentPage;
            dataGridComponent.CurrentPage++;

            if (numberOfPageBeforeClick != dataGridComponent.CurrentPage)
            {
                if (!dataGridComponent.IsAbbreviated)
                    MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
                else
                    MainDataGrid.ItemsSource = dataGridComponent.AbbreviatedPositionsOnCurrentPage();
            }
        }

        private void ChoosedShowMode_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            dataGridComponent.IsAbbreviated = false;
            MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
        }

        private void ChoosedShowMode_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            dataGridComponent.IsAbbreviated = true;
            MainDataGrid.ItemsSource = dataGridComponent.AbbreviatedPositionsOnCurrentPage();
        }
    }
}