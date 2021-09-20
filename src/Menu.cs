using System;
using System.Collections.Generic;
using CfgComparator.Models;

namespace CfgComparator
{
    public class Menu
    {
        public Record Source { get; }
        public Record Target { get; }
        public List<string> Unchanged { get; }
        public List<string> Modified { get; }
        public List<string> Removed { get; }
        public List<string> Added { get; }
        public Output Output { get; }

        public Menu(Record source, Record target, List<string> unchanged, List<string> modified, List<string> removed, List<string> added, Output output)
        {
            Source = source;
            Target = target;
            Unchanged = unchanged;
            Modified = modified;
            Removed = removed;
            Added = added;
            Output = output;
        }

        public void Start()
        {
            MenuText();

            while(true)
            {
                switch(Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            //
                            ContinueToMenu();
                            break;
                        }
                    case "2":
                        {
                            Console.Clear();
                            Output.Parameters(Unchanged);
                            ContinueToMenu();
                            break;
                        }
                    case "3":
                        {
                            Console.Clear();
                            Output.Parameters(Modified);
                            ContinueToMenu();
                            break;
                        }
                    case "4":
                        {
                            Console.Clear();
                            Output.Parameters(Removed);
                            ContinueToMenu();
                            break;
                        }
                    case "5":
                        {
                            Console.Clear();
                            Output.Parameters(Added);
                            ContinueToMenu();
                            break;
                        }
                    case "0":
                        {
                            Environment.Exit(0);
                            break;
                        }
                    default:
                        {
                            Console.Write("Invalid input!\n");
                            Console.Write("\nEnter your choice: ");
                            break;
                        }
                }
            }
        }
        private void MenuText()
        {
            Console.Clear();
            Output.InfoParameters(Source, Target, Unchanged, Modified, Removed, Added);
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1 - Filter parameters by id");
            Console.WriteLine("2 - Unchanged parameters");
            Console.WriteLine("3 - Modified parameters");
            Console.WriteLine("4 - Removed parameters");
            Console.WriteLine("5 - Added parameters");
            Console.WriteLine("0 - Exit");
            Console.Write("\nEnter your choice: ");
        }

        private void ContinueToMenu()
        {
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            MenuText();
        }
    }
}
