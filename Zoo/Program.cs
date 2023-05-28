using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Classes;

namespace Zoo
{
    partial class Program
    {
        
        public static void Main()
        {
            var (enclosureList, animalList, speciesList, employeeList, visitorList) = GetZooObjects();
            var (enclosureAdapters, animalAdapters, speciesAdapters, employeeAdapters, visitorAdapters) = GetZooInterfaceObjects(enclosureList, animalList, speciesList, employeeList, visitorList);
            //Lab4Work(enclosureList, animalList, speciesList, employeeList, visitorList);
            //Lab6Work(enclosureList, animalList, speciesList, employeeList, visitorList);
            CommandWork(enclosureAdapters, animalAdapters, speciesAdapters, employeeAdapters, visitorAdapters);
        }

    }
}