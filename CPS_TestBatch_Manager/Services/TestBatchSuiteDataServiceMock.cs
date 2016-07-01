using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CPS_TestBatch_Manager.Services
{
    public class TestBatchSuiteDataServiceMock : ITestBatchSuiteDataService
    {
        public TestBatchSuite CreateTestBatchSuite(string fileName)
        {
            return Helper.NewTestBatchSuite();            
        }
    }

    public static class Helper
    {
        public static TestBatchSuite NewTestBatchSuite()
        {
            var testBatchSuite = new TestBatchSuite();
            testBatchSuite.Name = "Test Batch Suite Name";
            testBatchSuite.TestBatches = new List<TestBatch>();
            testBatchSuite.TestBatches.Add(NewTestBatch());

            return testBatchSuite;
        }

        public static TestBatch NewTestBatch()
        {
            return new TestBatch() { Name = "Test Batch", BatchPrefix = "TEST_", CreationDate = DateTime.Now, Description = "This is a test batch", Id = 1, EQListSimulationInput = NewEqSimulatedInput() };
        }

        public static EqSimulatedInput NewEqSimulatedInput()
        {
            return new EqSimulatedInput() { ResponseSettings = NewResponseSettings(), Responses = NewResponses() };
        }

        private static ResponseSettings NewResponseSettings()
        {
            return new ResponseSettings() { QuestionnaireId = "QuestID", ResponseStatus = "RespStatus", ResponseChannel = NewResponseChannel() };
        }

        private static ResponseChannel NewResponseChannel()
        {
            return new ResponseChannel() { Id = 1, Value = "SomeResponseChannel" };
        }

        private static List<Response> NewResponses()
        {
            return new List<Response>() { NewResponse("response 1"), NewResponse("response 2") };
        }

        private static Response NewResponse(string responseFile)
        {
            return new Response() { CaseId = "12345", ResponseId = "1112345", ResponseFile = responseFile };
        }
    }
}
