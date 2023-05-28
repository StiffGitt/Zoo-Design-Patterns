using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Collections
{
    public interface IMyComparator
    {
        public int Compare(Object item1, Object item2);
    }
    public class CompareAnimalsByAge : IMyComparator
    {
        
        public int Compare(Object item1, Object item2)
        {
            IAnimal a1 = item1 as IAnimal;
            IAnimal a2 = item2 as IAnimal;

            if(a1.Age > a2.Age)
                return 1;
            else if (a1.Age == a2.Age)
                return 0;
            else 
                return -1;
        }
    }
    public class CompareInts : IMyComparator
    {

        public int Compare(Object item1, Object item2)
        {
            int i1 = (int)item1;
            int i2 = (int)item2;
            if (i1 > i2)
                return 1;
            else if (i1 == i2)
                return 0;
            else
                return -1;
        }
    }
    public class CompareByDefault : IMyComparator
    {
        public int Compare(Object item1, Object item2)
        {
            return 0;
        }
    }
}
