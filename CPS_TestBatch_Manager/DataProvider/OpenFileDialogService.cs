using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataProvider
{
    public class OpenFileDialogService: IIOService
    {
        public string OpenFileDialog()
        {
            var openFileDlg = new OpenFileDialog();

            openFileDlg.Filter = "Test Batch Files (.xml)|*.xml";
            openFileDlg.Multiselect = false;

            string selected_file = null;

            if (openFileDlg.ShowDialog() == true)
            {
                selected_file = openFileDlg.FileName;
            }

            return selected_file;
        }
    }
}
