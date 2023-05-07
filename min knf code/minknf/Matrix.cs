using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.HighPerformance;
namespace minknf
{
    class Matrix
    {
        static public int[,] deleteRowsAndColumns(int[,] matrix, List<int> indsRowsToDelete, List<int> indsColsToDelete)
        {
            //Console.WriteLine();
            //Console.WriteLine(matrix.GetLength(0) + " " + indsRowsToDelete.Count);
            //foreach(int i in indsRowsToDelete)
            //    Console.WriteLine(i);
            //Console.WriteLine(matrix.GetLength(1) + " " + indsColsToDelete.Count);
            int[,] withoutCore = new int[matrix.GetLength(0) - indsRowsToDelete.Count, matrix.GetLength(1) - indsColsToDelete.Count];
            for (int i = 0, i1 = 0; i < matrix.GetLength(0); i++)
            {
                if (indsRowsToDelete.Contains(i))
                    continue;
                for (int j = 0, j1 = 0; j < matrix.GetLength(1); j++)
                {
                    if (indsColsToDelete.Contains(j))
                        continue;

                    //Console.WriteLine("!!" + i + " " + j);
                    withoutCore[i1, j1++] = matrix[i, j];
                }
                i1++;
            }
            //Console.WriteLine("deleteRAndC end");
            return withoutCore;
        }
        static public List<int> getHeaviestRows(int[,] matrix)
        {
            int mxweight = 0;
            List<int> heaviestRows = new List<int>();
            for(int i = matrix.GetLength(0) - 1; i >= 0; i++)
            {
                int curweight = 0;
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    curweight += matrix[i, j];
                }
                if (curweight >= mxweight)
                {
                    mxweight = curweight;
                    heaviestRows.Add(i);
                }
            }
            return heaviestRows;
        }
        static public Tuple<int,int> getHeaviestRow(int[,] matrix)
        {
            int ind = 0;
            int mxweight = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                int curweight = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    curweight += matrix[i, j];
                }
                if (curweight >= mxweight)
                {
                    mxweight = curweight;
                    ind = i;
                }
            }
            return new Tuple<int,int>(ind,mxweight);
        }
        static public int weightOfRow(Span2D<int> span, int ind)
        {
            var row = span.GetRowSpan(ind);
            int weight = 0;
            foreach (int i in row)
                weight += i;
            return weight;
        }
    }
}
