using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Models
{
    public class Environment
    {
        [XmlAttributeAttribute()]
        public string Name { get; set; }

        [XmlAttributeAttribute()]
        public string EQInitInputFolder { get; set; }

        [XmlAttributeAttribute()]
        public string OutputFolder { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
