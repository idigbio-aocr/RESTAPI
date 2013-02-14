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

    public class ImageProcessRequestController : ApiController
    {
        public class PostSourceImageProcessRequestParams
        {
            public string sourceImageId { get; set; }
            public string processEngineId { get; set; }
            public string callbackUri { get; set; }
            public string userName { get; set; }
        }

        // POST api/ImageProcessRequest
        public HttpResponseMessage PostSourceImageProcessRequest(PostSourceImageProcessRequestParams p) //[FromBody] string sourceImageId, [FromBody] string processEngineId, [FromBody] string callbackUri, [FromBody] string userName)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            SourceImageProcessRequest retVal = null;
            SourceImageProcessRequest newItem = null;

            Guid mySourceImageId = Guid.Empty;
            if (!Guid.TryParse(p.sourceImageId, out mySourceImageId))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "sourceImageId parameter is invalid.");

            int myProcessEngineId = 0;
            if (!int.TryParse(p.processEngineId, out myProcessEngineId))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "processEngineId parameter is invalid.");

            String ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = true;

                // Ensure we have a corresponding source image.
                SourceImage mySourceImage =
                    (from x in dbContext.SourceImages
                     where x.sourceImageId == mySourceImageId
                     select x).FirstOrDefault();

                if (mySourceImage != null)
                {
                    newItem = dbContext.SourceImageProcessRequests.Create();
                    dbContext.SourceImageProcessRequests.Add(newItem);

                    if (!string.IsNullOrWhiteSpace(p.callbackUri))
                        newItem.callbackUri = p.callbackUri;

                    newItem.createdByUserName = p.userName;
                    newItem.createdUTCDateTime = DateTime.UtcNow;
                    newItem.ipAddress = ipAddress;
                    newItem.processEngineId = myProcessEngineId;
                    newItem.sourceImageId = mySourceImageId;
                    newItem.sourceImageProcessRequestId = Guid.NewGuid();

                    dbContext.SaveChanges();
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "sourceImageId parameter does not correspond to a known record.");
                }
            }


            // Now, let's get our return object.
            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                retVal =
                    (from x in dbContext.SourceImageProcessRequests
                     where x.sourceImageProcessRequestId == newItem.sourceImageProcessRequestId
                     select x).FirstOrDefault();
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, retVal);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { sourceImageProcessRequestId = retVal.sourceImageProcessRequestId }));
            return response;
        }

        public SourceImageProcessRequest GetSourceImageProcessRequest(string sourceImageProcessRequestId)
        {
            Guid myId = Guid.Empty;
            SourceImageProcessRequest retVal = null;
            SourceImageProcessRequest myOCRSimulation = null;

            if (!Guid.TryParse(sourceImageProcessRequestId, out myId))
                return retVal;

            #region Simulate OCR Processing
            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = true;    // Required for change tracking if we update the DB below.
                myOCRSimulation =
                    (from x in dbContext.SourceImageProcessRequests
                     where x.sourceImageProcessRequestId == myId
                     select x).FirstOrDefault();

                if (myOCRSimulation != null && !myOCRSimulation.resultCreatedUTCDateTime.HasValue)
                {   // First request for results, let's try to populate from our cached results.
                    dbContext.SourceImageProcessRequests.Attach(myOCRSimulation);

                    OCRCachedResult myOCRResult =
                    (from x in dbContext.OCRCachedResults
                     where x.sourceImageId == myOCRSimulation.sourceImageId
                        && x.processEngineId == myOCRSimulation.processEngineId
                     select x).FirstOrDefault();

                    if (myOCRResult != null)
                    {
                        myOCRSimulation.resultValue = myOCRResult.value;
                        myOCRSimulation.resultCreatedUTCDateTime = DateTime.UtcNow;
                    }
                    else
                    {
                        myOCRSimulation.resultCreatedUTCDateTime = DateTime.UtcNow;
                    }

                    // Handle the callback functionality.
                    if (!string.IsNullOrWhiteSpace(myOCRSimulation.callbackUri))
                    {
                        try
                        {
                            // Stream variables.
                            Stream responseStream = null;
                            StreamReader reader = null;
                            string myCallbackUri = String.Concat(myOCRSimulation.callbackUri, "?sourceImageProcessRequestId=", myOCRSimulation.sourceImageProcessRequestId.ToString());
                            HttpWebRequest request = WebRequest.Create(myOCRSimulation.callbackUri) as HttpWebRequest;
                            request.Timeout = 2000;
                            request.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(MyRemoteCertificateValidationCallback);

                            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                            request.Method = "GET";
                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                            // Parse the web response.
                            responseStream = response.GetResponseStream();
                            reader = new StreamReader(responseStream);

                            myOCRSimulation.resultCallbackUri = string.Concat(((int)response.StatusCode).ToString(), "/", Enum.GetName(typeof(HttpStatusCode), response.StatusCode),
                                "Response: ", reader.ReadToEnd());
                        }
                        catch (Exception ex)
                        {
                            myOCRSimulation.resultCallbackUri = ex.Message;
                        }
                    }

                    if (myOCRSimulation.resultCallbackUri != null && myOCRSimulation.resultCallbackUri.Length > 4000)
                        myOCRSimulation.resultCallbackUri = myOCRSimulation.resultCallbackUri.Substring(0, 4000);

                    dbContext.SaveChanges();
                }
            }
            #endregion

            using (var dbContext = new iDigBioEntities())   // Note, you need to make a new context in order to change the ProxyCreationEnabled property.
            {// Now, let's get our actual return value.
                dbContext.Configuration.ProxyCreationEnabled = false;  // Ensure the return object is serializable.
                retVal =
                    (from x in dbContext.SourceImageProcessRequests
                     where x.sourceImageProcessRequestId == myId
                     select x).FirstOrDefault();
            }

            return retVal;
        }


        private bool MyRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }

}
