using CPS_TestBatch_Manager.Models;
using Prism.Events;

namespace CPS_TestBatch_Manager.Events
{
    class TestCaseSavedEvent : PubSubEvent<EqTestCase>
    {
    }
}
