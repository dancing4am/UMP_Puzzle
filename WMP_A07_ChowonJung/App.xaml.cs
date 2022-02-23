/*
*	FILE			:	App.xaml.cs
*	PROJECT			:	Windows Programming - PROG2121 Assignment 07
*	NAME			:	Chowon Jung, Charng Gwon Lee
*	FIRST VERSION	:	2019-11-29
*	DESCRIPTION		:	This is the application build related file of Tile Puzzle application.
*/

using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WMP_A07_ChowonJung.Data;

namespace WMP_A07_ChowonJung
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        //private Dictionary<string, object> _store = new Dictionary<string, object>();
        //private readonly string _saveFileName = "store.xml";
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            // read game state data
            ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)Timer.localSettings.Values["GameState"];
            if (composite != null)
            {
                try
                {
                    // read last play time in
                    object playedTime_o = composite["PlayTime"];
                    string playedTime_s = (string)playedTime_o;
                    
                    // if first launch, set as 0
                    MainPage.isFirstLaunch = (bool)composite["IsFirstLaunch"];
                    if (MainPage.isFirstLaunch == true)
                    {
                        playedTime_s = "00:00:00.0";
                    }

                    // reset current timer and add with last played time stored
                    MainPage.timerView.Checker.Reset();
                    MainPage.timerView.lastPlayTime = TimeSpan.Parse(playedTime_s);
                    MainPage.timerView.timeOffset = MainPage.timerView.Checker.Elapsed + MainPage.timerView.lastPlayTime;
                    MainPage.timerView.PlayTimeUpdate();
                    MainPage.isNumeric = (bool)composite["IsNumeric"];
                    //// read tile states in
                    //MainPage.Images = (Image[,])composite["PlayTime"];
                    //MainPage.WinCombo = (Image[,])composite["WinCombo"];
                    //MainPage.Blocks = (BitmapImage[,])composite["Blocks"];

                    // set as not first launch
                    MainPage.isFirstLaunch = false;
                }
                catch
                {
                    // if fail to load data, start as first launch
                    MainPage.isFirstLaunch = true;
                }

            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }


        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            MainPage.timerView.SaveGame();

            /* --------------- enable this code for running purpose --------------- */
            // save current play time
            ApplicationDataContainer localSettings = Timer.localSettings;
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["PlayTime"] = MainPage.timerView.PlayTime;
            // save current play settings
            composite["IsNumeric"] = MainPage.isNumeric;
            composite["IsFirstLaunch"] = MainPage.isFirstLaunch;
            //// save tile states
            //composite["Blocks"] = MainPage.Blocks;
            //composite["Images"] = (object)MainPage.Images;
            //composite["WinCombo"] = (object)MainPage.WinCombo;
            localSettings.Values["GameState"] = composite;

            /* --------------- enable this code for debugging purpose --------------- */
            //ApplicationDataContainer localSettings = Timer.localSettings;
            //ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            //composite["PlayTime"] = "00:00:00.0";
            //// save current play settings
            //composite["IsNumeric"] = false;
            //composite["IsFirstLaunch"] = true;
            ////// save tile states
            ////composite["Blocks"] = MainPage.Blocks;
            ////composite["Images"] = (object)MainPage.Images;
            ////composite["WinCombo"] = (object)MainPage.WinCombo;
            //localSettings.Values["GameState"] = composite;

            deferral.Complete();
        }
    }
}
