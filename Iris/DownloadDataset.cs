using BigML;
using System; 
using System.Threading.Tasks;

namespace Iris
{
    class DownloadDataset
    {
        static void Main()
        {
            MainAsync().Wait();
            Console.ReadLine();
        }

        /// <summary>
        /// Simple sample that runs through all steps to explicitly create a
        /// local prediction from a csv file with the classic iris data.
        /// </summary>
        static async Task MainAsync()
        {
            // New BigML client with username and API key
            var client = new Client("myuser", "8169dabca34b6ae5612a47b63dd97bead3bfeXXX");

            // Get Dataset
            string datasetId = "dataset/589ce5d7014404556a000308";
            var dataset = await client.Get<DataSet>(datasetId);
            // No push, so we need to busy wait for the dataset to be processed.
            while ((dataset = await client.Get(dataset)).StatusMessage.NotSuccessOrFail())
            {
                await Task.Delay(10);
            }
            string fileName = "C:/Users/Jose/SourceCode/Repos/" + dataset.Id + ".csv";

            // Download dataset
            if (System.IO.File.Exists(fileName)) {
                System.IO.File.Delete(fileName);
            }
            await client.DownloadDataset(datasetId, fileName);
        }
    }
}
