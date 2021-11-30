using LinqToExcel;
using System;
using System.Collections.Generic;
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
            int threatID;
            if (!Int32.TryParse(Convert.ToString(row[0].Value), out threatID))
            {
                throw new ArgumentException("Failed to get data from file\n" +
                    "Cause: Invalid value in the column (required value: integer)\n" +
                    $"Current value: {Convert.ToString(row[0].Value)}\n" +
                    "Column: Threat ID\n");
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

            bool integrityViolation = false;
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

            bool accessibilityViolation = false;
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

        public static string CheckFileData(string filePath)
        {
            throw new NotImplementedException();
        }

        public static void SaveDataGridDataToFile(IList<ThreatInfoModel> dataInDataGrid, string fileName)
        {
            string fullPath = Directory.GetCurrentDirectory() + $@"\{fileName}";

            _Excel.Application excel = new _Excel.Application();
            _Excel.Workbook workbook = excel.Workbooks.Open(fullPath);
            _Excel.Worksheet worksheet = workbook.Worksheets[1];

            // Starting row is 3 because 1 and 2 rows for column names
            int startRow = 3;
            for (int i = 0; i < dataInDataGrid.Count(); i++)
            {
                worksheet.Cells[i + startRow, 1].Value = dataInDataGrid[i].ThreatID;
                worksheet.Cells[i + startRow, 2].Value = dataInDataGrid[i].ThreatName;
                worksheet.Cells[i + startRow, 3].Value = dataInDataGrid[i].DescriptionOfTheThreat;
                worksheet.Cells[i + startRow, 4].Value = dataInDataGrid[i].TheSourceOfTheThreat;
                worksheet.Cells[i + startRow, 5].Value = dataInDataGrid[i].ThreatImpactObject;
                worksheet.Cells[i + startRow, 6].Value = dataInDataGrid[i].BreachOfConfidentiality == true ? "1" : "0";
                worksheet.Cells[i + startRow, 7].Value = dataInDataGrid[i].IntegrityViolation == true ? "1" : "0";
                worksheet.Cells[i + startRow, 8].Value = dataInDataGrid[i].AccessibilityViolation == true ? "1" : "0";
                worksheet.Rows[i + startRow].RowHeight = 15;
            }

            workbook.SaveAs(fullPath, _Excel.XlFileFormat.xlWorkbookDefault, null, null);
            workbook.Close();
            excel.Quit();
        }

        public static ExcelQueryFactory GetExcelFromFile(string filePath)
        {
            if (filePath.Length == 0) throw new ArgumentException("File path should not be empty!");
            else if (filePath is null) throw new ArgumentNullException("File path should not be null!");
            else if (!File.Exists(filePath)) throw new FileNotFoundException("Could not find the file in the specified directory!");
            return new ExcelQueryFactory(filePath);
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
                    throw new ArgumentException(exc.Message + $"\nRow: {i + 2}");
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
    }
}