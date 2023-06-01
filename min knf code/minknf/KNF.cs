using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance;

namespace minknf
{
    class KNF
    {
        private List<DisjunctiveMonomial> monomials = new List<DisjunctiveMonomial>();
        private string sknf;
        public KNF(String knf)
        {
            sknf = knf;
            int exponent = (int)Math.Log2(knf.Length);
            
            for (int i = 0; i < knf.Length; i++)
            {
                if (knf[i] == '1')
                    continue;
                String binval = Convert.ToString(i, 2);
                binval = binval.PadLeft(exponent, '0');
                monomials.Add(new DisjunctiveMonomial(binval));
            }
        }
        //public KNF(String knf)
        //{
        //    knf = knf.Replace("(", String.Empty);
        //    knf = knf.Replace(")", String.Empty);
        //    string[] strmonomials = knf.Split('&');
        //    foreach (var monomial in strmonomials)
        //        monomials.Add(new DisjunctiveMonomial(monomial));

        //}
        public KNF(List<DisjunctiveMonomial> other)
        {
            monomials = new List<DisjunctiveMonomial>(other);
        }
        public KNF(KNF knf, List<int> monomialsToGetInds)
        {
            List<DisjunctiveMonomial> implicants = new List<DisjunctiveMonomial>();
            foreach(var i in monomialsToGetInds)
            {
                implicants.Add(knf.monomials[i]);
            }
            knf = new KNF(implicants);
        }

        public String MinimizeKnfGA(int epochs = 10_000, int populationSize = 100, double mutationChance = 1, double crossoverChance = 1)
        {
            if (!sknf.Contains('0'))
                return "мкнф не существует";
            else if (!sknf.Contains('1'))
                return "0";

            int[,] implicantMatrix = Consume();

            List<int> coreImplicants = GetCoreImplicantsIndexesInMatrix(implicantMatrix);

            int[,] matrixWithoutCore = MatrixCore(implicantMatrix, coreImplicants);

            List<int> indexesInMatrixWithoutCore = Enumerable.Range(0, implicantMatrix.GetLength(0)).ToList();
            indexesInMatrixWithoutCore = indexesInMatrixWithoutCore.Except(coreImplicants).ToList();
            coreImplicants = coreImplicants.Distinct().ToList();

            GA ga = new GA(matrixWithoutCore, epochs, populationSize, mutationChance, crossoverChance, indexesInMatrixWithoutCore);
            List<int> best = ga.GO();

            List<DisjunctiveMonomial> ans = new List<DisjunctiveMonomial>();
            foreach (var i in coreImplicants)
                ans.Add(monomials[i]);
            if (best != null)
                foreach (var i in best)
                    ans.Add(monomials[i]);
            this.monomials = ans;
            return this.ToString();
        }
        public int[,] Consume()
        {
            List<DisjunctiveMonomial> startingMonomials = new List<DisjunctiveMonomial>(monomials);
            List<DisjunctiveMonomial> consRes = this.ConsumeMonomials(this.monomials);


            consRes = consRes.Distinct(new DisjunctiveMonomialEqualityComparer()).ToList();



            int[,] implicantMatrix = new int[consRes.Count, startingMonomials.Count];

            for (int i = 0; i < consRes.Count; i++)
            {
                for (int j = 0; j < startingMonomials.Count; j++)
                {
                    implicantMatrix[i, j] = (consRes[i].DoesCover(startingMonomials[j]) ? 1 : 0);
                }
            }

            this.setMonomials(consRes);

            return implicantMatrix;
        }
        public String MinimizeKNFAnnealing()
        {
            if (!sknf.Contains('0'))
                return "мкнф не существует";
            else if (!sknf.Contains('1'))
                return "0";

            List<DisjunctiveMonomial> startingMonomials = new List<DisjunctiveMonomial>(monomials);
            List<DisjunctiveMonomial> consRes = this.ConsumeMonomials(this.monomials);


            consRes = consRes.Distinct(new DisjunctiveMonomialEqualityComparer()).ToList();



            int[,] implicantMatrix = new int[consRes.Count, startingMonomials.Count];

            for (int i = 0; i < consRes.Count; i++)
            {
                for (int j = 0; j < startingMonomials.Count; j++)
                {
                    implicantMatrix[i, j] = (consRes[i].DoesCover(startingMonomials[j]) ? 1 : 0);
                }
            }

            this.setMonomials(consRes);

            List<int> coreImplicants = GetCoreImplicantsIndexesInMatrix(implicantMatrix);

            int[,] matrixWithoutCore = MatrixCore(implicantMatrix, coreImplicants);



            List<int> indexesInMatrixWithoutCore = Enumerable.Range(0, implicantMatrix.GetLength(0)).ToList();
            indexesInMatrixWithoutCore = indexesInMatrixWithoutCore.Except(coreImplicants).ToList();
            coreImplicants = coreImplicants.Distinct().ToList();

            //GA ga = new GA(matrixWithoutCore, epochs, populationSize, mutationChance, crossoverChance, indexesInMatrixWithoutCore);
            //List<int> best = ga.GO();

            //List<DisjunctiveMonomial> ans = new List<DisjunctiveMonomial>();
            //foreach (var i in coreImplicants)
            //    ans.Add(monomials[i]);
            //if (best != null)
            //    foreach (var i in best)
            //        ans.Add(monomials[i]);
            //this.monomials = ans;
            //return this.ToString();
            return "123";
        }
        private void setMonomials(List<DisjunctiveMonomial> toSet)
        {
            this.monomials = toSet;
        }
        private int[,] MatrixCore(int[,] matrix, List<int> coreIndexesRows)
        {
            coreIndexesRows = coreIndexesRows.Distinct().ToList();
            List<int> coreIndexesColumn = new List<int>();
            for (int i = 0; i < coreIndexesRows.Count; i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[coreIndexesRows[i], j] == 1)
                    {
                        coreIndexesColumn.Add(j);
                    }
                }
            }
            coreIndexesColumn = coreIndexesColumn.Distinct().ToList();
            return Matrix.deleteRowsAndColumns(matrix, coreIndexesRows, coreIndexesColumn);
        }
        private List<int> GetCoreImplicantsIndexesInMatrix(int[,] matrix)
        {
            List<int> coreIndexesRow = new List<int>();
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                int ones = 0, pos = -1;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (matrix[i, j] == 1)
                    {
                        ones++;
                        pos = i;
                    }
                }
                if (ones == 1)
                    coreIndexesRow.Add(pos);
            }
            return coreIndexesRow;
        }
        private List<DisjunctiveMonomial> ConsumeMonomials(List<DisjunctiveMonomial> monomialsToConsume)
        {

            List<DisjunctiveMonomial> monomialsAfterConsumption = new List<DisjunctiveMonomial>();
            List<int> toDelete = new List<int>();
            for (int i = 0; i < monomialsToConsume.Count - 1; i++)
            {
                for (int j = i + 1; j < monomialsToConsume.Count; j++)
                {
                    if (monomialsToConsume[i].Distance(monomialsToConsume[j]) == 1)
                    {
                        monomialsAfterConsumption.Add(monomialsToConsume[i].Consume(monomialsToConsume[j]));
                        toDelete.Add(i);
                        toDelete.Add(j);
                    }
                }
            }
            toDelete = toDelete.Distinct().ToList();
            toDelete.Sort();
            for (int i = toDelete.Count - 1; i >= 0; i--)
                monomialsToConsume.RemoveAt(toDelete[i]);
            
            if (monomialsAfterConsumption.Count == 0)
                return monomialsToConsume;
            return monomialsToConsume.Concat(ConsumeMonomials(monomialsAfterConsumption)).ToList();
        }
        public override string ToString()
        {
            List<String> stringMonomialList = new List<String>();
            foreach (DisjunctiveMonomial monomial in monomials)
                stringMonomialList.Add(monomial.ToString());
            return string.Join("&", stringMonomialList.ToArray());
        }
    }
}
