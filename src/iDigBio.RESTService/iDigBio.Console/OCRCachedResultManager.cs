using iDigBio.Core;
using iDigBio.RESTService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDigBio.Cnsl
{
    public class OCRCachedResultManager
    {
        public void PopulateOCRCachedResult(SourceImageRepositoryProcessEngineConfig configuredEngine, Guid sourceImageRepositoryId)
        {
            SourceImage sourceImage = null;
            OCRCachedResult cachedResult = null;
            DirectoryInfo processEngineOutputDir = new DirectoryInfo(configuredEngine.outputDirectoryPath);

            if (!processEngineOutputDir.Exists)
                return;

            DirectoryInfo cachedResultDir = new DirectoryInfo(Path.Combine(processEngineOutputDir.FullName, iConst.OCRCACHEDRESULT_DIRECTORYNAME));
            if (!cachedResultDir.Exists)
                cachedResultDir.Create();

            using (var dbContext = new iDigBioEntities())
            {
                foreach (var fileItem in processEngineOutputDir.GetFiles("*.txt"))
                {
                    cachedResult =
                        (from cr in dbContext.OCRCachedResults.Include("SourceImage")
                         join si in dbContext.SourceImages
                            on cr.sourceImageId equals si.sourceImageId
                         where cr.processEngineId == configuredEngine.processEngineId
                            && si.fileName == fileItem.Name
                         select cr).FirstOrDefault();

                    if (cachedResult == null)
                    {   // We don't have a cached result for this process engine.
                        cachedResult = dbContext.OCRCachedResults.Create();
                        dbContext.OCRCachedResults.Add(cachedResult);

                        // Even if we don't have a cached result, we might still have a SourceImage record, let's check.
                        sourceImage =
                            (from x in dbContext.SourceImages
                             where x.sourceImageRepositoryId == sourceImageRepositoryId
                                && x.fileName == fileItem.Name
                             select x).FirstOrDefault();

                        if (sourceImage == null)
                        {   // We don't have a SourceImage record for this file yet.  Let's create one.
                            sourceImage = dbContext.SourceImages.Create();
                            dbContext.SourceImages.Add(sourceImage);

                            sourceImage.sourceImageRepositoryId = sourceImageRepositoryId;
                            sourceImage.sourceImageId = Guid.NewGuid();
                            sourceImage.fileName = fileItem.Name;

                        }

                        cachedResult.sourceImageId = sourceImage.sourceImageId;
                        cachedResult.processEngineId = configuredEngine.processEngineId;
                    }

                    cachedResult.value = File.ReadAllText(fileItem.FullName);
                    if (cachedResult.value.Length > 4000)
                        cachedResult.value = cachedResult.value.Substring(0, 4000);

                    dbContext.SaveChanges();
                    // Now, let's move the file to indicate it has already been processed.
                    fileItem.MoveTo(Path.Combine(cachedResultDir.FullName, fileItem.Name));
                }
            }
        }


        //public partial class SourceImageIdAndUrl
        //{
        //    public Guid sourceImageId { get; set; }
        //    public string url { get; set; }
        //}


        //private static List<OCRCachedResult> PopulateOCRCachedResult(SourceImageRepositoryProcessEngineConfig configuredEngine, List<SourceImageIdAndUrl> idsAndUrls)
        //{
        //    List<OCRCachedResult> retVal = new List<OCRCachedResult>();

        //    foreach (var item in idsAndUrls)
        //    {
        //        OCRCachedResult simResult = new OCRCachedResult() { processEngineId = configuredEngine.processEngineId, sourceImageId = item.sourceImageId };
        //        string fileName = ExtractFileNameFromUrl(item.url);
        //        string path = Path.Combine(configuredEngine.outputDirectoryPath, fileName);

        //        if (File.Exists(path))
        //            simResult.value = File.ReadAllText(path);

        //        retVal.Add(simResult);
        //    }

        //    return retVal;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// URL Examples:
        ///         http://storage.idigbio.org/ny/lichens/NY01281891_lg.jpg
        ///         http://storage.idigbio.org/vt/bryophytes/UVMVT002/UVMVT002023_lg.jpg
        /// </remarks>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ExtractFileNameFromUrl(string url)
        {
            string retVal = string.Empty;
            int lastSlashIndex = url.LastIndexOf('/');

            if (!string.IsNullOrWhiteSpace(url) && lastSlashIndex > 0)
            {
                retVal = url.Substring(url.LastIndexOf('/') + 1);
            }

            return retVal;
        }

    }
}
