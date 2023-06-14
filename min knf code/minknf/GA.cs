using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace minknf
{
    class GA : BestCoverageAlgo
    {
        private int epochs;
        private double mutationChance;
        private double crossoverChance;
        private int populationSize;
        PopulationMatrix populationmatrix;
        public GA(int[,] matrix, List<DisjunctiveMonomial> monomials, int epochs, int populationSize, double mutationChance, double crossoverChance, List<int> geneIndexes) : base(matrix, geneIndexes, monomials)
        {
            this.epochs = epochs;
            this.populationSize = populationSize;
            this.mutationChance = mutationChance;
            this.crossoverChance = crossoverChance;
            //coverageMatrix = matrix;
            //initPool();
            populationmatrix = new PopulationMatrix(populationSize, pool.Count(), pool);
        }

        public List<int> GO()
        {
            if (coverageMatrix.GetLength(0) == 0 || coverageMatrix.GetLength(1) == 0)
                return null;
            Console.WriteLine("genetic go");
            int mxmon = 0;
            foreach (var mon in monomials)
                mxmon = Math.Max(mon.GetSize(), mxmon);
            for (int i = 0; i < epochs; i++)
            {
                Tuple<int[], int[]> childs = null;
                childs = Crossover();
                Mutate(childs.Item1);
                Mutate(childs.Item2);
                double en = (double)(childs.Item1.ToList().Distinct().Count() * 10 + GetIndSize(childs.Item1.ToList())) / (double)(pool.Length * 10 + mxmon * pool.Length);
                Console.WriteLine(en);
                //Console.WriteLine(WeightOfIndividum(childs.Item1) + " " + WeightOfIndividum(childs.Item2));
                ReplaceRandomIndividum(childs.Item1);
                ReplaceRandomIndividum(childs.Item2);
            }
            return BestIndividum();
        }
        private Tuple<int[],int[]> Crossover()
        {
            //Console.WriteLine("crossover");
            int n = populationmatrix.GetRows();
            int[] a = Matrix.GetRow(populationmatrix.GetMatrix(), random.Next(0, n));
            int[] b = Matrix.GetRow(populationmatrix.GetMatrix(), random.Next(0, n));
            Tuple<int[], int[]> ans = new Tuple<int[], int[]>(a,b);
            if (random.NextDouble() <= crossoverChance) {
                ans = CrossoverOperation(a, b);
            }
            return ans;
        }
        private Tuple<int[], int[]> CrossoverOperation(int[] a, int[] b)
        {
            int[] child1 = new int[a.Length];
            int[] child2 = new int[a.Length];
            int crossoverPoint = random.Next(0, a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                if (i <= crossoverPoint)
                {
                    child1[i] = a[i];
                    child2[i] = b[i];
                }
                else
                {
                    child1[i] = b[i];
                    child2[i] = a[i];
                }
            }
            return new Tuple<int[], int[]>(child1, child2);
        }
        private void Mutate(int[] child)
        {
            if (random.NextDouble() > mutationChance)
                return;
            //Console.WriteLine("mutate");
            MutationOperation(child);
        }
        private void MutationOperation(int[] child)
        {
            int mutationPoint = random.Next(0, child.Length);
            int ind = random.Next(0, pool[mutationPoint].Count());
            while (pool[mutationPoint][ind] == child[mutationPoint] && pool[mutationPoint].Count() > 1)
            {
                ind = random.Next(0, pool[mutationPoint].Count());
            }
            child[mutationPoint] = pool[mutationPoint][ind];
        }
        private void ReplaceRandomIndividum(int[] child)
        {
            int individumToReplace = random.Next(0, populationmatrix.GetRows());
            int[] individum = new int[child.Length];
            for(int j = 0; j < populationmatrix.GetColumns(); j++)
            {
                individum[j] = populationmatrix[individumToReplace, j];
            }
            if(CompareFitness(child, individum))
            {
                for (int j = 0; j < populationmatrix.GetColumns(); j++)
                    populationmatrix[individumToReplace, j] = child[j];
            }
        }
        private List<int> BestIndividum()
        {
            int[] best = Matrix.GetRow(populationmatrix.GetMatrix(), 0);
            int[] current = new int[best.Length];
            for(int i = 0; i < populationmatrix.GetRows(); i++)
            {
                for(int j = 0; j < populationmatrix.GetColumns(); j++)
                {
                    current[j] = populationmatrix[i, j];
                }
                if (CompareFitness(current, best))
                    best = current;

            }

            List<int> ans = new List<int>();
            foreach(var i in best.Distinct().ToList())
            {
                ans.Add(absoluteIndexes[i]);
            }
            
            return ans;
        }
        private int GetIndSize(List<int> ind)
        {
            int size = 0;
            foreach (var atom in ind.Distinct().ToList())
            {
                size += monomials[atom].GetSize();
            }
            return size;
        }
    }
}
