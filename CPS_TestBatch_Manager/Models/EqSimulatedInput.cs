using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CPS_TestBatch_Manager.Models
{
    public class EqSimulatedInput
    {
        public ResponseSettings ResponseSettings { get; set; }       

        public List<Response> Responses { get; set; }
    }
}
