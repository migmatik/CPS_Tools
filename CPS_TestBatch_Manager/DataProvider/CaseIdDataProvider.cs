using CPS_TestBatch_Manager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataProvider
{
    public class CaseIdDataProvider : ICaseIdDataProvider
    {
        private Func<string, ICaseIdDataService> _dataServiceCreator;

        public CaseIdDataProvider(Func<string, ICaseIdDataService> dataServiceCreator)
        {
            _dataServiceCreator = dataServiceCreator;
        }

        public string GetAvailableCaseId(string questionnaireId)
        {
            var filename = GetFilename(questionnaireId);

            using (var service = _dataServiceCreator(filename))
            {
                return service.GetCaseId();
            }
        }

        private string GetFilename(string questionnaireId)
        {
            //TODO: get rid of magic strings, use a config file or something....
            return questionnaireId.Contains("N1")
                ? @"\\F7CPA-SVC01\EqResponses\Miguel_Tests\Build_E2\CaseIdForSimulation\CAT\Mig_N1_CaseId.txt"
                : @"\\F7CPA-SVC01\EqResponses\Miguel_Tests\Build_E2\CaseIdForSimulation\CAT\Mig_2A_CaseId.txt";
        }
    }
}
