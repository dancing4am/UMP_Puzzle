/*
*	FILE			:	Timer.cs
*	PROJECT			:	Windows Programming - PROG2121 Assignment 07
*	NAME			:	Chowon Jung
*	FIRST VERSION	:	2019-11-29
*	DESCRIPTION		:	This file contains the model class of the Tile Puzzle application.
*/


using System;
using System.Diagnostics;
using Windows.Storage;


namespace WMP_A07_ChowonJung.Data
{
    public static class Timer
    {
        /* Properties */
        public static Stopwatch CurrentPlayTime { get; set; }
        public static string file = "play_record.txt";
        public static StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public static string LastRecord { get; set; }


        /*
        *	Method		: WriteRecord()
        *	Description	: This method compares last record and highest record
        *	              and stores higher record into a textfile.
        *	Parameters	: string playTime : last play record in string
        *	Return		: void
        */
        public static async void WriteRecord(string playTime)
        {
            // read existing record
            ReadRecord();

            // if proper highest record exist
            if (LastRecord != null)
            {
                if (LastRecord != "None")
                {
                    // compare with last play record
                    TimeSpan lastRec = TimeSpan.Parse(LastRecord);
                    TimeSpan newRec = TimeSpan.Parse(playTime);

                    // new record is better than old one, so write a new record
                    if (TimeSpan.Compare(lastRec, newRec) == 1)
                    {
                        // write record
                        StorageFile recordFile = await storageFolder.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting);
                        await FileIO.WriteTextAsync(recordFile, playTime);
                    }
                }
                else if (LastRecord == "None")
                {
                    // write without compare when no record exists
                    StorageFile recordFile = await storageFolder.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(recordFile, playTime);
                }
            }
        }



        /*
        *	Method		: ReadRecord()
        *	Description	: This method reads existing highest record in for later use.
        *	Parameters	: N/A
        *	Return		: void
        */
        public static async void ReadRecord()
        {
            try
            {
                // try read current highest record in
                StorageFile existFile = await storageFolder.GetFileAsync(file);
                LastRecord = await FileIO.ReadTextAsync(existFile);

                // if no record, set as "None"
                if (LastRecord.Length < 8)
                {
                    LastRecord = "None";
                }
            }
            catch
            {
                // if error, set as "None"
                LastRecord = "None";
            }
        }



        /*
        *	Method		: Reset()
        *	Description	: This method resets exsting highest record data.
        *	Parameters	: N/A
        *	Return		: void
        */
        public static async void Reset()
        {
            try
            {
                // try reset
                StorageFile existFile = await storageFolder.GetFileAsync(file);
                await FileIO.WriteTextAsync(existFile, "None");
            }
            catch
            {
                // replace it if exists
                StorageFile recordFile = await storageFolder.CreateFileAsync(file, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(recordFile, "None");
            }
        }
    }
}
