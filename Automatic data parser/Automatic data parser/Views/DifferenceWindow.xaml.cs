using Automatic_data_parser.Model;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Automatic_data_parser
{
    /// <summary>
    /// Interaction logic for DifferenceWindow.xaml
    /// </summary>
    public partial class DifferenceWindow : Window
    {
        public DifferenceWindow(IList<ThreatInfoModel> localThreatData, IList<ThreatInfoModel> actualThreatData)
        {
            InitializeComponent();

            LocalDataGrid.ItemsSource = localThreatData;
            ActualDataGrid.ItemsSource = actualThreatData;
        }
    }
}
