using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace CropPlayer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = null;
        bool isDragging = false;

        TimeSpan startSpan;
        TimeSpan endSpan;

        ObservableCollection<PositionDuration> List = null; 

        public MainWindow()
        {
            InitializeComponent();

            List = new ObservableCollection<PositionDuration>();

            lvDuration.ItemsSource = List;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += Timer_Tick;

            PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                sbPosition.Value = media.Position.TotalSeconds;
                SetCurrentPosition();
            }
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (media.NaturalDuration.HasTimeSpan)
            {
                TimeSpan span = media.NaturalDuration.TimeSpan;
                sbPosition.Maximum = span.TotalSeconds;
                sbPosition.SmallChange = 1;
                sbPosition.LargeChange = Math.Min(10, span.Seconds / 10);
            }
            timer.Start();
        }

        private void sbPosition_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void sbPosition_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            media.Position = TimeSpan.FromSeconds(sbPosition.Value);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog().Value)
            {
                media.Source = new Uri(openFile.FileName);
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            media.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
        }

        private void btnB1s_Click(object sender, RoutedEventArgs e)
        {
            Move(1, false);
        }

        private void btnF1s_Click(object sender, RoutedEventArgs e)
        {
            Move(1);
        }

        private void btnB10s_Click(object sender, RoutedEventArgs e)
        {
            Move(10, false);
        }

        private void btnF10s_Click(object sender, RoutedEventArgs e)
        {
            Move(10);
        }

        private void Move(int seconds, bool isForward = true)
        {
            if (isForward)
                media.Position = media.Position + TimeSpan.FromSeconds(seconds);
            else
                media.Position = media.Position - TimeSpan.FromSeconds(seconds);
        }

        private void SetCurrentPosition()
        {
            tbCurrentPosition.Text = media.Position.ToString(@"hh\:mm\:ss");
        }

        private void SetStartPosition()
        {
            startSpan = media.Position;
            log.AppendText($"Start Position : {startSpan.ToString(@"hh\:mm\:ss")}{Environment.NewLine}");
        }

        private void SetEndPosition()
        {
            endSpan = media.Position;
            log.AppendText($"End Position : {endSpan.ToString(@"hh\:mm\:ss")}{Environment.NewLine}");
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSetStartPosition_Click(object sender, RoutedEventArgs e)
        {
            SetStartPosition();
        }

        private void btnSetEndPosition_Click(object sender, RoutedEventArgs e)
        {
            SetEndPosition();
        }

        private void btnSavePosition_Click(object sender, RoutedEventArgs e)
        {
            List.Add(new PositionDuration() { Start = startSpan, End = endSpan, Duration = endSpan - startSpan });
        }

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            var select = lvDuration.SelectedItem;

            if (select != null)
            {
                string start = ((PositionDuration)select).Start.ToString(@"hh\:mm\:ss");
                string duration = ((PositionDuration)select).Duration.ToString(@"hh\:mm\:ss");
                string name = ((PositionDuration)select).Name;

                //Process process = new Process();
                //process.StartInfo.FileName = "cmd.exe";
                //process.StartInfo.Arguments = "/k DIR";
                //process.StartInfo.UseShellExecute = false;
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardError = true;

                //process.OutputDataReceived += Process_OutputDataReceived;
                //process.ErrorDataReceived += Process_ErrorDataReceived;
                //process.Start();
                //process.BeginOutputReadLine();
                //process.BeginErrorReadLine();
                //process.WaitForExit();
            }



            //Process process = new Process();
            //process.StartInfo.FileName = "cmd.exe";
            //process.StartInfo.Arguments = "/k dir";
            //////process.StartInfo.UseShellExecute = false;
            //////process.StartInfo.RedirectStandardOutput = true;
            //////process.StartInfo.RedirectStandardError = true;

            //////process.OutputDataReceived += Process_OutputDataReceived;
            //////process.ErrorDataReceived += Process_ErrorDataReceived;
            //process.Start();
            ////process.BeginOutputReadLine();
            ////process.BeginErrorReadLine();
            ////process.WaitForExit();
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Dispatcher.Invoke(() => { log.AppendText(e.Data + Environment.NewLine); });
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //Dispatcher.Invoke(() => { log.AppendText(e.Data + Environment.NewLine); });
            
        }
    }

    public class PositionDuration
    {
        public string Name { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
