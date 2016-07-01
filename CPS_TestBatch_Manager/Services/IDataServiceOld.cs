using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Services
{
    public interface IDataServiceOld: IDisposable
    {
        TestBatch GetTestBatchById(int testBatchId);

        void SaveTestBatch(TestBatch testBatch, string outputDir, string suffix);

        void SaveTestBatchSuite(TestBatchSuite testBatchSuite);

        void DeleteTestBatch(int testBatchId);

        IEnumerable<LookuptItem> GetAllTestBatches();
    }
}
