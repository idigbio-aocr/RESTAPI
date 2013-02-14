namespace iDigBio.RESTService.Controllers
{
    using iDigBio.RESTService.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class ParsingResultsController : ApiController
    {
        public ParsingResultsController()
        {
        }

        // POST api/ParsingResults
        public HttpResponseMessage Post(ParsingResult newItem)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            newItem.ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            newItem.createdUTCDateTime = DateTime.UtcNow;

            ParsingResult retVal = null;

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = true;
                dbContext.ParsingResults.Add(newItem);
                dbContext.SaveChanges();
            }

            // Now, let's get our return object.
            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                retVal =
                    (from x in dbContext.ParsingResults
                     where (x.fileNameIdentifier == newItem.fileNameIdentifier || x.sourceUUID == newItem.sourceUUID)
                     && x.createdByUserName == newItem.createdByUserName
                     select x).OrderByDescending(x => x.createdUTCDateTime).Take(1).FirstOrDefault();
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, retVal);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { parsingResultsId = retVal.parsingResultId }));
            return response;
        }

        public ParsingResult Get(string identifier, string createdByUserName)
        {
            ParsingResult retVal = null;
            Guid myGuidIdentifier = Guid.Empty;

            Guid.TryParse(identifier, out myGuidIdentifier);

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                if (myGuidIdentifier != Guid.Empty)
                {
                    retVal =
                        (from x in dbContext.ParsingResults
                         where x.sourceUUID == myGuidIdentifier
                            && x.createdByUserName == createdByUserName
                         select x).OrderByDescending(x => x.createdUTCDateTime).Take(1).FirstOrDefault();
                }
                else
                {
                    retVal =
                        (from x in dbContext.ParsingResults
                         where x.fileNameIdentifier == identifier
                            && x.createdByUserName == createdByUserName
                         select x).OrderByDescending(x => x.createdUTCDateTime).Take(1).FirstOrDefault();
                }
            }

            return retVal;
        }

    }

}
