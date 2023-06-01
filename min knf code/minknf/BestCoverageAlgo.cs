using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace minknf
{
    class BestCoverageAlgo
    {
        protected int[,] coverageMatrix;
        protected private List<int>[] genePool;
        protected private List<int> geneIndexes;
        protected Random random = new Random();
        protected bool CompareFitness(int[] possibleReplace, int[] currentIndividum)
        {

            int replaceCount = possibleReplace.Distinct().Count();
            int curCount = currentIndividum.Distinct().Count();
            return (replaceCount < curCount) || (replaceCount == curCount && WeightOfIndividum(possibleReplace) > WeightOfIndividum(currentIndividum));
        }
        private int WeightOfIndividum(int[] individum)
        {
            int weight = 0;
            foreach (var i in individum.Distinct().ToArray())
            {
                for (int j = 0; j < coverageMatrix.GetLength(1); j++)
                    weight += coverageMatrix[i, j];
            }
            return weight;
        }
    }

}
