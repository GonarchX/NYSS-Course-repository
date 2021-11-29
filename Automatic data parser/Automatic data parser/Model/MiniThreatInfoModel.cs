using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_data_parser.Model
{
    class MiniThreatInfoModel
    {
        public int ThreatID { get; set; }
        public string ThreatName { get; set; }

        public MiniThreatInfoModel(int threatID, string threatName)
        {
            ThreatID = threatID;
            ThreatName = threatName;
        } 
    }
}
