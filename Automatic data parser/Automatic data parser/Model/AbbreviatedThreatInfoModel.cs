using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_data_parser.Model
{
    public class AbbreviatedThreatInfoModel
    {
        public int ThreatID { get; private set; }
        public string ThreatName { get; private set; }

        public AbbreviatedThreatInfoModel(int threatID, string threatName)
        {
            ThreatID = threatID;
            ThreatName = threatName;
        } 
    }
}
