using Zoo.Classes;
using Zoo.Collections;

namespace Zoo
{
    partial class Program
    {
        public static void Lab6Work(List<Enclosure> enclosureList, List<Animal> animalList, List<Species> speciesList, List<Employee> employeeList, List<Visitor> visitorList)
        {
            
            Heap<IAnimal> animalHeap = new Heap<IAnimal>(new CompareAnimalsByAge());
            var (enclosureAdapters, animalAdapters, speciesAdapters, employeeAdapters, visitorAdapters) = GetZooInterfaceObjects(enclosureList, animalList, speciesList, employeeList, visitorList);
            int i = 0;
            foreach (var animal in animalAdapters)
            {
                Console.WriteLine($"{++i}: {animal.Name}, age: {animal.Age}");
                animalHeap.Insert(animal);
            }
            Console.WriteLine("\n\n\n");
            i = 0;
            IMyIterator<IAnimal> it = animalHeap.GetForwardIterator(); 
            for (IAnimal animal = it.First(); !it.IsCompleted; animal = it.Next())
            {
                Console.WriteLine($"{++i}: {animal.Name}, age: {animal.Age}");
            }
            Console.WriteLine("\n\n\n");
            i = 0;
            IMyIterator<IAnimal> itRev = animalHeap.GetReverseIterator();
            for (IAnimal animal = itRev.First(); !itRev.IsCompleted; animal = itRev.Next())
            {
                Console.WriteLine($"{++i}: {animal.Name}, age: {animal.Age}");
            }
            Console.WriteLine("\n\n\n");
            IAnimal? a = Algorithms.Find(animalHeap.GetForwardIterator(), x => x.Age == 13);
            if (a != null)
                Console.WriteLine($"{a.Name}, age: {a.Age}");
            else
                Console.WriteLine("nie ma");
            Console.WriteLine("\n\n\n");
            i = 0;
            Algorithms.ForEach(animalHeap.GetForwardIterator(), (a) =>
            {
                Console.WriteLine($"{++i}: {a.Name}, age: {a.Age}");
            });
            Console.WriteLine("\n\n\n");
            int count = Algorithms.CountIf(animalHeap.GetForwardIterator(), x => x.Age >= 5);
            Console.WriteLine($"Count = {count}");

            //i = 0;
            //for (IAnimal animal = it.First(); !it.IsCompleted; animal = it.Next())
            //{
            //    Console.WriteLine($"{++i}: {animal.Name}, age: {animal.Age}");
            //}
            //Console.WriteLine("\n\n\n");
            //animalHeap.Remove();
            //i = 0;
            //Algorithms.ForEach(animalHeap.GetForwardIterator(), (a) =>
            //{
            //    Console.WriteLine($"{++i}: {a.Name}, age: {a.Age}");
            //});
            //Heap<int> intHeap = new Heap<int>(new CompareInts());
            //for (i = 0; i < 15; i++)
            //{
            //    intHeap.Insert(i);
            //}
            //Algorithms.ForEach(intHeap.GetForwardIterator(), (a) =>
            //{
            //    Console.WriteLine(a);
            //});
            //Algorithms.ForEach(intHeap.GetForwardIterator(), (a) =>
            //{
            //    Console.WriteLine(a);
            //});
        }
        public static Dictionary<string, IMyCollection<Object>> GetZooCollection(List<IEnclosure> enclosureList, List<IAnimal> animalList, List<ISpecies> speciesList, List<IEmployee> employeeList, List<IVisitor> visitorList)
        {
            Dictionary<string, IMyCollection<Object>> collections = new Dictionary<string, IMyCollection<Object>>();
            Heap<Object> animalHeap = new Heap<Object>(new CompareByDefault());
            foreach (IAnimal a in animalList)
            {
                animalHeap.Insert(a);
            }
            Heap<Object> enclosureHeap = new Heap<Object>(new CompareByDefault());
            foreach (IEnclosure a in enclosureList)
            {
                enclosureHeap.Insert(a);
            }
            Heap<Object> speciesHeap = new Heap<Object>(new CompareByDefault());
            foreach (ISpecies a in speciesList)
            {
                speciesHeap.Insert(a);
            }
            Heap<Object> employeeHeap = new Heap<Object>(new CompareByDefault());
            foreach (IEmployee a in employeeList)
            {
                employeeHeap.Insert(a);
            }
            Heap<Object> visitorHeap = new Heap<Object>(new CompareByDefault());
            foreach (IVisitor a in visitorList)
            {
                visitorHeap.Insert(a);
            }
            collections.Add("enclosure", enclosureHeap);
            collections.Add("animal", animalHeap);
            collections.Add("species", speciesHeap);
            collections.Add("employee", employeeHeap);
            collections.Add("visitor", visitorHeap);
            return collections;
        }
    }
}