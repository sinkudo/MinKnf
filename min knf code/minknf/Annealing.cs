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
        double T = 100;
        double curT = 100;
        // (0; 1)
        double coef = 1;

        List<int> canditateInds = new List<int>();
        public Annealing(int[,] matrix, List<int> absoluteIndexes, List<DisjunctiveMonomial> monomials): base(matrix, absoluteIndexes, monomials)
        {
            for(int i = 0; i < pool.Length; i++)
            {
                if (pool[i].Count > 1)
                    canditateInds.Add(i);
            }
        }
        public List<int> Anneal(double T, double minT, int function, int C, int repeats, double coef)
        {
            int mxmon = 0;
            foreach (var mon in monomials)
                mxmon = Math.Max(mon.GetSize(), mxmon);
            List<double> sizes = new List<double>();
            List<int> currentBrucock = CreateBrucock(pool);
            int currentSize = GetBrucockSize(currentBrucock);
            Console.WriteLine("---");
            int i = 0;
            curT = T;
            
            while (curT > minT && i < repeats)
            {
                List<int> newBrucock = ChangeBrucock(currentBrucock);
                int newBrucockSize = GetBrucockSize(newBrucock);
                double newBrusockEnergy = (double)(newBrucock.Distinct().Count() * 10 + GetBrucockSize(newBrucock)) / (double)(pool.Length * 10 + mxmon * pool.Length);
                double BrusockEnergy = (double)(currentBrucock.Distinct().Count() * 10 + GetBrucockSize(currentBrucock))/(double)(pool.Length * 10 + mxmon * pool.Length);
                double sizeDifference = (newBrusockEnergy - BrusockEnergy) * 10_000 * coef;
                if (sizeDifference <= 0)
                {
                    currentBrucock = newBrucock;
                }
                else
                {
                    double polikarpia = Math.Exp(-sizeDifference / curT);
                    double p = random.NextDouble();
                    if (p < polikarpia)
                    {
                        currentBrucock = newBrucock;
                    }
                } 
                sizes.Add(BrusockEnergy);
                switch (function)
                {
                    // Больцмана
                    case 1:
                        curT = T / Math.Log10(1 + i++);
                        break;
                    // Коши
                    case 2:
                        curT = T / i++;
                        break;
                    // Сверхбыстрый
                    case 3:
                        curT = T / Math.Exp(C / i++);
                        break;
                }
            }
            //Console.WriteLine(qqq + " " + www);

            //PrintBrucock("", currentBrucock);
            FileWorker.SaveData(sizes);

            return ToAbsolute(currentBrucock);
        }

        private List<int> ChangeBrucock(List<int> brucock)
        {
            List<int> newBrucock = new List<int>(brucock);
           
            //int count = (int)(0.5 * (currentT / T) * brucock.Count);
            int count = (int)Math.Ceiling((0.5 * (curT / T) * brucock.Count));

            //Console.WriteLine("count " +count);

            List<int> tmp = new List<int>();
            if (count >= canditateInds.Count)
                tmp = canditateInds;
            else
                tmp = canditateInds.OrderBy(x => random.Next()).Take(count).ToList();
            //PrintBrucock("brucock ", newBrucock);
            //PrintBrucock("tmp is ", tmp);
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
            //Console.WriteLine("123");
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
