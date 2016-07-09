using CPS_TestBatch_Manager.ViewModels;
using System;
using System.Windows;
using System.Configuration;
using System.Collections.Specialized;
using CPS_TestBatch_Manager.Configuration;



namespace CPS_TestBatch_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        //public MainWindow()
        //{ }

        public MainWindow(MainWindowViewModel viewModel)
        {
            if (viewModel == null) { throw new ArgumentNullException("viewmodel was not instatiated"); }

            InitializeComponent();

            // TODO: probably not necessary since file will be open from Main Menu, unless we want the app to load the last loaded file on startup
            //this.Loaded += MainWindow_Loaded;           
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var result = _viewModel.ConfirmClosingUnsavedTestCases();
            e.Cancel = result == Views.Dialogs.MessageDialogResult.No;
            base.OnClosing(e);
            Properties.Settings.Default.Save();
        }

        //void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //_viewModel.Load();
        //}              
    }
}
