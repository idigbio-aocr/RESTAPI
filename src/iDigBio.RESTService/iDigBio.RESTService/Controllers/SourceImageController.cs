namespace iDigBio.RESTService.Controllers
{
    using iDigBio.Core;
    using iDigBio.RESTService.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class SourceImagesController : ApiController
    {

        public IEnumerable<SourceImage> GetAllSourceImages()
        {
            List<SourceImage> mySourceImageList = new List<SourceImage>();

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false; 
                mySourceImageList =
                    (from x in dbContext.SourceImages
                     join y in dbContext.OCRCachedResults
                        on x.sourceImageId equals y.sourceImageId
                     where x.fileName != null
                        && x.url != null
                        && y.processEngineId == (int)enumProcessEngine.human
                     select x).Take(5).ToList();
            }

            return mySourceImageList;
        }

        public IEnumerable<SourceImage> GetSourceImageByFileName(string fileName)
        {
            List<SourceImage> retVal = null;

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                retVal =
                    (from x in dbContext.SourceImages
                     where x.fileName == fileName
                     select x).OrderByDescending(x => x.createdUTCDateTime).ToList();
            }

            return retVal;
        }

        public SourceImage GetSourceImageById(string sourceImageId)
        {
            Guid myId = Guid.Empty;
            Guid.TryParse(sourceImageId, out myId);
            SourceImage retVal = null;

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                retVal =
                    (from x in dbContext.SourceImages
                     where x.sourceImageId == myId
                     select x).FirstOrDefault();
            }

            return retVal;
        }

        public SourceImage GetSourceImageByAlternateId(string alternateId)
        {
            SourceImage retVal = null;

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                retVal =
                    (from x in dbContext.SourceImages
                     where x.alternateId == alternateId
                     select x).FirstOrDefault();
            }

            return retVal;
        }

        public IEnumerable<SourceImage> GetSourceImagesBySourceImageRepositoryId(string sourceImageRepositoryId)
        {
            Guid myId = Guid.Empty;
            Guid.TryParse(sourceImageRepositoryId, out myId);
            List<SourceImage> retVal = null;

            using (var dbContext = new iDigBioEntities())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                retVal =
                    (from x in dbContext.SourceImages
                     where x.sourceImageRepositoryId == myId
                     select x).ToList();
            }

            return retVal;
        }

        // Test Data
        //SourceImage[] sourceImages = new SourceImage[] 
        //{ 
        //    new SourceImage { SourceImageId = new Guid("78B2B044-616D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/ny/lichens/NY01281880_lg.jpg", AlternateIdentifier = "904637", SourceImageRepositoryId = new Guid("7392B8CE-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("79B2B044-616D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/ny/lichens/NY01281884_lg.jpg", AlternateIdentifier = "904638", SourceImageRepositoryId = new Guid("7392B8CE-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("7AB2B044-616D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/ny/lichens/NY01281886_lg.jpg", AlternateIdentifier = "904639", SourceImageRepositoryId = new Guid("7392B8CE-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("7BB2B044-616D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/ny/lichens/NY01281887_lg.jpg", AlternateIdentifier = "904640", SourceImageRepositoryId = new Guid("7392B8CE-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("7CB2B044-616D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/ny/lichens/NY01281888-1_lg.jpg", AlternateIdentifier = "904641", SourceImageRepositoryId = new Guid("7392B8CE-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("2C0E9CBA-626D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/duke/bryophytes/00010/0010811_1_lg.jpg", AlternateIdentifier = "1417841", SourceImageRepositoryId = new Guid("4B73A7FD-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("2D0E9CBA-626D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/duke/bryophytes/00014/0014958_1_lg.jpg", AlternateIdentifier = "1482420", SourceImageRepositoryId = new Guid("4B73A7FD-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("2E0E9CBA-626D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/duke/bryophytes/00179/0179051_1_lg.jpg", AlternateIdentifier = "1649765", SourceImageRepositoryId = new Guid("4B73A7FD-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("2F0E9CBA-626D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/duke/bryophytes/00179/0179700_1_lg.jpg", AlternateIdentifier = "1649766", SourceImageRepositoryId = new Guid("4B73A7FD-606D-E211-BE78-CC52AF888AB6") }, 
        //    new SourceImage { SourceImageId = new Guid("300E9CBA-626D-E211-BE78-CC52AF888AB6"), SourceUrl = "http://storage.idigbio.org/duke/bryophytes/00179/0179722_1_lg.jpg", AlternateIdentifier = "1649767", SourceImageRepositoryId = new Guid("4B73A7FD-606D-E211-BE78-CC52AF888AB6") } 
        //};

    }

}
