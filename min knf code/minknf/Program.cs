using System;
using System.Collections.Generic;
using System.Linq;


namespace minknf
{
    class Program
    {
        static void Main(string[] args)
        {
            String sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1vx2vx3)&(x1v-x2vx3)";
            //sknf = "(x1vx2vx3)&(x1vx2v-x3)&(-x1v-x2v-x3)";
            sknf = "(x1vx2v-x3v-x4)&(x1v-x2vx3vx4)&(x1v-x2vx3v-x4)&(x1v-x2v-x3v-x4)&(-x1vx2vx3vx4)&(-x1vx2vx3v-x4)&(-x1vx2v-x3vx4)&(-x1vx2v-x3v-x4)&(-x1v-x2vx3vx4)&(-x1v-x2vx3v-x4)";
            //sknf = "(x1vx2v-x3)&(x1v-x2vx3)&(-x1v-x2v-x3)&(x1v-x2v-x3)";
            //sknf = "(x1vx2v-x3v-x4vx5vx6)&(x1v-x2vx3vx4vx5v-x6)&(x1v-x2vx3v-x4vx5vx6)&(x1v-x2v-x3v-x4vx5vx6)&(-x1vx2vx3vx4vx5vx6)&(-x1vx2vx3v-x4vx5vx6)&(-x1vx2v-x3vx4v-x5vx6)&(-x1vx2v-x3v-x4vx5vx6)&(-x1v-x2vx3vx4v-x5vx6)&(-x1v-x2vx3v-x4vx5vx6)";

            KNF knf = new KNF(sknf);
            knf.MinimizeKnf();
            Console.WriteLine(knf.ToString());
            //Console.WriteLine(10_000 + 1);
        }
    }
}
