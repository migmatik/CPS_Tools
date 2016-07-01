using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Models
{
    public class EqTestSuite
    {
        [XmlIgnore()]
        public string FileName { get; set; }
      
        public List<EqTestCase> TestCases {get; set;}

        internal void UpdateTestCase(EqTestCase testCase)
        {
            var testCaseToUpdate = TestCases.SingleOrDefault(x => x.Id == testCase.Id);

            TestCases.Remove(testCaseToUpdate);
            TestCases.Add(testCase);            
            TestCases.Sort((x, y) => { return x.Id.CompareTo(y.Id); });
        }
    }
}
