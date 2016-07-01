using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataAccess
{
    public interface ICaseIdDataService: IDisposable 
    {
        string GetCaseId();
    }
}
