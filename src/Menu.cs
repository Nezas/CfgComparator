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

            var filtrationMethod = AnsiConsole.Prompt(
                new SelectionPrompt<FiltrationMethod>()
                    .Title("\nFilter parameters by:")
                    .PageSize(5)
                    .AddChoice(FiltrationMethod.Id)
                    .AddChoices(new[] {
                        FiltrationMethod.Status,
                    }));

            Console.Clear();
            ValidateFiltrationMethod(filtrationMethod);
            ContinueToMenu();
        }

        /// <summary>
        /// Validates given filtration method and executes it accordingly.
        /// </summary>
        /// <param name="filtrationMethod"></param>
        private void ValidateFiltrationMethod(FiltrationMethod filtrationMethod)
        {
            if(filtrationMethod == FiltrationMethod.Id)
            {
                FilterById();
            }
            else
            {
                FilterByStatus();
            }
        }

        /// <summary>
        /// Filter parameters by id.
        /// </summary>
        private void FilterById()
        {
            string id = AnsiConsole.Ask<string>("Enter the id: ");
            List<ParameterDifference> filteredParameters = ConfigurationsCompareResult.Differences.FindAll(x => x.Id.StartsWith(id));
            Output.Parameters(filteredParameters);
        }

        /// <summary>
        /// Filter parameters by status.
        /// </summary>
        private void FilterByStatus()
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

        /// <summary>
        /// Navigates user to the menu screen.
        /// </summary>
        private void ContinueToMenu()
        {
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            MainMenu();
        }

    }
}
