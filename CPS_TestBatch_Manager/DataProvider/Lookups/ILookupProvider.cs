using CPS_TestBatch_Manager.Models;
using System.Collections.Generic;

namespace CPS_TestBatch_Manager.DataProvider.Lookups
{
    public interface ILookupProvider<T>
    {
        //EqTestSuite LoadFile(string filename);
        IEnumerable<LookupItem> GetLookup();
        //IEnumerable<T> GetAllTestCases();
    }
}
