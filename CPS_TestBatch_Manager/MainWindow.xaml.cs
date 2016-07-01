using CPS_TestBatch_Manager.ViewModels;
using System;
using System.Windows;

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

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //_viewModel.Load();
        }              
    }
}
