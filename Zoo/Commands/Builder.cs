using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo
{
    public interface IBuilder
    {
        void BuildPart(string part, string value);
    }
    public class ZooBuilder : IBuilder
    {
        private Object product;
        private string @class = "";
        private Dictionary<string, Type[]> types;
        private Dictionary<(string, string), Action<Object, Object>> setters;
        private Dictionary<(string, string), string> fieldTypes;

        public ZooBuilder(Dictionary<string, Type[]> types, Dictionary<(string, string), Action<Object, Object>> setters, Dictionary<(string, string), string> fieldTypes)
        {
            this.types = types;
            this.setters = setters;
            this.fieldTypes = fieldTypes;
        }
        public void BuildPart(string part, string value)
        {

            if (setters.ContainsKey((@class, part)))
            {
                Object setVal = value;
                if (fieldTypes[(@class, part)] == "int")
                {
                    setVal = int.Parse(value);
                }
                setters[(@class, part)](product, setVal);
            }
            else
            {
                Console.WriteLine("wrong field");
            }
        }
        public Object GetProduct()
        {
            return this.product;
        }
        public bool SetObject(string type, Object obj) 
        {
            if (types.ContainsKey(type))
            {
                @class = type;
                product = obj;
                return true;
            }
            else
            {
                Console.WriteLine("no such class");
                return false;
            }
        }
        public bool Reset(string type, string representation)
        {
            
            if (types.ContainsKey(type))
            {
                int repIdx = 0;
                switch (representation)
                {
                    case "base":
                        repIdx = 0;
                        break;
                    case "secondary":
                        repIdx = 1;
                        break;
                    default:
                        Console.WriteLine("no such representation");
                        return false;
                }
                if (repIdx >= types[type].Length)
                {
                    Console.WriteLine("no such representation in particular class");
                    return false;
                }
                product = Activator.CreateInstance(types[type][repIdx]);
                this.@class = type;
                return true;
            }
            else
            {
                Console.WriteLine("no such class");
                return false;
            }
        }
    }
}
