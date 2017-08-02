using GoogleApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCBA
{
    class Program
    {
        static void Main(string[] args)
        {

            var datos = Google.GetData("PSUR", "BCBA", Interval.Day, PeriodType.d, 1,new DateTime(2015,11,01));
            var valor = Google.GetLastValue("PSUR");
        }


        
    }
}
