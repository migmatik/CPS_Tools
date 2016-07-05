using CPS_TestBatch_Manager.Models;

namespace CPS_TestBatch_Manager.Wrappers
{
    public class EnvironmentWrapper : ModelWrapper<Environment>
    {
        public EnvironmentWrapper(Environment model): base(model)
        {
            
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string EQInitInputFolder
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string OutputFolder
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
