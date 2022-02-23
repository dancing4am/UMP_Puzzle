/*
*	FILE			:	TimerViewModel.cs
*	PROJECT			:	Windows Programming - PROG2121 Assignment 07
*	NAME			:	Chowon Jung
*	FIRST VERSION	:	2019-11-29
*	DESCRIPTION		:	This file contains view model part of the Tile Puzzle application.
*/

using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using WMP_A07_ChowonJung.Data;


namespace WMP_A07_ChowonJung.ViewModel
{
    public class TimerViewModel : DependencyObject
    {
        /* Properties */
        public DispatcherTimer Clock = new DispatcherTimer();
        public Stopwatch Checker = new Stopwatch();
        public TimeSpan timeOffset = new TimeSpan(0, 0, 0);
        public TimeSpan lastPlayTime = new TimeSpan(0, 0, 0); 
        public string PlayTime          // current play time
        {
            get { return (string)GetValue(PlayTimeProperty); }
            set { SetValue(PlayTimeProperty, value); }
        }
        public string Highest           // highest record
        {
            get { return (string)GetValue(HighestRecordProperty); }
            set { SetValue(HighestRecordProperty, value); }
        }



        /* Dependency Properties */
        public static readonly DependencyProperty PlayTimeProperty =
        DependencyProperty.Register("PlayTime", typeof(string), typeof(TimerViewModel), new PropertyMetadata(null));
        public static readonly DependencyProperty HighestRecordProperty =
        DependencyProperty.Register("Highest", typeof(string), typeof(TimerViewModel), new PropertyMetadata(null));



        /* Constructor */
        public TimerViewModel()
        {
            // display highest record
            Highest_Update();

            // prepare for timer
            Clock.Tick += Timer_Tick;
            Clock.Interval = new TimeSpan(0, 0, 1);
            PlayTimeUpdate();
        }



        /*
        *	Method		: StartTimer()
        *	Description	: This method starts timer.
        *	Parameters	: N/A
        *	Return		: void
        */
        public void StartTimer()
        {
            Checker.Reset();
            Clock.Start();
            Checker.Start();
        }



        /*
        *	Method		: Highest_Update()
        *	Description	: This method updates highest record display.
        *	Parameters	: N/A
        *	Return		: void
        */
        public void Highest_Update()
        {
            Timer.ReadRecord();
            Highest = "Highest Record: " + Timer.LastRecord;
        }



        /*
        *	Method		: Timer_Tick()
        *	Description	: This method manipulates current play time display to be updated for every tick.
        *	Parameters	: object sender, object e
        *	Return		: void
        */
        private void Timer_Tick(object sender, object e)
        {
            PlayTimeUpdate();
        }



        /*
        *	Method		: PlayTimeUpdate()
        *	Description	: This method updates current game play time.
        *	Parameters	: N/A
        *	Return		: void
        */
        public void PlayTimeUpdate()
        {
            PlayTime = (Checker.Elapsed.Hours + timeOffset.Hours).ToString("00" + ":");
            PlayTime += (Checker.Elapsed.Minutes + timeOffset.Minutes).ToString("00" + ":");
            PlayTime += (Checker.Elapsed.Seconds + timeOffset.Seconds).ToString("00");
        }



        /*
        *	Method		: StopTimer()
        *	Description	: This method stops game timer and
        *	              sends elapsed time data to be tested if to record or not.
        *	Parameters	: N/A
        *	Return		: void
        */
        public void StopTimer()
        {
            // stop timer
            Clock.Stop();
            Checker.Stop();

            // send record to be tested if is highest record or not
            Timer.WriteRecord(Checker.Elapsed.ToString());
        }



        /*
        *	Method		: ResetRecord()
        *	Description	: This method resets existing highest record and its display.
        *	Parameters	: N/A
        *	Return		: void
        */
        public void ResetRecord()
        {
            // reset existing data
            Timer.Reset();
            Timer.ReadRecord();

            // reset current display
            Highest = "Highest Record: " + Timer.LastRecord;
        }



        /*
        *	Method		: SaveGame()
        *	Description	: This method is used to stop timers only right before application suspend/termination.
        *	Parameters	: N/A
        *	Return		: void
        */
        public void SaveGame()
        {
            Clock.Stop();
            Checker.Stop();
        }
    }
}
