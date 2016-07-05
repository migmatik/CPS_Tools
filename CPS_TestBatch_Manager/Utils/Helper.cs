using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS_TestBatch_Manager.Utils
{
    public class Helper
    {
        public static int GetMAD97(string inputString)
        {
            int remainder = 0;

            if (!string.IsNullOrEmpty(inputString))
            {
                remainder = (int)inputString[0];

                for (int charIdx = 1; charIdx < inputString.Length; charIdx++)
                {
                    int temp = (int)inputString[charIdx];
                    remainder = (remainder * 1000 + temp) % 97;
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Input string [" + inputString + "] is not valid", inputString));
            }

            return remainder % 97;
        }
    }
}
