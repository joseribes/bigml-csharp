using BigML;
using System; 
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Demo
{
    class ScriptList
    {
        static void Main()
        {
            MainAsync().Wait();
            Console.ReadLine();
        }

        /// <summary>
        /// List most recent scripts.
        /// </summary>
        static async Task MainAsync()
        {
            // New BigML client with username and API key
            var client = new Client("jribes", "8169dabca34b6ae5612a47b63dd97bead3bfe8c4");

            // Get Scripts
            Ordered<Script.Filterable, Script.Orderable, Script> result
                = (from s in client.ListScripts()
                   orderby s.Created descending
                   select s);

            Listing<Script> scripts = await result; // await response
            Execution.Arguments args = new Execution.Arguments();
            args.Inputs = new List<dynamic[]>();
            args.Inputs.Add(new dynamic[] {"dataset-id", "dataset/589ce5d7014404556a000308"});
            foreach (Script scr in scripts)
            {

                Execution ex = await client.CreateExecution(scr, "C# test", args);
                Console.WriteLine("Id: " + scr.Id + " - ResourceId: " + scr.Resource);
            }
        }
    }
}
