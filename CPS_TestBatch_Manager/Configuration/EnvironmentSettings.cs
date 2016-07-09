using CPS_TestBatch_Manager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Configuration
{
    public class EnvironmentConfig: ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public EnvironmentCollection Environments
        {
            get { return (EnvironmentCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class EnvironmentCollection: ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EnvironmentElement)element).Name;
        }
    }

    public class EnvironmentElement: ConfigurationElement, IEnvironment
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name 
        {
            get { return (string)base["Name"]; }
            set { base["Name"] = value; } 
        }

        [ConfigurationProperty("EQInitInputFolder", IsRequired = true)]
        public string EQInitInputFolder
        {
            get { return (string)base["EQInitInputFolder"]; }
            set { base["EQInitInputFolder"] = value; }
        }

        [ConfigurationProperty("OutputFolder", IsRequired = true)]
        public string OutputFolder
        {
            get { return (string)base["OutputFolder"]; }
            set { base["OutputFolder"] = value; }
        }        
    }

}
