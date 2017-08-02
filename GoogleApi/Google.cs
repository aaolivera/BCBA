using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;


namespace GoogleApi
{
    public static class Google
    {
        public static string GetData(string stock,string exchange,Interval interval, PeriodType periodType, int period,DateTime? startTime = null){
            if (startTime == null)
            {
                startTime = DateTime.Today;
            }
            var client = new WebClient();
            return client.DownloadString(String.Format("http://www.google.com/finance/getprices?q={0}&x={1}&i={2}&p={3}{4}&f=d,c,v,k,o,h,l&df=cpct&auto=0&ts={5}&ei=Ef6XUYDfCqSTiAKEMg", stock, exchange, (int)interval, period, periodType.ToString(), startTime.Value.ToUnix()));

        }

        public static string GetInfo(string stock, string exchange)
        {
            var client = new WebClient();
            return client.DownloadString(String.Format("http://finance.google.com/finance/info?client=ig&q={1}:{0}", stock, exchange)).Replace("/", "").Replace("[", "").Replace("]", "").Replace(@"\n", "");

        }

        public static string GetLastValue(string stock)
        {
            try
            {
                if (string.IsNullOrEmpty(stock)) return "-";
                var datos = Google.GetInfo(stock, "BCBA");
                var obj = (Dictionary<string, object>) new JavaScriptSerializer().Deserialize<object>(datos);

                return obj.ContainsKey("l") ? (string) obj["l"] : "-";
            }
            catch
            {
                return "-";
            }
        }
        
    }
}