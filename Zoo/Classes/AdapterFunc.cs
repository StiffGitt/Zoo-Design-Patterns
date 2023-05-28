using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Classes;

namespace Zoo
{
    public static class AdapterFunc
    {
        public static List<IAnimal> AnimalToInt(List<Animal> anims)
        {
            List<IAnimal> animals = new List<IAnimal>();
            foreach (Animal animal in anims)
            {
                animals.Add(new AnimalAdapter(animal));
            }
            return animals;
        }
        public static List<ISpecies> SpeciesToInt(List<Species> species) 
        {
            List<ISpecies> speciess = new List<ISpecies>();
            foreach (Species s in species)
            {
                speciess.Add(new SpeciesAdapter(s));
            }
            return speciess;
        }
        public static List<IEnclosure> EnclosureToInt(List<Enclosure> enclosure)
        {
            List<IEnclosure> enclosures = new List<IEnclosure>();
            foreach (Enclosure s in enclosure)
            {
                enclosures.Add(new EnclosureAdapter(s));
            }
            return enclosures;
        }
        
    }
}
