using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace minknf
{
    class Annealing : BestCoverageAlgo
    {
        List<DisjunctiveMonomial> monomials;
        public Annealing(int[,] matrix, List<int> absoluteIndexes, List<DisjunctiveMonomial> monomials): base(matrix, absoluteIndexes)
        {
            this.monomials = monomials;

            //suda smotri item1 = geneindexes, item2 = monomials
            foreach (var i in absoluteIndexes.Zip(monomials, Tuple.Create))
            {
                Console.WriteLine(i.Item1 + " " + i.Item2.ToString() + " " + i.Item2.GetSize());
            }

            //...
        }
        //public List<int> GO()
        //{
        //    //...
        //}
    }
}
