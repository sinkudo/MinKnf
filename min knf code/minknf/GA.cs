using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace minknf
{
    class GA
    {
        private int epochs;
        private double mutationChance;
        private double crossoverChance;
        private int populationSize;
        private List<int>[] genePool;
        private List<int> geneIndexes;

        PopulationMatrix populationmatrix;
        int[,] coverageMatrix;
        Random random = new Random();
        public GA(int[,] matrix, int epochs, int populationSize, double mutationChance, double crossoverChance, List<int> geneIndexes)
        {
            this.epochs = epochs;
            this.populationSize = populationSize;
            this.mutationChance = mutationChance;
            this.crossoverChance = crossoverChance;
            this.geneIndexes = geneIndexes;

            coverageMatrix = matrix;
            genePool = new List<int>[matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                List<int> columnPool = new List<int>();
                for (int i = 0; i < matrix.GetLength(0); i++)
                    if (matrix[i, j] == 1)
                        columnPool.Add(i);
                genePool[j] = columnPool;

            }
            populationmatrix = new PopulationMatrix(populationSize, genePool.Count(), genePool);
        }

        public List<int> GO()
        {
            if (coverageMatrix.GetLength(0) == 0)
                return null;
            Console.WriteLine("genetic go");
            for (int i = 0; i < epochs; i++)
            {
                Tuple<int[], int[]> childs = null;
                childs = Crossover();
                Mutate(childs.Item1);
                Mutate(childs.Item2);
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
            int ind = random.Next(0, genePool[mutationPoint].Count());
            while (genePool[mutationPoint][ind] == child[mutationPoint] && genePool[mutationPoint].Count() > 1)
            {
                //for(int i = 0; i < genePool)
                ind = random.Next(0, genePool[mutationPoint].Count());
            }
            child[mutationPoint] = genePool[mutationPoint][ind];
        }
        private void ReplaceRandomIndividum(int[] child)
        {
            //Console.WriteLine("replace");
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
        private bool CompareFitness(int[] possibleReplace, int[] currentIndividum)
        {

            //return Fitness(possibleReplace) < Fitness(currentIndividum);
            int replaceCount = possibleReplace.Distinct().Count();
            int curCount = currentIndividum.Distinct().Count();

            return (replaceCount < curCount) || (replaceCount == curCount && WeightOfIndividum(possibleReplace) > WeightOfIndividum(currentIndividum));
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
                ans.Add(geneIndexes[i]);
            }
            
            return ans;
        }
        private int WeightOfIndividum(int[] individum)
        {
            int weight = 0;
            foreach(var i in individum.Distinct().ToArray())
            {
                for (int j = 0; j < coverageMatrix.GetLength(1); j++)
                    weight += coverageMatrix[i, j];
            }
            return weight;
        }
    }
}
