using System;
using System.Collections.Generic;
using CfgComparator.Models;

namespace CfgComparator
{
    /// <summary>
    /// Console UI.
    /// </summary>
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

        /// <summary>
        /// Shows <see cref="MenuText"></see> and executes selected option.
        /// </summary>
        public void MainMenu()
        {
            MenuText();

            while(true)
            {
                switch(Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            Console.Write("Enter id: ");
                            string id = Console.ReadLine();
                            Console.WriteLine();
                            FilterParameters(id);
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

        /// <summary>
        /// Text to be displayed on the menu screen.
        /// </summary>
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

        /// <summary>
        /// Navigates user to the menu screen.
        /// </summary>
        private void ContinueToMenu()
        {
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            MenuText();
        }

        /// <summary>
        /// Filters parameters by the given id.
        /// </summary>
        /// <param name="id">User given id.</param>
        private void FilterParameters(string id)
        {
            List<string> filteredParameters = new();

            filteredParameters.AddRange(Removed.FindAll(x => x.StartsWith(id)));
            filteredParameters.AddRange(Added.FindAll(x => x.StartsWith(id)));
            filteredParameters.AddRange(Modified.FindAll(x => x.StartsWith(id)));
            filteredParameters.AddRange(Unchanged.FindAll(x => x.StartsWith(id)));

            Output.Parameters(filteredParameters);
        }
    }
}
