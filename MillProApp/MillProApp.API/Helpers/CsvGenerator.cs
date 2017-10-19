using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace MillProApp.API.Helpers
{
    public class CsvGenerator
    {
        public static string ToCsv<T>(string separator, IEnumerable<T> objectlist)
        {
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties();

            string header = String.Join(separator, fields.Select(f => f.Name).ToArray());

            StringBuilder csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectlist)
                csvdata.AppendLine(ToCsvFields(separator, fields, o));

            return csvdata.ToString();
        }

        public static string ToCsvFields(string separator, PropertyInfo[] fields, object o)
        {
            StringBuilder line = new StringBuilder();

            foreach (var f in fields)
            {
                if (line.Length > 0)
                    line.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    line.Append(x.ToString());
            }

            return line.ToString();
        }
    }
}