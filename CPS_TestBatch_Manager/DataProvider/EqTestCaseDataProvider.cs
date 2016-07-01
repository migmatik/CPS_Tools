using CPS_TestBatch_Manager.DataAccess;
using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataProvider
{
    public class EqTestCaseDataProvider: IEqTestCaseDataProvider
    {
        private Func<string, ITestCaseDataService> _dataServiceCreator;        
        //private Func<string, IDataService<EqTestCase>> _dataServiceCreator;
        private string _filename;

        public EqTestCaseDataProvider(string filename, Func<string, ITestCaseDataService> dataServiceCreator)
        {
            if (dataServiceCreator == null) { throw new ArgumentNullException("dataServiceCreator"); }

            _dataServiceCreator = dataServiceCreator;
            _filename = filename;
        }

        public EqTestCaseDataProvider(Func<string, ITestCaseDataService> dataServiceCreator)
        {
            if (dataServiceCreator == null) { throw new ArgumentNullException("dataServiceCreator"); }

            _dataServiceCreator = dataServiceCreator;
            //_filename = filename;
        }
        //public EqTestCaseDataProvider(Func<string, IDataService<EqTestCase>> dataServiceCreator)
        //{
        //    if (dataServiceCreator == null) { throw new ArgumentNullException("dataServiceCreator"); }

        //    _dataServiceCreator = dataServiceCreator;
        //}

        public EqTestCase GetEqTestCaseById(int id)
        {
            using (var service = _dataServiceCreator(_filename))
            {
                //TODO: optimize, currently this will read from the file instead of object... I think!
                //return service.GetById(id).TestCases.SingleOrDefault(x => x.Id == id);
                return service.GetTestCaseById(id);
            }
        }
        //public EqTestCase GetEqTestCaseById(int id)
        //{
        //    using (var service = _dataServiceCreator(""))
        //    {
        //        //TODO: optimize, currently this will read from the file instead of object... I think!
        //        return service.GetById(id);
        //    }
        //}
       
        public void DeleteEqTestCase(int id)
        {
            using (var service = _dataServiceCreator(_filename))
            {                
                service.DeleteTestCase(id);
            }
        }
      
        public void SaveEqTestCase(EqTestCase eqTestCase)
        {
            using (var service = _dataServiceCreator(_filename))
            {                
                //service.Save(outputFile, eqTestCase);
                service.SaveTestCase(eqTestCase);
            }
        }
    }
}
