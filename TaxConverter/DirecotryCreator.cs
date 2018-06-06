using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxConverter
{
    class DirecotyCreator
    {
        private String directory;
        private String cbrDirectory;
        private String archiveDirectory;
        private String logDirectory;

        public DirecotyCreator()
        {

        }

        public DirecotyCreator(String directory, String cbrDirectory, String archiveDirectory, String logDirectory)
        {
            this.directory = directory;
            this.cbrDirectory = cbrDirectory;
            this.archiveDirectory = archiveDirectory;
            this.logDirectory = logDirectory;
        }

        public void createDirectories()
        {
            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(cbrDirectory))
                    Directory.CreateDirectory(cbrDirectory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating CBR directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(archiveDirectory))
                    Directory.CreateDirectory(archiveDirectory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating Archive directory!");
                Console.WriteLine(ex.ToString());
            }

            try
            {
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error creating Log directory!");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
