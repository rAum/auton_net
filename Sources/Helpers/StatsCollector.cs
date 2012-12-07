using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Helpers
{
    public class StatsCollector
    {
        private IDictionary<string, List<double>> dict = new Dictionary<string, List<double>>();
        public void PutNewStat(string variable, double value)
        {
            //create vector for key if not exists
            if (!dict.ContainsKey(variable))
            {
                dict[variable] = new List<double>();
            }

            dict[variable].Add(value);
        }

        public void WriteStatsToFile(string fileName)
        {
            if(File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                //write keys
                foreach (string key in dict.Keys)
                {
                    sw.Write("{0}; ", key);
                }
                sw.WriteLine();

                //write values
                bool somethingWasWritten = true;
                int i = 0;
                while (somethingWasWritten)
                {
                    somethingWasWritten = false; 

                    foreach(string key in dict.Keys)
                    {
                        if (dict[key].Count > i)
                        {
                            sw.Write("{0}; ", dict[key][i]);
                            somethingWasWritten = true;
                        }
                        else
                        {
                            sw.Write(";");
                        }
                    }
                    sw.WriteLine();

                    i++;
                }

                sw.Close();
            }
        }

    }
}
