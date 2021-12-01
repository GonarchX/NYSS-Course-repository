using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using _Excel = Microsoft.Office.Interop.Excel;

namespace Automatic_data_parser.Model
{
    public static class Utils
    {
        /// <summary>
        /// Take the row from excel and parse information from it
        /// </summary>
        /// <param name="row">Specified row to parse</param>
        /// <returns>Parsed information from row as ThreatInfoModel</returns>
        public static ThreatInfoModel ParseInfoFromRow(LinqToExcel.Row row)
        {
            if (!Int32.TryParse(Convert.ToString(row[0].Value), out int threatID))
            {
                throw new ArgumentException("Failed to get data from file\n" +
                    "Cause: Invalid value in the column (required value: integer)\n" +
                    $"Current value: {Convert.ToString(row[0].Value)}\n" +
                    "Column: Threat ID");
            }
            string threatName = Convert.ToString(row[1].Value);
            string descriptionOfTheThreat = Convert.ToString(row[2].Value);
            string theSourceOfTheThreat = Convert.ToString(row[3].Value);
            string threatImpactObject = Convert.ToString(row[4].Value);
            bool breachOfConfidentiality;
            if (Convert.ToString(row[5].Value) != "0" && Convert.ToString(row[5].Value) != "1")
            {
                throw new ArgumentException("Failed to get data from file\n" +
                    "Cause: Invalid value in the column (required value: bool (0 or 1))\n" +
                    $"Current value: {Convert.ToString(row[5].Value)}\n" +
                    "Column: Breach Of Confidentiality");
            }
            else
            {
                breachOfConfidentiality = Convert.ToString(row[5].Value) == "1";
            }

            bool integrityViolation;
            if (Convert.ToString(row[6].Value) != "0" && Convert.ToString(row[6].Value) != "1")
            {
                throw new ArgumentException("Failed to get data from file\n" +
                    "Cause: Invalid value in the column (required value: bool (0 or 1))\n" +
                    $"Current value: {Convert.ToString(row[6].Value)}\n" +
                    "Column: Integrity Violation");
            }
            else
            {
                integrityViolation = Convert.ToString(row[6].Value) == "1";
            }

            bool accessibilityViolation;
            if (Convert.ToString(row[7].Value) != "0" && Convert.ToString(row[7].Value) != "1")
            {
                throw new ArgumentException("Failed to get data from file\n" +
                    "Cause: Invalid value in the column (required value: bool (0 or 1))\n" +
                    $"Current value: {Convert.ToString(row[7].Value)}\n" +
                    "Column: Accessibility Violation");
            }
            else
            {
                accessibilityViolation = Convert.ToString(row[7].Value) == "1";
            }

            return new ThreatInfoModel(threatID,
                                  threatName,
                                  descriptionOfTheThreat,
                                  theSourceOfTheThreat,
                                  threatImpactObject,
                                  breachOfConfidentiality,
                                  integrityViolation,
                                  accessibilityViolation);
        }

        public static void SaveThreatDataToFile(IList<ThreatInfoModel> dataInDataGrid, int numberOfColumns, string fileName)
        {
            string fullPath = Directory.GetCurrentDirectory() + $@"\{fileName}";

            _Excel.Application excel = new _Excel.Application();
            _Excel.Workbook workbook = excel.Workbooks.Open(fullPath);
            _Excel.Worksheet worksheet = workbook.Worksheets[1];
             
            // Starting row is 3 because 1 and 2 rows for column names
            int startRow = 3;

            object[,] arr = new object[dataInDataGrid.Count, numberOfColumns];
            for (int i = 0; i < dataInDataGrid.Count; i++)
            {
                arr[i, 0] = dataInDataGrid[i].ThreatID;
                arr[i, 1] = dataInDataGrid[i].ThreatName;
                arr[i, 2] = dataInDataGrid[i].DescriptionOfTheThreat;
                arr[i, 3] = dataInDataGrid[i].TheSourceOfTheThreat;
                arr[i, 4] = dataInDataGrid[i].ThreatImpactObject;
                arr[i, 5] = dataInDataGrid[i].BreachOfConfidentiality == true ? "1" : "0";
                arr[i, 6] = dataInDataGrid[i].IntegrityViolation == true ? "1" : "0";
                arr[i, 7] = dataInDataGrid[i].AccessibilityViolation == true ? "1" : "0";
            }

            _Excel.Range range = worksheet.Range[worksheet.Cells[startRow, 1], worksheet.Cells[dataInDataGrid.Count, numberOfColumns]];

            range.Value = arr;

            worksheet.Rows.RowHeight = 15;
            workbook.SaveAs(fullPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, _Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing);
            workbook.Close(true);
            excel.Quit();            
        }

        public static ExcelQueryFactory GetExcelFromFile(string fileName)
        {
            if (fileName.Length == 0) throw new ArgumentException("File name should not be empty!");
            else if (fileName is null) throw new ArgumentNullException("File name should not be null!");
            else if (!File.Exists(fileName)) throw new FileNotFoundException($"Could not find the file \"{fileName}\" in the project directory!");
            return new ExcelQueryFactory(fileName);
        }

        /// <summary>
        /// Take the excel file and parse information from it
        /// </summary>
        /// <param name="excelFile">Specified excel file to parse</param>
        /// <returns>Parsed information from excel file as list of ThreatInfoModel</returns>
        public static List<ThreatInfoModel> ParseExcelToThreatInfo(ExcelQueryFactory excelFile)
        {
            int rowCount = excelFile.Worksheet("Sheet").Count();
            // A2 - H(rowCount + 1) - range of cells in Excel with threat data
            var data = excelFile.WorksheetRange("A2", "H" + (rowCount + 1), "Sheet");
            List<ThreatInfoModel> threatData = new List<ThreatInfoModel>();

            int i = 1;
            foreach (var row in data)
            {
                try
                {
                    threatData.Add(ParseInfoFromRow(row));
                }
                catch (ArgumentException exc)
                {
                    threatData = null;
                    // Row = i + 2 because 2 rows in Excel table are rows for column names
                    throw new ArgumentException(exc.Message + $"\nRow: {i + 2}\n" +
                        $"File name: {excelFile.FileName}");
                }
                i++;
            }

            return threatData;
        }

        /// <summary>
        /// Downloads Excel from a specific website and saves it in the specified file directory
        /// </summary>
        /// <param name="url">Website URL</param>
        /// <param name="fileName">Path to file directory</param>
        public static void DownloadExcelFromWebsiteToDirectory(string url, string fileName)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, fileName);
            }
        }

        public static bool IsDifferenceRows(ThreatInfoModel firstRow, ThreatInfoModel secondRow)
        {
            if (firstRow.ThreatID != secondRow.ThreatID) { return true; }
            else if (firstRow.ThreatName != secondRow.ThreatName) { return true; }
            else if (firstRow.DescriptionOfTheThreat != secondRow.DescriptionOfTheThreat) { return true; }
            else if (firstRow.TheSourceOfTheThreat != secondRow.TheSourceOfTheThreat) { return true; }
            else if (firstRow.ThreatImpactObject != secondRow.ThreatImpactObject) { return true; }
            else if (firstRow.BreachOfConfidentiality != secondRow.BreachOfConfidentiality) { return true; }
            else if (firstRow.IntegrityViolation != secondRow.IntegrityViolation) { return true; }
            else if (firstRow.AccessibilityViolation != secondRow.AccessibilityViolation) { return true; }

            return false;
        }

        public static void GetDifferenceRows (IList<ThreatInfoModel> firstSourceData, IList<ThreatInfoModel> secondSourceData, out IList<ThreatInfoModel> firstDifferenceData, out IList<ThreatInfoModel> secondDifferenceData)
        {
            var firstSourceDataIDs = firstSourceData.Select(x => x.ThreatID).ToList();
            var secondSourceDataIDs = secondSourceData.Select(x => x.ThreatID).ToList();
            firstDifferenceData = new List<ThreatInfoModel>();
            secondDifferenceData = new List<ThreatInfoModel>();

            foreach (var currentThreatID in secondSourceDataIDs)
            {
                if (!firstSourceDataIDs.Contains(currentThreatID))
                {
                    secondDifferenceData.Add(secondSourceData.First(x => x.ThreatID == currentThreatID));
                    firstDifferenceData.Add(new ThreatInfoModel(secondDifferenceData.Last().ThreatID, "There was no such threat", "There was no such threat", "There was no such threat", "There was no such threat", false, false, false));
                }
                else
                {
                    var curLocalThreat = firstSourceData.First(x => x.ThreatID == currentThreatID);
                    var curLoadedThreat = secondSourceData.First(x => x.ThreatID == currentThreatID);
                    if (Utils.IsDifferenceRows(curLocalThreat, curLoadedThreat))
                    {
                        firstDifferenceData.Add(firstSourceData.First(x => x.ThreatID == currentThreatID));
                        secondDifferenceData.Add(secondSourceData.First(x => x.ThreatID == currentThreatID));
                    }
                }
            }
        }
    }
}