using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeApp.Models
{
    public class SensorData
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }

        public string TemperatureDisplay
        {
            get
            {
                return string.Format("{0:0.0} °C", this.Temperature);
            }
        }

        public string HumidityDisplay
        {
            get
            {
                return string.Format("{0:0.0} % RH", this.Humidity);
            }
        }
    }
}