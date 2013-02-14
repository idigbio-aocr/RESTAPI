using iDigBio.Core;
using iDigBio.RESTService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.IO;

namespace iDigBio.Cnsl
{
    class Program
    {
        static void Main(string[] args)
        {
            string scriptFile = @"D:\Code_Tmp\Powershell\b.ps1";
            FileInfo fiScriptFile = new FileInfo(scriptFile);

            if (!fiScriptFile.Exists)
                throw new InvalidOperationException(string.Format("Unable to locate script file {0} on disk.", scriptFile));

            string scriptToRun = File.ReadAllText(fiScriptFile.FullName);
            if (string.IsNullOrWhiteSpace(scriptToRun))
                throw new InvalidOperationException(string.Format("No content was found in script file {0}.", scriptFile));

            RunspaceConfiguration runspaceConfiguration = RunspaceConfiguration.Create();
            Runspace runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            runspace.Open();

            RunspaceInvoke scriptInvoker = new RunspaceInvoke(runspace);

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptToRun);

            // Execute PowerShell script
            pipeline.Invoke();

            // Close the runspace
            runspace.Close();

            //Console.WriteLine("Press any key to continue....");
            //Console.ReadKey();
        }

        //ImportOCRResultsFromOutputFiles();
        private static void ImportOCRResultsFromOutputFiles()
        {
            Guid sourceImageRepositoryId = new Guid("7392B8CE-606D-E211-BE78-CC52AF888AB6");

            using (var dbContext = new iDigBioEntities())
            {
                string myEnvironment = ConfigurationManager.AppSettings[iConst.CONFIG_ENVIRONMENT];

                // To start, let's get all of the processing engines that have configuration.
                List<SourceImageRepositoryProcessEngineConfig> configuredEngines =
                    (from x in dbContext.SourceImageRepositoryProcessEngineConfigs
                     where x.environment == myEnvironment
                     select x).ToList();

                // For each engine, let's loop through our SourceImage data and see if we can locate results.
                foreach (var configuredEngine in configuredEngines)
                {
                    Task.Run(async () =>
                    {
                        OCRCachedResultManager myManager = new OCRCachedResultManager();
                        myManager.PopulateOCRCachedResult(configuredEngine, sourceImageRepositoryId);
                    });

                    //break; // For testing, let's just do one.
                }
            }
        }



        // Superceded:
        //// For each engine, let's loop through our SourceImage data and see if we can locate results.
        //foreach (var configuredEngine in configuredEngines)
        //{
        //    int currentIndex = 0;
        //    int batchSize = 50;
        //    int recordsReturnedCount = 0;

        //    do
        //    {
        //        List<SourceImageIdAndUrl> idsAndUrls =
        //        (from x in dbContext.SourceImages
        //         join y in dbContext.OCRCachedResults
        //            on new { x.sourceImageId, configuredEngine.processEngineId } equals new { y.sourceImageId, y.processEngineId }
        //            into joinedSIM

        //         from defaultedSIM in joinedSIM.DefaultIfEmpty()
        //         where defaultedSIM.ocrSimulatedResultId == null
        //         orderby x.sourceImageId
        //         select new SourceImageIdAndUrl { sourceImageId = x.sourceImageId, url = x.url }
        //         ).Skip(currentIndex).Take(batchSize).ToList();


        //        Task.Run(async () =>
        //        {
        //            List<OCRCachedResult> ocrResults = PopulateOCRCachedResult(configuredEngine, idsAndUrls);
        //            foreach (var ocrResult in ocrResults)
        //            {
        //                if (!string.IsNullOrWhiteSpace(ocrResult.value))
        //                {   // Store the result in the database.
        //                }
        //            }
        //        });

        //        recordsReturnedCount = -1; // Kill the loop for testing purposes.

        //        recordsReturnedCount = idsAndUrls.Count();
        //        currentIndex += batchSize;
        //    } while (recordsReturnedCount == batchSize);
        //  }


    }
}
