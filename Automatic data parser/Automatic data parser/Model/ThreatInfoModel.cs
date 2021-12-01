using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_data_parser
{
    public class ThreatInfoModel
    {
        public int ThreatID { get; private set; }
        public string ThreatName { get; set; }
        public string DescriptionOfTheThreat { get; set; }
        public string TheSourceOfTheThreat { get; set; }
        public string ThreatImpactObject { get; set; }
        public bool BreachOfConfidentiality { get; set; }
        public bool IntegrityViolation { get; set; }
        public bool AccessibilityViolation { get; set; }

        public ThreatInfoModel(int threatID,
                          string threatName,
                          string descriptionOfTheThreat,
                          string theSourceOfTheThreat,
                          string threatImpactObject,
                          bool breachOfConfidentiality,
                          bool integrityViolation,
                          bool accessibilityViolation)
        {
            ThreatID = threatID;
            ThreatName = threatName;
            DescriptionOfTheThreat = descriptionOfTheThreat;
            TheSourceOfTheThreat = theSourceOfTheThreat;
            ThreatImpactObject = threatImpactObject;
            BreachOfConfidentiality = breachOfConfidentiality;
            IntegrityViolation = integrityViolation;
            AccessibilityViolation = accessibilityViolation;
        }
    }
}
