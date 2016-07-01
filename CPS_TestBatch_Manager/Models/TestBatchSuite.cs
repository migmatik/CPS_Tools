using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Models
{
    public class TestBatchSuite
    {
        [System.Xml.Serialization.XmlIgnore]
        public string FileName { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElementAttribute()]
        public string EqResponseParameterOptionsFile { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<ResponseChannel> ResponseChannelList { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<string> ResponseStatusList { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public List<string> QuestionnaireIdList { get; set; }

        public List<TestBatch> TestBatches { get; set;}

        public override string ToString()
        {
            return Name;
        }
    }
}
