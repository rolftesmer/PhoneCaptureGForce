using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Windows.Devices.Sensors;

namespace PhoneCaptureGForce
{
    public sealed partial class MainPage : Page
    {
        static string deviceConnectionString = "HostName=SSUGAIH.azure-devices.net;DeviceId=Windows10Phone;SharedAccessKey=KxPV2mfEvVMjYMvtjvIXOhzv79S7C2qNlYAWp1TQK1w=";
        //static string deviceConnectionString = "HostName=aciot20.azure-devices.net;DeviceId=Windows10Phone;SharedAccessKey=7gmQHO/fWk/xm3RWIiBjK+Cn9MRzBou+csCTC/WpYeA=";
        static DeviceClient deviceClient;
        int sending = 0;
        int count = 0;

        public MainPage()
        {
            this.InitializeComponent();
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Http1);

            DispatcherTimer timer = new Windows.UI.Xaml.DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Start();
        }

        void TimerTick(object sender, object e)
        {
            SendDeviceToCloudMessagesAsync();
        }

        private async void SendDeviceToCloudMessagesAsync()
        {
            byte[] byteData;
            string JSONString = "";
            Message EventToSend;

            count++;
            if (chkGoSlow.IsChecked == true)
            {
                if ( count %10 !=0 ) { return; } 
            }
            // Calculate Accelleration and Position
            Accelerometer accelerometer = Accelerometer.GetDefault();
            AccelerometerReading r = accelerometer.GetCurrentReading();
            double G = Math.Sqrt((r.AccelerationX * r.AccelerationX) + (r.AccelerationY * r.AccelerationY) + (r.AccelerationZ * r.AccelerationZ));

            // Create JSON String
            JSONString = "{\"DataTypeKey\": \"ACC\", \"Count\": " + count + ", \"X\": " + Math.Round(r.AccelerationX, 2) + ", \"Y\": " + Math.Round(r.AccelerationY, 2) + ", \"Z\": " + Math.Round(r.AccelerationZ, 2) + ", \"G\": " + Math.Round(G, 2) + "}";

            Label1.Text = JSONString;
            byteData = Encoding.UTF8.GetBytes(JSONString);
            EventToSend = new Message(byteData);

            if (sending == 1)
            {
                await deviceClient.SendEventAsync(EventToSend);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //SendDeviceToCloudMessagesAsync();
            sending = 1;
            LabelStatus.Text = "STARTED";
        } 

        private void CmdStop_Click(object sender, RoutedEventArgs e)
        {
            sending = 0;
            LabelStatus.Text = "STOPPED";
        }
    }
}
