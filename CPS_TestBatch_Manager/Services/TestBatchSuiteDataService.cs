using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Services
{
    public class TestBatchSuiteDataService : ITestBatchSuiteDataService
    {
        private XDocument _eqResponseParameters;

        public TestBatchSuite CreateTestBatchSuite(string fileName)
        {
            
            var ser = new XmlSerializer(typeof(TestBatchSuite));
            TestBatchSuite testBatchSuite; 
           
            using (XmlReader reader = XmlReader.Create(fileName))
            {
                testBatchSuite = (TestBatchSuite)ser.Deserialize(reader); 
            }

            _eqResponseParameters = GetParameterOptions(testBatchSuite.EqResponseParameterOptionsFile);
            testBatchSuite.ResponseChannelList = GetParameterOptionObjList("ResponseChannel");
            testBatchSuite.ResponseStatusList = GetParameterOptionList("ResponseStatus");
            testBatchSuite.QuestionnaireIdList = GetParameterOptionList("QuestionnaireId");

            testBatchSuite.FileName = fileName;
            return testBatchSuite;

        }

        private XDocument GetParameterOptions(string eqResponseParamOptionFile)
        {
            return XDocument.Load(eqResponseParamOptionFile);
        }

        private List<ResponseChannel> GetParameterOptionObjList(string param)
        {
            return _eqResponseParameters.Root.Descendants("option")
               .Where(p => p.Parent.Attribute("Name").Value == param)
               .Select(o => new ResponseChannel() { Value = o.Attribute("value").Value, Id = int.Parse(o.Attribute("id").Value) }).ToList();
        }        

        private List<string> GetParameterOptionList(string param)
        {
            return _eqResponseParameters.Root.Descendants("option")
                .Where(p => p.Parent.Attribute("Name").Value == param)
                .Select(o => o.Attribute("value").Value).ToList();
        }

        public void Save(TestBatchSuite testBatchSuite)
        {
            var ser = new XmlSerializer(typeof(TestBatchSuite));

            using(Stream fs = new FileStream(testBatchSuite.FileName, FileMode.Create))
            {
                XmlWriter writer = new XmlTextWriter(fs, Encoding.Unicode);                
                ser.Serialize(writer, testBatchSuite);
                writer.Close();
            }            
        }        
    }
}
