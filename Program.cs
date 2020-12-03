using System;
using System.Linq;
using System.Threading.Tasks;
using CarvedRockSoftware.Seeder.AzureSearch;
using CarvedRockSoftware.Seeder.AzureSql;
using CarvedRockSoftware.Seeder.AzureStorageQueues;
using CarvedRockSoftware.Seeder.AzureStorageTables;
using CarvedRockSoftware.Seeder.CosmosDB;
using Spectre.Console;

namespace CarvedRockSoftware.Seeder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            AnsiConsole.Render(new FigletText("Carved Rock Software").Color(Color.Red).Alignment(Justify.Left));

            if(!Enum.TryParse<MenuOption>(args.FirstOrDefault(), true, out var choice))
            {
                var input = AnsiConsole.Prompt(new TextPrompt<string>("Select a service to seed")
                    .AddChoice(MenuOption.AzureSearch.ToString())
                    .AddChoice(MenuOption.AzureSql.ToString())
                    .AddChoice(MenuOption.AzureStorageQueues.ToString())
                    .AddChoice(MenuOption.AzureStorageTables.ToString())
                    .AddChoice(MenuOption.CosmosDB.ToString())
                    .InvalidChoiceMessage("[red]That's not a valid choice[/]"));
                choice = Enum.Parse<MenuOption>(input, true);
            }
            
            ISeeder seeder = choice switch
            {
                MenuOption.AzureSearch => new AzureSearchSeeder(),
                MenuOption.AzureSql => new AzureSqlSeeder(),
                MenuOption.AzureStorageQueues => new AzureStorageQueuesSeeder(),
                MenuOption.AzureStorageTables => new AzureStorageTablesSeeder(),
                MenuOption.CosmosDB => new CosmosDBSeeder(),
                _ => throw new NotSupportedException()
            };

            try
            {
                await seeder.RunAsync();
            }
            catch (Exception exception)
            {
                AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
            }
        }
    }
}
