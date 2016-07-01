using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataProvider
{
    public interface ICaseIdDataProvider
    {
        string GetAvailableCaseId(string questionnaireId);
    }
}
