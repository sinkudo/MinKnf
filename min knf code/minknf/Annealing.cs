using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace minknf
{
    class Annealing : BestCoverageAlgo
    {
        List<DisjunctiveMonomial> monomials;
        double T = 100;
        double currentT = 100;
        public Annealing(int[,] matrix, List<int> absoluteIndexes, List<DisjunctiveMonomial> monomials): base(matrix, absoluteIndexes)
        {
            this.monomials = monomials;

            //suda smotri item1 = geneindexes, item2 = monomials
            //foreach (var i in absoluteIndexes.Zip(monomials, Tuple.Create))
            //{
            //    Console.WriteLine(i.Item1 + " " + i.Item2.ToString() + " " + i.Item2.GetSize());
            //}

            //Matrix.CoutMatr(matrix);

            //1. рандомно генерим брусок
            //2. тип получаем мономиалы, сайз
            //3. генерим рандомно измененный брусок
            //4. получаем сайз нового бруска
            //5. сравниваем, если сайз стал меньше -- берем его за основной.
            //    если сайз стал больше, то e ^ (-SIZE_DIFF / T)
            //6. с этой вероятностью меняем брусок.
            //на всем пути темпа падает

            //foreach (var i in this.pool) {
            //    foreach (var j in i)
            //    {
            //        Console.Write(j);
            //    }
            //    Console.WriteLine();

            //}

            //sum = 0;
            //foreach (var i in brusok)
            //{
            //    sum += monomials[i].GetSize();
            //}
            //...
        }
        //public List<int> GO()
        //{
        //    //...
        //}
        public void Anneal()
        {
            // Brucock (брусок) - это list рандомных значений из pool
            // atom - эл бруска 
            List<int> currentBrucock = CreateBrucock(pool);
            PrintBrucock("Брусок: ", currentBrucock);
            int currentSize = GetBrucockSize(currentBrucock);
            Console.WriteLine("Size: " + currentSize);
            Console.WriteLine("---");

            while (currentT > 0.1)
            {
                List<int> newBrucock = ChangeBrucock(currentBrucock);
                PrintBrucock("кандидат брусок: ", newBrucock);
                int newBrucockSize = GetBrucockSize(newBrucock);
                Console.WriteLine("New Brusok Size: " + newBrucockSize);
                // функция энергии
                int sizeDiffence = newBrucockSize - GetBrucockSize(currentBrucock);
                Console.WriteLine(sizeDiffence);
                if (sizeDiffence <= 0)
                {
                    currentBrucock = newBrucock;
                    PrintBrucock("size<=0, current brusok = ", currentBrucock);
                }
                else
                {
                    Console.WriteLine("size>0");
                    double polikarpia = Math.Exp(-sizeDiffence / currentT);
                    double p = random.NextDouble();
                    Console.WriteLine(polikarpia + ", " + p);
                    
                    if (polikarpia > p)
                    {
                        currentBrucock = newBrucock;
                    }
                }
                PrintBrucock("Itog brusok: ", currentBrucock);
                Console.WriteLine("Itog Size: " + GetBrucockSize(currentBrucock));
                Console.WriteLine("---");
                //currentT -= 1;
                currentT *= 0.5;
                // 0.5 * (T_i / T0)
            }
        }

        private List<int> ChangeBrucock(List<int> brucock)
        {
            List<int> newBrucock = new List<int>(brucock);
            int count = (int)(0.5 * (currentT / T) * brucock.Count);


            List<int> tmp = newBrucock.OrderBy(x => random.Next()).Take(count).ToList();
            PrintBrucock("tmp is ", tmp);

            for (int i = 0; i < brucock.Count; i++)
            {

            }
            
            for (int i = 0; i < count; i++)
            {
                // пикаем рнд эл из бруска
                int brucockIndex = random.Next(0, brucock.Count);

                //Console.WriteLine("Индекс элемента, который изменится: " + brucockIndex);
                int currentEl = brucock[brucockIndex];
                //Console.WriteLine("Элемент бруска: " + currentEl);

                // формируем список кандидат для поиска нового эла для бруска
                List<int> candidates = new List<int>(pool[brucockIndex]);
                //PrintBrucock("Кандидаты: ", candidates);
                // чтобы второй раз не пикнуть этот же эл
                candidates.Remove(currentEl);
                //PrintBrucock("Кандидаты после очищения: ", candidates);

                int candidateIndex = random.Next(0, candidates.Count);
                newBrucock[brucockIndex] = candidates[candidateIndex];
            }
            return newBrucock;
        }

        private List<int> CreateBrucock(List<int>[] pool)
        {
            Random random = new Random();
            List<int> brucock = new List<int>();
            foreach (var subpool in pool)
            {
                //string joinedList = String.Join(", ", subpool);
                //Console.WriteLine(joinedList, subpool.Count);
                
                int randomIndex = random.Next(0, subpool.Count);
                int randomItem = subpool[randomIndex];
                //Console.WriteLine("Random item: " + randomItem);
                brucock.Add(randomItem);
            }
            return brucock;
        }
        private void PrintBrucock(string s, List<int> brucock)
        {
            string joinedList = String.Join(", ", brucock);
            Console.WriteLine(s + joinedList, brucock.Count);
        }

        private int GetBrucockSize(List<int> brucock)
        {
            int size = 0;
            foreach (var atom in brucock.Distinct().ToList())
            {

                size += monomials[atom].GetSize();
            }
            return size;
        }

    }
}
