using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace minknf
{
    class FileParser
    {
        public static Dictionary<string, string> Parse(string filePath)
        {
            Dictionary<string, string> fileData = new Dictionary<string, string>();
            StreamReader reader = new StreamReader(filePath);
            string fileContents = reader.ReadToEnd();
            reader.Close();
            string[] lines = fileContents.Split('\n');
            foreach (string line in lines)
            {
                string[] parts = line.Trim().Split(':');
                if (parts.Length == 2)
                {
                    fileData[parts[0].Trim()] = parts[1].Trim();
                }
            }
            return fileData;
        }
    }
}
