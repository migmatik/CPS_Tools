using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CPS_TestBatch_Manager.Services
{
    public interface ITestBatchSuiteDataService
    {
        TestBatchSuite CreateTestBatchSuite(string fileName);        
    }
}
