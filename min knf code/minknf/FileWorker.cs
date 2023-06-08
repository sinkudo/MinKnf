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

        public static Dictionary<string, string> ReadParameterFile(string filePath)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');

                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        parameters[key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading parameter file: " + ex.Message);
            }

            return parameters;
        }
    }
}
