using System;
using System.IO;
using System.Collections;


    class Program
    {
        static void Main(string[] args)
        {
            string folderS = @"C:\Users\Daria\Desktop\Synchronization\Folder1";
            string folderR = @"C:\Users\Daria\Desktop\Synchronization\Folder2";
            Sync(folderS, folderR);
            Console.WriteLine();
            Console.WriteLine("Complete");
            Console.ReadLine();
        }

        public static void Sync(string folderS, string folderR)
        {
            DirectoryInfo source = new DirectoryInfo(folderS);
            DirectoryInfo replica = new DirectoryInfo(folderR);

            SyncAll(source, replica);
        }

        public static void SyncAll(DirectoryInfo source, DirectoryInfo replica)
        {
            Directory.CreateDirectory(replica.FullName);

            foreach(FileInfo file in source.GetFiles())
            {
                DateTime created = file.CreationTime;
                DateTime lastModified = file.LastWriteTime;

                if(File.Exists(Path.Combine(replica.FullName, file.Name)))
                {
                    string repFileName = Path.Combine(replica.FullName, file.Name);
                    FileInfo file2 = new FileInfo(repFileName);
                    DateTime lastM = file2.LastWriteTime;
                    Console.WriteLine(@"File {0}\{1} Already exists {2} last modified {3}", replica.FullName, file.Name, repFileName, lastM);

                    try{
                        if(lastModified > lastM)
                        {
                            Console.WriteLine(@"Source file {0}\{1} last modified {2} is newer than the replica file {3}\{4} last modified {5}",
                            file.DirectoryName, file.Name, lastModified.ToString(), replica.FullName, file.Name, lastM.ToString());
                            file.CopyTo(Path.Combine(replica.FullName, file.Name), true);
                        }
                        else{
                            Console.WriteLine(@"Dest file {0}\{1} skipped", replica.FullName, file.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine(@"Copying {0}\{1}", replica.FullName, file.Name);
                    file.CopyTo(Path.Combine(replica.FullName, file.Name), true);
                }
            }
            
        }
    }
