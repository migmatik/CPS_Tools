using Autofac;
using CPS_TestBatch_Manager.DataAccess;
using CPS_TestBatch_Manager.DataProvider;
using CPS_TestBatch_Manager.DataProvider.Lookups;
using CPS_TestBatch_Manager.Models;
using CPS_TestBatch_Manager.ViewModels;
using CPS_TestBatch_Manager.Views.Dialogs;
using Prism.Events;

namespace CPS_TestBatch_Manager.BootStrap
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
                        
            builder.RegisterType<EqTestCaseLookupProvider>().As<ILookupProvider<EqTestCase>>();
            builder.RegisterType<TestCaseEditViewModel>().As<ITestCaseEditViewModel>();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<EqTestCaseDataProvider>().As<IEqTestCaseDataProvider>();            
            builder.RegisterType<TestCaseFileDataService>().As<ITestCaseDataService>();
            builder.RegisterType<OpenFileDialogService>().As<IIOService>();
            builder.RegisterType<FileToPocoSerializer<EqTestSuite>>().As<IXmlSerializerService<EqTestSuite>>();
            builder.RegisterType<ResponseParameterOptionsService>().As<IXmlSerializerService<EqResponseParameters>>();
            builder.RegisterType<CaseIdDataProvider>().As<ICaseIdDataProvider>();
            builder.RegisterType<CaseIdFileDataService>().As<ICaseIdDataService>();

            return builder.Build();
            //adding a comment
        }
    }
}
