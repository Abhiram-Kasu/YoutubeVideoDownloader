using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using VideoLibrary;

namespace YoutubeVideoDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String path;
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

        
        public void Download() {

            var youTube = YouTube.Default; // starting point for YouTube actions
            var video = youTube.GetVideo(inputURL.Text); // gets a Video object with info about the video


            debugConsole.Text += String.Format("{0} Got youtube video object\r\n", DateTime.Now.ToString());
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path += @"\" + video.FullName;
            debugConsole.Text += String.Format("{0} Path of Downloaded Object is: \r\n" + path + "\r\n", DateTime.Now.ToString());
            File.WriteAllBytes(path, video.GetBytes());

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
