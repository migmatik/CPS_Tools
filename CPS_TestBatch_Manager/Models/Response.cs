using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Models
{
    public class Response
    {
        [XmlAttribute()]
        public string CaseId { get; set; }

        [XmlAttribute()]
        public string ResponseId { get; set; }

        [XmlAttribute()]
        public string ResponseFile { get; set; }
        
        public override string ToString()
        {
            return ResponseFile;
        }
    }
}
