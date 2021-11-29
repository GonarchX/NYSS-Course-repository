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
        //Abbreviated view 
        public bool IsAbbreviated { get; set; }

        //Pagination fields
        private static DataGridComponent dataGridPaggingInstance;
        private int positionsOnPageCount = 15;
        private int pagesCount;
        private int currentPage;

        public int PositionsCount { get; private set; }
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
        public ObservableCollection<ThreatInfoModel>[] ThreatInfoByPages { get; private set; }

        //Local data fields
        private ObservableCollection<ThreatInfoModel> threatInfoData;
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

        //INotifyPropertyChanged fields
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Singleton implementation
        private DataGridComponent(ObservableCollection<ThreatInfoModel> threatInfoModels)
        {
            ThreatInfoData = threatInfoModels ?? new ObservableCollection<ThreatInfoModel>();
        }

        public static DataGridComponent GetInstance(ObservableCollection<ThreatInfoModel> threatInfoModels)
        {
            if (dataGridPaggingInstance == null)
            {
                dataGridPaggingInstance = new DataGridComponent(threatInfoModels);
            }
            return dataGridPaggingInstance;
        }

        //Component methods
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

        public ObservableCollection<ThreatInfoModel> PositionsOnCurrentPage()
        {
            if (ThreatInfoData.Count == 0) return null;
            else
                return ThreatInfoByPages[CurrentPage - 1];
        }

        public ObservableCollection<AbbreviatedThreatInfoModel> AbbreviatedPositionsOnCurrentPage()
        {
            if (ThreatInfoData.Count == 0) return null;
            else
                return ConvertToAbbreviatedVersion(ThreatInfoByPages[CurrentPage - 1]);
        }
    }
}