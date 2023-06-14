using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace minknf
{
    class BestCoverageAlgo
    {
        protected int[,] coverageMatrix;
        protected private List<int>[] pool;
        protected private List<int> absoluteIndexes;
        protected Random random = new Random();
        protected List<DisjunctiveMonomial> monomials;
        protected bool CompareFitness(int[] possibleReplace, int[] currentIndividum)
        {

            int replaceCount = possibleReplace.Distinct().Count();
            int curCount = currentIndividum.Distinct().Count();
            return (replaceCount < curCount) || (replaceCount == curCount && WeightOfIndividum(possibleReplace) > WeightOfIndividum(currentIndividum));
        }
        protected BestCoverageAlgo(int[,] matrix, List<int> absoluteIndexes, List<DisjunctiveMonomial> monomials)
        {
            coverageMatrix = matrix;
            this.monomials = monomials;
            this.absoluteIndexes = absoluteIndexes;
            initPool();
        }
        public int WeightOfIndividum(int[] individum)
        {
            int weight = 0;
            foreach (var i in individum.Distinct().ToArray())
            {
                for (int j = 0; j < coverageMatrix.GetLength(1); j++)
                    weight += coverageMatrix[i, j];
            }
            return weight;
        }
        protected void initPool()
        {
            pool = new List<int>[coverageMatrix.GetLength(1)];
            for (int j = 0; j < coverageMatrix.GetLength(1); j++)
            {
                List<int> columnPool = new List<int>();
                for (int i = 0; i < coverageMatrix.GetLength(0); i++)
                    if (coverageMatrix[i, j] == 1)
                        columnPool.Add(i);
                pool[j] = columnPool;

            }
        }
    }

}
