using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;


namespace minknf
{
    class Program
    {
        static void Main(string[] args)
        {
            //incode without exe test

            //String sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1vx2vx3)&(x1v-x2vx3)";
            //sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1v-x2v-x3)";
            //sknf = "(x1vx2v-x3v-x4)&(x1v-x2vx3vx4)&(x1v-x2vx3v-x4)&(x1v-x2v-x3v-x4)&(-x1vx2vx3vx4)&(-x1vx2vx3v-x4)&(-x1vx2v-x3vx4)&(-x1vx2v-x3v-x4)&(-x1v-x2vx3vx4)&(-x1v-x2vx3v-x4)";
            //sknf = "(x1vx2v-x3)&(x1v-x2vx3)&(-x1v-x2v-x3)&(x1v-x2v-x3)";
            //sknf = "(x1vx2v-x3v-x4vx5vx6)&(x1v-x2vx3vx4vx5v-x6)&(x1v-x2vx3v-x4vx5vx6)&(x1v-x2v-x3v-x4vx5vx6)&(-x1vx2vx3vx4vx5vx6)&(-x1vx2vx3v-x4vx5vx6)&(-x1vx2v-x3vx4v-x5vx6)&(-x1vx2v-x3v-x4vx5vx6)&(-x1v-x2vx3vx4v-x5vx6)&(-x1v-x2vx3v-x4vx5vx6)";
            //var data = new Dictionary<string, string>()
            //{
            //    {"epochs", "10000"},
            //    {"mutationChance", "1"},
            //    {"crossoverChance", "1"},
            //    {"populationSize", "100"},
            //};
            
            
            //KNF knf = new KNF("(x1vx2v-x3v-x4vx5vx6)&(x1v-x2vx3vx4vx5v-x6)&(x1v-x2vx3v-x4vx5vx6)&(x1v-x2v-x3v-x4vx5vx6)&(-x1vx2vx3vx4vx5vx6)&(-x1vx2vx3v-x4vx5vx6)&(-x1vx2v-x3vx4v-x5vx6)&(-x1vx2v-x3v-x4vx5vx6)&(-x1v-x2vx3vx4v-x5vx6)&(-x1v-x2vx3v-x4vx5vx6)");
            //KNF knf = new KNF("(x1vx2vx3)&(x1vx2v-x3)&(-x1v-x2v-x3)");


            string currentDirectory = Environment.CurrentDirectory;
            // read knf from file
            string sknfFilePath = Path.Combine(currentDirectory, "sknf.txt");
            String sknf = String.Empty;
            try
            {
                sknf = File.ReadAllText(sknfFilePath);
            }
            catch
            {
                Console.WriteLine("Ошибка файла sknf.txt\nНажмите на любую кнопку");
                Console.ReadKey();
                System.Environment.Exit(1);
            }

            // read params
            string paramsFilePath = Path.Combine(currentDirectory, "params.txt");
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                data = FileParser.Parse(paramsFilePath);
            }
            catch
            {
                Console.WriteLine("Ошибка файла params.txt\nНажмите на любую кнопку");
                Console.ReadKey();
                System.Environment.Exit(1);
            }
            int epochs = Int32.Parse(data["epochs"]);
            int populationSize = Int32.Parse(data["populationSize"]);
            double mutationChance = Double.Parse(data["mutationChance"]);
            double crossoverChance = Double.Parse(data["crossoverChance"]);


            String outFilePath = Path.Combine(currentDirectory, "out.txt");

            if (StringChecking(sknf))
            {
                Console.WriteLine("Начало работы алгоритма...");
                KNF knf = new KNF(sknf);
                knf.MinimizeKnf(epochs, populationSize, mutationChance, crossoverChance);
                try
                {
                    File.WriteAllText(outFilePath, knf.ToString());
                    Console.WriteLine("Успешная запись в файл.\nНажмите на любую кнопку");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Произошла ошибка при записи в файл: " + ex.Message);
                    Console.WriteLine("Произошла ошибка при записи в файл");
                }
                //Console.WriteLine(knf.ToString());
            }
            else
            {
                Console.WriteLine("Неверный формат строки. Нажмите любую клавишу");
                File.WriteAllText(outFilePath, "Неверный формат строки!!!");
            }
            Console.ReadKey();
        }

        private static Boolean StringChecking(string str)
        {
            if (str == String.Empty)
            {
                return false;
            }
            str = str.Replace("(", String.Empty);
            str = str.Replace(")", String.Empty);
            string[] strings = str.Split("&");
            string heh = strings[strings.Length - 1];
            int n = heh[heh.Length - 1]- '0';
            foreach (var item in strings){
                if (!Regex.IsMatch(item, "[\\-xv0-9-]+") || !char.IsNumber(item[item.Length - 1]) || item[item.Length - 1] - '0' != n){
                    return false;
                }
                n = item[item.Length - 1] - '0';
                
                string[] bul = item.Split("v");
                if (bul.Length != n){
                    return false;
                }
                
                for (int i = 0; i < n; i++){
                    string temp_string = bul[i];
                    if (!Regex.IsMatch(temp_string, "x[0-9]+|\\-x[0-9]+"))
                    {
                        return false;
                    }
                    if ((temp_string[temp_string.Length - 1] - '0') != (i+1)){
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
