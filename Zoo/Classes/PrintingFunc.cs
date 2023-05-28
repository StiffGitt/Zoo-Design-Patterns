using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public static class PrintingFunc
    {
        public static string PrintZooObject(Object obj)
        {
            switch (obj)
            {
                case IEnclosure:
                    return PrintEnclosure(obj as IEnclosure);
                case IAnimal:
                    return PrintAnimal(obj as IAnimal);
                case ISpecies:
                    return PrintSpecies(obj as ISpecies);
                case IEmployee:
                    return PrintEmployee(obj as IEmployee);
                case IVisitor:
                    return PrintVisitor(obj as IVisitor);
                default:
                    return "not valid class";
            }
        }

        public static string PrintEnclosure(IEnclosure obj)
        {
            return $"Enclosure: {obj.Name}";
        }
        public static string PrintAnimal(IAnimal obj)
        {
            return $"Animal: {obj.Name}, Age: {obj.Age}";
        }
        public static string PrintSpecies(ISpecies obj)
        {
            return $"Species: {obj.Name}";
        }
        public static string PrintEmployee(IEmployee obj)
        {
            return $"Employee: {obj.Name} {obj.Surname}, Age: {obj.Age}";
        }
        public static string PrintVisitor(IVisitor obj)
        {
            return $"Visitor: {obj.Name} {obj.Surname}";
        }

    }
}
