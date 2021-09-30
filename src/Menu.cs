using System;
using System.Collections.Generic;
using Spectre.Console;
using CfgComparator.Models;
using CfgComparator.Enums;

namespace CfgComparator
{
    /// <summary>
    /// Console UI.
    /// </summary>
    public class Menu
    {
        public ConfigurationsCompareResult ConfigurationsCompareResult { get; }
        public Output Output { get; }

        public Menu(ConfigurationsCompareResult configurationsCompareResult, Output output)
        {
            ConfigurationsCompareResult = configurationsCompareResult;
            Output = output;
        }

        /// <summary>
        /// Shows main menu and executes selected option.
        /// </summary>
        public void MainMenu()
        {
            Console.Clear();
            Output.InfoParameters(ConfigurationsCompareResult);
            Console.WriteLine();

            var filtrationMethod = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Filter parameters by:")
                    .PageSize(5)
                    .AddChoice("ID")
                    .AddChoices(new[] {
                        "Status",
                    }));

            ValidateFiltrationMethod(filtrationMethod);
            ContinueToMenu();
        }

        /// <summary>
        /// Navigates user to the menu screen.
        /// </summary>
        private void ContinueToMenu()
        {
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            MainMenu();
        }

        /// <summary>
        /// Outputs filtered parameters by the given id.
        /// </summary>
        /// <param name="id">User given id.</param>
        private void OutputFilteredParameters(string id)
        {
            List<ParameterDifference> filteredParameters = ConfigurationsCompareResult.Differences.FindAll(x => x.Id.StartsWith(id));
            Output.Parameters(filteredParameters);
        }

        /// <summary>
        /// Validates given filtration method and executes it accoridngly
        /// </summary>
        /// <param name="filtrationMethod"></param>
        private void ValidateFiltrationMethod(string filtrationMethod)
        {
            Console.Clear();
            if(filtrationMethod == "ID")
            {
                string id = AnsiConsole.Ask<string>("Enter the id:");
                OutputFilteredParameters(id);
            }
            else if(filtrationMethod == "Status")
            {
                var status = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<ParameterStatus>()
                        .Title("Select statuses:")
                        .NotRequired()
                        .PageSize(7)
                        .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle an option, " +
                        "[green]<enter>[/] to execute)[/]").AddChoices(new ParameterStatus[]
                        {
                            ParameterStatus.Unchanged,
                            ParameterStatus.Modified,
                            ParameterStatus.Removed,
                            ParameterStatus.Added,
                        }));
                Output.Parameters(ConfigurationsCompareResult, status);
            }
        }
    }
}
