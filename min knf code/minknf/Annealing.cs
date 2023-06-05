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
        List<int> canditateInds = new List<int>();
        public Annealing(int[,] matrix, List<int> absoluteIndexes, List<DisjunctiveMonomial> monomials): base(matrix, absoluteIndexes)
        {
            this.monomials = monomials;
            for(int i = 0; i < pool.Length; i++)
            {
                if (pool[i].Count > 1)
                    canditateInds.Add(i);
            }
        }
        public List<int> Anneal()
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
                currentT -= 0.1;
                //currentT *= 0.5;
                // 0.5 * (T_i / T0)
            }
            return ToAbsolute(currentBrucock);
        }

        private List<int> ChangeBrucock(List<int> brucock)
        {
            List<int> newBrucock = new List<int>(brucock);
           
            //int count = (int)(0.5 * (currentT / T) * brucock.Count);
            int count = (int)Math.Ceiling((0.5 * (currentT / T) * brucock.Count));

            Console.WriteLine("count " +count);

            //List<int> tmp = Enumerable.Range(0, brucock.Count).OrderBy(x => random.Next()).Take(count).ToList();
            List<int> tmp = new List<int>();
            if (count >= canditateInds.Count)
                tmp = canditateInds;
            else
                tmp = canditateInds.OrderBy(x => random.Next()).Take(count).ToList();
            PrintBrucock("brucock ", newBrucock);
            PrintBrucock("tmp is ", tmp);
            foreach (var i in tmp)
            {
                List<int> candidates = new List<int>(pool[i]);
                int currentEl = brucock[i];
                candidates.Remove(currentEl);
                int candidateIndex = random.Next(0, candidates.Count);
                newBrucock[i] = candidates[candidateIndex];
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
        private List<int> ToAbsolute(List<int> brusok)
        {
            List<int> ans = new List<int>();
            foreach (var i in brusok.Distinct().ToList())
            {
                ans.Add(absoluteIndexes[i]);
            }
            //PrintBrucock("", ans);
            return ans;
        }
    }
}
