using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Zoo.Collections;

namespace Zoo
{
    public interface ICommand
    {
        public void Execute();
        public void UnExecute();
        public void SetReceiver(Receiver receiver);
    }
    public class ListCommand : ICommand
    {
        private Receiver receiver;
        public string @class;
        public ListCommand(){ }
        public ListCommand(Receiver receiver, string @class)
        {
            this.receiver = receiver;
            this.@class = @class;
            //receiver.AddCommand(this);
        }
        public void Execute() 
        {
            receiver.ExecuteList(@class);
        }
        public void UnExecute() { }
        public override string ToString()
        {
            return "list " + @class;
        }
        public void SetReceiver(Receiver receiver)
        {
            this.receiver = receiver;
        }

    }
    public class FindCommand : ICommand
    {
        private Receiver receiver;
        public string opts;
        public FindCommand() { }
        public FindCommand(Receiver receiver, string opts)
        {
            this.receiver = receiver;
            this.opts = opts;
            //receiver.AddCommand(this);
        }
        public void Execute()
        {
            receiver.ExecuteFind(opts);
        }
        public void UnExecute() { }
        public override string ToString()
        {
            return "find " + opts;
        }
        public void SetReceiver(Receiver receiver)
        {
            this.receiver = receiver;
        }
    }
    public class AddCommand : ICommand
    {
        private Receiver receiver;
        public string opts;
        public string reverseOpts;
        public List<string> args;
        public AddCommand() { }
        public AddCommand(Receiver receiver, string opts)
        {
            this.receiver = receiver;
            this.opts = opts;
            (args, bool res) = receiver.InitializeAdd(opts);
            if (!res || args.Last() == "EXIT")
                return;
            receiver.AddCommand(this);
        }
        public void Execute()
        {
            if(args.Count > 0)
                reverseOpts = receiver.ExecuteAdd(opts, args);
        }
        public void UnExecute()
        {
            if (reverseOpts != null)
            {
                receiver.ExecuteDelete(reverseOpts);
            }
            else
            {
                Console.WriteLine("Unable to undo");
            }
        }
        public override string ToString()
        {
            string s = "add " + opts + '\n';
            foreach (string arg in args)
                s += arg + '\n';
            return s;
        }
        public void SetReceiver(Receiver receiver)
        {
            this.receiver = receiver;
        }
    }
    public class EditCommand : ICommand
    {
        private Receiver receiver;
        public string opts;
        public List<string> args;
        public string reverseOpts;
        public List<string> reverseArgs;
        public EditCommand() { }
        public EditCommand(Receiver receiver, string opts)
        {
            this.receiver = receiver;
            this.opts = opts;
            (args, bool res) = receiver.InitializeEdit(opts);
            if (!res || args.Last() == "EXIT")
                return;
            receiver.AddCommand(this);
        }
        public void Execute()
        {
            (reverseOpts, reverseArgs) = receiver.ExecuteEdit(opts, args);
        }
        public void UnExecute()
        {
            receiver.ExecuteEdit(reverseOpts, reverseArgs);
        }
        public override string ToString()
        {
            string s = "edit " + opts + '\n';
            foreach (string arg in args)
                s += arg + '\n';
            return s;
        }
        public void SetReceiver(Receiver receiver)
        {
            this.receiver = receiver;
        }
    }
    public class DeleteCommand : ICommand
    {
        private Receiver receiver;
        public string opts;
        public string reverseOpts;
        public List<string> reverseArgs;
        public DeleteCommand() { }
        public DeleteCommand(Receiver receiver, string opts)
        {
            this.receiver = receiver;
            this.opts = opts;
            if(receiver.InitializeDelete(opts))
                receiver.AddCommand(this);

        }
        public void Execute()
        {
            (reverseOpts, reverseArgs) = receiver.ExecuteDelete(opts);
        }
        public void UnExecute()
        {
            receiver.ExecuteAdd(reverseOpts, reverseArgs);
        }
        public override string ToString()
        {
            return "delete " + opts;
        }
        public void SetReceiver(Receiver receiver)
        {
            this.receiver = receiver;
        }
    }
}
