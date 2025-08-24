using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace V2XApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
