using CPS_TestBatch_Manager.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CPS_TestBatch_Manager.Models
{
    public class EqInitInputFileCreator
    {
        //TODO: get this the proper way, no magic strings :) ...
        //private static readonly XDocument TemplateFile = XDocument.Load(@"C:\Users\Miguel\Documents\Visual Studio 2013\Projects\eq_simInputFileSamples\eq_simInputFileSamples\SIM_20166715592842_HEADER.xml");
        private static readonly XDocument TemplateFile = XDocument.Load(@"\\fld6filer\2016-PostCollect\SIMULATED_FILERS\CATWZ-DAT01\EQ\EQ_NEW_TASK\Copy\Miguel\EQINITSimulationTemplate.xml");

        //private static readonly string CAT_EQ_NEW_TASK_DIR = @"\\fld6filer\2016-PostCollect\SIMULATED_FILERS\CATWZ-DAT01\EQ\EQ_NEW_TASK\";
        //private static readonly string CAT_EQ_NEW_TASK_DIR = @"G:\temp\CPS_TestBatch_Manager_Outputs\";

        public static void Create(EqTestCaseWrapper testBatch, string eqInitInputFolder, string outputDir, string suffix)
        {
            var newEqInitInputFile = new XDocument(TemplateFile);
            UpdateKeyValue(newEqInitInputFile, "BATCH_ID", string.Concat(testBatch.EqCourierPrefix, suffix));
            //UpdateKeyValue(newEqInitInputFile, "MaxItemAlreadyUsed", testBatch.InputFiles.Count.ToString());
            UpdateKeyValue(newEqInitInputFile, "MaxItemAlreadyUsed", testBatch.EQListSimulationInput.Responses.Count.ToString());

            //TODO: need to add the proper path depending on environment...
            string filename = Path.Combine(outputDir, string.Concat(testBatch.EqCourierPrefix, testBatch.Name, "_", suffix, ".xml"));
            UpdateKeyValue(newEqInitInputFile, "EQSimulationInputFile", filename);
            newEqInitInputFile.Save(Path.Combine(eqInitInputFolder, string.Concat(testBatch.EqCourierPrefix, suffix, ".xml")));
            newEqInitInputFile.Save(Path.Combine(outputDir, string.Concat(testBatch.EqCourierPrefix, suffix, ".xml")));
        }

        private static void UpdateKeyValue(XDocument newEqInitInputFile, string keyName, string keyValue)
        {
            var nameSpace = newEqInitInputFile.Root.GetDefaultNamespace();

            newEqInitInputFile.Descendants(nameSpace + "Key").Where(e => e.Value == keyName)
                .Select(x => x.Parent.Element(nameSpace + "Value")).First().SetValue(keyValue);
        }

        public static string GenerateBatchSuffix()
        {
            var now = DateTime.Now;
            return string.Format("{0}{1:00}{2:00}{3:00}{4:00}{5:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        }


    }
}
