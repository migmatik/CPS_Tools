using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataProvider
{
    public interface IEqTestCaseDataProvider
    {
        EqTestCase GetEqTestCaseById(int id);

        void SaveEqTestCase(EqTestCase eqTestCase);

        void DeleteEqTestCase(int id);
    }
}
