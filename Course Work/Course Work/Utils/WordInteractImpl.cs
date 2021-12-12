using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _Word = Microsoft.Office.Interop.Word;

namespace Course_Work.Utils
{
    internal class WordInteractImpl : IWordInteract
    {
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
