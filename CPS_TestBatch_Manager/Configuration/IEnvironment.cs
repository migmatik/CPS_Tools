using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Configuration
{
    public interface IEnvironment
    {
        string Name { get; set; }
        string EQInitInputFolder { get; set; }
        string OutputFolder { get; set; }
    }
}
