using System.IO.Ports;
using System.Collections.Generic;
using System.Threading; // for cross-thread safety
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace V2XApp
{
    public partial class MainWindow : Window
    {
        private SerialPort? serialPort;
        private Queue<char> buffer = new Queue<char>(16);
        private int confidenceCounter = 0;
        private const int confidenceThreshold = 1;
        private string targetPattern = "0101010101010101";

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Load;
        }

        private void MainWindow_Load(object sender, RoutedEventArgs e)
        {
            try
            {
                serialPort = new SerialPort("COM4", 115200);
                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening serial port: {ex.Message}");
            }
        }
            
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort == null) return;

            string incoming = serialPort.ReadExisting();

            foreach (char c in incoming)
            {
                if (c == '0' || c == '1')
                {
                    if (buffer.Count == 16)
                        buffer.Dequeue();

                    buffer.Enqueue(c);

                    if (buffer.Count == 16)
                    {
                        string currentPattern = new string(buffer.ToArray());
                        if (currentPattern == targetPattern)
                            confidenceCounter++;
                        else
                            confidenceCounter = Math.Max(0, confidenceCounter - 1);

                        if (confidenceCounter == confidenceThreshold)
                        {
                            confidenceCounter = 0;

                            Dispatcher.Invoke(async () => await FlashStatus(Status1, "Received at " + DateTime.Now.ToLongTimeString()));
                        }
                    }
                }
            }
        }
      
        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            await FlashStatus(Status1, "Received at " + DateTime.Now.ToLongTimeString());
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            await FlashStatus(Status2, "Received at " + DateTime.Now.ToLongTimeString());
        }

        private async void Button3_Click(object sender, RoutedEventArgs e)
        {
            await FlashStatus(Status3, "Received at " + DateTime.Now.ToLongTimeString());
        }

        private async void Button4_Click(object sender, RoutedEventArgs e)
        {
            await FlashStatus(Status4, "Received at " + DateTime.Now.ToLongTimeString());
        }

        private async Task FlashStatus(System.Windows.Controls.TextBlock statusBlock, string message)
        {
            // Flash color or bold
            var originalBrush = statusBlock.Foreground;
            var originalWeight = statusBlock.FontWeight;

            statusBlock.Text = message;
            statusBlock.Foreground = Brushes.Red;
            statusBlock.FontWeight = FontWeights.ExtraBold;

            await Task.Delay(1000); // 1 second delay

            // Revert
            statusBlock.Foreground = originalBrush;
            statusBlock.FontWeight = originalWeight;
        }
    }
}
