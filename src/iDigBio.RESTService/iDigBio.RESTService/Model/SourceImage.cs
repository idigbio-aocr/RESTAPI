//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iDigBio.RESTService.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SourceImage
    {
        public SourceImage()
        {
            this.OCRCachedResults = new HashSet<OCRCachedResult>();
            this.SourceImageProcessRequests = new HashSet<SourceImageProcessRequest>();
        }
    
        public System.Guid sourceImageId { get; set; }
        public System.Guid sourceImageRepositoryId { get; set; }
        public string fileName { get; set; }
        public string url { get; set; }
        public string alternateId { get; set; }
        public System.DateTime createdUTCDateTime { get; set; }
    
        public virtual ICollection<OCRCachedResult> OCRCachedResults { get; set; }
        public virtual ICollection<SourceImageProcessRequest> SourceImageProcessRequests { get; set; }
        public virtual SourceImageRepository SourceImageRepository { get; set; }
    }
}