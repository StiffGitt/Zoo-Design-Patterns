using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Zoo.Classes;
using Zoo.Collections;

namespace Zoo
{
    partial class Program
    {
        public static void CommandWork(List<IEnclosure> enclosureList, List<IAnimal> animalList, List<ISpecies> speciesList, List<IEmployee> employeeList, List<IVisitor> visitorList)
        {

            Dictionary<string, IMyCollection<Object>> collections = GetZooCollection(enclosureList, animalList, speciesList, employeeList, visitorList);

            MyConsole myConsole = new MyConsole(collections);
            myConsole.Run();
            
        }
        
    }
}
