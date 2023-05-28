using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Zoo.Classes;
using Zoo.Collections;
using static System.Collections.Specialized.BitVector32;

namespace Zoo
{
    public class Receiver
    {
        private Dictionary<string, IMyCollection<Object>> collections;
        private Dictionary<(string, string), Func<Object, Object>> getters;
        Dictionary<Type, string> representations;
        private ZooBuilder builder;
        private List<ICommand> commandList;
        private List<ICommand> undoCommandList;
        public TextReader sourceFileReader;
        public bool isLoading = false;

        public Receiver(Dictionary<string, IMyCollection<object>> collections, Dictionary<(string, string), Func<object, object>> getters, ZooBuilder builder, Dictionary<Type, string> representations)
        {
            this.collections = collections;
            this.getters = getters;
            this.builder = builder;
            this.representations = representations;
            commandList = new List<ICommand>();
            undoCommandList = new List<ICommand>();
        }
        public void AddCommand(ICommand command)
        {
            commandList.Add(command);
        }
        public void ExecuteList(string @class)
        {
            if (!collections.ContainsKey(@class))
            {
                Console.WriteLine("No such class");
                return;
            }
            Algorithms.ForEach(collections[@class].GetForwardIterator(), (obj) =>
            {
                Console.WriteLine(PrintingFunc.PrintZooObject(obj));
            });
        }
        //public bool InitializeFind(string opts)
        //{
        //    var list = FindInCollection(opts);
        //    if (list.Count == 0)
        //        return false;
        //    else
        //        return true;
        //}
        public void ExecuteFind(string opts)
        {
            var list = FindInCollection(opts);
            foreach (Object obj in list)
            {
                Console.WriteLine(PrintingFunc.PrintZooObject(obj));
            }
        }
        private List<Object> FindInCollection(string opts)
        {
            char[] operators = { '>', '<', '=' };
            string[] splitted = opts.Split(' ');
            string @class = splitted[0];
            if (!collections.ContainsKey(@class))
            {
                Console.WriteLine("No such class");
                return new List<Object>();
            }
            List<string> reqs = new List<string>();
            for (int i = 1; i < splitted.Count(); i++)
            {
                if (splitted[i].Contains('"'))
                    reqs.Add((splitted[i] + ' ' + splitted[++i]).Replace("\"", ""));
                else
                    reqs.Add(splitted[i]);
            }
            List<Object> list = new List<Object>();
            Algorithms.ForEach(collections[@class].GetForwardIterator(), (obj) =>
            {
                list.Add(obj);
            });
            foreach (string req in reqs)
            {
                List<Object> newList = new List<Object>();
                int idx = req.IndexOfAny(operators);
                if (idx < 0)
                {
                    return new List<Object>();
                    Console.WriteLine("invalid arguments");
                }
                string key = req.Substring(0, idx);
                char @operator = req[idx];
                Func<Object, Object, bool> pred = (x, y) =>
                {
                    if (x is string)
                    {
                        if (@operator == '=')
                            return string.Compare((string)x, (string)y) == 0;
                        if (@operator == '<')
                            return string.Compare((string)x, (string)y) < 0;
                        if (@operator == '>')
                            return string.Compare((string)x, (string)y) > 0;
                    }
                    else if (x is int)
                    {
                        if (@operator == '=')
                            return (int)x == (int)y;
                        if (@operator == '<')
                            return (int)x < (int)y;
                        if (@operator == '>')
                            return (int)x > (int)y;
                    }
                    Console.WriteLine("wrong operator");
                    return false;
                };
                string value = req.Substring(idx + 1);
                if (!getters.ContainsKey((@class, key)))
                {
                    Console.WriteLine("invalid field");
                    return new List<Object>();
                }
                Func<Object, Object> get = getters[(@class, key)];
                foreach (Object obj in list)
                {
                    Object objVal = get(obj);
                    bool isValid = false;
                    if (objVal is int)
                        isValid = pred(objVal, int.Parse(value));
                    else if (objVal is string)
                        isValid = pred(objVal, value);
                    if (isValid)
                        newList.Add(obj);
                }
                list.Clear();
                list.AddRange(newList);
            }
            return list;
        }
        public (List<string>, bool) InitializeAdd(string opts)
        {
            string[] repOpts = { "base", "secondary" };
            string[] splitted = opts.Split(' ');
            if (splitted.Length != 2 || !repOpts.Contains(splitted[1]))
            {
                Console.Write("Wrong arguments");
                return (new List<string>(), false);
            }
            string @class = splitted[0];
            string representation = splitted[1];
            return ReadChanges(@class);
        }
        public string ExecuteAdd(string opts, List<string> args)
        {
            string[] outCommands = { "DONE", "EXIT" };
            string[] splitted = opts.Split(' ');
            string @class = splitted[0];
            string representation = splitted[1];
            if (!builder.Reset(@class, representation))
                return "";
            foreach(string input in args)
            {
                if (outCommands.Contains(input))
                    break;
                int idx = input.IndexOf('=');
                string key = input.Substring(0, idx);
                string val = input.Substring(idx + 1);
                builder.BuildPart(key, val);
            }
            if (args.Last() == "DONE")
            {
                Object toAdd = builder.GetProduct();
                collections[@class].Insert(toAdd);
                return GetReverseOpts(@class, toAdd);
            }
            return "";
        }
        private string GetReverseOpts(string @class, Object obj)
        {
            string s = @class;
            foreach (var (pair,getter) in getters)
            {
                if (pair.Item1 == @class)
                {
                    s += $" {pair.Item2}={getter(obj).ToString()}";
                }
            }
            return s;
        }
        private List<string> GetReverseArgs(string @class, Object obj)
        {
            string s = @class;
            List<string> args = new List<string>();
            foreach (var (pair, getter) in getters)
            {
                if (pair.Item1 == @class)
                {
                     args.Add($"{pair.Item2}={getter(obj).ToString()}");
                }
            }
            args.Add("DONE");
            return args;
        }
        public (List<string>, bool) InitializeEdit(string opts)
        {
            var list = FindInCollection(opts);
            if (list.Count != 1)
            {
                Console.WriteLine("wrong requirements");
                return (new List<string>(), false);
            }
            string @class = opts.Split(' ')[0];
            return ReadChanges(@class);
        }
        public (string, List<string>) ExecuteEdit(string opts, List<string> args)
        {
            string[] outCommands = { "DONE", "EXIT" };
            if (args.Count == 0 || args.Last() != "DONE")
                return ("", new List<string>());
            Object obj = FindInCollection(opts).First();
            string @class = opts.Split(' ')[0];
            List<string> revArgs = GetReverseArgs(@class, obj);
            if (!builder.SetObject(@class, obj))
                return ("", new List<string>());
            
            foreach (string input in args)
            {
                if (outCommands.Contains(input))
                    break;
                int idx = input.IndexOf('=');
                string key = input.Substring(0, idx);
                string val = input.Substring(idx + 1);
                builder.BuildPart(key, val);
            }
            obj = builder.GetProduct();
            return (GetReverseOpts(@class, obj), revArgs);
        }
        private (List<string>, bool) ReadChanges(string @class)
        {
            List<string> list = new List<string>();
            string input = "";
            string[] outCommands = { "DONE", "EXIT" };
            Console.Write($"Available fields:");
            foreach (var obj in getters)
            {
                if (obj.Key.Item1 == @class)
                    Console.Write(' ' + obj.Key.Item2);
            }
            Console.WriteLine();
            while (true)
            {
                input = Console.ReadLine();
                if (outCommands.Contains(input))
                {
                    break;
                }
                int idx = input.IndexOf('=');
                if (idx < 0)
                {
                    Console.WriteLine("Wrong Input");
                    continue;
                }
                list.Add(input);
            }
            if (input == "DONE")
            {
                Console.WriteLine($"{@class} created");
                //collections[@class].Insert(builder.GetProduct());
            }
            else if (input == "EXIT")
            {
                Console.WriteLine($"{@class} creation abandoned");
            }
            list.Add(input);
            return (list, true);
        }
        public bool InitializeDelete(string opts)
        {
            if (FindInCollection(opts).Count != 1)
            {
                Console.WriteLine("wrong requirements");
                return false;
            }
            return true;
        }
        public (string, List<string>) ExecuteDelete(string opts)
        {
            string[] splitted = opts.Split(' ');
            string @class = splitted[0];
            var list = FindInCollection(opts);
            if (list.Count != 1)
            {
                return ("", new List<string>());
            }
            Object obj = list.First();
            string revOpts = $"{@class} {representations[obj.GetType()]}";
            List<string> revArgs = GetReverseArgs(@class, obj); 
            collections[@class].RemoveItem(obj);
            return (revOpts, revArgs);
        }
        public void QPrint()
        {
            foreach (ICommand command in commandList)
                Console.WriteLine(command);
        }
        public void QExport(string opts)
        {
            string filename;
            string format;
            if (opts.IndexOf(' ') < 0)
            {
                format = "XML";
                filename = opts;
            }
            else
            {
                filename = opts.Substring(0, opts.IndexOf(' '));
                format = opts.Substring(opts.IndexOf(' ') + 1);
            }
            filename = Directory.GetCurrentDirectory() + '\\' +filename;
            if (format == "plaintext")
                SaveToTxt(filename);
            else if (format == "XML")
                SaveToXml(filename);
            Console.WriteLine("saving as: " + filename);
        }
        public void QCommit()
        {
            foreach (ICommand command in commandList)
            {
                command.Execute();
            }
            commandList.Clear();
        }
        public void QLoad(string opts)
        {
            string filename = opts;
            filename = Directory.GetCurrentDirectory() + '\\' + filename;
            if (!File.Exists(filename))
            {
                Console.WriteLine("invalid file name");
            }
            if (Path.GetExtension(filename) == ".txt")
                ReadFromTxt(filename);
            else if (Path.GetExtension(filename) == ".xml")
            {
                ListOfICommand commands = ReadFromXml(filename);
                foreach (ICommand command in commands)
                {
                    command.SetReceiver(this);
                }
                commandList.AddRange(commands);
                foreach (var command in commands)
                {
                    command.Execute();
                }
            }
        }
        public void QDismiss()
        {
            commandList.Clear();
            Console.WriteLine("queue dismissed");
        }
        public void Undo()
        {
            if (commandList.Count > 0)
            {
                commandList.Last().UnExecute();
                undoCommandList.Add(commandList.Last());
                commandList.RemoveAt(commandList.Count - 1);
            }
            else
            {
                Console.WriteLine("No commands to undo");
            }
        }
        public void Redo()
        {
            if (undoCommandList.Count > 0)
            {
                undoCommandList.Last().Execute();
                commandList.Add(undoCommandList.Last());
                undoCommandList.RemoveAt(undoCommandList.Count - 1);
            }
            else
            {
                Console.WriteLine("No commands to redo");
            }
        }
        private void SaveToTxt(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (FileStream fs = File.Create(filename))
                {
                    foreach (ICommand command in commandList)
                    {
                        Byte[] text = new UTF8Encoding(true).GetBytes(command.ToString() + '\n');
                        fs.Write(text, 0, text.Length);
                    }
                }

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
        private void SaveToXml(string filename)
        {
            //XmlSerializer serializer = new XmlSerializer(typeof(List<ICommand>));

            //using (TextWriter writer = new StreamWriter(filename))
            //{
            //    // Serializacja listy do XML
            //    serializer.Serialize(writer, commandList);
            //}
            var commands = new ListOfICommand();
            commands.AddRange(commandList);
            var xmlSerializer = new XmlSerializer(commands.GetType());
            var stringBuilder = new StringBuilder();
            var xmlTextWriter = XmlTextWriter.Create(stringBuilder, new XmlWriterSettings { NewLineChars = "\r\n", Indent = true });
            //xmlSerializer.Serialize(xmlTextWriter, commands);
            using (TextWriter writer = new StreamWriter(filename))
            {
                // Serializacja listy do XML
                xmlSerializer.Serialize(writer, commands);
            }
            //Console.WriteLine(stringBuilder.ToString());
        }
        private ListOfICommand ReadFromXml(string filename)
        {
            ListOfICommand commands;
            var xmlSerializer = new XmlSerializer(typeof(ListOfICommand));
            using (Stream reader = new FileStream(filename, FileMode.Open))
            {
                 commands = (ListOfICommand)xmlSerializer.Deserialize(reader);
            }
            return commands;
        }
        private void ReadFromTxt(string filename)
        {
            FileInfo sourceFile = new FileInfo(filename);
            TextReader sourceFileReader = new StreamReader(sourceFile.FullName);
            Console.SetIn(sourceFileReader);
            this.sourceFileReader = sourceFileReader;
            Console.SetOut(new StringWriter());
            isLoading = true;
        }
    }
    
}
