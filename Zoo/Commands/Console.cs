using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoo.Collections;

namespace Zoo
{
    public class MyConsole
    {
        //Dictionary<string, IMyCollection<Object>> collections;
        private Receiver receiver;
        private Dictionary<string, Func<Receiver, string, ICommand>> commands;
        private Dictionary<string, Action<string>> hActions;


        public MyConsole(Dictionary<string, IMyCollection<Object>> collections)
        {
            this.receiver = InitializeReceiver(collections);
            this.commands = InitializeCommands();
        }
        public void Run()
        {
            string input = "";
            while (input != "exit")
            {
                Console.WriteLine("\nEnter command");
                input = Console.ReadLine();
                if (input == null)
                {
                    var stdin = new StreamReader(Console.OpenStandardInput());
                    Console.SetIn(stdin);
                    if (receiver.sourceFileReader != null)
                    {
                        receiver.isLoading = false;
                        receiver.sourceFileReader.Close();
                        receiver.sourceFileReader = null;
                    }
                    var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                    standardOutput.AutoFlush = true;
                    Console.SetOut(standardOutput);
                    continue;
                }
                string commandName;
                string commandOpts;
                if (input.IndexOf(' ') < 0)
                {
                    commandName = input;
                    commandOpts = "";
                    //continue;
                }
                else
                {
                    commandName = input.Substring(0, input.IndexOf(' '));
                    commandOpts = input.Substring(input.IndexOf(' ') + 1);
                }
                if (commands.ContainsKey(commandName))
                {
                    //Object[] args = { receiver, commandOpts };
                    //ICommand command = (ICommand)Activator.CreateInstance(commands[commandName], args);
                    ICommand command = commands[commandName](receiver, commandOpts);
                    //if(!receiver.isLoading)
                    command.Execute();
                    //receiver.AddCommand(command);
                }
                else if(hActions.ContainsKey(commandName))
                {
                    //string action = commandOpts;
                    //if (commandOpts.IndexOf(' ') >= 0)
                    //{
                    //    action = commandOpts.Substring(0, commandOpts.IndexOf(' '));
                    //    commandOpts = commandOpts.Substring(commandOpts.IndexOf(' ') + 1);
                    //}
                    //if (hActions.ContainsKey(action))
                    //{
                    //    hActions[action](commandOpts);
                    //}
                    hActions[commandName](commandOpts);
                }
            }
        }

        private Dictionary<string, Func<Receiver, string, ICommand>> InitializeCommands()
        {
            //Dictionary<string, Type> commands = new Dictionary<string, Type>();
            //commands.Add("list", typeof(ListCommand));
            //commands.Add("find", typeof(FindCommand));
            //commands.Add("add", typeof(AddCommand));
            Dictionary<string, Func<Receiver, string, ICommand>> commands = new Dictionary<string, Func<Receiver, string, ICommand>>();
            commands.Add("list", (x, y) => new ListCommand(x, y));
            commands.Add("find", (x, y) => new FindCommand(x, y));
            commands.Add("add", (x, y) => new AddCommand(x, y));
            commands.Add("edit", (x, y) => new EditCommand(x, y));
            commands.Add("delete", (x, y) => new DeleteCommand(x, y));
            hActions = new Dictionary<string, Action<string>>();
            hActions.Add("history", (x) => receiver.QPrint());
            hActions.Add("export", (x) => receiver.QExport(x));
            hActions.Add("undo", (x) => receiver.Undo());
            hActions.Add("redo", (x) => receiver.Redo());
            //hActions.Add("commit", (x) => receiver.QCommit());
            //hActions.Add("dismiss", (x) => receiver.QDismiss());
            hActions.Add("load", (x) => receiver.QLoad(x));
            return commands;
        }
        private Receiver InitializeReceiver(Dictionary<string, IMyCollection<Object>> collections)
        {
            Dictionary<(string, string), Func<object, object>> getters = new Dictionary<(string, string), Func<object, object>>();
            getters.Add(("enclosure", "name"), x => ((IEnclosure)x).Name);
            getters.Add(("animal", "name"), x => ((IAnimal)x).Name);
            getters.Add(("animal", "age"), x => ((IAnimal)x).Age);
            getters.Add(("species", "name"), x => ((ISpecies)x).Name);
            getters.Add(("employee", "name"), x => ((IEmployee)x).Name);
            getters.Add(("employee", "surname"), x => ((IEmployee)x).Surname);
            getters.Add(("employee", "age"), x => ((IEmployee)x).Age);
            getters.Add(("visitor", "name"), x => ((IVisitor)x).Name);
            getters.Add(("visitor", "surname"), x => ((IVisitor)x).Surname);

            Dictionary<(string, string), Action<Object, Object>> setters = new Dictionary<(string, string), Action<Object, Object>>();
            setters.Add(("enclosure", "name"), (x, y) => ((IEnclosure)x).Name = (string)y);
            setters.Add(("animal", "name"), (x, y) => ((IAnimal)x).Name = (string)y);
            setters.Add(("animal", "age"), (x, y) => ((IAnimal)x).Age = (int)y);
            setters.Add(("species", "name"), (x, y) => ((ISpecies)x).Name = (string)y);
            setters.Add(("employee", "name"), (x, y) => ((IEmployee)x).Name = (string)y);
            setters.Add(("employee", "surname"), (x, y) => ((IEmployee)x).Surname = (string)y);
            setters.Add(("employee", "age"), (x, y) => ((IEmployee)x).Age = (int)y);
            setters.Add(("visitor", "name"), (x, y) => ((IVisitor)x).Name = (string)y);
            setters.Add(("visitor", "surname"), (x, y) => ((IVisitor)x).Surname = (string)y);

            Dictionary<(string, string), string> fieldTypes = new Dictionary<(string, string), string>();
            fieldTypes.Add(("enclosure", "name"), "string");
            fieldTypes.Add(("animal", "name"), "string");
            fieldTypes.Add(("animal", "age"), "int");
            fieldTypes.Add(("species", "name"), "string");
            fieldTypes.Add(("employee", "name"), "string");
            fieldTypes.Add(("employee", "surname"), "string");
            fieldTypes.Add(("employee", "age"), "int");
            fieldTypes.Add(("visitor", "name"), "string");
            fieldTypes.Add(("visitor", "surname"), "string");

            Dictionary<string, Type[]> types = new Dictionary<string, Type[]>();
            types.Add("enclosure", new Type[] { typeof(EnclosureAdapter), typeof(EnclosureMAdapter) });
            types.Add("animal", new Type[] { typeof(AnimalAdapter), typeof(AnimalMAdapter) });
            types.Add("species", new Type[] { typeof(SpeciesAdapter), typeof(SpeciesMAdapter) });
            types.Add("employee", new Type[] { typeof(EmployeeAdapter), typeof(EmployeeMAdapter) });
            types.Add("visitor", new Type[] { typeof(VisitorAdapter), typeof(VisitorMAdapter) });

            Dictionary<Type, string> reps = new Dictionary<Type, string>();
            reps.Add(typeof(EnclosureAdapter), "base");
            reps.Add(typeof(AnimalAdapter), "base");
            reps.Add(typeof(SpeciesAdapter), "base");
            reps.Add(typeof(EmployeeAdapter), "base");
            reps.Add(typeof(VisitorAdapter), "base");
            reps.Add(typeof(EnclosureMAdapter), "secondary");
            reps.Add(typeof(AnimalMAdapter), "secondary");
            reps.Add(typeof(SpeciesMAdapter), "secondary");
            reps.Add(typeof(EmployeeMAdapter), "secondary");
            reps.Add(typeof(VisitorMAdapter), "secondary");

            ZooBuilder builder = new ZooBuilder(types, setters, fieldTypes);
            return new Receiver(collections, getters, builder, reps);
        }
    }
}
