using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_TestBatch_Manager.Models
{
    public class ResponseSettings
    {
        public ResponseChannel ResponseChannel { get; set; }

        public string QuestionnaireId { get; set; }

        public string ResponseStatus { get; set; }
    }
}
