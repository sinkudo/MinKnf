using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace minknf
{
    class Program
    {
        static void Main(string[] args)
        {
            String sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1vx2vx3)&(x1v-x2vx3)";
            //sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1v-x2v-x3)";
            sknf = "(x1vx2v-x3v-x4)&(x1v-x2vx3vx4)&(x1v-x2vx3v-x4)&(x1v-x2v-x3v-x4)&(-x1vx2vx3vx4)&(-x1vx2vx3v-x4)&(-x1vx2v-x3vx4)&(-x1vx2v-x3v-x4)&(-x1v-x2vx3vx4)&(-x1v-x2vx3v-x4)";
            //sknf = "(x1vx2v-x3)&(x1v-x2vx3)&(-x1v-x2v-x3)&(x1v-x2v-x3)";
            //sknf = "(x1vx2v-x3v-x4vx5vx6)&(x1v-x2vx3vx4vx5v-x6)&(x1v-x2vx3v-x4vx5vx6)&(x1v-x2v-x3v-x4vx5vx6)&(-x1vx2vx3vx4vx5vx6)&(-x1vx2vx3v-x4vx5vx6)&(-x1vx2v-x3vx4v-x5vx6)&(-x1vx2v-x3v-x4vx5vx6)&(-x1v-x2vx3vx4v-x5vx6)&(-x1v-x2vx3v-x4vx5vx6)";
            //Console.WriteLine(StringChecking(sknf));
            //KNF knf = new KNF(sknf);
            //knf.MinimizeKnf();
            //Console.WriteLine(knf.ToString());
            //Console.WriteLine(10_000 + 1);
            var data = new Dictionary<string, string>()
            {
                {"epochs", "10000"},
                {"mutationChance", "1"},
                {"crossoverChance", "1"},
                {"populationSize", "100"},
            };
            int[,] a = new int[2, 2];
            GA ga = new GA(a, data);
            Console.WriteLine(ga.epochs);
            Console.WriteLine(ga.mutationChance);
            Console.WriteLine(ga.crossoverChance);
            Console.WriteLine(ga.populationSize);

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
