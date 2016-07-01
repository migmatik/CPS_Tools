using System;

namespace CPS_TestBatch_Manager.DataAccess
{
    public interface ICaseIdDataService: IDisposable 
    {
        string GetCaseId();
    }
}
