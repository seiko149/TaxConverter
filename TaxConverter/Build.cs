using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace TaxConverter
{
    //text git

    class Build
    {
        String directory;
        String cbrDirectory;
        String archiveDirectory;
        String timeStamp;
        Log log;

        Boolean french;

        //load the direcotries to be used and the timestamp
        public Build(String directory, String cbrDirectory, String archiveDirectory, String timeStamp, Log log)
        {
            this.directory = directory;
            this.cbrDirectory = cbrDirectory;
            this.archiveDirectory = archiveDirectory;
            this.timeStamp = timeStamp;
            this.log = log;

            //check whether we are using a "." or "," for decimal seperator
            string uiSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (uiSep == ".")
            {
                french = false;
            }
            else
            {
                french = true;
            }
        }

        //Read the JSON file and save to String
        public String loadFile(String file)
        {
            log.append("Loading file: " + Path.GetFileName(file));
            StreamReader streamReader = new StreamReader(file);
            String JSONFile = streamReader.ReadToEnd();
            streamReader.Close();
            log.append("Returning " + Path.GetFileName(file) + "as String");
            return JSONFile;
        }

        //parse the JSON String file and return an array of JSON lines
        public List<String> loadJson(String JSONFile)
        {
            int BracketCount = 0;
            StringBuilder stringBuilder = new StringBuilder();
            List<String> JsonItems = new List<String>();
            List<string> JsonItems2 = new List<String>();
            // List<String> finishedArray = new List<String>();

            foreach (char c in JSONFile)
            {
                if (c == '{')
                    ++BracketCount;
                else if (c == '}')
                    --BracketCount;
                stringBuilder.Append(c);

                if (BracketCount == 0 && c != ' ')
                {
                    JsonItems.Add(stringBuilder.ToString());
                    stringBuilder = new StringBuilder();
                }
            }

            //add only lines that start with {
            log.append("Cleaning file");
            for (int i = 0; i < JsonItems.Count; i++)
            {
                if (JsonItems[i].StartsWith("{"))
                {
                    JsonItems2.Add(JsonItems[i]);
                }
            }

            return JsonItems2;
        }

        //Create an array with the data needed from the JSON strings
        public List<List<String>> buildCBR(List<String> JsonItems)
        {
            List<List<String>> finishedArray = new List<List<String>>();

            finishedArray.Add(new List<String> {"scenario", "postal", "code", "date", "rate"});

            log.append("Deserializing .json file");

            for (int i = 0; i < JsonItems.Count; i++)
            {
                try
                {
                    Tax tax = JsonConvert.DeserializeObject<Tax>(JsonItems[i]);

                    if (!french)
                    {
                        finishedArray.Add(new List<String> { tax.ScenarioId, tax.ShipToPostalCode.Substring(0, 5), tax.TaxCode, DateTime.Parse(tax.EffDate).ToShortDateString(), tax.Tax_Rate });
                    }
                    else
                    {
                        finishedArray.Add(new List<String> { tax.ScenarioId, tax.ShipToPostalCode.Substring(0, 5), tax.TaxCode, DateTime.Parse(tax.EffDate).ToShortDateString(), tax.Tax_Rate.Replace(".", ",") });
                    }
                                       
                    //finishedArray.Add(tax.ScenarioId + "|" + "TAXC1_|" + tax.Tax_Rate + "|" + tax.ShipToPostalCode.Substring(0, 5) + "|" + tax.TaxCode + "|" + DateTime.Parse(tax.EffDate).ToShortDateString());//Console.WriteLine(tax.ShipToPostalCode);  \\ + zeroValue +
                }
                catch (Exception e)
                {
                    log.append("Error deserializing .json file");
                    Console.WriteLine(e);
                }
            }
            return finishedArray;
        }
        //Write the CBR Data
        public void writeData(List<List<String>> finishedArray, String file)
        {
            var headers = finishedArray.First();
            var result = finishedArray.Skip(1).GroupBy(s => new {scenario = s[headers.IndexOf("scenario")], postal = s[headers.IndexOf("postal")], code = s[headers.IndexOf("code")], date = s[headers.IndexOf("date")]})
                .Select(g => new
                {
                scenario = g.Key.scenario,
                postal = g.Key.postal,
                code = g.Key.code,
                date = g.Key.date,
                Total = g.Select(s => double.Parse(s[headers.IndexOf("rate")])).Sum()
                });

            List<String> test = new List<String>();

            foreach (var item in result)
            {

                //update 05/12/17 replace . with ,
                test.Add("TAXC1_|" + (item.Total * 100).ToString().Replace(".", ",") + "|" + item.postal + "|" + item.code + "|" + item.date);
               // test.Add("TAXC1_|" + item.Total * 100 + "|" + item.postal + "|" + item.code + "|"  + item.date);
            }

            //log.append("Writing converted .json file to " + Path.GetFileNameWithoutExtension(file) + ".csv");
            System.IO.File.WriteAllLines(cbrDirectory + timeStamp + Path.GetFileNameWithoutExtension(file) + ".csv", test);
        }
        //move files to archive
        public void moveFiles(String file)
        {
            if (File.Exists(archiveDirectory + timeStamp + Path.GetFileName(file)))
            {
                log.append("File already exists: " + archiveDirectory + timeStamp + Path.GetFileName(file));
                File.Delete(archiveDirectory + timeStamp + Path.GetFileName(file));
            }
            log.append("Moving file: " + Path.GetFileName(file) + " to " + archiveDirectory + timeStamp + Path.GetFileName(file));
            File.Move(file, archiveDirectory + timeStamp + Path.GetFileName(file));
        }
    }
}
