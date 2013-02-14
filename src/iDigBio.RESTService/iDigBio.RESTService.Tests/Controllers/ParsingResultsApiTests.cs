using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Text;
//using NUnit.Framework;
using iDigBio.RESTService.Model;
using iDigBio.RESTService.Controllers;
using iDigBio.Core;
using System.IO;

namespace iDigBio.RESTService.Tests.Controllers
{
    [TestClass]
    public class ParsingResultApiTests : ApiTestBase
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void AddNewParsingResultUsingSourceUUID()
        {
            var client = CreateWebClient();
            const string url = ApiUrlRoot + "/parsingresults";
            const string method = "POST";

            ParsingResult myNewParsingResult = GetParsingResultMockObject(1);
            string contentJSON = Newtonsoft.Json.JsonConvert.SerializeObject(myNewParsingResult);
            client.Headers.Add("Content-Type", "application/json");
            var response = client.UploadString(url, method, contentJSON);

            Console.Write(response);
        }

        [TestMethod]
        public void AddNewParsingResultUsingFileName()
        {
            var client = CreateWebClient();
            const string url = ApiUrlRoot + "/parsingresults";
            const string method = "POST";

            ParsingResult myNewParsingResult = GetParsingResultMockObject(2);
            string contentJSON = Newtonsoft.Json.JsonConvert.SerializeObject(myNewParsingResult);

            contentJSON = contentJSON.Replace(",\"sourceUUID\":null", string.Empty);
            contentJSON = contentJSON.Replace(",\"createdUTCDateTime\":\"0001-01-01T00:00:00\"", string.Empty);
            contentJSON = contentJSON.Replace(",\"ipAddress\":null", string.Empty);

            client.Headers.Add("Content-Type", "application/json");
            var response = client.UploadString(url, method, contentJSON);

            Console.Write(response);
        }

        [TestMethod]
        public void GetSingleParsingResultBySourceUUID()
        {
            Guid sourceUUID = new Guid("B4DCD40F-422E-4E70-BA46-1C3DFD98254B");
            //Console.WriteLine(string.Format("parsingResultId = {0}", parsingResultId));

            var client = CreateWebClient();
            //string url = string.Format("{0}/parsingresults?{1}", ApiUrlRoot, parsingResultId);
            string url = string.Format("{0}/parsingresults?identifier={1}", ApiUrlRoot, sourceUUID);

            client.Headers.Add("Content-Type", "application/json");
            var response = client.DownloadString(url); //method = "GET";

            Console.Write(response);
        }

        [TestMethod]
        public void GetSingleParsingResultByFileName()
        {
            string fileName = "NY01075759_lg.csv";

            var client = CreateWebClient();
            string url = string.Format("{0}/parsingresults?identifier={1}", ApiUrlRoot, fileName);

            client.Headers.Add("Content-Type", "application/json");
            var response = client.DownloadString(url); //method = "GET";

            Console.Write(response);
        }

        [TestMethod]
        public void WriteCSVTest()
        {
            string fileName = "D:\\foo2.csv";
            FileInfo myFile = new FileInfo(fileName);
            ParsingResult myNewParsingResult = GetParsingResultMockObject(1);

            if (myFile.Exists)
                myFile.Delete();

            ParsingResultExport.ExportParsingResult(myNewParsingResult, fileName);
            myFile = new FileInfo(fileName); // Refresh the object to get a current "Exists" value.
            Assert.IsTrue(myFile.Exists, "CSV output file does not exist.");
        }


        [TestMethod]
        public void GetSingleParsingResultAsCSVBySourceUUID()
        {
            string fileName = "D:\\foo3.csv";
            FileInfo myFile = new FileInfo(fileName);
            string createdByUserName = "foo";
            Guid sourceUUID = new Guid("18E2EEF2-D426-4F6C-BBF7-F79A234E4F81");
            Console.WriteLine(string.Format("sourceUUID = {0}", sourceUUID));

            if (myFile.Exists)
                myFile.Delete();

            var client = CreateWebClient();
            string url = string.Format("{0}/parsingresultscsv?identifier={1}&createdByUserName={2}", ApiUrlRoot, sourceUUID, createdByUserName);

            client.DownloadFile(url, fileName); //method = "GET";

            myFile = new FileInfo(fileName); // Refresh the object to get a current "Exists" value.
            Assert.IsTrue(myFile.Exists, "CSV output file does not exist.");
        }

        [TestMethod]
        public void GetSingleParsingResultAsCSVByFileName()
        {
            string fileName = "D:\\foo3.csv";
            FileInfo myFile = new FileInfo(fileName);
            string createdByUserName = "Bryan";
            string identifier = "NY01075759_lg.csv";

            if (myFile.Exists)
                myFile.Delete();

            var client = CreateWebClient();
            string url = string.Format("{0}/parsingresultscsv?identifier={1}&createdByUserName={2}", ApiUrlRoot, identifier, createdByUserName);

            client.DownloadFile(url, fileName); //method = "GET";

            myFile = new FileInfo(fileName); // Refresh the object to get a current "Exists" value.
            Assert.IsTrue(myFile.Exists, "CSV output file does not exist.");
        }

        private ParsingResult GetParsingResultMockObject(int objectIndex)
        {
            ParsingResult retVal = null;

            switch (objectIndex)
            {
                case 1:
                    retVal = new ParsingResult()
                    {
                        aocrverbatimInstitution = "my aocrverbatimInstitution",
                        aocrverbatimScientificName = "my aocrverbatimScientificName",
                        dwccatalogNumber = "my dwccatalogNumber",
                        dwccountry = "my dwccountry",
                        dwccounty = "my dwccounty",
                        dwcdatasetName = "my dwcdatasetName",
                        dwcdateIdentified = "my dwcdateIdentified",
                        dwcdecimalLatitude = "my dwcdecimalLatitude",
                        dwcdecimalLongitude = "my dwcdecimalLongitude",
                        dwceventDate = "my dwceventDate",
                        dwcfieldNotes = "my dwcfieldNotes",
                        dwchabitat = "my dwchabitat",
                        dwcidentifiedBy = "my dwcidentifiedBy",
                        dwcmunicipality = "my dwcmunicipality",
                        dwcrecordedBy = "my dwcrecordedBy",
                        dwcrecordNumber = "my dwcrecordNumber",
                        dwcscientificName = "my dwcscientificName",
                        dwcsex = "my dwcsex",
                        dwcstateProvince = "my dwcstateProvince",
                        dwcsubstrate = "my dwcsubstrate",
                        dwcverbatimCoordinates = "my dwcverbatimCoordinates",
                        dwcverbatimElevation = "my dwcverbatimElevation",
                        dwcverbatimEventDate = "my dwcverbatimEventDate",
                        dwcverbatimLatitude = "my dwcverbatimLatitude",
                        dwcverbatimLocality = "my dwcverbatimLocality",
                        dwcverbatimLongitude = "my dwcverbatimLongitude",
                        fileNameIdentifier = "my fileNameIdentifier",
                        //ipAddress = "123.345.678",
                        createdByUserName = "foo",
                        //createdUTCDateTime = DateTime.UtcNow,
                        sourceUUID = Guid.NewGuid() //.ToString()
                    };

                    break;
                case 2:
                    retVal = new ParsingResult()
                    {
                        aocrverbatimInstitution = "New York Botanical Garden",
                        aocrverbatimScientificName = "",
                        dwccatalogNumber = "1075759",
                        dwccountry = "U.S.A.",
                        dwccounty = "Rockland",
                        dwcdatasetName = "Lichens of New York State",
                        dwcdateIdentified = "",
                        dwcdecimalLatitude = "",
                        dwcdecimalLongitude = "",
                        dwceventDate = "4/19/1998",
                        dwcfieldNotes = "",
                        dwchabitat = "mixed hardwood-hemlock forest with granitic erratics.",
                        dwcidentifiedBy = "Bryan's Record",
                        dwcmunicipality = "",
                        dwcrecordedBy = "Richard C. Harris",
                        dwcrecordNumber = "42164",
                        dwcscientificName = "Polycoccum minutulum Kocourkova & F. Berger",
                        dwcsex = "",
                        dwcstateProvince = "NEW YORK",
                        dwcsubstrate = "on Trapelia placodioides Coppins & P. James",
                        dwcverbatimCoordinates = "41°11'N, 74°08'W",
                        dwcverbatimElevation = "ca. 240 m",
                        dwcverbatimEventDate = "4/19/1998",
                        dwcverbatimLatitude = "41°11'N",
                        dwcverbatimLocality = "Harriman State Park, along Woodtown Road West near dam at S end of Lake Sebago along Seven Lakes Drive",
                        dwcverbatimLongitude = "74°08'W",
                        fileNameIdentifier = "NY01075759_lg.csv",
                        //ipAddress = "123.345.678",
                        createdByUserName = "Bryan",
                        //createdUTCDateTime = DateTime.UtcNow,
                        //sourceUUID = Guid.NewGuid() //.ToString()
                    };

                    break;
                default:
                    break;
            }

            return retVal;
        }

    }
}
