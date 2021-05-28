using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VideoLibrary;

namespace YoutubeVideoDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String path;
        int start;
        public MainWindow()
        {
            InitializeComponent();
            downloadButton.Content = "Download";
            inputURL.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            inputURL.MaxLines = 1;
            inputURL.TextWrapping = TextWrapping.NoWrap;

            debugConsole.Text = "Debug\r\n";
            debugConsole.TextWrapping = TextWrapping.WrapWithOverflow;
            debugConsole.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            debugConsole.IsReadOnly = true;
          
        }
       
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        //Code Function taken from user "Александр Пекшев" on https://stackoverflow.com/questions/4502037/where-is-the-application-doevents-in-wpf
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render,
                                                  new Action(delegate { }));
        }
        public void updateToo(int target) {


            for (int i = target - (int)ProgressBar.Value; i <= target; i++) {
                ProgressBar.Value++;
                
                

            }
            ProgressBar.Value = target;
            DoEvents();
            Thread.Sleep(500);

        }

        
        public void Download() {
            start = 0;

            var youTube = YouTube.Default; // starting point for YouTube actions
            updateToo(25);
            
            var video = youTube.GetVideo(inputURL.Text); // gets a Video object with info about the video
            updateToo(50);

            debugConsole.Text += String.Format("{0} Got youtube video object\r\n", DateTime.Now.ToString());
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path += @"\" + video.FullName;
            updateToo(75);
            debugConsole.Text += String.Format("{0} Path of Downloaded Object is: \r\n" + path + "\r\n", DateTime.Now.ToString());
            File.WriteAllBytes(path, video.GetBytes());
            updateToo(100);

        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            Download();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(path);
            debugConsole.Text += String.Format("{0} Copyed Path\r\n", DateTime.Now.ToString());
        }
    }
}
