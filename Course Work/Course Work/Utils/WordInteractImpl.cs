using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Word = Microsoft.Office.Interop.Word;

namespace Course_Work.Utils
{
    /// <summary>
    /// Provides the ability to interaction with word files
    /// </summary>
    internal class WordInteractImpl : IWordInteract
    {
        /// <summary>
        /// Create an empty file in specified directory
        /// </summary>
        /// <param name="filePath">Path of creating file</param>
        public void CreateEmptyWordFile(string filePath)
        {
            _Word.Application application = null;
            try
            {
                application = new _Word.Application();
                var document = application.Documents.Add();
                var paragraph = document.Paragraphs.Add();
                paragraph.Range.Text = "";

                application.ActiveDocument.SaveAs(filePath);
                document.Close();
            }
            finally
            {
                if (application != null)
                {
                    application.Quit();
                }
            }
        }

        /// <summary>
        /// Load data from specified file
        /// </summary>
        /// <param name="filePath">>The path to the file from which the information will be taken</param>
        /// <returns>Data from file</returns>
        public string LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) CreateEmptyWordFile(filePath);

            _Word.Application app = new _Word.Application();
            _Word.Document doc = app.Documents.Open(filePath);

            doc.Save();
            string wordData = doc.Content.Text;
            doc.Close();
            app.Quit();

            return wordData;
        }

        /// <summary>
        /// Save information to specified file
        /// </summary>
        /// <param name="filePath">The path to the file into which the information will be loaded</param>
        /// <param name="inputData">Data to save</param>
        public void SaveToFile(string filePath, string inputData)
        {
            if (!File.Exists(filePath)) CreateEmptyWordFile(filePath);

            _Word.Application app = new _Word.Application();
            _Word.Document doc = app.Documents.Open(filePath);
            
            doc.Content.Text = inputData;
            doc.Save();
            doc.Close();
            app.Quit();
        }
    }
}
