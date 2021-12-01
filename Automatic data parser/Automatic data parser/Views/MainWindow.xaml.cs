using Automatic_data_parser.Model;
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
            ExcelQueryFactory excelQueryFactory = null;
            bool IsCorrectStart = true;

            try
            {
                excelQueryFactory = Utils.GetExcelFromFile(LOCAL_DATA_FILE_NAME);
            }
            catch (Exception exc)
            {
                if (MessageBox.Show(
                    $"Could not get the file from the system.\n" +
                    $"Reason: {exc.Message}\n\n" +
                    $"Choosing \"yes\" will try to download excel file \n\n" +
                    $"Choosing \"no\" will close the program", "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Utils.DownloadExcelFromWebsiteToDirectory(URL, LOCAL_DATA_FILE_NAME);
                    excelQueryFactory = Utils.GetExcelFromFile(LOCAL_DATA_FILE_NAME);
                }
                else
                {
                    //Close the main window if it was not possible to load the file with information about threats
                    Close();
                    IsCorrectStart = false;
                }
            }

            if (IsCorrectStart)
            {
                try
                {
                    ObservableCollection<ThreatInfoModel> observableThreatData = new ObservableCollection<ThreatInfoModel>(Utils.ParseExcelToThreatInfo(excelQueryFactory));
                    dataGridComponent = DataGridComponent.GetInstance(observableThreatData);
                    InitializeComponent();
                    DataContext = dataGridComponent;
                    MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
                }
                catch (ArgumentException exc)
                {
                    MessageBox.Show(exc.Message);
                    Close();
                }
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
            if (dataGridComponent != null)
            {
                dataGridComponent.IsAbbreviated = false;
                MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
            }
        }

        private void ChoosedShowMode_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            dataGridComponent.IsAbbreviated = true;
            MainDataGrid.ItemsSource = dataGridComponent.AbbreviatedPositionsOnCurrentPage();
        }

        private void SaveData_Button_Click(object sender, RoutedEventArgs e)
        {
            Utils.SaveThreatDataToFile(dataGridComponent.ThreatInfoData, MainDataGrid.Columns.Count(), LOCAL_DATA_FILE_NAME);
        }

        private void SyncData_Button_Click(object sender, RoutedEventArgs e)
        {
            Utils.DownloadExcelFromWebsiteToDirectory(URL, LOADED_DATA_FILE_NAME);

            List<ThreatInfoModel> localThreatData;
            List<ThreatInfoModel> loadedThreatData;
            try
            {
                loadedThreatData = new List<ThreatInfoModel>(Utils.ParseExcelToThreatInfo(Utils.GetExcelFromFile(LOADED_DATA_FILE_NAME)));
                localThreatData = new List<ThreatInfoModel>(Utils.ParseExcelToThreatInfo(Utils.GetExcelFromFile(LOCAL_DATA_FILE_NAME)));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

            Utils.GetDifferenceRows(localThreatData, loadedThreatData, out IList<ThreatInfoModel> localDifferenceRows, out IList<ThreatInfoModel> loadedDifferenceRows);

            if (MessageBox.Show(
                    $"Number of distinct lines: {localDifferenceRows.Count()}\n" +
                    $"Do you want to see the difference ?\n" +
                    $"\"Yes\" - Open a window with differences\n" +
                    $"\"No\" - Continue without opening the window with differences",
                    "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DifferenceWindow differenceWindow = new DifferenceWindow(localDifferenceRows, loadedDifferenceRows) { Owner = this };
                differenceWindow.ShowDialog();
            }

            if (MessageBox.Show(
                    $"Do you want to save actual version file?",
                    "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Utils.SaveThreatDataToFile(loadedThreatData, MainDataGrid.Columns.Count(), LOCAL_DATA_FILE_NAME);
                dataGridComponent.ThreatInfoData = new ObservableCollection<ThreatInfoModel>(loadedThreatData);
                MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
            }
        }
    }
}