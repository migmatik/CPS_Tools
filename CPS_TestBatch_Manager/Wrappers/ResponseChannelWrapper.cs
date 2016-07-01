using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_TestBatch_Manager.Wrappers
{
    public class ResponseChannelWrapper: ModelWrapper<ResponseChannel>
    {        
        public ResponseChannelWrapper(ResponseChannel model): base(model)
        {            
        }

        public string Value
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }
    }
}
