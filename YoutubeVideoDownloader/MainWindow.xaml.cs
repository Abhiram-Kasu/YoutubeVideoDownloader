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

using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;



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
        public void document(String content) {

            debugConsole.Text += String.Format("{0} " + content + "\r\n", DateTime.Now.ToString());
            DoEvents();
        }

        
        public async void Download(bool? mp4) {
            String link = inputURL.Text;
            link.Replace("https://www.youtube.com/watch?v=", "");
            
            
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(link);
            updateToo(20);
            document("Got video and created youtube client");
            
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path += @"\" + video.Title;
            updateToo(40);
            document("Created path with video name as file");

            
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(link);
            var audiostream = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            updateToo(60);
            document("Got audio Stream");
            if ((bool)mp4)
            {
                path += ".mp3";
                await youtube.Videos.Streams.DownloadAsync(audiostream, path);
                String mp4path = path.Substring(0, path.Length - 4);
                mp4path += ".mp4";
                
                
                var videostream = streamManifest.GetVideoOnlyStreams().Where(s => s.Container == Container.Mp4).GetWithHighestVideoQuality();
                document("Got video stream");
                await youtube.Videos.Streams.DownloadAsync(videostream, mp4path);

                document("Downloaded Video to : " + mp4path);
                DoEvents();
                
                var combinedstreams = new IStreamInfo[] { audiostream, videostream };
                ProgressBar.IsIndeterminate = true;
                DoEvents();

                await youtube.Videos.DownloadAsync(combinedstreams, new ConversionRequestBuilder(mp4path).Build());

                /*await youtube.Videos.DownloadAsync(link, path);*/
              

                updateToo(80);
                ProgressBar.IsIndeterminate = false;
                DoEvents();
                document(path);
                /*await youtube.Videos.DownloadAsync(combinedstreams, new ConversionRequestBuilder("Test.mp4").Build());*/
                document("Downloaded MP4 file to: " + path);
                updateToo(100);
            }
            else {
                path += ".mp3";
                await youtube.Videos.Streams.DownloadAsync(audiostream, path);
                updateToo(100);
                document("Downlaoded audio file to:" + path);
            }
            
            


            
            
            
           
            
            

        }

        private void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            Download(boolMP4.IsChecked);
        }

      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(path);
            debugConsole.Text += String.Format("{0} Copyed Path\r\n", DateTime.Now.ToString());
        }
    }
}
