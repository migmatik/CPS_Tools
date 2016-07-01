using CPS_TestBatch_Manager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataProvider
{
    public class CaseIdDataProvider: ICaseIdDataProvider
    {
        private Func<string, ICaseIdDataService> _dataServiceCreator;

        public CaseIdDataProvider(Func<string, ICaseIdDataService> dataServiceCreator)
        {
            _dataServiceCreator = dataServiceCreator;
        }

        public string GetAvailableCaseId(string questionnaireId)
        {
            var filename = GetFilename(questionnaireId);

            using(var service = _dataServiceCreator(filename))
            {
                return service.GetCaseId();
            }
        }

        private string GetFilename(string questionnaireId)
        {
            return questionnaireId.Contains("N1") 
                ? @"G:\temp\CPS_TestBatch_Manager_Outputs\CaseIdFiles\Mig_N1_CaseId.txt"
                : @"G:\temp\CPS_TestBatch_Manager_Outputs\CaseIdFiles\Mig_2A_CaseId.txt";
        }
    }
}
