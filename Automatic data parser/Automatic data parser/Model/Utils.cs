using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Automatic_data_parser.Model
{
    public static class Utils
    {
        public static ThreatInfoModel GetInfoFromRow(LinqToExcel.Row row)
        {
            int threatID = Int32.Parse(Convert.ToString(row[0].Value));
            string threatName = Convert.ToString(row[1].Value);
            string descriptionOfTheThreat = Convert.ToString(row[2].Value);
            string theSourceOfTheThreat = Convert.ToString(row[3].Value);
            string threatImpactObject = Convert.ToString(row[4].Value);
            bool breachOfConfidentiality = Convert.ToString(row[5].Value) == "1";
            bool integrityViolation = Convert.ToString(row[6].Value) == "1";
            bool accessibilityViolation = Convert.ToString(row[7].Value) == "1";

            return new ThreatInfoModel(threatID,
                                  threatName,
                                  descriptionOfTheThreat,
                                  theSourceOfTheThreat,
                                  threatImpactObject,
                                  breachOfConfidentiality,
                                  integrityViolation,
                                  accessibilityViolation);
        }

        public static string ReadFromFile(string filePath)
        {
            //return File.ReadAllText($@"{filePath}", Encoding.ASCII);
            throw new NotImplementedException();
        }

        public static string CheckFileData(string filePath)
        {
            throw new NotImplementedException();
        }

        public static string UpdateFileData(string filePath)
        {
            throw new NotImplementedException();
        }

        public static ExcelQueryFactory GetExcelFromFile(string filePath) => new ExcelQueryFactory(filePath);

        public static List<ThreatInfoModel> ParseExcelToThreatInfo(ExcelQueryFactory excelFile)
        {
            int rowCount = excelFile.Worksheet("Sheet").Count();
            var data = excelFile.WorksheetRange("A2", "H" + (rowCount + 1), "Sheet");
            List<ThreatInfoModel> threatData = new List<ThreatInfoModel>();

            foreach (var row in data)
            {
                threatData.Add(GetInfoFromRow(row));
            }

            return threatData;
        }

        public static void DownloadExcelFromWebsiteToDirectory(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, fileName);
            }
        }
    }
}