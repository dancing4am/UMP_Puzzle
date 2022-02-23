/*
*	FILE			:	MainPage.xaml.cs
*	PROJECT			:	Windows Programming - PROG2121 Assignment 07
*	NAME			:	Chowon Jung, Charng Gwon Lee
*	FIRST VERSION	:	2019-11-29
*	DESCRIPTION		:	This is the MainPage of Tile Puzzle application.
*/


using System;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WMP_A07_ChowonJung.ViewModel;



namespace WMP_A07_ChowonJung
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        /* Properties */
        string filePath;
        public static BitmapImage[,] Blocks = new BitmapImage[4, 4];
        public static Image[,] Images = new Image[4, 4];
        public static Image[,] WinCombo = new Image[4, 4];
        public static Button[,] Tiles = new Button[4, 4];
        bool noShuffle = false;
        public static bool isNumeric = false;
        public static bool isFirstLaunch = true;
        public static TimerViewModel timerView = new TimerViewModel();

        /*
        *	Method		: ()
        *	Description	: 
        *	Parameters	: N/A
        *	Return		: void
        */
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainWindow_Loaded;

            // connect with tile images
            Images[0, 0] = img00;
            Images[0, 1] = img01;
            Images[0, 2] = img02;
            Images[0, 3] = img03;
            Images[1, 0] = img10;
            Images[1, 1] = img11;
            Images[1, 2] = img12;
            Images[1, 3] = img13;
            Images[2, 0] = img20;
            Images[2, 1] = img21;
            Images[2, 2] = img22;
            Images[2, 3] = img23;
            Images[3, 0] = img30;
            Images[3, 1] = img31;
            Images[3, 2] = img32;
            Images[3, 3] = img33;

            // connect with time images
            WinCombo[0, 0] = img00;
            WinCombo[0, 1] = img01;
            WinCombo[0, 2] = img02;
            WinCombo[0, 3] = img03;
            WinCombo[1, 0] = img10;
            WinCombo[1, 1] = img11;
            WinCombo[1, 2] = img12;
            WinCombo[1, 3] = img13;
            WinCombo[2, 0] = img20;
            WinCombo[2, 1] = img21;
            WinCombo[2, 2] = img22;
            WinCombo[2, 3] = img23;
            WinCombo[3, 0] = img30;
            WinCombo[3, 1] = img31;
            WinCombo[3, 2] = img32;
            WinCombo[3, 3] = img33;

            // connect with tile buttons
            Tiles[0, 0] = btn00;
            Tiles[0, 1] = btn01;
            Tiles[0, 2] = btn02;
            Tiles[0, 3] = btn03;
            Tiles[1, 0] = btn10;
            Tiles[1, 1] = btn11;
            Tiles[1, 2] = btn12;
            Tiles[1, 3] = btn13;
            Tiles[2, 0] = btn20;
            Tiles[2, 1] = btn21;
            Tiles[2, 2] = btn22;
            Tiles[2, 3] = btn23;
            Tiles[3, 0] = btn30;
            Tiles[3, 1] = btn31;
            Tiles[3, 2] = btn32;
            Tiles[3, 3] = btn33;

            // if first launch, start as numeric game
            if (isFirstLaunch == true)
            {
                Load_Numbers();
            }
        }



        /*
        *	Method		: MainWindow_Loaded()
        *	Description	: This method is invoked my main window load to create essential class instance.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // create TimerViewModel instance
            DataContext = timerView;

            // read highest record in
            timerView.Highest_Update();
        }



        /*
        *	Method		: Load_Button_Click()
        *	Description	: This method loads image into tiles.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            Open_File();
        }



        /*
        *	Method		: Camera_Button_Click()
        *	Description	: This method executes camera and loads photo into tiles.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void Camera_Button_Click(object sender, RoutedEventArgs e)
        {
            Camera();
        }



        /*
        *	Method		: To_Number_Button_Click()
        *	Description	: This method changes picture-oriented game to a number-oriented game.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void To_Number_Button_Click(object sender, RoutedEventArgs e)
        {
            To_Numbers();
        }



        /*
        *	Method		: New_Number_Button_Click()
        *	Description	: This method loads new number-oriented game.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void New_Number_Button_Click(object sender, RoutedEventArgs e)
        {
            Load_Numbers();
        }



        /*
        *	Method		: Leaderboard_Button_Click()
        *	Description	: This method displays leaderboard containing highest record.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void Leaderboard_Button_Click(object sender, RoutedEventArgs e)
        {
            Leaderboard();
        }



        /*
        *	Method		: ResetRecord_Click()
        *	Description	: This method resets highest record.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void ResetRecord_Click(object sender, RoutedEventArgs e)
        {
            // reset existing record
            timerView.ResetRecord();

            // make sure updated
            HighestRank_text.UpdateLayout();
        }



        /*
        *	Method		: Button_Click_Move()
        *	Description	: This method checks if the pressed button neighbors an empty tile
        *	              and moves the corresponding tile into the empty spot if exists.
        *	Parameters	: object sender, RoutedEventArgs e
        *	Return		: void
        */
        private void Button_Click_Move(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;

            // navigate button position
            Button btn = (Button)sender;
            if (btn.Name != Tiles[i, j].Name)
            {
                while (btn.Name != Tiles[i, j].Name)
                {
                    if (j < 3)
                    {
                        j++;
                    }
                    else if (i < 3)
                    {
                        i++;
                        j = 0;
                    }
                }
            }

            int a = i;
            int b = j;

            // compare if corresponding button image has an empty tile neighbor
            if (Images[a, b].Source != null)
            {
                if (b > 0)
                {
                    if (Images[a, b-1].Source == null)
                    {
                        b--;
                    }
                }
                if (b < 3)
                {
                    if (Images[a, b + 1].Source == null)
                    {
                        b++;
                    }
                }
                if (a > 0)
                {
                    if (Images[a - 1, b].Source == null)
                    {
                        a--;
                    }
                }
                if (a < 3)
                {
                    if (Images[a + 1, b].Source == null)
                    {
                        a++;
                    }
                }
            }

            if (Images[i, j].Source != Images[a, b].Source)
            {
                // if has an empty tile around, move
                ImageSource temp = Images[i, j].Source;
                Images[i, j].Source = Images[a, b].Source;
                Images[a, b].Source = temp;

                // check if win
                if (Check_Win() == true)
                {
                    Leaderboard();
                    Win_Effect();
                }
            }
        }



        /*
        *	Method		: Camera()
        *	Description	: This method executes camera and loads photo into tiles.
        *	Parameters	: N/A
        *	Return		: void
        */
        private async void Camera()
        {
            // use Windows.Media.Capture.CameraCaptureUI API to capture a photo
            CameraCaptureUI dialog = new CameraCaptureUI();
            Size aspectRatio = new Size(20, 20);
            dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

            StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);

            // take a photo and create tiles with it
            if (file != null)
            {
                filePath = file.Path;
                if (file != null)
                {
                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                        PlaceImg(decoder);
                    }
                }
                isNumeric = false;

                // start game when ready
                GameStart();
            }
        }



        /*
        *	Method		: Leaderboard()
        *	Description	: This method displays leaderboard containing highest record.
        *	Parameters	: N/A
        *	Return		: void
        */
        private void Leaderboard()
        {
            timerView.Highest_Update();
            HighestRank_text.UpdateLayout();
        }



        /*
        *	Method		: Check_Win()
        *	Description	: This method tests win states.
        *	Parameters	: N/A
        *	Return		: bool : true if win
        */
        private bool Check_Win()
        {
            if (Images[3, 3].Source == null)
            {
                if (Images[2, 3].Source == WinCombo[2, 3].Source)
                {
                    if (Images[3, 2].Source == WinCombo[3, 2].Source)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (Images[i, j].Source != WinCombo[i, j].Source)
                                {
                                    // return false if any wrong match found
                                    return false;
                                }
                            }
                        }
                        // true if all tiles match to win combo
                        return true;
                    }
                }
            }
            // return false if any wrong match found
            return false;
        }



        /*
        *	Method		: Win_Effect()
        *	Description	: This method display winning effect.
        *	Parameters	: N/A
        *	Return		: void
        */
        private void Win_Effect()
        {
            // stop timer
            timerView.StopTimer();

            // display win effect
            Notice_btn.Visibility = Visibility.Visible;
            Notice_text.Text = "Congrats!";
        }



        /*
        *	Method		: Open_File()
        *	Description	: This methods opens a new image file and create tiles with it.
        *	Parameters	: N/A
        *	Return		: void
        */
        public async void Open_File()
        {
            // prepare file open
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;

            // limit file extensions
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                filePath = file.Path;
                if (file != null)
                {
                    // open and create tiles if file is valid
                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                        PlaceImg(decoder);
                    }
                }
                isNumeric = false;

                // start game
                GameStart();
            }
        }



        /*
        *	Method		: To_Numbers()
        *	Description	: This method changes current tiles into numeric tiles.
        *	Parameters	: N/A
        *	Return		: void
        */
        public async void To_Numbers()
        {
            // prepare number image file open
            string NumberImageFile = @"Assets\Numbers.png";
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(NumberImageFile);

            filePath = file.Path;
            if (file != null)
            {
                // open image when valid
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                    // prepare cutting and storing
                    var imgHeight = decoder.PixelHeight / 4;
                    var imgWidth = decoder.PixelWidth / 4;
                    Blocks = new BitmapImage[4, 4];

                    // cut the image and store as tiles
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (i == 3 && j == 3)
                            {
                                Blocks[i, j] = null;
                                continue;
                            }
                            InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream(); //memory for loading the image into the encoder
                            BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder); //bitmap encoder ini
                            BitmapBounds bounds = new BitmapBounds(); //used to tranform the encoder to specify which part of the image will be used
                            bounds.Height = imgHeight;
                            bounds.Width = imgWidth;
                            bounds.X = 0 + imgWidth * (uint)i;
                            bounds.Y = 0 + imgHeight * (uint)j;
                            enc.BitmapTransform.Bounds = bounds;   //to tell the encoder how the cutting will be done
                            try
                            {
                                await enc.FlushAsync(); //actual cutting
                            }
                            catch (Exception ex)
                            {
                                string s = ex.ToString();
                            }

                            BitmapImage bImg = new BitmapImage(); //an image to be displayed
                            bImg.SetSource(ras);
                            Blocks[i, j] = bImg; //to save the bitmap images
                        }
                    }
                }
                // read current tile arrangement
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int a = 0;
                        int b = 0;
                        if (Images[i, j].Source == WinCombo[a, b].Source)
                        {
                            Images[i, j].Source = Blocks[a, b];
                        }
                        while (Images[i, j].Source != WinCombo[a, b].Source)
                        {
                            b++;
                            if (b >= 4)
                            {
                                a++;
                                b = 0;
                            }
                            if (a >= 4)
                            {
                                break;
                            }
                            // replace with numeric tiles
                            if (Images[i, j].Source == WinCombo[a, b].Source)
                            {
                                Images[i, j].Source = Blocks[a, b];
                            }
                        }
                    }
                }

                for (int i = 0; i < 4; i++)   //creating new winning combination
                {
                    for (int j = 0; j < 4; j++)
                    {
                        WinCombo[i, j] = new Image();
                        WinCombo[i, j].Source = Blocks[i, j];
                    }
                }
                isNumeric = true;

                // start game without shuffle
                GameStartNoShuffle();
            }
        }



        /*
        *	Method		: Load_Numbers()
        *	Description	: This method runs new number oriented game.
        *	Parameters	: N/A
        *	Return		: void
        */
        public async void Load_Numbers()
        {
            // prepare file read in
            string NumberImageFile = @"Assets\Numbers.png";
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(NumberImageFile);

            filePath = file.Path;
            if (file != null)
            {
                // open and create tiles when valid
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                    PlaceImg(decoder);
                }
                isNumeric = true;

                // start game
                GameStart();
            }
        }



        /*
        *	Method		: PlaceImg()
        *	Description	: This method cuts loaded image and create tiles with them.
        *	Parameters	: BitmapDecoder decoder : loaded image
        *	Return		: void
        */
        private async void PlaceImg(BitmapDecoder decoder)
        {
            // measure height and width to cut
            var imgHeight = decoder.PixelHeight / 4;
            var imgWidth = decoder.PixelWidth / 4;

            // cut into tiles
            Blocks = new BitmapImage[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == 3 && j == 3)
                    {
                        // empty 16th tile
                        Images[i, j].Source = null;
                        continue;
                    }
                    InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream(); //memory for loading the image into the encoder
                    BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder); //bitmap encoder ini
                    BitmapBounds bounds = new BitmapBounds(); //used to tranform the encoder to specify which part of the image will be used
                    bounds.Height = imgHeight;
                    bounds.Width = imgWidth;
                    bounds.X = 0 + imgWidth * (uint)i;
                    bounds.Y = 0 + imgHeight * (uint)j;
                    enc.BitmapTransform.Bounds = bounds;   //to tell the encoder how the cutting will be done
                    try
                    {
                        await enc.FlushAsync(); //actual cutting
                    }
                    catch (Exception ex)
                    {
                        string s = ex.ToString();
                    }

                    BitmapImage bImg = new BitmapImage(); //an image to be displayed
                    bImg.SetSource(ras);
                    Blocks[i, j] = bImg; //to save the bitmap images
                    Images[i, j].Source = Blocks[i, j]; //blocks are the source for images
                }
            }
            for (int i = 0; i < 4; i++)   //creating the winning combination
            {
                for (int j = 0; j < 4; j++)
                {
                    WinCombo[i, j] = new Image();
                    WinCombo[i, j].Source = Images[i, j].Source;
                }
            }
        }



        /*
        *	Method		: Shuffle()
        *	Description	: This mehthod shuffles existing tiles.
        *	Parameters	: N/A
        *	Return		: void
        */
        private void Shuffle()
        {
            // set range to create random array
            int rowNum = Images.GetUpperBound(0) + 1;
            int colNum = Images.GetUpperBound(1) + 1;
            int cellNum = rowNum * colNum;

            Image[,] tempArray = Images;

            // randomize the array.
            Random rand = new Random();
            for (int i = 0; i < cellNum - 1; i++)
            {
                // pick a random cell between i and the end of the array.
                int j = rand.Next(i, cellNum);

                // convert to row/column indexes.
                int row_i = i / colNum;
                int col_i = i % colNum;
                int row_j = j / colNum;
                int col_j = j % colNum;

                // swap cells i and j.
                ImageSource temp = Images[row_i, col_i].Source;
                Images[row_i, col_i].Source = Images[row_j, col_j].Source;
                Images[row_j, col_j].Source = temp;

                // replace existing array with newly randomized one
                if (Images[3, 3].Source != null)
                {
                    int a = 0;
                    int b = 0;
                    while (Images[3, 3].Source != null)
                    {
                        if (Images[a, b].Source == null)
                        {
                            Images[a, b].Source = Images[3, 3].Source;
                            Images[3, 3].Source = null;
                        }
                        else if (b < 3)
                        {
                            b++;
                        }
                        else if (a < 3)
                        {
                            b = 0;
                            a++;
                        }
                    }
                }
            }

            // start timer
            timerView.StartTimer();
        }



        /*
        *	Method		: Notice_Click()
        *	Description	: This method disables display blocking botton.
        *	Parameters	: N/A
        *	Return		: void
        */
        private void Notice_Click(object sender, RoutedEventArgs e)
        {
            Notice_text.Text = "";
            Notice_btn.Visibility = Visibility.Collapsed;
        }



        /*
        *	Method		: GameStart()
        *	Description	: This method display ready message on screen.
        *	Parameters	: N/A
        *	Return		: void
        */
        private void GameStart()
        {
            Start_text.Text = "Ready, Go!";
            Start_btn.Visibility = Visibility.Visible;
        }



        /*
        *	Method		: GameStartNoShuffle()
        *	Description	: This method starts/resumes game without shuffling tiles.
        *	Parameters	: N/A
        *	Return		: void
        */
        private void GameStartNoShuffle()
        {
            noShuffle = true;
            Start_text.Text = "Ready, Go!";
            Start_btn.Visibility = Visibility.Visible;
        }



        /*
        *	Method		: Start_Click()
        *	Description	: This method disables game start message button and starts game.
        *	Parameters	: N/A
        *	Return		: void
        */
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Start_text.Text = "";
            Start_btn.Visibility = Visibility.Collapsed;
            if (noShuffle == false)
            {
                Shuffle();
            }
            noShuffle = false;
        }
    }
}
