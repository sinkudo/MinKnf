using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace minknf
{
    class DisjunctiveMonomialEqualityComparer : IEqualityComparer<DisjunctiveMonomial>
    {
        public bool Equals(DisjunctiveMonomial x, DisjunctiveMonomial y)
        {
            return x.GetDisjuncts().SequenceEqual(y.GetDisjuncts());
        }

        public int GetHashCode(DisjunctiveMonomial obj)
        {
            //int vars = obj.GetDisjuncts().GetHashCode();
            //return vars;

            return ((IStructuralEquatable)obj.GetDisjuncts()).GetHashCode(EqualityComparer<int>.Default);
        }
    }
}
