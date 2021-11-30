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
        private readonly string LOCAL_DATA_FILE_NAME = "Test_EmptyFile.xlsx";
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
                ObservableCollection<ThreatInfoModel> observableThreatData = null;
                try
                {
                    observableThreatData = new ObservableCollection<ThreatInfoModel>(Utils.ParseExcelToThreatInfo(excelQueryFactory));
                }
                catch (ArgumentException exc)
                {
                    MessageBox.Show(exc.Message);
                }

                dataGridComponent = DataGridComponent.GetInstance(observableThreatData);
                InitializeComponent();
                ChoosedShowMode_CheckBox.IsChecked = true;
                DataContext = dataGridComponent;
                MainDataGrid.ItemsSource = dataGridComponent.PositionsOnCurrentPage();
            }
        }

        private void UpdateData_Button_Click(object sender, RoutedEventArgs e)
        {
            /*string path = "TestText.dsadsa";
            //if (!File.Exists(LOCAL_DATA_FILE_NAME))
            if (!File.Exists("TestText.dsadsa"))
            {
                bool isFileExists = false;
                do
                {
                    if (MessageBox.Show(
                    $"Excel table was not found, would you like to try update again?\n\n" +
                    $"Choosing \"yes\" this window will be displayed again if the file is not found\n\n" +
                    $"Choosing \"no\" A new local file will be created copying the web version ", "Question",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        isFileExists = File.Exists(path);
                        //EXcel
                        //WorkBooks
                    }
                    else
                    {
                        //if no
                    }
                } while (true);
            }

            //while ()
            //MSG box Во время сохранения информации не был найден локальный файл с эксель таблицей, хотите попробовать сохранить еще раз?
            // Да - снова сообщение
            // Нет - Будет создан новый локальный файл, в таком случае вы не увидите разницу между изменениями локального файла и актуальной версией

            //TODOODODODOODDOODOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //Добавить кейс, когда во время корректной работы программы пользователь переместил локальный эксель куда-нибудь из рабочей директории
            //if файла нету, то известить об этом пользователя, мол тогда начнется загрузка файла и никаких изменений он не увидит

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
            }*/
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

        private void SaveData_Button_Click(object sender, RoutedEventArgs e)
        {
            Utils.SaveDataGridDataToFile(dataGridComponent.ThreatInfoData, LOCAL_DATA_FILE_NAME);
        }
    }
}