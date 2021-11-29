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
    public class DataGridPagging : INotifyPropertyChanged
    {
        private int positionsOnPageCount = 15;
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
        private int pagesCount;
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
        private int currentPage;
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

                    OnPropertyChanged();
                }
                else threatInfoData = new ObservableCollection<ThreatInfoModel>();
            }
        }
        public ObservableCollection<ThreatInfoModel>[] ThreatInfoByPages { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public DataGridPagging(ObservableCollection<ThreatInfoModel> threatInfoModels)
        {
            ThreatInfoData = threatInfoModels ?? new ObservableCollection<ThreatInfoModel>();
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SeparatePositionsByPages()
        {
            ThreatInfoByPages = new ObservableCollection<ThreatInfoModel>[pagesCount];

            for (int localCurrentPage = 0; localCurrentPage < pagesCount; localCurrentPage++)
            {
                ThreatInfoByPages[localCurrentPage] = new ObservableCollection<ThreatInfoModel>();
                int positionsOnCurrentPageCount = 0;
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
            if (ThreatInfoData.Count == 0) return ThreatInfoData;
            else
            {
                return ThreatInfoByPages[CurrentPage - 1];
            }
        }
    }
}
