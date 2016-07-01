using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Services
{
    public class FileDataService: IDataServiceOld
    {
        public TestBatch GetTestBatchById(int testBatchId)
        {
            throw new NotImplementedException();
        }

        public void SaveTestBatchSuite(TestBatchSuite testBatchSuite)
        {
            var ser = new XmlSerializer(typeof(TestBatchSuite), "");
            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); 
           
            using (StreamWriter writer = new StreamWriter(testBatchSuite.FileName))
            {                
                ser.Serialize(writer, testBatchSuite, ns);                
            }    
        }

        public void DeleteTestBatch(int testBatchId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LookuptItem> GetAllTestBatches()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        public void SaveTestBatch(TestBatch testBatch, string outputDir, string suffix)
        {
            var ser = new XmlSerializer(testBatch.EQListSimulationInput.GetType(), "");
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            
            string filename = Path.Combine(outputDir, string.Concat(testBatch.BatchPrefix, testBatch.Name, "_", suffix, ".xml"));

            using (StreamWriter writer = new StreamWriter(filename))
            {
                ser.Serialize(writer, testBatch.EQListSimulationInput, ns);
            }   
        }
    }
}
