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
    
    public partial class SourceImageProcessRequest
    {
        public System.Guid sourceImageProcessRequestId { get; set; }
        public System.Guid sourceImageId { get; set; }
        public int processEngineId { get; set; }
        public string callbackUri { get; set; }
        public string ipAddress { get; set; }
        public string resultValue { get; set; }
        public string resultCallbackUri { get; set; }
        public Nullable<System.DateTime> resultCreatedUTCDateTime { get; set; }
        public string createdByUserName { get; set; }
        public System.DateTime createdUTCDateTime { get; set; }
    
        public virtual ProcessEngine ProcessEngine { get; set; }
        public virtual SourceImage SourceImage { get; set; }
    }
}
