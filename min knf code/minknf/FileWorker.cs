using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace minknf
{
    class FileWorker
    {
        public static void SaveData(List<int> data)
        {
            // Specify the file path
            string currentDirectory = Environment.CurrentDirectory;
            string dataFilePath = Path.Combine(currentDirectory, "data.txt");

            // Convert the List<int> to a string representation
            string dataString = string.Join(Environment.NewLine, data);

            // Write the numbers to the file
            File.WriteAllText(dataFilePath, dataString);
        }
    }
}
