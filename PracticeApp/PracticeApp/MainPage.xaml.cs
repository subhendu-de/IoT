using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using PracticeApp.Models;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PracticeApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _timer = new DispatcherTimer();
        private GpioPin _pin = null;
        private IDht _dht = null;

        static DeviceClient deviceClient;
        static string DeviceConnectionString = "<<device connection string>>";

        public MainPage()
        {
            this.InitializeComponent();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;

            _pin = GpioController.GetDefault().OpenPin(17, GpioSharingMode.Exclusive);
            _dht = new Dht11(_pin, GpioPinDriveMode.Input);

            deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

            _timer.Start();
        }

        private async void _timer_Tick(object sender, object e)
        {
            DhtReading _reading = new DhtReading();
            _reading = await _dht.GetReadingAsync().AsTask();

            if(_reading.IsValid)
            {
                SensorData _data = new SensorData()
                {
                    Temperature = Convert.ToSingle(_reading.Temperature),
                    Humidity = Convert.ToSingle(_reading.Humidity)
                };

                var messageString = JsonConvert.SerializeObject(_data);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    grdDashboard.DataContext = _data;
                });
            }
        }
    }
}
