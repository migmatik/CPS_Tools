using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataAccess
{
    public interface ITestCaseDataService: IDisposable 
    {
        EqTestCase GetTestCaseById(int id);

        void SaveTestCase(EqTestCase testCase);

        void DeleteTestCase(int id);

        IEnumerable<EqTestCase> GetAllTestCases();
    }
}
