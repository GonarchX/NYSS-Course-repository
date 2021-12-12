using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_Work.Utils
{
    /// <summary>
    /// Interface to interacting with Word files
    /// </summary>
    internal interface IWordInteract
    {
        void CreateEmptyWordFile(string filePath);
        void SaveToFile(string filePath, string inputData);
        string LoadFromFile(string filePath);
    }
}