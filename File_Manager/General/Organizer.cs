﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO.Compression;
using System.Threading;

namespace File_Manager.General
{
    public static class Organizer
    {

        public static void createTimestampFolders(String sourceDirectory, String targetDirectory, String folderFormat, String fileType)
        {
            DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
            FileInfo[] sourceFiles = sourceInfo.GetFiles(String.Format("*{0}", fileType)); //Getting Text files
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            String folderStringFormat = String.Empty;
            String workingDirectory = String.Format(@"{0}/{1}_buffer_temp", targetDirectory, System.IO.Path.GetFileName(sourceDirectory));

            sourceFiles = sourceFiles.OrderBy(f => f.Name).ToArray();


            bool isNotFirst = false;




            foreach (FileInfo file in sourceFiles) {


                if (Regex.IsMatch(file.Name, folderFormat) == true)
                {
                    Regex rege = new Regex(String.Format("{0}", folderFormat));
                    var results = rege.Matches(file.Name);
                    DateTime time = new DateTime();

                    String folderName = String.Empty;
                    DateTime.TryParseExact(String.Format("{0}", results[0].Groups[1]), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out time);

                    folderName = time.Date.ToString("MM-dd-yyyy");

                    if (!isNotFirst) { startDate = time; isNotFirst = true; };



                    if (!System.IO.Directory.Exists(String.Format(@"{0}\{1}", workingDirectory, time.Date.ToString())))
                    {
                        Directory.CreateDirectory(String.Format(@"{0}\{1}", workingDirectory, folderName));
                    }

                    if (!Directory.Exists(String.Format(@"{0}\{1}\{2}", workingDirectory, folderName, file.Name)))
                    {

                        File.Copy(file.FullName, (String.Format(@"{0}\{1}\{2}", workingDirectory, folderName, file.Name)));

                    }
                    endDate = time;
                }
                
            }

            foreach (var sourceFile in sourceFiles)
            {
                sourceFile.Delete();
            }

            if (startDate != endDate) { folderStringFormat = "{0}/{1}_{2}.zip"; } else { folderStringFormat = "{0}/{1}.zip"; };



            //compressTargetFolder(folderStringFormat, workingDirectory, targetDirectory, startDate, endDate);
            //Directory.Delete(workingDirectory, true);


        }

        public static void compressTargetFolder(String folderStringFormat, String zipDirectory, String targetDirectory, DateTime startDate, DateTime endDate)
        {

            ZipFile.CreateFromDirectory(zipDirectory, String.Format(folderStringFormat, targetDirectory, startDate.ToString("MM-dd-yyyy"), endDate.ToString("MM-dd-yyyy")));

        }


    }
}
