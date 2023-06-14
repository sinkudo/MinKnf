using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;


namespace minknf
{
    class Program
    {
        static void Main(string[] args)
        {
            //string currentDirectory = Environment.CurrentDirectory;
            //string vectorFilePath = Path.Combine(currentDirectory, "vector.txt");
            //String vector = String.Empty;
            //try
            //{
            //    vector = File.ReadAllText(vectorFilePath);
            //}
            //catch
            //{
            //    Console.WriteLine("Ошибка файла vector.txt\nНажмите на любую кнопку");
            //    Console.ReadKey();
            //    System.Environment.Exit(1);
            //}
            //if (vector == String.Empty)
            //{
            //    Console.WriteLine("Файл vector.txt пуст\nНажмите на любую кнопку");
            //    Console.ReadKey();
            //    System.Environment.Exit(1);
            //}


            //// read params
            //string paramsFilePath = Path.Combine(currentDirectory, "params.txt");
            //Dictionary<string, string> parameters = FileWorker.ReadParameterFile(paramsFilePath);
            //double minimalTemperature = double.Parse(parameters["Минимальная температура"]);
            //double maximumTemperature = double.Parse(parameters["Максимальная температура"]);
            //int temperatureChangeFunction = int.Parse(parameters["Функция изменения температуры (1-3)"]);
            //int constantInSuperFast = int.Parse(parameters["Константа в сверхбыстром"]);
            //int numberOfRepetitions = int.Parse(parameters["Количество повторений"]);
            //double coef = double.Parse(parameters["Коэффициент в функции энергии"]);

            
            //string resFilePath = Path.Combine(currentDirectory, "res.txt");
            //if (ValidateVector(vector)) {
            //    Console.WriteLine("Начало работы алгоритма...");
            //    KNF knf = new KNF(vector);

            //    string res = knf.MinimizeKNFAnnealing(maximumTemperature, minimalTemperature, temperatureChangeFunction, constantInSuperFast, numberOfRepetitions, coef);
            //    File.WriteAllText(resFilePath, res);
            //}
            //else
            //{
            //    Console.WriteLine("Неверный формат строки. Нажмите любую клавишу");
            //    File.WriteAllText(resFilePath, "Неверный формат вектора!!!");
            //}
            //Console.WriteLine("Алгоритм завершил работу. Проверьте res.txt");
            //Console.ReadKey();


            //incode without exe test

            
            KNF knf = new KNF("10011001101111100100101001000000100000111111111111011011011000001010111110001111100000100000111101000001001110000001101111111001111101000000010111011001011011110101011000001111110101011110001001100010100110100100100000100100111001101101110010000010001110000010011101000101101000010011011000101101010111111111010000010010000100010101000101011001001000101101010101011001011000001111101100110101011101001000101000001110101000001100010110111011011110010010001000010101111101010111111001100110010010110110011100110100");
            //KNF knf = new KNF("1111");
            Console.WriteLine(knf.MinimizeKnfGA());
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
