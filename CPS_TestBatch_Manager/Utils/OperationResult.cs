using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Utils
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public List<string> Messages { get; private set; }

        public OperationResult()
        {
            Success = true;
            Messages = new List<string>();
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }
    }
}
