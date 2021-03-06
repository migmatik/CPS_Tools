﻿using System.Xml.Serialization;

namespace CPS_TestBatch_Manager.Models
{
    public class EqTestCase
    {
        [XmlAttributeAttribute()]
        public int Id { get; set; }

        [XmlAttributeAttribute()]
        public string Name { get; set; }

        [XmlAttributeAttribute()]
        public string EqCourierPrefix { get; set; }

        public string Description { get; set; }

        public EqSimulatedInput EQListSimulationInput { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
