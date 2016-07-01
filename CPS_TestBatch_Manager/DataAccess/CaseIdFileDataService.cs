using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.DataAccess
{
    public class CaseIdFileDataService: ICaseIdDataService
    {
        private string _filename;

        public CaseIdFileDataService(string filename)
        {
            _filename = filename;
        }

        public string GetCaseId()
        {
            var caseIdList = File.ReadLines(_filename).ToList();
            var caseId = caseIdList.First();
            UpdateAvailableAndUsedCaseIdFiles(caseIdList, caseId);

            return caseId;            
        }

        private void UpdateAvailableAndUsedCaseIdFiles(List<string> caseIdList, string caseId)
        {
            caseIdList.Remove(caseId);

            using (TextWriter tw = new StreamWriter(_filename))
            {
                foreach (var item in caseIdList)
                {
                    tw.WriteLine(item);
                }
            }

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_filename);
            var fileDir = Path.GetDirectoryName(_filename);
            var usedCaseIdFileName = Path.Combine(fileDir, string.Concat(fileNameWithoutExtension, "_Used.txt"));

            using (StreamWriter sw = File.AppendText(usedCaseIdFileName))
            {
                sw.WriteLine(caseId);               
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
