using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TaxConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load properties 
            List<String> listProperties = new List<String>();
            StreamReader streamReaderProperties = null;


            string fullName = Assembly.GetEntryAssembly().Location;
            string myName = Path.GetDirectoryName(fullName);
           
            //get directory of the .exe    
            String systemProperties = myName + @"\system.properties";

            try
            {
                streamReaderProperties = new StreamReader(systemProperties);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Environment.Exit(1);
            }

            //Load properties into Array
            String line;
            while ((line = streamReaderProperties.ReadLine()) != null)
            {
                listProperties.Add(line);
            }

            streamReaderProperties.Close();

            String[] path = new String[2];
            for (int i = 0; i < listProperties.Count; i++)
            {
                path = listProperties[i].Split('=');
            }

            String directory = path[1];
            String cbrDirectory = directory + @"\CBR\"; ;
            String archiveDirectory = directory + @"\Archive\";
            String logDirectory = directory + @"\Log\";
            String logFile = logDirectory + @"log.txt";
            System.Diagnostics.Debug.WriteLine(logFile);

            //Get Time
            String timeStamp;
            Time time = new Time();
            timeStamp = time.getTimeStamp();

            //Create Directories if they dont exist
            DirecotyCreator directoryCreator = new DirecotyCreator(directory, cbrDirectory, archiveDirectory, logDirectory);
            directoryCreator.createDirectories();

            //Generate Log File
            StreamWriter w = File.AppendText(logFile);
        //    File.GetAccessControl(logFile);
            Log log = new Log(w);

            //Load .json files from root folder
            string[] files = System.IO.Directory.GetFiles(directory, "*.json");
            log.append(files.Length + " files found");

            if (files.Length < 1)
            {
                log.append("No .json files found");
                w.Close();
                Environment.Exit(1);
            }

            Build build = new Build(directory, cbrDirectory, archiveDirectory, timeStamp, log);

            for (int i = 0; i < files.Length; i++)
            {
                String JSONFile = build.loadFile(files[i]);
                List<String> JSONItems = build.loadJson(JSONFile);

                List<List<String>> finishedArray = build.buildCBR(JSONItems);
                build.writeData(finishedArray, files[i]);
                build.moveFiles(files[i]);
            }
            w.Close();
        }
    }
}
