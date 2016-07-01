using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPS_TestBatch_Manager.Models
{
    public class EqTestCase
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Id { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string EqCourierPrefix { get; set; }

        public string Description { get; set; }

        public EqSimulatedInput EQListSimulationInput { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
