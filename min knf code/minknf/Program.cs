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
            sknf = "(x1vx2v-x3v-x4)&(x1v-x2vx3vx4)&(x1v-x2vx3v-x4)&" +
                "(x1v-x2v-x3v-x4)&(-x1vx2vx3vx4)&(-x1vx2vx3v-x4)&(-x1vx2v-x3vx4)&" +
                "(-x1vx2v-x3v-x4)&(-x1v-x2vx3vx4)&(-x1v-x2vx3v-x4)";
            //KNF knf = new KNF("(x1vx2vx3)&(-x1v-x2v-x3)");
            KNF knf = new KNF(sknf);
            //Console.WriteLine(knf.ToString());
            //KNF minknf = knf.ConsumeMonomials(knf.monomials);
            knf.MinimizeKnf();
            Console.WriteLine(knf.ToString());
            //List<int> q = new List<int>();
            //q.Add(2);
            //foreach(int i in q)
            //    Console.WriteLine(i);
            //Console.WriteLine(knf.ToString());
            //Console.WriteLine(q.Count);
            //foreach (DisjunctiveMonomial i in q)
            //{
            //    Console.WriteLine(i.ToString());
            //}

            //DisjunctiveMonomial a = new DisjunctiveMonomial("x1v-x2v-x3");
            //DisjunctiveMonomial b = new DisjunctiveMonomial("-x2v-x3", 3);
            //foreach(int i in a.vars)
            //{
            //    Console.Write("{0} ", i);
            //}
            //Console.WriteLine();
            //foreach (int i in b.vars)
            //{
            //    Console.Write("{0} ", i);
            //}
            //Console.WriteLine(b.DoesCover(a));
        }
    }
}
