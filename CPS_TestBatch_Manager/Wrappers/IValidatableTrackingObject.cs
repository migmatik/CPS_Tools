using System.ComponentModel;

namespace CPS_TestBatch_Manager.Wrappers
{
    public interface IValidatableTrackingObject : IRevertibleChangeTracking, INotifyPropertyChanged
    {
        bool IsValid { get; }
    }
}
