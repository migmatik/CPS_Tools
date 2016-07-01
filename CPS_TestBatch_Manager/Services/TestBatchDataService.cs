using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Services
{
    public class TestBatchDataService: ITestBatchDataService
    {
        private readonly Func<IDataServiceOld> _dataServiceCreator;        
       
        public TestBatchDataService(Func<IDataServiceOld> dataServiceCreator)
        {
            if (dataServiceCreator == null) { throw new ArgumentNullException("dataServiceCreator not instanciated"); }

            _dataServiceCreator = dataServiceCreator;
        }        

        public TestBatch GetTestBatchById(int id)
        {
            using(var dataService = _dataServiceCreator())
            {
                return dataService.GetTestBatchById(id);
            }
        }

        public void SaveTestBatchSuite(TestBatchSuite file)
        {
            using (var dataService = _dataServiceCreator())
            {
                dataService.SaveTestBatchSuite(file);
            }
        }

        public void DeleteTestBatch(int id)
        {
            using (var dataService = _dataServiceCreator())
            {
                dataService.DeleteTestBatch(id);
            }
        }

        public void SaveTestBatch(TestBatch testBatch, string outputDir, string suffix)
        {
            using (var dataService = _dataServiceCreator())
            {
                dataService.SaveTestBatch(testBatch, outputDir, suffix);
            }
        }
    }
}
