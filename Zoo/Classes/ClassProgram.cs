using Zoo.Classes;

namespace Zoo
{
    partial class Program
    {
        public static (List<Enclosure>, List<Animal>, List<Species>, List<Employee>, List<Visitor>) GetZooObjects()
        {
            Species meerkat = new Species("Meerkat", new List<Species>());
            Species kakapo = new Species("Kakapo", new List<Species>());
            Species bengalTiger = new Species("Bengal Tiger", new List<Species>());
            Species panda = new Species("Panda", new List<Species>());
            Species python = new Species("Python", new List<Species>());
            Species dungenessCrab = new Species("Dungeness Crab", new List<Species>());
            Species gopher = new Species("Gopher", new List<Species>());
            Species cats = new Species("Cats", new List<Species>());
            Species penguin = new Species("Penguin", new List<Species>());
            // Dodawanie obiektów do listy
            meerkat.FavouriteFoods = new List<Species> { meerkat };
            bengalTiger.FavouriteFoods = new List<Species> { panda, gopher, cats };
            python.FavouriteFoods = new List<Species> { panda, bengalTiger };
            dungenessCrab.FavouriteFoods = new List<Species> { python };
            cats.FavouriteFoods = new List<Species> { gopher };
            penguin.FavouriteFoods = new List<Species> { bengalTiger };

            List<Species> speciesList = new List<Species> { meerkat, kakapo, bengalTiger, panda, python, dungenessCrab, gopher, cats, penguin };

            Animal harold = new Animal("Harold", 2, meerkat);
            Animal ryan = new Animal("Ryan", 1, meerkat);
            Animal jenkins = new Animal("Jenkins", 15, kakapo);
            Animal kaka = new Animal("Kaka", 10, kakapo);
            Animal ada = new Animal("Ada", 13, bengalTiger);
            Animal jett = new Animal("Jett", 2, panda);
            Animal conda = new Animal("Conda", 4, python);
            Animal samuel = new Animal("Samuel", 2, python);
            Animal claire = new Animal("Claire", 2, dungenessCrab);
            Animal andy = new Animal("Andy", 3, gopher);
            Animal arrow = new Animal("Arrow", 5, cats);
            Animal arch = new Animal("Arch", 1, penguin);
            Animal ubuntu = new Animal("Ubuntu", 1, penguin);
            Animal fedora = new Animal("Fedora", 1, penguin);

            List<Animal> animalList = new List<Animal> { harold, ryan, jenkins, kaka, ada, jett, conda, samuel, claire, andy, arrow, arch, ubuntu, fedora };

            Employee ricardo = new Employee("Ricardo", "Stallmano", 73, new List<Enclosure>());
            Employee steve = new Employee("Steve", "Irvin", 43, new List<Enclosure>());

            List<Employee> employeeList = new List<Employee> { ricardo, steve };

            // tworzenie obiektów klasy Enclosure
            Enclosure enclosure1 = new Enclosure("311", new List<Species> { penguin, python, panda }, animalList, ricardo);
            Enclosure enclosure2 = new Enclosure("Break", new List<Species> { cats, gopher, meerkat }, animalList, steve);
            Enclosure enclosure3 = new Enclosure("Jurasic Park", new List<Species> { kakapo, bengalTiger, dungenessCrab }, animalList, steve);

            // dodanie enclosurów do listy enclosures pracowników
            ricardo.Enclosures = new List<Enclosure> { enclosure1 };
            steve.Enclosures = new List<Enclosure> { enclosure2, enclosure3 };

            List<Enclosure> enclosureList = new List<Enclosure> { enclosure1, enclosure2, enclosure3 };

            Visitor visitor1 = new Visitor("Tomas", "German", new List<Enclosure> { enclosure1, enclosure2 });
            Visitor visitor2 = new Visitor("Silvester", "Ileen", new List<Enclosure> { enclosure3 });
            Visitor visitor3 = new Visitor("Basil", "Bailey", new List<Enclosure> { enclosure1, enclosure3 });
            Visitor visitor4 = new Visitor("Ryker", "Polly", new List<Enclosure> { enclosure2 });
            List<Visitor> visitorList = new List<Visitor> { visitor1, visitor2, visitor3, visitor4 };

            return (enclosureList, animalList, speciesList, employeeList, visitorList);
        }
        public static (List<IEnclosure>, List<IAnimal>, List<ISpecies>, List<IEmployee>, List<IVisitor>) GetZooInterfaceObjects(List<Enclosure> enclosureList, List<Animal> animalList, List<Species> speciesList, List<Employee> employeeList, List<Visitor> visitorList)
        {
            List<IEnclosure> enclosureAdapters = new List<IEnclosure>();
            foreach (Enclosure it in enclosureList)
            {
                enclosureAdapters.Add(new EnclosureAdapter(it));
            }
            List<IAnimal> animalAdapters = new List<IAnimal>();
            foreach (Animal it in animalList)
            {
                animalAdapters.Add(new AnimalAdapter(it));
            }
            List<ISpecies> speciesAdapters = new List<ISpecies>();
            foreach (Species it in speciesList)
            {
                speciesAdapters.Add(new SpeciesAdapter(it));
            }
            List<IEmployee> employeeAdapters = new List<IEmployee>();
            foreach (Employee it in employeeList)
            {
                employeeAdapters.Add(new EmployeeAdapter(it));
            }
            List<IVisitor> visitorAdapters = new List<IVisitor>();
            foreach (Visitor it in visitorList)
            {
                visitorAdapters.Add(new VisitorAdapter(it));
            }
            return (enclosureAdapters, animalAdapters, speciesAdapters, employeeAdapters, visitorAdapters);
        }
        public static void Lab4Work(List<Enclosure> enclosureList, List<Animal> animalList, List<Species> speciesList, List<Employee> employeeList, List<Visitor> visitorList)
        {
            List<EnclosureB> enclosureBList = new List<EnclosureB>();
            foreach (Enclosure it in enclosureList)
            {
                enclosureBList.Add(new EnclosureB(it.Name, it.Animals, it.Employee));
            }
            List<AnimalB> animalBList = new List<AnimalB>();
            foreach (Animal animal in animalList)
            {
                animalBList.Add(new AnimalB(animal.Name, animal.Age, animal.Species));
            }
            List<SpeciesB> speciesBList = new List<SpeciesB>();
            foreach (Species it in speciesList)
            {
                speciesBList.Add(new SpeciesB(it.Name, it.FavouriteFoods));
            }



            // Dla reprezentacji 5
            Console.WriteLine("Dla reprezentacji 5: ");
            List<IEnclosure> enclosureAdaptersB = new List<IEnclosure>();
            foreach (EnclosureB it in enclosureBList)
            {
                enclosureAdaptersB.Add(new EnclosureBAdapter(it));
            }
            List<IAnimal> animalAdaptersB = new List<IAnimal>();
            foreach (AnimalB it in animalBList)
            {
                animalAdaptersB.Add(new AnimalBAdapter(it));
            }
            List<IEnclosure> L1 = new List<IEnclosure>();
            foreach (IEnclosure e in enclosureAdaptersB)
            {
                int sum = 0;
                int i = 0;
                foreach (IAnimal a in e.Animals)
                {
                    i++;
                    sum += a.Age;
                }
                if (sum / i < 3)
                    L1.Add(e);
            }
            foreach (IEnclosure e in L1)
            {
                Console.WriteLine(e.Name);
            }

            // Dla reprezentacji 6
            Console.WriteLine("Dla reprezentacji 6: ");
            List<EnclosureM> enclosureMList = new List<EnclosureM>();
            foreach (Enclosure it in enclosureList)
            {
                enclosureMList.Add(new EnclosureM(it));
            }
            List<IEnclosure> enclosureAdaptersM = new List<IEnclosure>();
            foreach (EnclosureM it in enclosureMList)
            {
                enclosureAdaptersM.Add(new EnclosureMAdapter(it, animalList, employeeList));
            }
            List<IEnclosure> L2 = new List<IEnclosure>();
            foreach (IEnclosure e in enclosureAdaptersM)
            {
                int sum = 0;
                int i = 0;
                foreach (IAnimal a in e.Animals)
                {
                    i++;
                    sum += a.Age;
                }
                if (sum / i < 3)
                    L2.Add(e);
            }
            foreach (IEnclosure e in L2)
            {
                Console.WriteLine(e.Name);
            }

            //var test = new EnclosureM(enclosure1);
            //foreach (var it in test.map)
            //{
            //    Console.WriteLine($"key: {it.Key} val: {it.Value}");
            //}
            //EnclosureMAdapter testAd = new EnclosureMAdapter(test,animalList,employeeList);
            //Console.Write(enclosure1);
            //Console.WriteLine(testAd.Name);
            //Console.WriteLine(testAd.Employee);
            //foreach (var a in testAd.Animals)
            //{
            //    Console.WriteLine(a);
            //}
            //{
            //    Console.WriteLine(ad.Name);
            //}
            //EnclosureB enclosureB1 = new EnclosureB(enclosure1.Name, enclosure1.Animals, enclosure1.Employee);
            //IEnclosure adapter = new EnclosureBAdapter(enclosureB1);
            //
            //Console.WriteLine(enclosure1);
            //Console.WriteLine(adapter.Name);
            //Console.WriteLine(adapter.Animals.First().Name);
            //Console.WriteLine(adapter.Employee.Name);
            //Console.WriteLine(adapter.);
            //Console.WriteLine(panda.name);                             
            //Console.WriteLine(enclosure1.Animals.First().name);

            //panda.name = "kung fu panda";
            //Console.WriteLine(panda.name);
            //Console.WriteLine(bengalTiger.favouriteFoods.First().name);
        }
    }
}