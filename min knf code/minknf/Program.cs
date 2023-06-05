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

            //string currentDirectory = Environment.CurrentDirectory;
            //// read knf from file
            //string sknfFilePath = Path.Combine(currentDirectory, "sknf.txt");
            //String sknf = String.Empty;
            //try
            //{
            //    sknf = File.ReadAllText(sknfFilePath);
            //}
            //catch
            //{
            //    Console.WriteLine("Ошибка файла sknf.txt\nНажмите на любую кнопку");
            //    Console.ReadKey();
            //    System.Environment.Exit(1);
            //}
            //if (sknf == String.Empty)
            //{
            //    Console.WriteLine("Файл sknf.txt пуст\nНажмите на любую кнопку");
            //    Console.ReadKey();
            //    System.Environment.Exit(1);
            //}

            //// read params
            //string paramsFilePath = Path.Combine(currentDirectory, "params.txt");
            //Dictionary<string, string> data = new Dictionary<string, string>();
            //data = FileParser.Parse(paramsFilePath);
            //if (data == null)
            //{
            //    Console.WriteLine("Ошибка файла params.txt\nФайл отсутствует, либо пуст. Нажмите на любую кнопку");
            //    Console.ReadKey();
            //    System.Environment.Exit(1);
            //}

            //int epochs = int.Parse(data["epochs"]);
            //int populationSize = int.Parse(data["populationSize"]);
            //double mutationChance = double.Parse(data["mutationChance"]);
            //double crossoverChance = double.Parse(data["crossoverChance"]);

            //String outFilePath = Path.Combine(currentDirectory, "out.txt");

            //if (ValidateVector(sknf))
            //{
            //    Console.WriteLine("Начало работы алгоритма...");
            //    KNF knf = new KNF(sknf);

            //    knf.MinimizeKnf(epochs, populationSize, mutationChance, crossoverChance);
            //    try
            //    {

            //        if (knf.ToString() != String.Empty)
            //        {
            //            if (!sknf.Contains('1'))
            //            {
            //                File.WriteAllText(outFilePath, "0");
            //            }
            //            else
            //            {
            //                File.WriteAllText(outFilePath, knf.ToString());
            //            }
            //        }
            //        else
            //        {
            //            File.WriteAllText(outFilePath, "мнкф не существует");
            //        }
            //        Console.WriteLine("Успешная запись в файл.\nНажмите на любую кнопку");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Произошла ошибка при записи в файл: " + ex.Message);
            //        Console.WriteLine("Произошла ошибка при записи в файл");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Неверный формат строки. Нажмите любую клавишу");
            //    File.WriteAllText(outFilePath, "Неверный формат строки!!!");
            //}
            //Console.ReadKey();



            KNF knf = new KNF("1111111111110111101101111111011101110111110101111101011111111111");
            Console.WriteLine(knf.MinimizeKNFAnnealing());
            //Console.WriteLine(knf.MinimizeKnfGA());



            //incode without exe test

            //String sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1vx2vx3)&(x1v-x2vx3)";
            //String sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1v-x2v-x3)";
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
            //knf = new KNF(sknf);
            //knf =     new KNF("(x1vx2v-x3v-x4vx5vx6)&(x1v-x2vx3vx4vx5v-x6)&(x1v-x2vx3v-x4vx5vx6)&(x1v-x2v-x3v-x4vx5vx6)&(-x1vx2vx3vx4vx5vx6)&(-x1vx2vx3v-x4vx5vx6)&(-x1vx2v-x3vx4v-x5vx6)&(-x1vx2v-x3v-x4vx5vx6)&(-x1v-x2vx3vx4v-x5vx6)&(-x1v-x2vx3v-x4vx5vx6)");
            //knf.MinimizeKnf(data["epochs"], data["populationSize"], data["mutationChance"], data["crossoverChance"]);
            //KNF knf = new KNF("1111111111110111101101111111011101110111110101111101011111111111");
            //KNF knf = new KNF("1111");
            //Console.WriteLine(knf.MinimizeKnf());
            //Console.WriteLine(ValidateVector(""));

            //ориг 1111111111110111101101111111011101110111110101111101011111111111

            //res  1111111111110111101101111111011101110111110101111101011111111111
        }

        private static Boolean ValidateVector(string vector)
        {
            // false при пустом вводе
            if (vector == String.Empty) return false;
            // посимвольная проверка
            foreach (char c in vector)
            {
                if (c != '0' && c != '1')
                {
                    return false;
                }
            }
            // истина, если все предыдущие проверки были пройдены + проверка на степень 2
            return IsPowerOfTwo(vector.Length);
        }
        public static bool IsPowerOfTwo(int number)
        {
            if (number <= 0)
            {
                return false;
            }
            // Check if the number has only one bit set to 1
            return (number & (number - 1)) == 0;
        }
        private static Boolean StringChecking(string str)
        {
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
