using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Classes
{
    class EnclosureB
    {
        public byte[] data;
        public int name_addr;
        public int name_count;
        public int animals_addr;
        public int animals_count;
        public int employee_addr;
        public int employee_count;

        public EnclosureB(string name, List<Animal> animals, Employee employee)
        {
            byte[] bName = ByteTranformer.ObjectToByteArray(name);
            byte[] bAnimals = ByteTranformer.ObjectToByteArray(animals);
            byte[] bEmployee = ByteTranformer.ObjectToByteArray(employee);

            data = new byte[bName.Length + bAnimals.Length + bEmployee.Length];
            Buffer.BlockCopy(bName, 0, data, 0, bName.Length);
            Buffer.BlockCopy(bAnimals, 0, data, bName.Length, bAnimals.Length);
            Buffer.BlockCopy(bEmployee, 0, data, bName.Length + bAnimals.Length, bEmployee.Length);
            name_addr = 0;
            animals_addr = bName.Length;
            employee_addr = bName.Length + bAnimals.Length;
            name_count = 1;
            animals_count = animals.Count;
            employee_count = 1;
        }
    }
    class EnclosureBAdapter : IEnclosure
    {
        private readonly EnclosureB enclosureB;

        public EnclosureBAdapter(EnclosureB enclosureB)
        {
            this.enclosureB = enclosureB;
        }
        public string Name
        {
            get { return (string)ByteTranformer.ByteArrayToObject(enclosureB.data, enclosureB.name_addr, enclosureB.animals_addr - enclosureB.name_addr); }
            set { }
        }
        public List<IAnimal> Animals
        {
            get { return AdapterFunc.AnimalToInt((List<Animal>)ByteTranformer.ByteArrayToObject(enclosureB.data, enclosureB.animals_addr, enclosureB.employee_addr - enclosureB.animals_addr)); }
        }
        public IEmployee Employee
        {
            get { return new EmployeeAdapter((Employee)ByteTranformer.ByteArrayToObject(enclosureB.data, enclosureB.employee_addr, enclosureB.data.Length - enclosureB.employee_addr)); }
        }
    }
    class AnimalB
    {
        public readonly byte[] data;
        public int name_addr;
        public int name_count;
        public int age_addr;
        public int age_count;
        public int species_addr;
        public int species_count;

        public AnimalB(string name, int age, Species species)
        {
            byte[] d1 = ByteTranformer.ObjectToByteArray(name);
            byte[] d2 = ByteTranformer.ObjectToByteArray(age);
            byte[] d3 = ByteTranformer.ObjectToByteArray(species);

            data = new byte[d1.Length + d2.Length + d3.Length];
            Buffer.BlockCopy(d1, 0, data, 0, d1.Length);
            Buffer.BlockCopy(d2, 0, data, d1.Length, d2.Length);
            Buffer.BlockCopy(d3, 0, data, d1.Length + d2.Length, d3.Length);
            name_addr = 0;
            age_addr = d1.Length;
            species_addr = d1.Length + d2.Length;
            name_count = 1;
            age_count = 1;
            species_count = 1;
        }
    }
    class AnimalBAdapter : IAnimal
    {
        private readonly AnimalB animalB;

        public AnimalBAdapter(AnimalB animalB)
        {
            this.animalB = animalB;
        }

        public string Name { get { return (string)ByteTranformer.ByteArrayToObject(animalB.data, animalB.name_addr, animalB.age_addr - animalB.name_addr); } set { } }
        public int Age { get { return (int)ByteTranformer.ByteArrayToObject(animalB.data, animalB.age_addr, animalB.species_addr - animalB.age_addr); } set { } }
        public ISpecies Species { get { return new SpeciesAdapter((Species)ByteTranformer.ByteArrayToObject(animalB.data, animalB.species_addr, animalB.data.Length - animalB.species_addr)); } }
    }

    class SpeciesB
    {
        public readonly byte[] data;
        public int name_addr;
        public int name_count;
        public int favouriteFoods_addr;
        public int favouriteFoods_count;

        public SpeciesB(string name, List<Species> favouriteFoods)
        {
            byte[] d1 = ByteTranformer.ObjectToByteArray(name);
            byte[] d2 = ByteTranformer.ObjectToByteArray(favouriteFoods);

            data = new byte[d1.Length + d2.Length];
            Buffer.BlockCopy(d1, 0, data, 0, d1.Length);
            Buffer.BlockCopy(d2, 0, data, d1.Length, d2.Length);
            name_addr = 0;
            favouriteFoods_addr = d1.Length;
            name_count = 1;
            favouriteFoods_count = 1;
        }

    }
    class SpeciesBAdapter : ISpecies
    {
        private readonly SpeciesB speciesB;

        public SpeciesBAdapter(SpeciesB speciesB)
        {
            this.speciesB = speciesB;
        }

        public string Name { get { return (string)ByteTranformer.ByteArrayToObject(speciesB.data, speciesB.name_addr, speciesB.favouriteFoods_addr - speciesB.name_addr); } set { } }

        public List<ISpecies> FavouriteFoods { get { return AdapterFunc.SpeciesToInt((List<Species>)ByteTranformer.ByteArrayToObject(speciesB.data, speciesB.favouriteFoods_addr, speciesB.data.Length - speciesB.favouriteFoods_addr)); } }
    }

    class EmployeeB
    {
        public readonly byte[] data;
        public int name_addr;
        public int name_count;
        public int surname_addr;
        public int surname_count;
        public int age_addr;
        public int age_count;
        public int enclosures_addr;
        public int enclosures_count;

        public EmployeeB(string name, string surname, int age, List<Enclosure> enclosures)
        {
            byte[] d1 = ByteTranformer.ObjectToByteArray(name);
            byte[] d2 = ByteTranformer.ObjectToByteArray(surname);
            byte[] d3 = ByteTranformer.ObjectToByteArray(age);
            byte[] d4 = ByteTranformer.ObjectToByteArray(enclosures);

            data = new byte[d1.Length + d2.Length + d3.Length + d4.Length];
            Buffer.BlockCopy(d1, 0, data, 0, d1.Length);
            Buffer.BlockCopy(d2, 0, data, d1.Length, d2.Length);
            Buffer.BlockCopy(d3, 0, data, d1.Length + d2.Length, d3.Length);
            Buffer.BlockCopy(d4, 0, data, d1.Length + d2.Length + d3.Length, d4.Length);
            name_addr = 0;
            surname_addr = d1.Length;
            age_addr = d1.Length + d2.Length;
            enclosures_addr = d1.Length + d2.Length + d3.Length;
            name_count = 1;
            surname_count = 1;
            age_count = 1;
            enclosures_count = enclosures.Count;
        }
    }
    class EmployeeBAdapter : IEmployee
    {
        private readonly EmployeeB employeeB;

        public EmployeeBAdapter(EmployeeB employeeB)
        {
            this.employeeB = employeeB;
        }
        public string Name { get { return (string)ByteTranformer.ByteArrayToObject(employeeB.data, employeeB.name_addr, employeeB.surname_addr - employeeB.name_addr); } set { } }

        public string Surname { get { return (string)ByteTranformer.ByteArrayToObject(employeeB.data, employeeB.surname_addr, employeeB.age_addr - employeeB.surname_addr); } set { } }

        public int Age { get { return (int)ByteTranformer.ByteArrayToObject(employeeB.data, employeeB.age_addr, employeeB.enclosures_addr - employeeB.age_addr); } set { } }

        public List<IEnclosure> Enclosures { get { return AdapterFunc.EnclosureToInt((List<Enclosure>)ByteTranformer.ByteArrayToObject(employeeB.data, employeeB.enclosures_addr, employeeB.data.Length - employeeB.enclosures_addr)); } }
    }

    class VisitorB
    {
        public readonly byte[] data;
        public int name_addr;
        public int name_count;
        public int surname_addr;
        public int surname_count;
        public int visitedEnclosures_addr;
        public int visitedEnclosures_count;

        public VisitorB(string name, string surname, List<Enclosure> visitedEnclosures)
        {
            byte[] d1 = ByteTranformer.ObjectToByteArray(name);
            byte[] d2 = ByteTranformer.ObjectToByteArray(surname);
            byte[] d3 = ByteTranformer.ObjectToByteArray(visitedEnclosures);

            data = new byte[d1.Length + d2.Length + d3.Length];
            Buffer.BlockCopy(d1, 0, data, 0, d1.Length);
            Buffer.BlockCopy(d2, 0, data, d1.Length, d2.Length);
            Buffer.BlockCopy(d3, 0, data, d1.Length + d2.Length, d3.Length);
            name_addr = 0;
            surname_addr = d1.Length;
            visitedEnclosures_addr = d1.Length + d2.Length;
            name_count = 1;
            surname_count = 1;
            visitedEnclosures_count = visitedEnclosures.Count;
        }
    }
    class VisitorBAdapter : IVisitor
    {
        private readonly VisitorB visitorB;

        public VisitorBAdapter(VisitorB visitorB)
        {
            this.visitorB = visitorB;
        }

        string IVisitor.Name { get { return (string)ByteTranformer.ByteArrayToObject(visitorB.data, visitorB.name_addr, visitorB.surname_addr - visitorB.name_addr); } set { } }

        string IVisitor.Surname { get { return (string)ByteTranformer.ByteArrayToObject(visitorB.data, visitorB.surname_addr, visitorB.visitedEnclosures_addr - visitorB.surname_addr); } set { } }

        List<IEnclosure> IVisitor.VisitedEnclosures { get { return AdapterFunc.EnclosureToInt((List<Enclosure>)ByteTranformer.ByteArrayToObject(visitorB.data, visitorB.visitedEnclosures_addr, visitorB.data.Length - visitorB.visitedEnclosures_addr)); } }
    }


    public static class ByteTranformer
    {
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static object ByteArrayToObject(byte[] arrBytes, int off, int count)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, off, count);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
    }
}
