using System;
using System.Collections.Generic;
using System.Text;

namespace minknf
{
    class PopulationMatrix
    {
        int[,] matrix;
        public PopulationMatrix(int n, int m)
        {
            Random random = new Random();
            matrix = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matrix[i, j] = random.Next(0, 2);
              
        }
        public int GetColumns()
        {
            return matrix.GetLength(1); 
        }
        public int GetRows()
        {
            return matrix.GetLength(0);
        }
        public int this[int i, int j]
        {
            get => matrix[i,j];
            set => matrix[i,j] = value;
        }
        public int[,] GetMatrix()
        {
            return matrix;
        }
    }
}
