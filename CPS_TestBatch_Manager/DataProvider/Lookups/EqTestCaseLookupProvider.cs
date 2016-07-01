using CPS_TestBatch_Manager.DataAccess;
using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPS_TestBatch_Manager.DataProvider.Lookups
{
    public class EqTestCaseLookupProvider: ILookupProvider<EqTestCase>
    {       
        //private readonly Func<string, IDataService<EqTestSuite>> _dataServiceCreator;
        //private readonly Func<string, ITestCaseDataService> _dataServiceCreator;
        private readonly Func<string, ITestCaseDataService> _dataServiceCreator;

        public EqTestSuite _testCaseSuite;
        private string _filename;

        //public EqTestCaseLookupProvider(string filename, Func<string, ITestCaseDataService> dataServiceCreator)
        //{
        //    if (dataServiceCreator == null) { throw new ArgumentNullException("dataServiceCreator"); }

        //    _dataServiceCreator = dataServiceCreator;
        //    _filename = filename;
        //}

        public EqTestCaseLookupProvider(string filename, Func<string, ITestCaseDataService> dataServiceCreator)
        {
            if (dataServiceCreator == null) { throw new ArgumentNullException("dataServiceCreator"); }

            _dataServiceCreator = dataServiceCreator;
            _filename = filename;
        } 

        public IEnumerable<LookupItem> GetLookup()
        {
            using (var service = _dataServiceCreator(_filename))
            {
                return service.GetAllTestCases()
                    .Select(x => new LookupItem { Id = x.Id, DisplayValue = x.Name })
                    .OrderBy(l => l.DisplayValue)
                    .ToList();
            }

            //return _testCaseSuite.TestCases
            //    .Select(x => new LookupItem { Id = x.Id, DisplayValue = x.Name })
            //    .OrderBy(l => l.DisplayValue)
            //    .ToList();
        }

        //public EqTestSuite LoadFile(string filename)
        //{                       
        //    using (var service = _dataServiceCreator(filename))
        //    {
        //        _testCaseSuite = service.Get();
        //    }

        //    return _testCaseSuite;
        //}

        //public IEnumerable<EqTestCase> GetAllTestCases()
        //{
        //    return _testCaseSuite.TestCases;
        //}
    }
}
