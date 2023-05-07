using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace minknf
{
    class GA
    {
        PopulationMatrix populationmatrix;
        int[,] coverageMatrix;
        Random random = new Random();
        int epochs = 10_000;
        double mutationChance = 1;
        int populationSize = 100;
        public GA(int[,] matrix)
        {
            coverageMatrix = matrix;
            populationmatrix = new PopulationMatrix(populationSize, matrix.GetLength(0));
            //Console.WriteLine(matrix.GetLength(0) + " " + matrix.GetLength(1));
            for (int i = 0; i < populationmatrix.GetRows(); i++)
            {
                for (int j = 0; j < populationmatrix.GetColumns(); j++)
                {
                    populationmatrix[i, j] = random.Next(0, 2);
                }
            }
            //CoutMatrix();
        }
        public int[] GO()
        {
            if (coverageMatrix.GetLength(0) == 0)
                return null;
            Console.WriteLine("genetic go");
            for (int i = 0; i < epochs; i++)
            {
                bool correct1 = false;
                bool correct2 = false;
                Tuple<int[], int[]> childs = null;

                while (!correct1 && !correct2)
                {
                    childs = Crossover();
                    correct1 = ValidateChild(childs.Item1);
                    correct2 = ValidateChild(childs.Item2);
                    if (correct1)
                    {
                        Mutate(childs.Item1);
                        correct1 = ValidateChild(childs.Item1);
                    }
                    else if (correct2)
                    {
                        Mutate(childs.Item2);
                        correct2 = ValidateChild(childs.Item2);
                    }
                }
                if (correct1)
                {
                    ReplaceRandomIndividum(childs.Item1);
                }
                else if (correct2)
                {
                    ReplaceRandomIndividum(childs.Item2);
                }
            }
            //Console.WriteLine("population");
            //for (int i = 0; i < populationmatrix.GetRows(); i++)
            //{
            //    for (int j = 0; j < populationmatrix.GetColumns(); j++)
            //        Console.Write("{0} ", populationmatrix[i, j]);
            //    Console.WriteLine();
            //}
            return BestIndividum();
        }
        private Tuple<int[],int[]> Crossover()
        {
            int n = populationmatrix.GetRows();
            int m = populationmatrix.GetColumns();

            //int crossoverPoint = random.Next(0, m);

            int[] child1 = new int[m];
            int[] child2 = new int[m];

            bool correct1 = false;
            bool correct2 = false;
            while (!correct1 && !correct2)
            {
                int parent1ind = random.Next(0, n);
                int parent2ind = random.Next(0, n);
                int crossoverPoint = random.Next(0, m);
                for (int i = 0; i < m; i++)
                {
                    if (i <= crossoverPoint)
                    {
                        child1[i] = populationmatrix[parent1ind, i];
                        child2[i] = populationmatrix[parent2ind, i];
                    }
                    else
                    {
                        child1[i] = populationmatrix[parent2ind, i];
                        child2[i] = populationmatrix[parent1ind, i];
                    }
                }
                correct1 = ValidateChild(child1);
                correct2 = ValidateChild(child2);
            }
            return new Tuple<int[], int[]>(child1, child2);
        }
        private bool ValidateChild(int[] child)
        {
            //Console.WriteLine("validation");
            int[] coverage = new int[coverageMatrix.GetLength(1)];

            //Console.WriteLine("child");
            //foreach(var i in child)
            //{
            //    Console.Write("{0} ", i);
            //}
            //Console.WriteLine();

            for (int i = 0; i < child.Length; i++)
            {
                if(child[i] == 1)
                {
                    for(int j = 0; j < coverageMatrix.GetLength(1); j++)
                    {
                        coverage[j] = Math.Max(coverage[j], coverageMatrix[i, j]);
                    }
                }
            }

            //foreach(var i in coverage)
            //    Console.Write("{0} ", i);
            //Console.WriteLine();
            //Console.WriteLine(DoesCover(coverage));
            //Console.WriteLine("validation over");

            return DoesCover(coverage);
        }
        private void Mutate(int[] child)
        {
            if (random.NextDouble() > mutationChance)
                return;
            MutationOperation(child);
            if (!ValidateChild(child))
                MutationOperation(child);
        }
        private void MutationOperation(int[] child)
        {
            int mutationPoint = random.Next(0, child.Length);
            child[mutationPoint] = (child[mutationPoint] + 1) % 2;
        }
        public void CoutMatrix()
        {
            for (int i = 0; i < populationmatrix.GetRows(); i++)
            {
                for (int j = 0; j < populationmatrix.GetColumns(); j++)
                {
                    Console.Write("{0} ", populationmatrix[i, j]);
                }
                Console.WriteLine();
            }
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
        private int Fitness(int[] individum)
        {
            return individum.Sum();
        }
        private bool CompareFitness(int[] possibleReplace, int[] currentIndividum)
        {
            return Fitness(possibleReplace) < Fitness(currentIndividum);
        }
        private int[] BestIndividum()
        {
            int[] best = Enumerable.Repeat<int>(1, populationmatrix.GetColumns()).ToArray();
            int[] current = new int[populationmatrix.GetColumns()];
            for(int i = 0; i < populationmatrix.GetRows(); i++)
            {
                for(int j = 0; j < populationmatrix.GetColumns(); j++)
                {
                    current[j] = populationmatrix[i, j];
                }

                //Console.WriteLine("current");
                //foreach (var q in current)
                //    Console.Write("{0} ", q);
                //Console.WriteLine();
                //Console.WriteLine("best");
                //foreach (var q in best)
                //    Console.Write("{0} ", q);
                //Console.WriteLine();
                //Console.WriteLine(ValidateChild(current) + " " + CompareFitness(current, best));

                if (ValidateChild(current) && CompareFitness(current, best))
                {
                     best = (int[])current.Clone();
                }
            }
            return best;
        }
        private int WeightOfIndividum(int[] individum)
        {
            return individum.Sum();
        }
        private bool DoesCover(int[] coverage)
        {
            return !coverage.Contains(0);
        }
    }
}
