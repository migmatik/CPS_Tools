using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Services
{
    public interface ITestBatchDataService
    {
        //IEnumerable<TestBatch> GetTestBatches();
        TestBatch GetTestBatchById(int id);

        void SaveTestBatchSuite(TestBatchSuite testBatchSuite);

        void SaveTestBatch(TestBatch testBatch, string outputDir, string suffix);

        void DeleteTestBatch(int id);

    }
}
