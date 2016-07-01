using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Models
{
    public class ResponseChannel
    {
        [XmlText()]
        public string Value { get; set; }

        [XmlAttribute("id")]
        public int Id { get; set; }

        public override string ToString()
        {
            return Value;
        }

    }
}
