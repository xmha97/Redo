using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Graphics;
using Windows.UI;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.UI.Xaml.Documents;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.AccessCache;
using Windows.Storage.Provider;
using Redo.Helpers;

namespace Redo
{
    public sealed partial class MainWindow : Window
    {
        DispatcherTimer _timer;
        enum WorkModes
        {
            Ready = 0,
            Recording = 1,
            Playing = 2,
        }
        WorkModes WorkMode;
        int TimerCounter = 0;
        string OutputFile = string.Empty;

        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(titleBar);
            this.Title = "Redo";
            this.AppWindow.SetIcon("Redo.ico");
            WindowHelper.HideWindow(this);
            var compactPresenter = Microsoft.UI.Windowing.CompactOverlayPresenter.Create();
            AppWindow.SetPresenter(compactPresenter);
            AppWindow.Resize(new SizeInt32(258, 162));
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, object e)
        {
            switch (WorkMode)
            {
                case WorkModes.Recording:
                    OutputFile += string.Format("MouseMove({0}, {1})\r\nSleep(100)\r\n", MouseHelper.GetCursorPosition().X, MouseHelper.GetCursorPosition().Y);
                    break;
                case WorkModes.Playing:
                    if (TimerCounter + 1 > 100)
                    {
                        TimerCounter = 0;
                        PlayToggleAction();
                    }
                    else
                    {
                        TimerCounter += 1;
                        MouseHelper.SetCursorPosition(new System.Drawing.Point(TimerCounter * 10, TimerCounter * 10));
                        WindowHelper.SetValue(this,TimerCounter,100);
                    }
                    break;
            }
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            RecordToggleAction();
        }

        private async void RecordToggleAction()
        {
            if (WorkMode.Equals(WorkModes.Ready))
            {
                MediaHelper.RedoPlaySounds(true);
                WorkMode = WorkModes.Recording;
                playButton.IsEnabled = false;
                settingsButton.IsEnabled = false;
                recordIconFadeStoryboard.Begin();
                recordFontIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                recordButton.Background = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
                OutputFile = string.Format("#requires AutoHotkey v2.0\r\n;@Ahk2Exe-SetProductName Redo\r\n;@Ahk2Exe-SetVersion {0}\r\nCoordMode(\"Mouse\", \"Screen\")\r\n", DateTime.Now.ToString("yy.M.d.H"));
                _timer.Start();
            }
            else
            {

                //disable the button to avoid double-clicking
                recordButton.IsEnabled = false;

                MediaHelper.RedoPlaySounds(false);
                WorkMode = WorkModes.Ready;
                recordIconFadeStoryboard.Stop();
                recordFontIcon.Foreground = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
                recordButton.ClearValue(Button.BackgroundProperty);
                _timer.Stop();

                // Create a file picker
                FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();

                // Retrieve the window handle (HWND) of the current WinUI 3 window.
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

                // Initialize the file picker with the window handle (HWND).
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

                // Set options for your file picker
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("AutoHotkey Script", new List<string>() { ".ahk" });
                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "Redo.ahk";

                // Open the picker for the user to pick a file
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    System.IO.File.WriteAllText(file.Path, OutputFile);
                }

                recordButton.IsEnabled = true;
                playButton.IsEnabled = true;
                settingsButton.IsEnabled = true;
            }
        }

        private void PlayToggleAction()
        {
            if (WorkMode.Equals(WorkModes.Ready))
            {
                MediaHelper.RedoPlaySounds(true);
                WorkMode = WorkModes.Playing;
                recordButton.IsEnabled = false;
                settingsButton.IsEnabled = false;
                playIconFadeStoryboard.Begin();
                recordFontIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 154, 157, 159));
                playFontIcon.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                playButton.Background = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
                _timer.Start();
            }
            else
            {
                MediaHelper.RedoPlaySounds(false);
                WorkMode = WorkModes.Ready;
                recordButton.IsEnabled = true;
                settingsButton.IsEnabled = true;
                playIconFadeStoryboard.Stop();
                recordFontIcon.Foreground = new SolidColorBrush((Color)Application.Current.Resources["SystemAccentColor"]);
                playFontIcon.ClearValue(Button.ForegroundProperty);
                playButton.Background = new SolidColorBrush(Colors.Transparent);
                _timer.Stop();
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayToggleAction();
        }
    }
}
