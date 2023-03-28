using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace minknf
{
    class DisjunctiveMonomial
    {
        private int[] vars;

        public DisjunctiveMonomial(int size)
        {
            vars = new int[size];
        }

        public DisjunctiveMonomial(String str)
        {
            str = str.Replace("v", String.Empty);
            str = str.Replace("x", String.Empty);

            vars = new int[str.Replace("-", String.Empty).Length];

            bool xval = false;
            foreach(var c in str)
            {
                if (c == '-')
                {
                    xval = true;
                    continue;
                }
                vars[int.Parse(c.ToString()) - 1] = Convert.ToInt32(xval);
                xval = false;
            }
        }
        public DisjunctiveMonomial(String str, int sz)
        {
            str = str.Replace("v", String.Empty);
            str = str.Replace("x", String.Empty);
            vars = Enumerable.Repeat(2, sz).ToArray(); ;
            bool xval = false;
            foreach (var c in str)
            {
                if (c == '-')
                {
                    xval = true;
                    continue;
                }
                vars[int.Parse(c.ToString()) - 1] = Convert.ToInt32(xval);
                xval = false;
            }
        }
        public DisjunctiveMonomial(DisjunctiveMonomial other)
        {
            this.vars = new int[other.GetSize()];
            other.vars.CopyTo(this.vars, 0);
        }

        public int Distance(DisjunctiveMonomial other)
        {
            int dist = 0;
            for(int i = 0; i < this.GetSize(); i++)
            {
                dist += (this[i] == other[i] ? 0 : 1);
            }
            return dist;
        }

        public DisjunctiveMonomial Consume(DisjunctiveMonomial other)
        {
            if (this.Distance(other) != 1)
                throw new Exception("Нельзя полготить");

            DisjunctiveMonomial newMonomial = new DisjunctiveMonomial(this);
            for (int i = 0; i < this.GetSize(); i++)
                if (this[i] != other[i] && this[i] != 2)
                {
                    newMonomial[i] = 2;
                    break;
                }
            return newMonomial;
        }
        public bool DoesCover(DisjunctiveMonomial other)
        {
            if (this.GetSize() != other.GetSize())
                throw new Exception("Разные размеры");

            for(int i = 0; i < this.GetSize(); i++)
            {
                if (this[i] != other[i] && this[i] != 2 && other[i] != 2)
                    return false;
            }
            return true;
        }
        public int[] GetDisjuncts()
        {
            return vars;
        }
        public int GetSize()
        {
            return vars.Length;
        }

        public int this[int key]
        {
            get => vars[key];
            set => vars[key] = value;
        }

        public override string ToString()
        {
            string result = "(";
            for(int i = 0; i < vars.Length; i++)
            {
                if (vars[i] == 2)
                    continue;

                if (vars[i] == 1)
                    result += "-";

                result += "x" + (i+1) + "v";
            }
            result = result.Remove(result.Length - 1);
            result += ")";
            return result;
        }
    }
}
