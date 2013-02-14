namespace iDigBio.RESTService.Controllers
{
    using iDigBio.RESTService.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class ParsingResultsCSVController : ApiController
    {
        public ParsingResultsCSVController()
        {
        }

        public HttpResponseMessage Get(string identifier, string createdByUserName)
        {
            ParsingResult itemToExport = null;
            Guid myGuidIdentifier = Guid.Empty;

            Guid.TryParse(identifier, out myGuidIdentifier);

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                if (myGuidIdentifier != Guid.Empty)
                {
                    itemToExport =
                        (from x in dbContext.ParsingResults
                         where x.sourceUUID == myGuidIdentifier
                         && x.createdByUserName == createdByUserName
                         select x).OrderByDescending(x => x.createdUTCDateTime).Take(1).FirstOrDefault();
                }
                else
                {
                    itemToExport =
                        (from x in dbContext.ParsingResults
                         where x.fileNameIdentifier == identifier
                         && x.createdByUserName == createdByUserName
                         select x).OrderByDescending(x => x.createdUTCDateTime).Take(1).FirstOrDefault();
                }
            }

            var content = new MultipartFormDataContent();
            HttpResponseMessage response = null;

            if (itemToExport != null)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
                MemoryStream stream = ParsingResultExport.ExportParsingResult(itemToExport) as MemoryStream;

                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = string.Concat(identifier, ".csv")
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return response;
        }

    }

}
