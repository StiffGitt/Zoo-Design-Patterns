
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public interface IEnclosure
    {
        public string Name{ get; set; }
        public List<IAnimal> Animals { get; }
        public IEmployee Employee { get; }
    }
    public interface IAnimal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public ISpecies Species { get; }
    }
    public interface ISpecies
    {
        public string Name { get; set; }
        public List<ISpecies> FavouriteFoods { get; }
    }
    public interface IEmployee
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public List<IEnclosure> Enclosures { get; }
    }
    public interface IVisitor
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<IEnclosure> VisitedEnclosures { get; }
    }
    [Serializable]
    public class Enclosure
    {
        public string Name
        {
            get;set;
        }
        public List<Animal> Animals
        {
            get;set;
        }
        public Employee Employee
        {
            get; set;
        }
        public Enclosure(string name, List<Species> species, List<Animal> animals, Employee employee)
        {
            Name = name;
            Employee = employee;
            Animals = new List<Animal>();
            foreach (Animal animal in animals)
            {
                if (species.Contains(animal.Species))
                {
                    Animals.Add(animal);
                }
            }
        }

        public Enclosure()
        {
            Name = "default";
        }

        public override string ToString()
        {
            string s = $"Enclosure: {Name}, Employee: {Employee.Name}, Animals: ";
            foreach (Animal animal in Animals) { s += $"{animal.Name}, "; }
            return s;

        }

    }
    public class EnclosureAdapter : IEnclosure
    {
        private Enclosure enclosure;
        public EnclosureAdapter(Enclosure enclosure)
        {
            this.enclosure = enclosure;
        }
        public EnclosureAdapter()
        {
            this.enclosure = new Enclosure();
        }

        public string Name { get { return enclosure.Name; }
            set
            {
                enclosure.Name = value;
            } }

        public List<IAnimal> Animals 
        {
            get 
            {
                return AdapterFunc.AnimalToInt(enclosure.Animals);
            } 
        }

        public IEmployee Employee { get { return new EmployeeAdapter(enclosure.Employee); } }
    }

    [Serializable]
    public class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Species Species { get; set; }

        public Animal(string name, int age, Species species)
        {
            Name = name;
            Age = age;
            Species = species;
        }

        public Animal()
        {
            Name = "default";
            Age = 0;
        }

        public override string ToString()
        {
            return $"Animal: {Name}, Age: {Age}, Species: {Species.Name}";
        }
    }
    public class AnimalAdapter : IAnimal
    {
        private Animal animal;

        public AnimalAdapter()
        {
            this.animal = new Animal();
        }

        public AnimalAdapter(Animal animal)
        {
            this.animal = animal;
        }

        public string Name { get { return animal.Name; }
            set
            {
                animal.Name = value;
            } }

        public int Age { get { return animal.Age; } 
        set 
        { 
            animal.Age = value; 
        }}

        public ISpecies Species { get { return new SpeciesAdapter(animal.Species); } }
    }

    [Serializable]
    public class Species 
    {
        public string Name { get; set; }
        public List<Species> FavouriteFoods { get; set; }


        public Species(string name, List<Species> favouriteFoods)
        {
            this.Name = name;
            this.FavouriteFoods = favouriteFoods;
        }

        public Species()
        {
            Name = "default";
        }

        public override string ToString()
        {
            string s = $"Species: {Name}, FavouriteFoods: ";
            foreach (var e in FavouriteFoods)
            {
                s += e.Name + ", ";
            }
            return s;
        }
    }

    public class SpeciesAdapter : ISpecies
    {
        private Species species;

        public SpeciesAdapter()
        {
            this.species = new Species();
        }

        public SpeciesAdapter(Species species)
        {
            this.species = species;
        }

        public string Name { get { return species.Name; } set { species.Name = value; } }

        public List<ISpecies> FavouriteFoods
        {
            get
            {
                return AdapterFunc.SpeciesToInt(species.FavouriteFoods);
            }
        }
    }

    [Serializable]
    public class Employee
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public List<Enclosure> Enclosures { get; set; }


        public Employee(string name, string surname, int age, List<Enclosure> enclosures)
        {
            this.Name = name;
            this.Surname = surname;
            this.Age = age;
            this.Enclosures = enclosures;
        }

        public Employee()
        {
            Name = "default";
            Surname = "default";
            Age = 0;
        }

        public override string ToString()
        {
            string s = $"Employee: {Name} {Surname}, Age: {Age}, Enclosures: ";
            foreach (var e in Enclosures)
            {
                s += e.Name + ", ";
            }
            return s;
        }
    }
    public class EmployeeAdapter : IEmployee
    {
        private Employee employee;

        public EmployeeAdapter()
        {
            this.employee = new Employee();
        }

        public EmployeeAdapter(Employee employee)
        {
            this.employee = employee;
        }

        public string Name { get { return employee.Name; }
            set
            {
                employee.Name = value;
            } }

        public string Surname { get { return employee.Surname; }
            set
            {
                employee.Surname = value;
            }}

        public int Age { get { return employee.Age; }
            set
            {
                employee.Age = value;
            }
        }

        public List<IEnclosure> Enclosures
        {
            get
            {
                return AdapterFunc.EnclosureToInt(employee.Enclosures);
            }
        }
    }

    public class Visitor
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Enclosure> VisitedEnclosures { get; set; }


        public Visitor(string name, string surname, List<Enclosure> visitedEnclosures)
        {
            this.Name = name;
            this.Surname = surname;
            this.VisitedEnclosures = visitedEnclosures;
        }

        public Visitor()
        {
            Name = "default";
            Surname = "default";
        }

        public override string ToString()
        {
            string s = $"Employee: {Name} {Surname}, Visited Enclosures: ";
            foreach (var e in VisitedEnclosures)
            {
                s += e.Name + ", ";
            }
            return s;
        }
    }
    public class VisitorAdapter : IVisitor
    {
        private Visitor visitor;

        public VisitorAdapter()
        {
            this.visitor = new Visitor();
        }

        public VisitorAdapter(Visitor visitor)
        {
            this.visitor = visitor;
        }

        public string Name { get { return visitor.Name; }
            set
            {
                visitor.Name = value;
            }
        }

        public string Surname { get { return visitor.Surname; }
            set
            {
                visitor.Surname = value;
            }
        }

        public List<IEnclosure> VisitedEnclosures
        {
            get
            {
                return AdapterFunc.EnclosureToInt(visitor.VisitedEnclosures);
            }
        }
    }
}