using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System.Text;
using Windows.Devices.Sensors;


using Newtonsoft.Json;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhoneCaptureGForce
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        static string iotHubUri = "{iot hub hostname}";
        static string deviceKey = "{device key}";
        static string deviceConnectionString = "HostName=aciot20.azure-devices.net;DeviceId=Windows10Phone;SharedAccessKey=7gmQHO/fWk/xm3RWIiBjK+Cn9MRzBou+csCTC/WpYeA=";
        static DeviceClient deviceClient;
        int sending = 0;
        int count = 0;


        public MainPage()
        {
            
            this.InitializeComponent();
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Http1);

            DispatcherTimer timer = new Windows.UI.Xaml.DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = new TimeSpan(0, 0, 0,0,50);
            timer.Start();
        }

        void TimerTick(object sender, object e)
        {
            SendDeviceToCloudMessagesAsync();
        }


        private async void SendDeviceToCloudMessagesAsync() {

            count++;
            if (chkGoSlow.IsChecked == true)
            {
                if ( count %20 !=0 ) { return; }
            }
            Accelerometer accelerometer = Accelerometer.GetDefault();
            
            AccelerometerReading r = accelerometer.GetCurrentReading();
            string msgstring = "datatype,X,Y,Z,G,temperature,humidity\r\n";

            double G =  Math.Sqrt((r.AccelerationX * r.AccelerationX) + (r.AccelerationY * r.AccelerationY) + (r.AccelerationZ * r.AccelerationZ));

            msgstring = msgstring + "ACC,"+ Math.Round( r.AccelerationX,2) + "," + Math.Round( r.AccelerationY,2)  + "," + Math.Round( r.AccelerationZ,2) + "," +  Math.Round(G,2) + ",0,0";
            Label1.Text = msgstring;


            Message msg = new Message(Encoding.ASCII.GetBytes(msgstring));

            if (sending == 1) {
                 await deviceClient.SendEventAsync(msg);
            }

        }
      

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //SendDeviceToCloudMessagesAsync();
            sending = 1;

        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {
            sending = 0;

        }
    }
}
