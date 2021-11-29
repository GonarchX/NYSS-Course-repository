using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_data_parser.Model
{
    public class DataGridComponent : INotifyPropertyChanged
    {
        #region Abbreviated view 
        /// <summary>
        /// Show mode of data in DataGrid;
        /// <para>True - show abbreviated data;</para>
        /// <para>False - show full data</para>
        /// </summary>
        public bool IsAbbreviated { get; set; }
        #endregion

        #region Pagination fields
        private static DataGridComponent dataGridPaggingInstance;
        private int positionsOnPageCount = 15;
        private int pagesCount;
        private int currentPage;
        #endregion

        #region Pagination properties
        /// <summary>
        /// Number of rows with data in excel file
        /// </summary>
        public int PositionsCount { get; private set; }
        /// <summary>
        /// Number of rows on one page in datagrid
        /// </summary>
        public int PositionsOnPageCount
        {
            get
            {
                return positionsOnPageCount;
            }
            private set
            {
                if (value > 0 && value <= PositionsCount)
                {
                    positionsOnPageCount = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Summary number of pages in datagrid
        /// </summary>
        public int PagesCount
        {
            get
            {
                return pagesCount;
            }
            private set
            {
                pagesCount = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                if (PagesCount == 0)
                {
                    currentPage = 0;
                    OnPropertyChanged();
                }
                else if (value > 0 && value <= PagesCount)
                {
                    currentPage = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Paginated threat Information
        /// <para>This is done to improve the performance of the program, </para>
        /// <para>When page is switch, you get ready data from array of paginated data.</para>
        /// </summary>
        public ObservableCollection<ThreatInfoModel>[] ThreatInfoByPages { get; private set; }
        #endregion

        #region Fields with threat info
        private ObservableCollection<ThreatInfoModel> threatInfoData;
        /// <summary>
        /// Property which contain  all parsed data from excel
        /// </summary>
        public ObservableCollection<ThreatInfoModel> ThreatInfoData
        {
            get
            {
                return threatInfoData;
            }
            set
            {
                if (value != null)
                {
                    threatInfoData = value;
                    PositionsCount = value.Count();
                    if (PositionsCount % PositionsOnPageCount == 0)
                    {
                        PagesCount = PositionsCount / PositionsOnPageCount;
                    }
                    else
                    {
                        PagesCount = (PositionsCount / PositionsOnPageCount) + 1;
                    }
                    CurrentPage = 1;
                    SeparatePositionsByPages();
                }
                else threatInfoData = new ObservableCollection<ThreatInfoModel>();
                OnPropertyChanged();
            }
        }
        #endregion

        #region INotifyPropertyChanged fields
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Singleton implementation
        private DataGridComponent(ObservableCollection<ThreatInfoModel> threatInfoModels)
        {
            ThreatInfoData = threatInfoModels ?? new ObservableCollection<ThreatInfoModel>();
        }

        /// <summary>
        /// Return instance of DataGridComponent
        /// <para>If instance is null then will created</para>
        /// </summary>
        /// <param name="threatInfoModels">List of parsed data from excel</param>
        /// <returns>The only created instance of the class</returns>
        public static DataGridComponent GetInstance(ObservableCollection<ThreatInfoModel> threatInfoModels)
        {
            if (dataGridPaggingInstance == null)
            {
                dataGridPaggingInstance = new DataGridComponent(threatInfoModels);
            }
            return dataGridPaggingInstance;
        }
        #endregion

        #region Component methods
        private ObservableCollection<AbbreviatedThreatInfoModel> ConvertToAbbreviatedVersion(
            ObservableCollection<ThreatInfoModel> fullThreatInfo)
        {
            ObservableCollection<AbbreviatedThreatInfoModel> abbreviatedThreatInfo = new ObservableCollection<AbbreviatedThreatInfoModel>();

            foreach (var threatInfo in fullThreatInfo)
            {
                abbreviatedThreatInfo.Add(new AbbreviatedThreatInfoModel(threatInfo.ThreatID, threatInfo.ThreatName));
            }

            return abbreviatedThreatInfo;
        }

        // This method fill ThreatInfoByPages pages by pages
        private void SeparatePositionsByPages()
        {
            ThreatInfoByPages = new ObservableCollection<ThreatInfoModel>[PagesCount];

            for (int localCurrentPage = 0; localCurrentPage < PagesCount; localCurrentPage++)
            {
                ThreatInfoByPages[localCurrentPage] = new ObservableCollection<ThreatInfoModel>();
                int positionsOnCurrentPageCount;
                int startIndex = PositionsOnPageCount * (localCurrentPage);
                // If count of remaining position >= count of position on page, then we put on page all required positions
                if ((PositionsCount - (PositionsOnPageCount * (localCurrentPage))) / PositionsOnPageCount > 0)
                {
                    positionsOnCurrentPageCount = PositionsOnPageCount;
                }
                // else we put only remaining positions
                else
                {
                    positionsOnCurrentPageCount = (PositionsCount - (PositionsOnPageCount * (localCurrentPage))) % PositionsOnPageCount;
                }

                int endIndex = startIndex + positionsOnCurrentPageCount;

                for (int j = startIndex; j < endIndex; j++)
                {
                    ThreatInfoByPages[localCurrentPage].Add(threatInfoData[j]);
                }
            }
        }

        
        /// <returns>Positions on current page in datagrid</returns>
        public ObservableCollection<ThreatInfoModel> PositionsOnCurrentPage()
        {
            if (ThreatInfoData.Count == 0) return null;
            else
                return ThreatInfoByPages[CurrentPage - 1];
        }

        /// <returns>Abbreviated positions on current page in datagrid</returns>
        public ObservableCollection<AbbreviatedThreatInfoModel> AbbreviatedPositionsOnCurrentPage()
        {
            if (ThreatInfoData.Count == 0) return null;
            else
                return ConvertToAbbreviatedVersion(ThreatInfoByPages[CurrentPage - 1]);
        }
        #endregion
    }
}