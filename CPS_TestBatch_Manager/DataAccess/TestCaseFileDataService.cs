using CPS_TestBatch_Manager.DataAccess;
using CPS_TestBatch_Manager.DataProvider;
using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataAccess
{
    public class TestCaseFileDataService: ITestCaseDataService
    {        
        private Func<string, IXmlSerializerService<EqTestSuite>> _testSuiteSerializerCreator;
        private string _filename;

        public TestCaseFileDataService(string filename, Func<string, IXmlSerializerService<EqTestSuite>> testSuiteSerializerCreator)
        {
            _testSuiteSerializerCreator = testSuiteSerializerCreator;
            _filename = filename;
        }
        
        public EqTestCase GetTestCaseById(int id)
        {
            var testCases = ReadFromFile();
            return testCases.Single(tc => tc.Id == id);
        }

        public void SaveTestCase(EqTestCase testCase)
        {
            if(testCase.Id <= 0)
            {
                InsertTestCase(testCase);
            }
            else
            {
                UpdateTestCase(testCase);
            }
        }

        private void UpdateTestCase(EqTestCase testCase)
        {
            var testCases = ReadFromFile().ToList();
            var testCaseToReplace = testCases.Single(tc => tc.Id == testCase.Id);
            var indexOfTestCaseToReplace = testCases.IndexOf(testCaseToReplace);
            testCases.Insert(indexOfTestCaseToReplace, testCase);
            testCases.Remove(testCaseToReplace);
            SaveToFile(testCases);
        }

        private void SaveToFile(List<EqTestCase> testCases)
        {
            var testSuite = new EqTestSuite { TestCases = testCases, FileName = _filename };

            using (var service = _testSuiteSerializerCreator(_filename))
            {
                service.ObjectToXmlFile(testSuite);
            }
        }

        private void InsertTestCase(EqTestCase testCase)
        {
            throw new NotImplementedException();
        }

        public void DeleteTestCase(int id)
        {
            var testCases = ReadFromFile().ToList();
            var testCaseToDelete = testCases.Single(tc => tc.Id == id);
            testCases.Remove(testCaseToDelete);
            SaveToFile(testCases);
        }

        public IEnumerable<EqTestCase> GetAllTestCases()
        {
            return ReadFromFile();
        }

        private IEnumerable<EqTestCase> ReadFromFile()
        {
            EqTestSuite testSuite;

            using (var service = _testSuiteSerializerCreator(_filename))
            {             
                testSuite = service.XmlFileToObject();
            }

            return testSuite.TestCases;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
