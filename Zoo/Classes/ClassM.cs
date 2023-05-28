using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    class EnclosureM
    {
        public int id;
        public Dictionary<string, string> map;
        private static int counter = 0;

        public EnclosureM():this(new Enclosure())
        {
            //id = counter++;
            //map = new Dictionary<string, string>();
        }
        public EnclosureM(Enclosure enclosure)
        {
            map = MapFunc.GetMap(enclosure);
            id = counter++;
        }
    }
    class EnclosureMAdapter : IEnclosure
    {
        private EnclosureM enclosureM;
        private readonly List<Animal> animalsList;
        private readonly List<Employee> employeeList;

        public EnclosureMAdapter(EnclosureM enclosureM, List<Animal> animalsList, List<Employee> employeeList)
        {
            this.enclosureM = enclosureM;
            this.animalsList = animalsList;
            this.employeeList = employeeList;
        }

        public EnclosureMAdapter()
        {
            this.enclosureM = new EnclosureM();
            this.animalsList = new List<Animal>();
            this.employeeList = new List<Employee>();
        }

        public string Name { get { return enclosureM.map["Name"]; } set { enclosureM.map["Name"] = value; } }

        public List<IAnimal> Animals
        {
            get
            {
                List<IAnimal> animals = new List<IAnimal>();
                for (int i = 0; i < int.Parse(enclosureM.map["Animals.Size()"]); i++)
                {
                    foreach (Animal a in animalsList)
                    {
                        if (a.ToString() == enclosureM.map[$"Animals[{i}]"])
                        {
                            animals.Add(new AnimalAdapter(a));
                            break;
                        }
                    }
                }
                return animals;
            }
        }

        public IEmployee Employee
        {
            get
            {
                foreach (Employee e in employeeList)
                {
                    if (e.ToString() == enclosureM.map[$"Employee"])
                    {
                        return new EmployeeAdapter(e);
                    }
                }
                return null;
            }
        }
    }
    class AnimalM
    {
        public int id;
        public Dictionary<string, string> map;
        private static int counter = 0;

        public AnimalM() : this(new Animal())
        {
            //id = counter++;
            //map = new Dictionary<string, string>();
        }
        public AnimalM(Animal animal)
        {
            map = MapFunc.GetMap(animal);
            id = counter++;
        }
    }
    class AnimalMAdapter : IAnimal
    {
        private AnimalM animalM;
        private readonly List<Species> speciesList;

        public AnimalMAdapter(AnimalM animalM, List<Species> speciesList)
        {
            this.animalM = animalM;
            this.speciesList = speciesList;
        }
        public AnimalMAdapter()
        {
            this.animalM = new AnimalM();
            this.speciesList = new List<Species>();
        }

        public string Name { get { return animalM.map["Name"]; } set { animalM.map["Name"] = value; } }

        public int Age { get { return int.Parse(animalM.map["Age"]); } set { animalM.map["Age"] = value.ToString(); } }

        public ISpecies Species
        {
            get
            {
                foreach (Species e in speciesList)
                {
                    if (e.ToString() == animalM.map[$"Employee"])
                    {
                        return new SpeciesAdapter(e);
                    }
                }
                return null;
            }
        }
    }

    class SpeciesM
    {
        public int id;
        public Dictionary<string, string> map;
        private static int counter = 0;

        public SpeciesM(): this(new Species())
        {
            //id = counter++;
            //map = new Dictionary<string, string>();
        }
        public SpeciesM(Species species)
        {
            map = MapFunc.GetMap(species);
            id = counter++;
        }
    }
    class SpeciesMAdapter : ISpecies
    {
        private SpeciesM speciesM;
        private readonly List<Species> speciesList;

        public SpeciesMAdapter(SpeciesM speciesM, List<Species> speciesList)
        {
            this.speciesM = speciesM;
            this.speciesList = speciesList;
        }
        public SpeciesMAdapter()
        {
            this.speciesM = new SpeciesM();
            this.speciesList = new List<Species>();
        }
        public string Name { get { return speciesM.map["Name"]; } set { speciesM.map["Name"] = value; } }

        public List<ISpecies> FavouriteFoods
        {
            get
            {
                List<ISpecies> favouriteFoods = new List<ISpecies>();
                for (int i = 0; i < int.Parse(speciesM.map["FavouriteFoods.Size()"]); i++)
                {
                    foreach (Species a in speciesList)
                    {
                        if (a.ToString() == speciesM.map[$"FavouriteFoods[{i}]"])
                        {
                            favouriteFoods.Add(new SpeciesAdapter(a));
                            break;
                        }
                    }
                }
                return favouriteFoods;
            }
        }
    }

    class EmployeeM
    {
        public int id;
        public Dictionary<string, string> map;
        private static int counter = 0;

        public EmployeeM():this(new Employee())
        {
            //id = counter++;
            //map = new Dictionary<string, string>();
        }
        public EmployeeM(Employee employee)
        {
            map = MapFunc.GetMap(employee);
            id = counter++;
        }
    }
    class EmployeeMAdapter : IEmployee
    {
        private EmployeeM employeeM;
        private readonly List<Enclosure> enclosureList;

        public EmployeeMAdapter(EmployeeM employeeM, List<Enclosure> enclosureList)
        {
            this.employeeM = employeeM;
            this.enclosureList = enclosureList;
        }
        public EmployeeMAdapter()
        {
            this.employeeM = new EmployeeM();
            this.enclosureList = new List<Enclosure>();
        }
        public string Name { get { return employeeM.map["Name"]; } set { employeeM.map["Name"] = value; } }


        public string Surname { get { return employeeM.map["Surname"]; } set { employeeM.map["Surname"] = value; } }

        public int Age { get { return int.Parse(employeeM.map["Age"]); } set { employeeM.map["Age"] = value.ToString(); } }

        public List<IEnclosure> Enclosures
        {
            get
            {
                List<IEnclosure> enclosures = new List<IEnclosure>();
                for (int i = 0; i < int.Parse(employeeM.map["Enclosures.Size()"]); i++)
                {
                    foreach (Enclosure a in enclosureList)
                    {
                        if (a.ToString() == employeeM.map[$"Enclosures[{i}]"])
                        {
                            enclosures.Add(new EnclosureAdapter(a));
                            break;
                        }
                    }
                }
                return enclosures;
            }
        }
    }

    class VisitorM
    {
        public int id;
        public Dictionary<string, string> map;
        private static int counter = 0;

        public VisitorM():this(new Visitor())
        {
            //id = counter++;
            //map = new Dictionary<string, string>();
        }
        public VisitorM(Visitor visitor)
        {
            map = MapFunc.GetMap(visitor);
            id = counter++;
        }
    }
    class VisitorMAdapter : IVisitor
    {
        private VisitorM visitorM;
        private readonly List<Enclosure> enclosureList;

        public VisitorMAdapter(VisitorM visitorM, List<Enclosure> enclosureList)
        {
            this.visitorM = visitorM;
            this.enclosureList = enclosureList;
        }
        public VisitorMAdapter()
        {
            this.visitorM = new VisitorM();
            this.enclosureList = new List<Enclosure>();
        }

        public string Name { get { return visitorM.map["Name"]; } set { visitorM.map["Name"] = value; } }


        public string Surname { get { return visitorM.map["Surname"]; } set { visitorM.map["Name"] = value; } }

        public List<IEnclosure> VisitedEnclosures
        {
            get
            {
                List<IEnclosure> enclosures = new List<IEnclosure>();
                for (int i = 0; i < int.Parse(visitorM.map["VisitedEnclosures.Size()"]); i++)
                {
                    foreach (Enclosure a in enclosureList)
                    {
                        if (a.ToString() == visitorM.map[$"VisitedEnclosures[{i}]"])
                        {
                            enclosures.Add(new EnclosureAdapter(a));
                            break;
                        }
                    }
                }
                return enclosures;
            }
        }
    }

    public static class MapFunc
    {
        public static bool IsList(object o)
        {
            return o is IList &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        public static Dictionary<string, string> GetMap(object o)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (PropertyInfo p in o.GetType().GetProperties())
            {
                var pName = p.Name;
                var pVal = p.GetValue(o);
                if (pVal == null)
                {
                    map.Add(pName, "null");
                    continue;
                }
                if (IsList(pVal))
                {
                    int i = 0;
                    var en = (IEnumerable)pVal;
                    foreach (var it in en)
                    {
                        map.Add($"{pName}[{i++}]", it.ToString());
                    }
                    map.Add($"{pName}.Size()", $"{i}");
                }
                else
                {
                    map.Add(pName, pVal.ToString());
                }
            }
            return map;
        }
    }
}
