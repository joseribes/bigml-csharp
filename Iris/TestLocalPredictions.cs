using BigML;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iris
{
    class TestLocalPredictions
    {
        static void Main()
        {
            MainAsync().Wait();
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            // Configuration to read decimal numbers rightly
            System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            var client = new Client("rm1", "c4aef8539db2e8dbbee17ab0d188dec28f1e7933", vpcDomain: "realmatch.vpc.bigml.io", useContextInAwaits:false);
            Ensemble ensemble;
            string ensembleId = "ensemble/57f05b2b4fb512058e000b45";
            // No push, so we need to busy wait for the source to be processed.
            while ((ensemble = await client.Get<Ensemble>(ensembleId)).StatusMessage.NotSuccessOrFail())
            {
                await Task.Delay(5);
            }
            var localEnsemble = ensemble.EnsembleStructure;

            // populate the tree of each Model
            Model modelInEnsemble = null;
            modelInEnsemble = new Model();
            string modelId;
            for (int i = 0; i < ensemble.Models.Count; i++)
            {
                modelId = ensemble.Models[i];
                while ((modelInEnsemble = await client.Get<Model>(modelId)).StatusMessage.NotSuccessOrFail())
                {
                    await Task.Delay(2);
                }

                if (i == 0)
                {
                    // the first model processes fields information
                    localEnsemble.addLocalModel(modelInEnsemble.ModelStructure(null));
                } else
                {
                    // following models use fields information from the first model
                    localEnsemble.addLocalModel(modelInEnsemble.ModelStructure(localEnsemble._models[0].Fields));
                }
                Console.WriteLine((i+1) + "/" + ensemble.Models.Count + " models loaded");
            }
            
            Dictionary<string, dynamic> inputData;
            Dictionary<object, object> results;

            string fileName = "Validation_2016-09-29_16-08-45.csv";

            var reader = new System.IO.StreamReader(System.IO.File.OpenRead(@"C:/Users/Jose/Downloads/" + fileName));
            List<string> listFields = new List<string>();
            List<string> listIDs = new List<string>();
            List<dynamic> listValues = new List<dynamic>();
            string line;
            
            // process headers. get the fieldID of each input
            line = reader.ReadLine();
            listFields.AddRange(line.Split(';'));

            int index;
            string fieldName;
            for (index = 0; index < listFields.Count; index++)
            {
                fieldName = listFields[index];
                listIDs.Add(modelInEnsemble.ModelStructure().getFieldByName(fieldName).Id);
            }

            string[] values;
            double diff = 0.0; // each prediction error
            double totalDiffPow = 0.0; // used as sumattory of errors
            int N = 0;
            
            if (modelInEnsemble != null)
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(';');
                    listValues.Clear();
                    listValues.AddRange(values);

                    inputData = new Dictionary<string, dynamic>();
                    for (index = 0; index < listFields.Count; index++)
                    {
                        fieldName = listFields[index];

                        if (modelInEnsemble.ModelStructure().getFieldByName(fieldName).Optype == OpType.Numeric)
                        {
                            listValues[index] = Convert.ToDouble(listValues[index], provider);
                        }

                        inputData.Add(listIDs[index], listValues[index]);
                    }
                    N += 1;

                    results = localEnsemble.predict(inputData, byName:false, combiner: Combiner.Plurality, addDistribution: false);

                    // In this example objective field in the first column of CSV file
                    diff = Convert.ToDouble(listValues[0]) - (double)results["prediction"];
                    totalDiffPow += (diff * diff);
                }
            }

            Console.WriteLine(N + " predictions done");
            Console.WriteLine("");

            // Calculates RMSE = sqrt( sum( predicted-actual )^2 / N)
            var RMSE = Math.Sqrt(totalDiffPow / N);
            Console.WriteLine("SE: " + totalDiffPow);
            Console.WriteLine("MSE: " + totalDiffPow / N);
            Console.WriteLine("RMSE: " + RMSE);
        }
    }
}
