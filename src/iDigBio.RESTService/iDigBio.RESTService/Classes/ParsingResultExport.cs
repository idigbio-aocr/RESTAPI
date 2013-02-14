using iDigBio.Core;
using iDigBio.RESTService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace iDigBio.RESTService
{
    public class ParsingResultExport
    {
        public static void ExportParsingResult(ParsingResult resultToExport, string outputFilePath)
        {
            CsvExport myExport = new CsvExport();
            AddParsingResultToNewRow(myExport, resultToExport);
            myExport.ExportToFile(outputFilePath);
        }

        public static Stream ExportParsingResult(ParsingResult resultToExport)
        {
            CsvExport myExport = new CsvExport();
            AddParsingResultToNewRow(myExport, resultToExport);
            return myExport.ExportToStream();
        }

        private static void AddParsingResultToNewRow(CsvExport myExport, ParsingResult resultToExport)
        {
            myExport.AddRow();
            myExport["dwc:recordedBy"] = resultToExport.dwcrecordedBy;
            myExport["dwc:recordNumber"] = resultToExport.dwcrecordNumber;
            myExport["dwc:verbatimCoordinates"] = resultToExport.dwcverbatimCoordinates;
            myExport["dwc:verbatimEventDate"] = resultToExport.dwcverbatimEventDate;
            myExport["dwc:eventDate"] = resultToExport.dwcverbatimEventDate;
            myExport["dwc:municipality"] = resultToExport.dwcmunicipality;
            myExport["dwc:county"] = resultToExport.dwccounty;
            myExport["dwc:stateProvince"] = resultToExport.dwcstateProvince;
            myExport["dwc:country"] = resultToExport.dwccountry;
            myExport["aocr:verbatimScientificName"] = resultToExport.aocrverbatimScientificName;
            myExport["dwc:verbatimLocality"] = resultToExport.dwcverbatimLocality;
            myExport["dwc:habitat"] = resultToExport.dwchabitat;
            myExport["dwc:substrate"] = resultToExport.dwcsubstrate;
            myExport["dwc:verbatimElevation"] = resultToExport.dwcverbatimElevation;
            myExport["dwc:identifiedBy"] = resultToExport.dwcidentifiedBy;
            myExport["dwc:dateIdentified"] = resultToExport.dwcdateIdentified;
            myExport["dwc:verbatimLatitude"] = resultToExport.dwcverbatimLatitude;
            myExport["dwc:verbatimLongitude"] = resultToExport.dwcverbatimLongitude;
            myExport["dwc:catalogNumber"] = resultToExport.dwccatalogNumber;
            myExport["aocr:verbatimInstitution"] = resultToExport.aocrverbatimInstitution;
            myExport["dwc:datasetName"] = resultToExport.dwcdatasetName;
            myExport["dwc:scientificName"] = resultToExport.dwcscientificName;
            myExport["dwc:decimalLatitude"] = resultToExport.dwcdecimalLatitude;
            myExport["dwc:decimalLongitude"] = resultToExport.dwcdecimalLongitude;
            myExport["dwc:fieldNotes"] = resultToExport.dwcfieldNotes;
            myExport["dwc:sex"] = resultToExport.dwcsex;
        }

    }
}